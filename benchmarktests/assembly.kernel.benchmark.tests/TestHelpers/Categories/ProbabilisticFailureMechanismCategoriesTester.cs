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
        private readonly double signallingNorm;
        private readonly MethodResultsListing methodResult;
        private readonly bool mechanismNotApplicable;

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

            var areEqualCategoryLimitsFailureMechanism = AssertEqualCategoriesList(expectedFailureMechanismCategories, categoriesListFailureMechanism);
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

                areEqualCategoryLimitsFailureMechanismSections = AssertEqualCategoriesList(categoriesListFailureMechanismSection, expectedFailureMechanismSectionCategories);
                methodResult.Wbi01 = GetUpdatedMethodResult(methodResult.Wbi01, areEqualCategoryLimitsFailureMechanismSections);
            }

            return areEqualCategoryLimitsFailureMechanism && areEqualCategoryLimitsFailureMechanismSections;
        }
    }
}