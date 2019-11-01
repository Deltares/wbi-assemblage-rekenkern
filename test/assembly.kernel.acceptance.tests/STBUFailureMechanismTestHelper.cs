using System;
using System.Linq;
using assemblage.kernel.acceptance.tests.TestHelpers;
using assembly.kernel.acceptance.tests.data.Input.FailureMechanisms;
using assembly.kernel.acceptance.tests.data.Input.FailureMechanismSections;
using Assembly.Kernel.Implementations;
using Assembly.Kernel.Model;
using Assembly.Kernel.Model.CategoryLimits;
using Assembly.Kernel.Model.FmSectionTypes;
using NUnit.Framework;

namespace assemblage.kernel.acceptance.tests
{
    public class STBUFailureMechanismTestHelper : IFailureMechanismResultTestHelper
    {
        private readonly StbuExpectedFailureMechanismResult expectedFailureMechanismResult;

        public STBUFailureMechanismTestHelper(IExpectedFailureMechanismResult expectedFailureMechanismResult)
        {
            this.expectedFailureMechanismResult = expectedFailureMechanismResult as StbuExpectedFailureMechanismResult;
            if (this.expectedFailureMechanismResult == null)
            {
                throw new ArgumentException();
            }
        }

        public void TestSimpleAssessment()
        {
            var assembler = new AssessmentResultsTranslator();

            foreach (var section in expectedFailureMechanismResult.Sections)
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

        public void TestDetailedAssessment()
        {
            var assembler = new AssessmentResultsTranslator();

            foreach (var section in expectedFailureMechanismResult.Sections)
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

        public void TestTailorMadeAssessment()
        {
            var assembler = new AssessmentResultsTranslator();

            foreach (var section in expectedFailureMechanismResult.Sections)
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

        public void TestCombinedAssessment()
        {
            var assembler = new AssessmentResultsTranslator();

            if (expectedFailureMechanismResult != null)
            {
                foreach (var section in expectedFailureMechanismResult.Sections.OfType<STBUFailureMechanismSection>())
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

        public void TestAssessmentSectionResult()
        {
            var assembler = new FailureMechanismResultAssembler();

            // WBI-1A-1
            EFailureMechanismCategory result = assembler.AssembleFailureMechanismWbi1A1(
                expectedFailureMechanismResult.Sections.Select(CreateFmSectionAssemblyDirectResult),
                false
            );

            Assert.AreEqual(expectedFailureMechanismResult.ExpectedAssessmentResult, result);
        }

        public void TestAssessmentSectionResultTemporal()
        {
            var assembler = new FailureMechanismResultAssembler();

            // WBI-1A-1
            EFailureMechanismCategory result = assembler.AssembleFailureMechanismWbi1A1(
                expectedFailureMechanismResult.Sections.Select(CreateFmSectionAssemblyDirectResult),
                true
            );

            Assert.AreEqual(expectedFailureMechanismResult.ExpectedAssessmentResultTemporal, result);
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
                new FmSectionCategory(EFmSectionCategory.IIv, 0.0, expectedFailureMechanismResult.ExpectedSectionsCategoryDivisionProbability),
                new FmSectionCategory(EFmSectionCategory.Vv, expectedFailureMechanismResult.ExpectedSectionsCategoryDivisionProbability, 1.0)
            });
        }
    }
}