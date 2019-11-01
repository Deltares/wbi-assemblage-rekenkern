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
    public class Group5NoDetailedAssessmentFailureMechanismTestHelper : IFailureMechanismResultTestHelper
    {
        private readonly Group4Or5ExpectedFailureMechanismResult expectedFailureMechanismResult;

        public Group5NoDetailedAssessmentFailureMechanismTestHelper(IExpectedFailureMechanismResult expectedFailureMechanismResult)
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
                var group5NoDetailedAssessmentFailureMechanismSection = section as Group5NoDetailedAssessmentFailureMechanismSection;
                if (group5NoDetailedAssessmentFailureMechanismSection != null)
                {
                    // WBI-0E-2
                    FmSectionAssemblyIndirectResult result = assembler.TranslateAssessmentResultWbi0E2(group5NoDetailedAssessmentFailureMechanismSection.SimpleAssessmentResult);
                    var expectedResult = group5NoDetailedAssessmentFailureMechanismSection.ExpectedSimpleAssessmentAssemblyResult as FmSectionAssemblyIndirectResult;
                    Assert.AreEqual(expectedResult.Result, result.Result);
                }
            }
        }

        public void TestDetailedAssessment()
        {
            // No test to perform
            // TODO: How to return feedback on whether a test has been performed?
        }

        public void TestTailorMadeAssessment()
        {
            var assembler = new AssessmentResultsTranslator();

            foreach (var section in expectedFailureMechanismResult.Sections)
            {
                var group5NoDetailedAssessmentFailureMechanismSection = section as Group5NoDetailedAssessmentFailureMechanismSection;
                if (group5NoDetailedAssessmentFailureMechanismSection != null)
                {
                    // WBI-0T-1
                    FmSectionAssemblyIndirectResult result = assembler.TranslateAssessmentResultWbi0T2(group5NoDetailedAssessmentFailureMechanismSection.TailorMadeAssessmentResult);

                    var expectedResult = group5NoDetailedAssessmentFailureMechanismSection.ExpectedTailorMadeAssessmentAssemblyResult as FmSectionAssemblyIndirectResult;
                    Assert.AreEqual(expectedResult.Result, result.Result);
                }
            }
        }

        public void TestCombinedAssessment()
        {
            var assembler = new AssessmentResultsTranslator();

            if (expectedFailureMechanismResult != null)
            {
                foreach (var section in expectedFailureMechanismResult.Sections.OfType<Group5NoDetailedAssessmentFailureMechanismSection>())
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