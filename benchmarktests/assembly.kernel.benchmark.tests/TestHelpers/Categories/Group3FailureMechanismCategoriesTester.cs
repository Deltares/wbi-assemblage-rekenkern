﻿#region Copyright (C) Rijkswaterstaat 2019. All rights reserved
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
using System.Linq;
using assembly.kernel.benchmark.tests.data.Input.FailureMechanisms;
using assembly.kernel.benchmark.tests.data.Input.FailureMechanismSections;
using assembly.kernel.benchmark.tests.data.Result;
using Assembly.Kernel.Implementations;
using Assembly.Kernel.Model;
using Assembly.Kernel.Model.CategoryLimits;
using Assembly.Kernel.Model.FmSectionTypes;

namespace assembly.kernel.benchmark.tests.TestHelpers.Categories
{
    /// <summary>
    /// Categories tester for failure mechanisms in group 3.
    /// </summary>
    public class Group3FailureMechanismCategoriesTester : ICategoriesTester
    {
        private readonly Group3ExpectedFailureMechanismResult failureMechanismResult;
        private readonly double lowerBoundaryNorm;
        private readonly bool mechanismNotApplicable;
        private readonly MethodResultsListing methodResults;
        private readonly double signallingNorm;

        /// <summary>
        /// Creates a new instance of <see cref="Group3FailureMechanismCategoriesTester"/>.
        /// </summary>
        /// <param name="methodResults">The method results.</param>
        /// <param name="expectedFailureMechanismResult">The expected failure mechanism result.</param>
        /// <param name="lowerBoundaryNorm">The lower boundary norm.</param>
        /// <param name="signallingNorm">The signalling norm.</param>
        public Group3FailureMechanismCategoriesTester(MethodResultsListing methodResults,
                                                      IExpectedFailureMechanismResult expectedFailureMechanismResult,
                                                      double lowerBoundaryNorm, double signallingNorm)
        {
            failureMechanismResult = expectedFailureMechanismResult as Group3ExpectedFailureMechanismResult;
            this.lowerBoundaryNorm = lowerBoundaryNorm;
            this.signallingNorm = signallingNorm;
            this.methodResults = methodResults;
            if (failureMechanismResult == null || double.IsNaN(lowerBoundaryNorm) || double.IsNaN(signallingNorm))
            {
                throw new ArgumentException();
            }

            mechanismNotApplicable = expectedFailureMechanismResult.Sections.Count() == 1 &&
                                     expectedFailureMechanismResult.Sections
                                                                   .OfType<FailureMechanismSectionBase<EFmSectionCategory>>()
                                                                   .First()
                                                                   .ExpectedCombinedResult == EFmSectionCategory.NotApplicable;
        }

        public bool? TestCategories()
        {
            if (mechanismNotApplicable)
            {
                return null;
            }

            var calculator = new CategoryLimitsCalculator();

            // test section categories
            CategoriesList<FmSectionCategory> categoriesListFailureMechanismSection =
                calculator.CalculateFmSectionCategoryLimitsWbi01(
                    new AssessmentSection(1.0, signallingNorm, lowerBoundaryNorm),
                    new Assembly.Kernel.Model.FailureMechanism(failureMechanismResult.LengthEffectFactor,
                                                               failureMechanismResult.FailureMechanismProbabilitySpace));
            var expectedFailureMechanismSectionCategories = failureMechanismResult.ExpectedFailureMechanismSectionCategories;

            var assertEqualCategoriesList =
                AssertHelper.AssertEqualCategoriesList<FmSectionCategory, EFmSectionCategory>(
                    categoriesListFailureMechanismSection, expectedFailureMechanismSectionCategories);
            methodResults.Wbi01 = BenchmarkTestHelper.GetUpdatedMethodResult(methodResults.Wbi01, assertEqualCategoriesList);

            return assertEqualCategoriesList;
        }
    }
}