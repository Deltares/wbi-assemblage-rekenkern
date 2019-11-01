using Assembly.Kernel.Model.AssessmentResultTypes;
using Assembly.Kernel.Model.FmSectionTypes;

namespace assembly.kernel.acceptance.tests.data.Input.FailureMechanismSections
{
    public class Group4NoDetailedAssessmentFailureMechanismSection : FailureMechanismSectionBase<EFmSectionCategory>
    {
        public EAssessmentResultTypeE1 SimpleAssessmentResult { get; set; }

        // No detailed assessment

        public EAssessmentResultTypeT1 TailorMadeAssessmentResult { get; set; }
    }
}
