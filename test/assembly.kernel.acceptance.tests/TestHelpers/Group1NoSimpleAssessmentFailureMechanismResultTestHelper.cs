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
    // TODO: Lot of duplication with ProbabilisticFailureMechanismResultTestHelper  
    public class Group1NoSimpleAssessmentFailureMechanismResultTestHelper : IFailureMechanismResultTestHelper
    {
        private readonly ProbabilisticExpectedFailureMechanismResult expectedFailureMechanismResult;

        public Group1NoSimpleAssessmentFailureMechanismResultTestHelper(IExpectedFailureMechanismResult expectedFailureMechanismResult)
        {
            this.expectedFailureMechanismResult = expectedFailureMechanismResult as ProbabilisticExpectedFailureMechanismResult;
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
                var probabilisticSection = section as Group1NoSimpleAssessmentFailureMechanismSection;
                if (probabilisticSection != null)
                {
                    // WBI-0E-3
                    FmSectionAssemblyDirectResultWithProbability result = assembler.TranslateAssessmentResultWbi0E3(probabilisticSection.SimpleAssessmentResult);
                    var expectedResult = probabilisticSection.ExpectedSimpleAssessmentAssemblyResult as
                        FmSectionAssemblyDirectResultWithProbability;
                    Assert.AreEqual(expectedResult.Result, result.Result);
                    Assert.AreEqual(expectedResult.FailureProbability, result.FailureProbability);
                }
            }
        }

        public void TestDetailedAssessment()
        {
            var assembler = new AssessmentResultsTranslator();

            foreach (var section in expectedFailureMechanismResult.Sections)
            {
                var probabilisticSection = section as Group1NoSimpleAssessmentFailureMechanismSection;
                if (probabilisticSection != null)
                {
                    // WBI-0G-3
                    var result = assembler.TranslateAssessmentResultWbi0G3(
                        probabilisticSection.DetailedAssessmentResult,
                        probabilisticSection.DetailedAssessmentResultProbability,
                        expectedFailureMechanismResult.ExpectedFailureMechanismSectionCategories);

                    var expectedResult =
                        probabilisticSection.ExpectedDetailedAssessmentAssemblyResult as
                            FmSectionAssemblyDirectResultWithProbability;
                    Assert.AreEqual(expectedResult.Result, result.Result);
                    Assert.AreEqual(expectedResult.FailureProbability, result.FailureProbability);
                }
            }
        }

        public void TestTailorMadeAssessment()
        {
            var assembler = new AssessmentResultsTranslator();

            foreach (var section in expectedFailureMechanismResult.Sections)
            {
                var probabilisticSection = section as Group1NoSimpleAssessmentFailureMechanismSection;
                if (probabilisticSection != null)
                {
                    // WBI-0T-3
                    var result = assembler.TranslateAssessmentResultWbi0T3(
                        probabilisticSection.TailorMadeAssessmentResult,
                        probabilisticSection.TailorMadeAssessmentResultProbability,
                        expectedFailureMechanismResult.ExpectedFailureMechanismSectionCategories);

                    var expectedResult =
                        probabilisticSection.ExpectedTailorMadeAssessmentAssemblyResult as
                            FmSectionAssemblyDirectResultWithProbability;
                    Assert.AreEqual(expectedResult.Result, result.Result);
                    Assert.AreEqual(expectedResult.FailureProbability, result.FailureProbability);
                }
            }
        }

        public void TestCombinedAssessment()
        {
            var assembler = new AssessmentResultsTranslator();

            if (expectedFailureMechanismResult != null)
            {
                foreach (var section in expectedFailureMechanismResult.Sections.OfType<IProbabilisticMechanismSection>())
                {
                    // WBI-0A-1 (direct with probability)
                    var result = assembler.TranslateAssessmentResultWbi0A1(
                            section.ExpectedSimpleAssessmentAssemblyResult as FmSectionAssemblyDirectResultWithProbability,
                            section.ExpectedDetailedAssessmentAssemblyResult as FmSectionAssemblyDirectResultWithProbability,
                            section.ExpectedTailorMadeAssessmentAssemblyResult as FmSectionAssemblyDirectResultWithProbability);

                    Assert.IsInstanceOf<FmSectionAssemblyDirectResultWithProbability>(result);
                    Assert.AreEqual(section.ExpectedCombinedResult, result.Result);
                    Assert.AreEqual(section.ExpectedCombinedResultProbability, result.FailureProbability);
                }
            }
        }

        public void TestAssessmentSectionResult()
        {
            var assembler = new FailureMechanismResultAssembler();

            // WBI-1B-1
            FailureMechanismAssemblyResult result = assembler.AssembleFailureMechanismWbi1B1(new FailureMechanism(expectedFailureMechanismResult.LengthEffectFactor,
                    expectedFailureMechanismResult.FailureMechanismProbabilitySpace),
                expectedFailureMechanismResult.Sections.Select(CreateFmSectionAssemblyDirectResultWithProbability),
                expectedFailureMechanismResult.ExpectedFailureMechanismCategories,
                false
            );

            Assert.AreEqual(expectedFailureMechanismResult.ExpectedAssessmentResult, result.Category);
            Assert.AreEqual(expectedFailureMechanismResult.ExpectedAssessmentResultProbability, result.FailureProbability);
        }

        public void TestAssessmentSectionResultTemporal()
        {
            var assembler = new FailureMechanismResultAssembler();

            // WBI-1B-1
            FailureMechanismAssemblyResult result = assembler.AssembleFailureMechanismWbi1B1(new FailureMechanism(expectedFailureMechanismResult.LengthEffectFactor,
                    expectedFailureMechanismResult.FailureMechanismProbabilitySpace),
                expectedFailureMechanismResult.Sections.Select(CreateFmSectionAssemblyDirectResultWithProbability),
                expectedFailureMechanismResult.ExpectedFailureMechanismCategories,
                true
            );

            Assert.AreEqual(expectedFailureMechanismResult.ExpectedAssessmentResultTemporal, result.Category);
            Assert.AreEqual(expectedFailureMechanismResult.ExpectedAssessmentResultProbabilityTemporal, result.FailureProbability);
        }

        private FmSectionAssemblyDirectResultWithProbability CreateFmSectionAssemblyDirectResultWithProbability(IFailureMechanismSection section)
        {
            var directMechanismSection = section as FailureMechanismSectionBase<EFmSectionCategory>;
            var probabilisticMechanismSection = section as IProbabilisticMechanismSection;
            return new FmSectionAssemblyDirectResultWithProbability(directMechanismSection.ExpectedCombinedResult,
                probabilisticMechanismSection.ExpectedCombinedResultProbability);
        }
    }
}