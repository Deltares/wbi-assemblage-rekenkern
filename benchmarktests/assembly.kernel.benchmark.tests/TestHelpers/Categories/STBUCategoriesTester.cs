using System;
using System.Linq;
using assembly.kernel.benchmark.tests.data.Input.FailureMechanisms;
using assembly.kernel.benchmark.tests.data.Input.FailureMechanismSections;
using assembly.kernel.benchmark.tests.data.Result;
using Assembly.Kernel.Implementations;
using Assembly.Kernel.Model.CategoryLimits;
using Assembly.Kernel.Model.FmSectionTypes;

namespace assembly.kernel.benchmark.tests.TestHelpers.Categories
{
    public class STBUCategoriesTester : BenchmarkTestsBase, ICategoriesTester
    {
        private readonly StbuExpectedFailureMechanismResult failureMechanismResult;
        private readonly double norm;
        private readonly MethodResultsListing methodResult;
        private readonly bool mechanismNotApplicable;

        public STBUCategoriesTester(MethodResultsListing methodResult, IExpectedFailureMechanismResult expectedFailureMechanismResult, double signallingNorm, double lowerBoundaryNorm)
        {
            failureMechanismResult = expectedFailureMechanismResult as StbuExpectedFailureMechanismResult;
            this.methodResult = methodResult;
            if (failureMechanismResult == null)
            {
                throw new ArgumentException();
            }
            this.norm = failureMechanismResult.UseSignallingNorm ? signallingNorm : lowerBoundaryNorm;
            if (double.IsNaN(norm))
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
            var categoriesList = calculator.CalculateFmSectionCategoryLimitsWbi02(norm,
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