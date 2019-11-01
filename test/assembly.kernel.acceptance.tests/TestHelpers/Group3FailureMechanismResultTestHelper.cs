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
    public class Group3FailureMechanismResultTestHelper : IFailureMechanismResultTestHelper
    {
        private readonly Group3ExpectedFailureMechanismResult expectedFailureMechanismResult;

        public Group3FailureMechanismResultTestHelper(IExpectedFailureMechanismResult expectedFailureMechanismResult)
        {
            this.expectedFailureMechanismResult = expectedFailureMechanismResult as Group3ExpectedFailureMechanismResult;
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
                var group3FailureMechanismSection = section as Group3FailureMechanismSection;
                if (group3FailureMechanismSection != null)
                {
                    // WBI-0E-1
                    FmSectionAssemblyDirectResultWithProbability result = assembler.TranslateAssessmentResultWbi0E1(group3FailureMechanismSection.SimpleAssessmentResult);
                    var expectedResult = group3FailureMechanismSection.ExpectedSimpleAssessmentAssemblyResult as FmSectionAssemblyDirectResult;
                    Assert.AreEqual(expectedResult.Result, result.Result);
                }
            }
        }

        public void TestDetailedAssessment()
        {
           // Not a method in the kernel. User enters category directly in benchmark. WBI-0G-6 should be tested, but is not in the benchmark at this moment.
        }

        public void TestTailorMadeAssessment()
        {
            var assembler = new AssessmentResultsTranslator();

            foreach (var section in expectedFailureMechanismResult.Sections)
            {
                var group3FailureMechanismSection = section as Group3FailureMechanismSection;
                if (group3FailureMechanismSection != null)
                {
                    // WBI-0T-4
                    var result = assembler.TranslateAssessmentResultWbi0T4(
                        group3FailureMechanismSection.TailorMadeAssessmentResult,
                        group3FailureMechanismSection.TailorMadeAssessmentResultCategory);
                   
                    var expectedResult = group3FailureMechanismSection.ExpectedTailorMadeAssessmentAssemblyResult as FmSectionAssemblyDirectResult;
                    Assert.AreEqual(expectedResult.Result, result.Result);
                }
            }
        }

        public void TestCombinedAssessment()
        {
            var assembler = new AssessmentResultsTranslator();

            if (expectedFailureMechanismResult != null)
            {
                foreach (var section in expectedFailureMechanismResult.Sections.OfType<Group3FailureMechanismSection>())
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