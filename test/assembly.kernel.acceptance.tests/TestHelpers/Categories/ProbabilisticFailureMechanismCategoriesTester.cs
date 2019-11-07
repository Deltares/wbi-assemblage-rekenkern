using System;
using assembly.kernel.acceptance.tests.data.Input.FailureMechanisms;
using assembly.kernel.acceptance.tests.data.Result;
using Assembly.Kernel.Implementations;
using Assembly.Kernel.Model;

namespace assemblage.kernel.acceptance.tests.TestHelpers.Categories
{
    public class ProbabilisticFailureMechanismCategoriesTester : BenchmarkTestsBase, ICategoriesTester
    {
        private readonly ProbabilisticExpectedFailureMechanismResult failureMechanismResult;
        private readonly double lowerBoundaryNorm;
        private readonly double signallingNorm;
        private readonly MethodResultsListing methodResult;

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
        }

        public bool TestCategories()
        {
            var calculator = new CategoryLimitsCalculator();
            // Test failure mechanism categories
            var categoriesListFailureMechanism = calculator.CalculateFailureMechanismCategoryLimitsWbi11(
                new AssessmentSection(1.0, signallingNorm, lowerBoundaryNorm), 
                new Assembly.Kernel.Model.FailureMechanism(failureMechanismResult.LengthEffectFactor,
                    failureMechanismResult.FailureMechanismProbabilitySpace));
            var expectedFailureMechanismCategories = failureMechanismResult.ExpectedFailureMechanismCategories;

            // test section categories
            var categoriesListFailureMechanismSection = calculator.CalculateFmSectionCategoryLimitsWbi01(
                new AssessmentSection(1.0, signallingNorm, lowerBoundaryNorm),
                new Assembly.Kernel.Model.FailureMechanism(failureMechanismResult.LengthEffectFactor,
                    failureMechanismResult.FailureMechanismProbabilitySpace));
            var expectedFailureMechanismSectionCategories = failureMechanismResult.ExpectedFailureMechanismSectionCategories;

            var areEqualCategoryLimitsFailureMechanism = AssertEqualCategoriesList(expectedFailureMechanismCategories, categoriesListFailureMechanism);
            methodResult.Wbi11 = GetUpdatedMethodResult(methodResult.Wbi11, areEqualCategoryLimitsFailureMechanism);

            var areEqualCategoryLimitsFailureMechanismSections = AssertEqualCategoriesList(categoriesListFailureMechanismSection, expectedFailureMechanismSectionCategories);
            methodResult.Wbi01 = GetUpdatedMethodResult(methodResult.Wbi01, areEqualCategoryLimitsFailureMechanismSections);

            return areEqualCategoryLimitsFailureMechanism &&
                   areEqualCategoryLimitsFailureMechanismSections;
        }
    }
}