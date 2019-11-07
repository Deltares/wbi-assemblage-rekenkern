using Assembly.Kernel.Model.AssessmentResultTypes;
using Assembly.Kernel.Model.FmSectionTypes;

namespace assembly.kernel.benchmark.tests.data.Input.FailureMechanismSections
{
    public class ProbabilisticFailureMechanismSection : FailureMechanismSectionBase<EFmSectionCategory>, IProbabilisticMechanismSection
    {
        public EAssessmentResultTypeE1 SimpleAssessmentResult { get; set; }

        public double SimpleAssessmentResultProbability { get; set; }

        public EAssessmentResultTypeG2 DetailedAssessmentResult { get; set; }

        public double DetailedAssessmentResultProbability { get; set; }

        public EAssessmentResultTypeT3 TailorMadeAssessmentResult { get; set; }

        public double TailorMadeAssessmentResultProbability { get; set; }

        public double ExpectedCombinedResultProbability { get; set; }

        public double LengthEffectFactor { get; set; }
    }
}
