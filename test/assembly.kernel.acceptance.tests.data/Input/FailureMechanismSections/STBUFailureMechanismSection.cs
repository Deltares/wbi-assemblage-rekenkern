using Assembly.Kernel.Model.AssessmentResultTypes;
using Assembly.Kernel.Model.FmSectionTypes;

namespace assembly.kernel.acceptance.tests.data.Input.FailureMechanismSections
{
    public class STBUFailureMechanismSection : FailureMechanismSectionBase<EFmSectionCategory>
    {
        public EAssessmentResultTypeE1 SimpleAssessmentResult { get; set; }

        public EAssessmentResultTypeG2 DetailedAssessmentResult { get; set; }

        public double DetailedAssessmentResultProbability { get; set; }

        public EAssessmentResultTypeT4 TailorMadeAssessmentResult { get; set; }

        public double TailorMadeAssessmentResultProbability { get; set; }
    }
}
