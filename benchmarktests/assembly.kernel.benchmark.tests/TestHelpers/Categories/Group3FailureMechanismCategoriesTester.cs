using System;
using assembly.kernel.benchmark.tests.data.Input.FailureMechanisms;
using assembly.kernel.benchmark.tests.data.Result;
using Assembly.Kernel.Implementations;
using Assembly.Kernel.Model;

namespace assembly.kernel.benchmark.tests.TestHelpers.Categories
{
    public class Group3FailureMechanismCategoriesTester : BenchmarkTestsBase, ICategoriesTester
    {
        private readonly Group3ExpectedFailureMechanismResult failureMechanismResult;
        private readonly double lowerBoundaryNorm;
        private readonly double signallingNorm;
        private readonly MethodResultsListing methodResult;

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
        }

        public bool TestCategories()
        {
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