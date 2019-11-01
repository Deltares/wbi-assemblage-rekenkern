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
    public class Group4FailureMechanismTestHelper : IFailureMechanismResultTestHelper
    {
        private readonly Group4Or5ExpectedFailureMechanismResult expectedFailureMechanismResult;

        public Group4FailureMechanismTestHelper(IExpectedFailureMechanismResult expectedFailureMechanismResult)
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
                var group4FailureMechanismSection = section as Group4FailureMechanismSection;
                if (group4FailureMechanismSection != null)
                {
                    // WBI-0E-1
                    FmSectionAssemblyDirectResult result = assembler.TranslateAssessmentResultWbi0E1(group4FailureMechanismSection.SimpleAssessmentResult);
                    var expectedResult = group4FailureMechanismSection.ExpectedSimpleAssessmentAssemblyResult as FmSectionAssemblyDirectResult;
                    Assert.AreEqual(expectedResult.Result, result.Result);
                }
            }
        }

        public void TestDetailedAssessment()
        {
            var assembler = new AssessmentResultsTranslator();

            foreach (var section in expectedFailureMechanismResult.Sections)
            {
                var group4FailureMechanismSection = section as Group4FailureMechanismSection;
                if (group4FailureMechanismSection != null)
                {
                    // WBI-0G-1
                        FmSectionAssemblyDirectResult result = assembler.TranslateAssessmentResultWbi0G1(group4FailureMechanismSection.DetailedAssessmentResult);

                    var expectedResult =
                        group4FailureMechanismSection.ExpectedDetailedAssessmentAssemblyResult as
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
                var group4FailureMechanismSection = section as Group4FailureMechanismSection;
                if (group4FailureMechanismSection != null)
                {
                    // WBI-0T-1
                    var result = assembler.TranslateAssessmentResultWbi0T1(
                        group4FailureMechanismSection.TailorMadeAssessmentResult);

                    var expectedResult = group4FailureMechanismSection.ExpectedTailorMadeAssessmentAssemblyResult as FmSectionAssemblyDirectResult;
                    Assert.AreEqual(expectedResult.Result, result.Result);
                }
            }
        }

        public void TestCombinedAssessment()
        {
            var assembler = new AssessmentResultsTranslator();

            if (expectedFailureMechanismResult != null)
            {
                foreach (var section in expectedFailureMechanismResult.Sections.OfType<Group4FailureMechanismSection>())
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
    }
}