using Assembly.Kernel.Model.AssessmentResultTypes;

namespace assembly.kernel.acceptance.tests.data.FailureMechanismSections
{
    public class STBUFailureMechanismSection : FailureMechanismSectionBase
    {
        public EAssessmentResultTypeE1 SimpleAssessmentResult { get; set; }

        public EAssessmentResultTypeG2 DetailedAssessmentResult { get; set; }

        public double DetailedAssessmentResultProbability { get; set; }

        public EAssessmentResultTypeT4 TailorMadeAssessmentResult { get; set; }

        public double TailorMadeAssessmentResultProbability { get; set; }
    }
}
