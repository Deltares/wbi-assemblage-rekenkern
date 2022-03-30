#region Copyright (C) Rijkswaterstaat 2022. All rights reserved

// Copyright (C) Rijkswaterstaat 2022. All rights reserved.
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
// All names, logos, and references to "Rijkswaterstaat" are registered trademarks of
// Rijkswaterstaat and remain full property of Rijkswaterstaat at all times.
// All rights reserved.

#endregion

using System;
using System.Collections;
using assembly.kernel.benchmark.tests.data.Input.FailureMechanisms;
using assembly.kernel.benchmark.tests.data.Result;
using Assembly.Kernel.Model.Categories;
using NUnit.Framework;

namespace assembly.kernel.benchmark.tests.TestHelpers.FailureMechanism
{
    /// <summary>
    /// Base class for failure mechanism result tester.
    /// </summary>
    /// <typeparam name="TFailureMechanismResult">The type of failure mechanism result.</typeparam>
    public abstract class FailureMechanismResultTesterBase : IFailureMechanismResultTester
    {
        protected readonly ExpectedFailureMechanismResult ExpectedFailureMechanismResult;
        protected readonly MethodResultsListing MethodResults;
        protected readonly CategoriesList<InterpretationCategory> InterpretationCategories;

        /// <summary>
        /// Creates a new instance of <see cref="FailureMechanismResultTesterBase{TFailureMechanismResult}"/>.
        /// </summary>
        /// <param name="methodResults">The method results.</param>
        /// <param name="expectedFailureMechanismResult">The expected failure mechanism results.</param>
        /// <param name="interpretationCategories"></param>
        protected FailureMechanismResultTesterBase(MethodResultsListing methodResults,
            ExpectedFailureMechanismResult expectedFailureMechanismResult,
            CategoriesList<InterpretationCategory> interpretationCategories)
        {
            ExpectedFailureMechanismResult = expectedFailureMechanismResult;
            MethodResults = methodResults;
            InterpretationCategories = interpretationCategories;
            if (ExpectedFailureMechanismResult == null || InterpretationCategories == null)
            {
                throw new ArgumentException();
            }
        }

        public virtual bool TestCombinedAssessment()
        {
            try
            {
                TestCombinedAssessmentInternal();
                SetCombinedAssessmentMethodResult(true);
                return true;
            }
            catch (AssertionException e)
            {
                foreach (DictionaryEntry entry in e.Data)
                {
                    Console.WriteLine("{0}: Gecombineerde faalkans per vak - vaknaam '{1}' : {2}", ExpectedFailureMechanismResult.Name,
                        entry.Key,((AssertionException)entry.Value).Message);
                }
                SetCombinedAssessmentMethodResult(false);
                return false;
            }
        }

        public virtual bool TestAssessmentSectionResult()
        {
            try
            {
                TestAssessmentSectionResultInternal();
                SetAssessmentSectionMethodResult(true);
                return true;
            }
            catch (AssertionException e)
            {
                Console.WriteLine("{0}: Faalkans per traject - {1}", ExpectedFailureMechanismResult.Name,
                                  e.Message);
                SetAssessmentSectionMethodResult(false);
                return false;
            }
        }

        public virtual bool TestAssessmentSectionResultPartial()
        {
            try
            {
                TestAssessmentSectionResultPartialInternal();
                SetAssessmentSectionMethodResultPartial(true);
                return true;
            }
            catch (AssertionException e)
            {
                Console.WriteLine("{0}: Voorlopig toetsoordeel per traject - {1}", ExpectedFailureMechanismResult.Name,
                                  e.Message);
                SetAssessmentSectionMethodResultPartial(false);
                return false;
            }
        }

        protected abstract void SetCombinedAssessmentMethodResult(bool result);

        protected virtual void TestCombinedAssessmentInternal() { }

        protected abstract void SetAssessmentSectionMethodResult(bool result);

        protected virtual void TestAssessmentSectionResultInternal() { }

        protected abstract void SetAssessmentSectionMethodResultPartial(bool result);

        protected virtual void TestAssessmentSectionResultPartialInternal() { }
    }
}