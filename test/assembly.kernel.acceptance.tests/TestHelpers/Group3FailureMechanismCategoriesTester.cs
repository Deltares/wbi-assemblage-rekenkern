using System;
using assembly.kernel.acceptance.tests.data.Input.FailureMechanisms;
using Assembly.Kernel.Implementations;
using Assembly.Kernel.Model;

namespace assemblage.kernel.acceptance.tests.TestHelpers
{
    public class Group3FailureMechanismCategoriesTester : BenchmarkTestsBase, ICategoriesTester
    {
        private readonly Group3ExpectedFailureMechanismResult failureMechanismResult;
        private readonly double lowerBoundaryNorm;
        private readonly double signallingNorm;

        public Group3FailureMechanismCategoriesTester(IExpectedFailureMechanismResult expectedFailureMechanismResult, double lowerBoundaryNorm, double signallingNorm)
        {
            failureMechanismResult = expectedFailureMechanismResult as Group3ExpectedFailureMechanismResult;
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

            // test section categories
            var categoriesListFailureMechanismSection = calculator.CalculateFmSectionCategoryLimitsWbi01(
                new AssessmentSection(1.0, signallingNorm, lowerBoundaryNorm),
                new FailureMechanism(failureMechanismResult.LengthEffectFactor,
                    failureMechanismResult.FailureMechanismProbabilitySpace));
            var expectedFailureMechanismSectionCategories = failureMechanismResult.ExpectedFailureMechanismSectionCategories;

            return AssertEqualCategoriesList(categoriesListFailureMechanismSection, expectedFailureMechanismSectionCategories);
        }
    }
}