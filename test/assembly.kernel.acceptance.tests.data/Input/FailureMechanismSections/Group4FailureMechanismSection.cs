using Assembly.Kernel.Model.AssessmentResultTypes;
using Assembly.Kernel.Model.FmSectionTypes;

namespace assembly.kernel.acceptance.tests.data.Input.FailureMechanismSections
{
    public class Group4FailureMechanismSection : FailureMechanismSectionBase<EFmSectionCategory>
    {
        public EAssessmentResultTypeE1 SimpleAssessmentResult { get; set; }

        public EAssessmentResultTypeG1 DetailedAssessmentResult { get; set; }

        public EAssessmentResultTypeT1 TailorMadeAssessmentResult { get; set; }
    }
}
