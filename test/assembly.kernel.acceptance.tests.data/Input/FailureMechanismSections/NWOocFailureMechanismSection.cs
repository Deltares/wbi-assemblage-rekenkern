using Assembly.Kernel.Model;
using Assembly.Kernel.Model.AssessmentResultTypes;

namespace assembly.kernel.acceptance.tests.data.Input.FailureMechanismSections
{
    public class NWOocFailureMechanismSection : FailureMechanismSectionBase<EIndirectAssessmentResult>
    {
        public EAssessmentResultTypeE2 SimpleAssessmentResult { get; set; }

        public EAssessmentResultTypeG1 DetailedAssessmentResult { get; set; }

        public EAssessmentResultTypeT2 TailorMadeAssessmentResult { get; set; }
    }
}
