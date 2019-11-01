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
    public class ProbabilisticFailureMechanismResultTestHelper : IFailureMechanismResultTestHelper
    {
        private readonly ProbabilisticFailureMechanismResult failureMechanismResult;

        public ProbabilisticFailureMechanismResultTestHelper(IFailureMechanismResult failureMechanismResult)
        {
            this.failureMechanismResult = failureMechanismResult as ProbabilisticFailureMechanismResult;
            if (this.failureMechanismResult == null)
            {
                throw new ArgumentException();
            }
        }

        public void TestSimpleAssessment()
        {
            var assembler = new AssessmentResultsTranslator();

            foreach (var section in failureMechanismResult.Sections)
            {
                var probabilisticSection = section as ProbabilisticFailureMechanismSection;
                if (probabilisticSection != null)
                {
                    FmSectionAssemblyDirectResultWithProbability result = assembler.TranslateAssessmentResultWbi0E1(probabilisticSection.SimpleAssessmentResult);
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

            foreach (var section in failureMechanismResult.Sections)
            {
                var probabilisticSection = section as ProbabilisticFailureMechanismSection;
                if (probabilisticSection != null)
                {
                    FmSectionAssemblyDirectResultWithProbability result;
                    if (failureMechanismResult.Type == MechanismType.STBI ||
                        failureMechanismResult.Type == MechanismType.STPH)
                    {
                        result = assembler.TranslateAssessmentResultWbi0G5(probabilisticSection.LengthEffectFactor,
                            probabilisticSection.DetailedAssessmentResult,
                            probabilisticSection.DetailedAssessmentResultProbability,
                            failureMechanismResult.ExpectedFailureMechanismSectionCategories);
                    }
                    else
                    {
                        result = assembler.TranslateAssessmentResultWbi0G3(
                            probabilisticSection.DetailedAssessmentResult,
                            probabilisticSection.DetailedAssessmentResultProbability,
                            failureMechanismResult.ExpectedFailureMechanismSectionCategories);
                    }

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

            foreach (var section in failureMechanismResult.Sections)
            {
                var probabilisticSection = section as ProbabilisticFailureMechanismSection;
                if (probabilisticSection != null)
                {
                    FmSectionAssemblyDirectResultWithProbability result;
                    if (failureMechanismResult.Type == MechanismType.STBI ||
                        failureMechanismResult.Type == MechanismType.STPH)
                    {
                        result = assembler.TranslateAssessmentResultWbi0T5(probabilisticSection.LengthEffectFactor,
                            probabilisticSection.TailorMadeAssessmentResult,
                            probabilisticSection.TailorMadeAssessmentResultProbability,
                            failureMechanismResult.ExpectedFailureMechanismSectionCategories);
                    }
                    else
                    {
                        result = assembler.TranslateAssessmentResultWbi0T3(
                            probabilisticSection.TailorMadeAssessmentResult,
                            probabilisticSection.TailorMadeAssessmentResultProbability,
                            failureMechanismResult.ExpectedFailureMechanismSectionCategories);
                    }

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
            var assembler = new FailureMechanismResultAssembler();

            if (failureMechanismResult != null)
            {
                foreach (var section in failureMechanismResult.Sections.OfType<IProbabilisticMechanismSection>())
                {
                    // WBI-1B-1
                    var result = assembler.AssembleFailureMechanismWbi1B1(new FailureMechanism(failureMechanismResult.LengthEffectFactor,
                            failureMechanismResult.FailureMechanismProbabilitySpace),
                        new[]
                        {
                            section.ExpectedSimpleAssessmentAssemblyResult as FmSectionAssemblyDirectResultWithProbability,
                            section.ExpectedDetailedAssessmentAssemblyResult as FmSectionAssemblyDirectResultWithProbability,
                            section.ExpectedTailorMadeAssessmentAssemblyResult as FmSectionAssemblyDirectResultWithProbability,
                        },
                        failureMechanismResult.ExpectedFailureMechanismCategories,
                        false
                    );

                    Assert.AreEqual(section.ExpectedCombinedResult, result.Category);
                    Assert.AreEqual(section.ExpectedCombinedResultProbability, result.FailureProbability);
                }
            }
        }

        public void TestAssessmentSectionResult()
        {
            var assembler = new FailureMechanismResultAssembler();

            // WBI-1B-1
            FailureMechanismAssemblyResult result = assembler.AssembleFailureMechanismWbi1B1(new FailureMechanism(failureMechanismResult.LengthEffectFactor,
                    failureMechanismResult.FailureMechanismProbabilitySpace),
                failureMechanismResult.Sections.Select(CreateFmSectionAssemblyDirectResultWithProbability),
                failureMechanismResult.ExpectedFailureMechanismCategories,
                false
            );

            Assert.AreEqual(failureMechanismResult.ExpectedAssessmentResult, result.Category);
            Assert.AreEqual(failureMechanismResult.ExpectedAssessmentResultProbability, result.FailureProbability);
        }

        public void TestAssessmentSectionResultTemporal()
        {
            var assembler = new FailureMechanismResultAssembler();

            // WBI-1B-1
            FailureMechanismAssemblyResult result = assembler.AssembleFailureMechanismWbi1B1(new FailureMechanism(failureMechanismResult.LengthEffectFactor,
                    failureMechanismResult.FailureMechanismProbabilitySpace),
                failureMechanismResult.Sections.Select(CreateFmSectionAssemblyDirectResultWithProbability),
                failureMechanismResult.ExpectedFailureMechanismCategories,
                true
            );

            Assert.AreEqual(failureMechanismResult.ExpectedAssessmentResultTemporal, result.Category);
            Assert.AreEqual(failureMechanismResult.ExpectedAssessmentResultProbabilityTemporal, result.FailureProbability);
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
