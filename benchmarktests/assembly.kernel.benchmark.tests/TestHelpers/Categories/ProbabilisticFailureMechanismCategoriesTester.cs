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
using System.Linq;
using assembly.kernel.benchmark.tests.data.Input.FailureMechanisms;
using assembly.kernel.benchmark.tests.data.Input.FailureMechanismSections;
using assembly.kernel.benchmark.tests.data.Result;
using Assembly.Kernel.Implementations;
using Assembly.Kernel.Model;
using Assembly.Kernel.Model.FmSectionTypes;

namespace assembly.kernel.benchmark.tests.TestHelpers.Categories
{
    public class ProbabilisticFailureMechanismCategoriesTester : BenchmarkTestsBase, ICategoriesTester
    {
        private readonly ProbabilisticExpectedFailureMechanismResult failureMechanismResult;
        private readonly double lowerBoundaryNorm;
        private readonly bool mechanismNotApplicable;
        private readonly MethodResultsListing methodResult;
        private readonly double signallingNorm;

        public ProbabilisticFailureMechanismCategoriesTester(MethodResultsListing methodResult, IExpectedFailureMechanismResult expectedFailureMechanismResult, double lowerBoundaryNorm, double signallingNorm)
        {
            this.methodResult = methodResult;
            failureMechanismResult = expectedFailureMechanismResult as ProbabilisticExpectedFailureMechanismResult;
            this.lowerBoundaryNorm = lowerBoundaryNorm;
            this.signallingNorm = signallingNorm;
            if (failureMechanismResult == null || double.IsNaN(lowerBoundaryNorm) || double.IsNaN(signallingNorm))
            {
                throw new ArgumentException();
            }

            mechanismNotApplicable = expectedFailureMechanismResult.Sections.Count() == 1 &&
                                     expectedFailureMechanismResult.Sections
                                         .OfType<FailureMechanismSectionBase<EFmSectionCategory>>().First()
                                         .ExpectedCombinedResult == EFmSectionCategory.NotApplicable;
        }

        public bool? TestCategories()
        {
            var calculator = new CategoryLimitsCalculator();
            // Test failure mechanism categories
            var categoriesListFailureMechanism = calculator.CalculateFailureMechanismCategoryLimitsWbi11(
                new AssessmentSection(1.0, signallingNorm, lowerBoundaryNorm), 
                new Assembly.Kernel.Model.FailureMechanism(failureMechanismResult.LengthEffectFactor,
                    failureMechanismResult.FailureMechanismProbabilitySpace));
            var expectedFailureMechanismCategories = failureMechanismResult.ExpectedFailureMechanismCategories;

            var areEqualCategoryLimitsFailureMechanism = Assert.AssertEqualCategoriesList(expectedFailureMechanismCategories, categoriesListFailureMechanism);
            methodResult.Wbi11 = GetUpdatedMethodResult(methodResult.Wbi11, areEqualCategoryLimitsFailureMechanism);

            var areEqualCategoryLimitsFailureMechanismSections = true;
            if (!mechanismNotApplicable)
            {
                // test section categories
                var categoriesListFailureMechanismSection = calculator.CalculateFmSectionCategoryLimitsWbi01(
                    new AssessmentSection(1.0, signallingNorm, lowerBoundaryNorm),
                    new Assembly.Kernel.Model.FailureMechanism(failureMechanismResult.LengthEffectFactor,
                        failureMechanismResult.FailureMechanismProbabilitySpace));
                var expectedFailureMechanismSectionCategories = failureMechanismResult.ExpectedFailureMechanismSectionCategories;

                areEqualCategoryLimitsFailureMechanismSections = Assert.AssertEqualCategoriesList(categoriesListFailureMechanismSection, expectedFailureMechanismSectionCategories);
                methodResult.Wbi01 = GetUpdatedMethodResult(methodResult.Wbi01, areEqualCategoryLimitsFailureMechanismSections);
            }

            return areEqualCategoryLimitsFailureMechanism && areEqualCategoryLimitsFailureMechanismSections;
        }
    }
}