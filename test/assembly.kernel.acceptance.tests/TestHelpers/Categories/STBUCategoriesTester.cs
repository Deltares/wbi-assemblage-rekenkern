using System;
using assembly.kernel.acceptance.tests.data.Input.FailureMechanisms;
using assembly.kernel.acceptance.tests.data.Result;
using Assembly.Kernel.Implementations;
using Assembly.Kernel.Model.CategoryLimits;
using Assembly.Kernel.Model.FmSectionTypes;

namespace assemblage.kernel.acceptance.tests.TestHelpers.Categories
{
    public class STBUCategoriesTester : BenchmarkTestsBase, ICategoriesTester
    {
        private readonly StbuExpectedFailureMechanismResult failureMechanismResult;
        private readonly double signallingNorm;
        private readonly MethodResultsListing methodResult;

        public STBUCategoriesTester(MethodResultsListing methodResult, IExpectedFailureMechanismResult expectedFailureMechanismResult, double signallingNorm)
        {
            failureMechanismResult = expectedFailureMechanismResult as StbuExpectedFailureMechanismResult;
            this.signallingNorm = signallingNorm;
            this.methodResult = methodResult;
            if (failureMechanismResult == null || double.IsNaN(signallingNorm))
            {
                throw new ArgumentException();
            }
        }

        public bool TestCategories()
        {
            var calculator = new CategoryLimitsCalculator();
            var categoriesList = calculator.CalculateFmSectionCategoryLimitsWbi02(signallingNorm,
                new Assembly.Kernel.Model.FailureMechanism(failureMechanismResult.LengthEffectFactor,
                    failureMechanismResult.FailureMechanismProbabilitySpace));

            var assertEqualCategoriesList = AssertEqualCategoriesList(GetExpectedCategories(), categoriesList);
            methodResult.Wbi02 = GetUpdatedMethodResult(methodResult.Wbi02, assertEqualCategoriesList);

            return assertEqualCategoriesList;
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