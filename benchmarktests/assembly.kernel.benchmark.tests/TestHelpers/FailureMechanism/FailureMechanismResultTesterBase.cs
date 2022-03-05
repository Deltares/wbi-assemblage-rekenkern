#region Copyright (C) Rijkswaterstaat 2019. All rights reserved
// Copyright (C) Rijkswaterstaat 2019. All rights reserved.
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
using assembly.kernel.benchmark.tests.data.Input.FailureMechanisms;
using assembly.kernel.benchmark.tests.data.Result;
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

        /// <summary>
        /// Creates a new instance of <see cref="FailureMechanismResultTesterBase{TFailureMechanismResult}"/>.
        /// </summary>
        /// <param name="methodResults">The method results.</param>
        /// <param name="expectedFailureMechanismResult">The expected failure mechanism results.</param>
        protected FailureMechanismResultTesterBase(MethodResultsListing methodResults,
                                                   ExpectedFailureMechanismResult expectedFailureMechanismResult)
        {
            ExpectedFailureMechanismResult = expectedFailureMechanismResult;
            MethodResults = methodResults;
            if (ExpectedFailureMechanismResult == null)
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
                Console.WriteLine("{0}: Gecombineerd toetsoordeel per vak - {1}", ExpectedFailureMechanismResult.Name,
                                  e.Message);
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
                Console.WriteLine("{0}: Toetsoordeel per traject - {1}", ExpectedFailureMechanismResult.Name,
                                  e.Message);
                SetAssessmentSectionMethodResult(false);
                return false;
            }
        }

        public virtual bool TestAssessmentSectionResultTemporal()
        {
            try
            {
                TestAssessmentSectionResultTemporalInternal();
                SetAssessmentSectionMethodResultTemporal(true);
                return true;
            }
            catch (AssertionException e)
            {
                Console.WriteLine("{0}: Voorlopig toetsoordeel per traject - {1}", ExpectedFailureMechanismResult.Name,
                                  e.Message);
                SetAssessmentSectionMethodResultTemporal(false);
                return false;
            }
        }

        protected abstract void SetCombinedAssessmentMethodResult(bool result);

        protected virtual void TestCombinedAssessmentInternal() { }

        protected abstract void SetAssessmentSectionMethodResult(bool result);

        protected virtual void TestAssessmentSectionResultInternal() { }

        protected abstract void SetAssessmentSectionMethodResultTemporal(bool result);

        protected virtual void TestAssessmentSectionResultTemporalInternal() { }
    }
}