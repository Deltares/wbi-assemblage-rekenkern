// Copyright (C) Stichting Deltares and State of the Netherlands 2023. All rights reserved.
//
// This file is part of the Assembly kernel.
//
// Assembly kernel is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU Lesser General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public License
// along with this program. If not, see <http://www.gnu.org/licenses/>.
//
// All names, logos, and references to "Deltares" are registered trademarks of
// Stichting Deltares and remain full property of Stichting Deltares at all times.
// All rights reserved.

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Assembly.Kernel.Acceptance.TestUtil;
using Assembly.Kernel.Acceptance.TestUtil.Data.Input.FailureMechanisms;
using Assembly.Kernel.Acceptance.TestUtil.Data.Result;
using Assembly.Kernel.Model;
using Assembly.Kernel.Model.Categories;
using NUnit.Framework;

namespace Assembly.Kernel.Acceptance.Test.TestHelpers.FailureMechanism
{
    /// <summary>
    /// Base class for failure mechanism result tester.
    /// </summary>
    public abstract class FailureMechanismResultTesterBase
    {
        protected readonly ExpectedFailureMechanismResult ExpectedFailureMechanismResult;
        protected readonly MethodResultsListing MethodResults;
        protected readonly CategoriesList<InterpretationCategory> InterpretationCategories;

        /// <summary>
        /// Creates a new instance of <see cref="FailureMechanismResultTesterBase"/>.
        /// </summary>
        /// <param name="methodResults">The method results.</param>
        /// <param name="expectedFailureMechanismResult">The expected failure mechanism results.</param>
        /// <param name="interpretationCategories">The interpretation categories needed as input for the calculations.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="expectedFailureMechanismResult"/>
        /// or <paramref name="interpretationCategories"/> is <c>null</c>.</exception>
        protected FailureMechanismResultTesterBase(MethodResultsListing methodResults,
                                                   ExpectedFailureMechanismResult expectedFailureMechanismResult,
                                                   CategoriesList<InterpretationCategory> interpretationCategories)
        {
            if (expectedFailureMechanismResult == null)
            {
                throw new ArgumentNullException(nameof(expectedFailureMechanismResult));
            }

            if (interpretationCategories == null)
            {
                throw new ArgumentNullException(nameof(interpretationCategories));
            }

            ExpectedFailureMechanismResult = expectedFailureMechanismResult;
            MethodResults = methodResults;
            InterpretationCategories = interpretationCategories;
        }

        public bool TestFailureMechanismSectionResults()
        {
            try
            {
                TestFailureMechanismSectionResultsInternal();
                SetFailureMechanismSectionMethodResults();
                return true;
            }
            catch (AssertionException e)
            {
                foreach (DictionaryEntry entry in e.Data)
                {
                    Console.WriteLine($"{ExpectedFailureMechanismResult.Name}: Gecombineerde faalkans per vak - vaknaam '{entry.Key}' " +
                                      $": {((AssertionException) entry.Value).Message}");
                }

                SetFailureMechanismSectionMethodResults();
                return false;
            }
        }

        public bool TestFailureMechanismResult()
        {
            try
            {
                TestFailureMechanismResultInternal(false, ExpectedFailureMechanismResult.ExpectedCombinedProbability);
                SetFailureMechanismMethodResult(false, true);
                return true;
            }
            catch (AssertionException e)
            {
                Console.WriteLine($"{ExpectedFailureMechanismResult.Name}: Faalkans per traject - {e.Message}");
                SetFailureMechanismMethodResult(false, false);
                return false;
            }
        }

        public bool TestFailureMechanismResultPartial()
        {
            try
            {
                TestFailureMechanismResultInternal(true, ExpectedFailureMechanismResult.ExpectedCombinedProbabilityPartial);
                SetFailureMechanismMethodResult(true, true);
                return true;
            }
            catch (AssertionException e)
            {
                Console.WriteLine($"{ExpectedFailureMechanismResult.Name}: Voorlopig toetsoordeel per traject - {e.Message}");
                SetFailureMechanismMethodResult(true, false);
                return false;
            }
        }

        public bool TestFailureMechanismTheoreticalBoundaries()
        {
            try
            {
                TestFailureMechanismTheoreticalBoundariesInternal(false, ExpectedFailureMechanismResult.ExpectedTheoreticalBoundaries);
                SetFailureMechanismTheoreticalBoundariesResult(false, true);
                return true;
            }
            catch (AssertionException e)
            {
                Console.WriteLine($"{ExpectedFailureMechanismResult.Name}: Theoretische grenzen per faalmechanisme - {e.Message}");
                SetFailureMechanismTheoreticalBoundariesResult(false, false);
                return false;
            }
        }

        public bool TestFailureMechanismTheoreticalBoundariesPartial()
        {
            try
            {
                TestFailureMechanismTheoreticalBoundariesInternal(true, ExpectedFailureMechanismResult.ExpectedTheoreticalBoundariesPartial);
                SetFailureMechanismTheoreticalBoundariesResult(true, true);
                return true;
            }
            catch (AssertionException e)
            {
                Console.WriteLine($"{ExpectedFailureMechanismResult.Name}: Voorlopige theoretische grenzen per faalmechanisme - {e.Message}");
                SetFailureMechanismTheoreticalBoundariesResult(true, false);
                return false;
            }
        }

        protected abstract void TestFailureMechanismSectionResultsInternal();

        protected abstract void SetFailureMechanismSectionMethodResults();

        protected abstract void TestFailureMechanismResultInternal(bool partial, Probability expectedProbability);

        protected abstract void TestFailureMechanismTheoreticalBoundariesInternal(bool partial, BoundaryLimits expectedBoundaries);

        protected abstract void SetFailureMechanismTheoreticalBoundariesResult(bool partial, bool result);

        protected static EAnalysisState GetAnalysisState(ESectionInitialMechanismProbabilitySpecification relevance,
                                                         ERefinementStatus refinementStatus)
        {
            if (relevance == ESectionInitialMechanismProbabilitySpecification.NotRelevant)
            {
                return EAnalysisState.NotRelevant;
            }

            switch (refinementStatus)
            {
                case ERefinementStatus.NotNecessary:
                    return relevance == ESectionInitialMechanismProbabilitySpecification.RelevantNoProbabilitySpecification
                               ? EAnalysisState.NoProbabilityEstimationNecessary
                               : EAnalysisState.ProbabilityEstimated;
                case ERefinementStatus.Necessary:
                    return EAnalysisState.ProbabilityEstimationNecessary;
                case ERefinementStatus.Performed:
                    return EAnalysisState.ProbabilityEstimated;
                default:
                    throw new InvalidEnumArgumentException(nameof(refinementStatus), (int) refinementStatus, typeof(ERefinementStatus));
            }
        }

        protected static void ThrowAssertionExceptionWithGivenErrors(IDictionary<string, AssertionException> errorsList)
        {
            var exception = new AssertionException("Errors occurred");

            foreach (KeyValuePair<string, AssertionException> error in errorsList)
            {
                exception.Data.Add(error.Key, error.Value);
            }

            throw exception;
        }

        private void SetFailureMechanismMethodResult(bool partial, bool result)
        {
            if (ExpectedFailureMechanismResult.AssemblyMethod == "P1")
            {
                if (partial)
                {
                    MethodResults.Boi1A1P = BenchmarkTestHelper.GetUpdatedMethodResult(MethodResults.Boi1A1P, result);
                }
                else
                {
                    MethodResults.Boi1A1 = BenchmarkTestHelper.GetUpdatedMethodResult(MethodResults.Boi1A1, result);
                }
            }
            else
            {
                if (partial)
                {
                    MethodResults.Boi1A2P = BenchmarkTestHelper.GetUpdatedMethodResult(MethodResults.Boi1A2P, result);
                }
                else
                {
                    MethodResults.Boi1A2 = BenchmarkTestHelper.GetUpdatedMethodResult(MethodResults.Boi1A2, result);
                }
            }
        }
    }
}