using System;
using assembly.kernel.acceptance.tests.data.Input.FailureMechanisms;
using Assembly.Kernel.Implementations;
using Assembly.Kernel.Model;
using Assembly.Kernel.Model.CategoryLimits;
using Assembly.Kernel.Model.FmSectionTypes;

namespace assemblage.kernel.acceptance.tests.TestHelpers
{
    public class STBUCategoriesTester : BenchmarkTestsBase, ICategoriesTester
    {
        private readonly StbuExpectedFailureMechanismResult failureMechanismResult;
        private readonly double signallingNorm;

        public STBUCategoriesTester(IExpectedFailureMechanismResult expectedFailureMechanismResult, double signallingNorm)
        {
            failureMechanismResult = expectedFailureMechanismResult as StbuExpectedFailureMechanismResult;
            this.signallingNorm = signallingNorm;
            if (failureMechanismResult == null || double.IsNaN(signallingNorm))
            {
                throw new ArgumentException();
            }
        }

        public bool TestCategories()
        {
            var calculator = new CategoryLimitsCalculator();
            var categoriesList = calculator.CalculateFmSectionCategoryLimitsWbi02(signallingNorm,
                new FailureMechanism(failureMechanismResult.LengthEffectFactor,
                    failureMechanismResult.FailureMechanismProbabilitySpace));

            return AssertEqualCategoriesList(GetExpectedCategories(), categoriesList);
        }

        private CategoriesList<FmSectionCategory> GetExpectedCategories()
        {
            return new CategoriesList<FmSectionCategory>(new[]
            {
                new FmSectionCategory(EFmSectionCategory.IIv, 0.0, failureMechanismResult.ExpectedSectionsCategoryDivisionProbability),
                new FmSectionCategory(EFmSectionCategory.Vv, failureMechanismResult.ExpectedSectionsCategoryDivisionProbability, 1.0)
            });
        }
    }
}