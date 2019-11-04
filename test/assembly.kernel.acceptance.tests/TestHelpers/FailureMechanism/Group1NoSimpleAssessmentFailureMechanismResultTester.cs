using System.Linq;
using assembly.kernel.acceptance.tests.data.Input.FailureMechanisms;
using assembly.kernel.acceptance.tests.data.Input.FailureMechanismSections;
using Assembly.Kernel.Implementations;
using Assembly.Kernel.Model;
using Assembly.Kernel.Model.FmSectionTypes;
using NUnit.Framework;

namespace assemblage.kernel.acceptance.tests.TestHelpers.FailureMechanism
{
    // TODO: Lot of duplication with ProbabilisticFailureMechanismResultTester  
    public class Group1NoSimpleAssessmentFailureMechanismResultTester : FailureMechanismResultTesterBase<ProbabilisticExpectedFailureMechanismResult>
    {
        public Group1NoSimpleAssessmentFailureMechanismResultTester(IExpectedFailureMechanismResult expectedFailureMechanismResult) : base(expectedFailureMechanismResult)
        {
        }

        protected override void TestSimpleAssessmentInternal()
        {
            var assembler = new AssessmentResultsTranslator();

            foreach (var section in ExpectedFailureMechanismResult.Sections)
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

        protected override void TestDetailedAssessmentInternal()
        {
            var assembler = new AssessmentResultsTranslator();

            foreach (var section in ExpectedFailureMechanismResult.Sections)
            {
                var probabilisticSection = section as Group1NoSimpleAssessmentFailureMechanismSection;
                if (probabilisticSection != null)
                {
                    // WBI-0G-3
                    var result = assembler.TranslateAssessmentResultWbi0G3(
                        probabilisticSection.DetailedAssessmentResult,
                        probabilisticSection.DetailedAssessmentResultProbability,
                        ExpectedFailureMechanismResult.ExpectedFailureMechanismSectionCategories);

                    var expectedResult =
                        probabilisticSection.ExpectedDetailedAssessmentAssemblyResult as
                            FmSectionAssemblyDirectResultWithProbability;
                    Assert.AreEqual(expectedResult.Result, result.Result);
                    Assert.AreEqual(expectedResult.FailureProbability, result.FailureProbability);
                }
            }
        }

        protected override void TestTailorMadeAssessmentInternal()
        {
            var assembler = new AssessmentResultsTranslator();

            foreach (var section in ExpectedFailureMechanismResult.Sections)
            {
                var probabilisticSection = section as Group1NoSimpleAssessmentFailureMechanismSection;
                if (probabilisticSection != null)
                {
                    // WBI-0T-3
                    var result = assembler.TranslateAssessmentResultWbi0T3(
                        probabilisticSection.TailorMadeAssessmentResult,
                        probabilisticSection.TailorMadeAssessmentResultProbability,
                        ExpectedFailureMechanismResult.ExpectedFailureMechanismSectionCategories);

                    var expectedResult =
                        probabilisticSection.ExpectedTailorMadeAssessmentAssemblyResult as
                            FmSectionAssemblyDirectResultWithProbability;
                    Assert.AreEqual(expectedResult.Result, result.Result);
                    Assert.AreEqual(expectedResult.FailureProbability, result.FailureProbability);
                }
            }
        }

        protected override void TestCombinedAssessmentInternal()
        {
            var assembler = new AssessmentResultsTranslator();

            if (ExpectedFailureMechanismResult != null)
            {
                foreach (var section in ExpectedFailureMechanismResult.Sections.OfType<IProbabilisticMechanismSection>())
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

        protected override void TestAssessmentSectionResultInternal()
        {
            var assembler = new FailureMechanismResultAssembler();

            // WBI-1B-1
            FailureMechanismAssemblyResult result = assembler.AssembleFailureMechanismWbi1B1(new Assembly.Kernel.Model.FailureMechanism(ExpectedFailureMechanismResult.LengthEffectFactor,
                    ExpectedFailureMechanismResult.FailureMechanismProbabilitySpace),
                ExpectedFailureMechanismResult.Sections.Select(CreateFmSectionAssemblyDirectResultWithProbability),
                ExpectedFailureMechanismResult.ExpectedFailureMechanismCategories,
                false
            );

            Assert.AreEqual(ExpectedFailureMechanismResult.ExpectedAssessmentResult, result.Category);
            Assert.AreEqual(ExpectedFailureMechanismResult.ExpectedAssessmentResultProbability, result.FailureProbability);
        }

        protected override void TestAssessmentSectionResultTemporalInternal()
        {
            var assembler = new FailureMechanismResultAssembler();

            // WBI-1B-1
            FailureMechanismAssemblyResult result = assembler.AssembleFailureMechanismWbi1B1(new Assembly.Kernel.Model.FailureMechanism(ExpectedFailureMechanismResult.LengthEffectFactor,
                    ExpectedFailureMechanismResult.FailureMechanismProbabilitySpace),
                ExpectedFailureMechanismResult.Sections.Select(CreateFmSectionAssemblyDirectResultWithProbability),
                ExpectedFailureMechanismResult.ExpectedFailureMechanismCategories,
                true
            );

            Assert.AreEqual(ExpectedFailureMechanismResult.ExpectedAssessmentResultTemporal, result.Category);
            Assert.AreEqual(ExpectedFailureMechanismResult.ExpectedAssessmentResultProbabilityTemporal, result.FailureProbability);
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