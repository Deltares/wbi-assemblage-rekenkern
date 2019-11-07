using System.Linq;
using assembly.kernel.acceptance.tests.data.Input.FailureMechanisms;
using assembly.kernel.acceptance.tests.data.Input.FailureMechanismSections;
using assembly.kernel.acceptance.tests.data.Result;
using Assembly.Kernel.Implementations;
using Assembly.Kernel.Model;
using Assembly.Kernel.Model.CategoryLimits;
using Assembly.Kernel.Model.FmSectionTypes;
using NUnit.Framework;

namespace assemblage.kernel.acceptance.tests.TestHelpers.FailureMechanism
{
    public class StbuFailureMechanismTester : FailureMechanismResultTesterBase<StbuExpectedFailureMechanismResult>
    {
        public StbuFailureMechanismTester(MethodResultsListing methodResults, IExpectedFailureMechanismResult expectedFailureMechanismResult) : base(methodResults, expectedFailureMechanismResult)
        {
        }

        protected override void TestSimpleAssessmentInternal()
        {
            var assembler = new AssessmentResultsTranslator();

            foreach (var section in ExpectedFailureMechanismResult.Sections)
            {
                var stbuFailureMechanismSection = section as STBUFailureMechanismSection;
                if (stbuFailureMechanismSection != null)
                {
                    // WBI-0E-1
                    FmSectionAssemblyDirectResult result = assembler.TranslateAssessmentResultWbi0E1(stbuFailureMechanismSection.SimpleAssessmentResult);
                    var expectedResult = stbuFailureMechanismSection.ExpectedSimpleAssessmentAssemblyResult as FmSectionAssemblyDirectResult;
                    Assert.AreEqual(expectedResult.Result, result.Result);
                }
            }
        }

        protected override void TestDetailedAssessmentInternal()
        {
            var assembler = new AssessmentResultsTranslator();

            foreach (var section in ExpectedFailureMechanismResult.Sections)
            {
                var stbuFailureMechanismSection = section as STBUFailureMechanismSection;
                if (stbuFailureMechanismSection != null)
                {
                    // WBI-0G-3
                    FmSectionAssemblyDirectResult result = assembler.TranslateAssessmentResultWbi0G3(
                        stbuFailureMechanismSection.DetailedAssessmentResult,
                        stbuFailureMechanismSection.DetailedAssessmentResultProbability,
                        GetSTBUCategories());

                    var expectedResult =
                        stbuFailureMechanismSection.ExpectedDetailedAssessmentAssemblyResult as
                            FmSectionAssemblyDirectResult;
                    Assert.AreEqual(expectedResult.Result, result.Result);
                }
            }
        }

        protected override void TestTailorMadeAssessmentInternal()
        {
            var assembler = new AssessmentResultsTranslator();

            foreach (var section in ExpectedFailureMechanismResult.Sections)
            {
                var stbuFailureMechanismSection = section as STBUFailureMechanismSection;
                if (stbuFailureMechanismSection != null)
                {
                    // WBI-0T-7
                    FmSectionAssemblyDirectResult result = assembler.TranslateAssessmentResultWbi0T7(
                        stbuFailureMechanismSection.TailorMadeAssessmentResult,
                        stbuFailureMechanismSection.TailorMadeAssessmentResultProbability,
                        GetSTBUCategories());

                    var expectedResult = stbuFailureMechanismSection.ExpectedTailorMadeAssessmentAssemblyResult as FmSectionAssemblyDirectResult;
                    Assert.AreEqual(expectedResult.Result, result.Result);
                }
            }
        }

        protected override void TestCombinedAssessmentInternal()
        {
            var assembler = new AssessmentResultsTranslator();

            if (ExpectedFailureMechanismResult != null)
            {
                foreach (var section in ExpectedFailureMechanismResult.Sections.OfType<STBUFailureMechanismSection>())
                {
                    // WBI-0A-1 (direct with probability)
                    var result = assembler.TranslateAssessmentResultWbi0A1(
                        section.ExpectedSimpleAssessmentAssemblyResult as FmSectionAssemblyDirectResult,
                        section.ExpectedDetailedAssessmentAssemblyResult as FmSectionAssemblyDirectResult,
                        section.ExpectedTailorMadeAssessmentAssemblyResult as FmSectionAssemblyDirectResult);

                    Assert.IsInstanceOf<FmSectionAssemblyDirectResult>(result);
                    Assert.AreEqual(section.ExpectedCombinedResult, result.Result);
                }
            }
        }

        protected override void TestAssessmentSectionResultInternal()
        {
            var assembler = new FailureMechanismResultAssembler();

            // WBI-1A-1
            EFailureMechanismCategory result = assembler.AssembleFailureMechanismWbi1A1(
                ExpectedFailureMechanismResult.Sections.Select(CreateFmSectionAssemblyDirectResult),
                false
            );

            Assert.AreEqual(ExpectedFailureMechanismResult.ExpectedAssessmentResult, result);
        }

        protected override void TestAssessmentSectionResultTemporalInternal()
        {
            var assembler = new FailureMechanismResultAssembler();

            // WBI-1A-1
            EFailureMechanismCategory result = assembler.AssembleFailureMechanismWbi1A1(
                ExpectedFailureMechanismResult.Sections.Select(CreateFmSectionAssemblyDirectResult),
                true
            );

            Assert.AreEqual(ExpectedFailureMechanismResult.ExpectedAssessmentResultTemporal, result);
        }

        protected override void SetSimpleAssessmentMethodResult(bool result)
        {
            MethodResults.Wbi0E1 = GetUpdatedMethodResult(MethodResults.Wbi0E1, result);
        }

        protected override void SetDetailedAssessmentMethodResult(bool result)
        {
            MethodResults.Wbi0G3 = GetUpdatedMethodResult(MethodResults.Wbi0G3, result);
        }

        protected override void SetTailorMadeAssessmentMethodResult(bool result)
        {
            MethodResults.Wbi0T7 = GetUpdatedMethodResult(MethodResults.Wbi0T7, result);
        }

        protected override void SetCombinedAssessmentMethodResult(bool result)
        {
            MethodResults.Wbi0A1 = GetUpdatedMethodResult(MethodResults.Wbi0A1, result);
        }

        protected override void SetAssessmentSectionMethodResult(bool result)
        {
            MethodResults.Wbi1A1 = GetUpdatedMethodResult(MethodResults.Wbi1A1, result);
        }

        protected override void SetAssessmentSectionMethodResultTemporal(bool result)
        {
            MethodResults.Wbi1A1T = GetUpdatedMethodResult(MethodResults.Wbi1A1T, result);
        }

        private FmSectionAssemblyDirectResult CreateFmSectionAssemblyDirectResult(IFailureMechanismSection section)
        {
            var directMechanismSection = section as FailureMechanismSectionBase<EFmSectionCategory>;
            return new FmSectionAssemblyDirectResult(directMechanismSection.ExpectedCombinedResult);
        }

        private CategoriesList<FmSectionCategory> GetSTBUCategories()
        {
            return new CategoriesList<FmSectionCategory>(new[]
            {
                new FmSectionCategory(EFmSectionCategory.IIv, 0.0, ExpectedFailureMechanismResult.ExpectedSectionsCategoryDivisionProbability),
                new FmSectionCategory(EFmSectionCategory.Vv, ExpectedFailureMechanismResult.ExpectedSectionsCategoryDivisionProbability, 1.0)
            });
        }
    }
}