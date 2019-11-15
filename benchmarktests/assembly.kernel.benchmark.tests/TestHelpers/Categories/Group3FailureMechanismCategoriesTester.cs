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
    public class Group3FailureMechanismCategoriesTester : BenchmarkTestsBase, ICategoriesTester
    {
        private readonly Group3ExpectedFailureMechanismResult failureMechanismResult;
        private readonly double lowerBoundaryNorm;
        private readonly double signallingNorm;
        private readonly MethodResultsListing methodResult;
        private readonly bool mechanismNotApplicable;

        public Group3FailureMechanismCategoriesTester(MethodResultsListing methodResult, IExpectedFailureMechanismResult expectedFailureMechanismResult, double lowerBoundaryNorm, double signallingNorm)
        {
            failureMechanismResult = expectedFailureMechanismResult as Group3ExpectedFailureMechanismResult;
            this.lowerBoundaryNorm = lowerBoundaryNorm;
            this.signallingNorm = signallingNorm;
            this.methodResult = methodResult;
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
            if (mechanismNotApplicable)
            {
                return null;
            }

            var calculator = new CategoryLimitsCalculator();

            // test section categories
            var categoriesListFailureMechanismSection = calculator.CalculateFmSectionCategoryLimitsWbi01(
                new AssessmentSection(1.0, signallingNorm, lowerBoundaryNorm),
                new Assembly.Kernel.Model.FailureMechanism(failureMechanismResult.LengthEffectFactor,
                    failureMechanismResult.FailureMechanismProbabilitySpace));
            var expectedFailureMechanismSectionCategories = failureMechanismResult.ExpectedFailureMechanismSectionCategories;

            var assertEqualCategoriesList = AssertEqualCategoriesList(categoriesListFailureMechanismSection, expectedFailureMechanismSectionCategories);
            methodResult.Wbi01 = GetUpdatedMethodResult(methodResult.Wbi01, assertEqualCategoriesList);

            return assertEqualCategoriesList;
        }
    }
}