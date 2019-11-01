using System;
using System.Linq;
using assembly.kernel.acceptance.tests.data.Input.FailureMechanisms;
using assembly.kernel.acceptance.tests.data.Input.FailureMechanismSections;
using Assembly.Kernel.Implementations;
using Assembly.Kernel.Model;
using Assembly.Kernel.Model.FmSectionTypes;
using NUnit.Framework;

namespace assemblage.kernel.acceptance.tests.TestHelpers
{
    public class NWOocFailureMechanismTestHelper : IFailureMechanismResultTestHelper
    {
        private readonly Group4Or5ExpectedFailureMechanismResult expectedFailureMechanismResult;

        public NWOocFailureMechanismTestHelper(IExpectedFailureMechanismResult expectedFailureMechanismResult)
        {
            this.expectedFailureMechanismResult = expectedFailureMechanismResult as Group4Or5ExpectedFailureMechanismResult;
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
                var nwOocFailureMechanismSection = section as NWOocFailureMechanismSection;
                if (nwOocFailureMechanismSection != null)
                {
                    // WBI-0E-4
                    FmSectionAssemblyIndirectResult result = assembler.TranslateAssessmentResultWbi0E4(nwOocFailureMechanismSection.SimpleAssessmentResult);
                    var expectedResult = nwOocFailureMechanismSection.ExpectedSimpleAssessmentAssemblyResult as FmSectionAssemblyIndirectResult;
                    Assert.AreEqual(expectedResult.Result, result.Result);
                }
            }
        }

        public void TestDetailedAssessment()
        {
            var assembler = new AssessmentResultsTranslator();

            foreach (var section in expectedFailureMechanismResult.Sections)
            {
                var nwOocFailureMechanismSection = section as NWOocFailureMechanismSection;
                if (nwOocFailureMechanismSection != null)
                {
                    // WBI-0G-2
                    FmSectionAssemblyIndirectResult result = assembler.TranslateAssessmentResultWbi0G2(nwOocFailureMechanismSection.DetailedAssessmentResult);

                    var expectedResult =
                        nwOocFailureMechanismSection.ExpectedDetailedAssessmentAssemblyResult as
                            FmSectionAssemblyIndirectResult;
                    Assert.AreEqual(expectedResult.Result, result.Result);
                }
            }
        }

        public void TestTailorMadeAssessment()
        {
            var assembler = new AssessmentResultsTranslator();

            foreach (var section in expectedFailureMechanismResult.Sections)
            {
                var nwOocFailureMechanismSection = section as NWOocFailureMechanismSection;
                if (nwOocFailureMechanismSection != null)
                {
                    // WBI-0T-1
                    FmSectionAssemblyIndirectResult result = assembler.TranslateAssessmentResultWbi0T2(nwOocFailureMechanismSection.TailorMadeAssessmentResult);

                    var expectedResult = nwOocFailureMechanismSection.ExpectedTailorMadeAssessmentAssemblyResult as FmSectionAssemblyIndirectResult;
                    Assert.AreEqual(expectedResult.Result, result.Result);
                }
            }
        }

        public void TestCombinedAssessment()
        {
            var assembler = new AssessmentResultsTranslator();

            if (expectedFailureMechanismResult != null)
            {
                foreach (var section in expectedFailureMechanismResult.Sections.OfType<NWOocFailureMechanismSection>())
                {
                    // WBI-0A-1 (direct with probability)
                    var result = assembler.TranslateAssessmentResultWbi0A1(
                        section.ExpectedSimpleAssessmentAssemblyResult as FmSectionAssemblyIndirectResult,
                        section.ExpectedDetailedAssessmentAssemblyResult as FmSectionAssemblyIndirectResult,
                        section.ExpectedTailorMadeAssessmentAssemblyResult as FmSectionAssemblyIndirectResult);
                
                    Assert.IsInstanceOf<FmSectionAssemblyIndirectResult>(result);
                    Assert.AreEqual(section.ExpectedCombinedResult, result.Result);
                }
            }
        }

        public void TestAssessmentSectionResult()
        {
            var assembler = new FailureMechanismResultAssembler();

            // WBI-1A-2
            var result = assembler.AssembleFailureMechanismWbi1A2(
                expectedFailureMechanismResult.Sections.Select(CreateFmSectionAssemblyIndirectResult),
                false
            );

            Assert.AreEqual(expectedFailureMechanismResult.ExpectedAssessmentResult, result);
        }

        public void TestAssessmentSectionResultTemporal()
        {
            var assembler = new FailureMechanismResultAssembler();

            // WBI-1A-2
            var result = assembler.AssembleFailureMechanismWbi1A2(
                expectedFailureMechanismResult.Sections.Select(CreateFmSectionAssemblyIndirectResult),
                true
            );

            Assert.AreEqual(expectedFailureMechanismResult.ExpectedAssessmentResultTemporal, result);
        }

        private FmSectionAssemblyIndirectResult CreateFmSectionAssemblyIndirectResult(IFailureMechanismSection section)
        {
            var directMechanismSection = section as FailureMechanismSectionBase<EIndirectAssessmentResult>;
            return new FmSectionAssemblyIndirectResult(directMechanismSection.ExpectedCombinedResult);
        }
    }
}