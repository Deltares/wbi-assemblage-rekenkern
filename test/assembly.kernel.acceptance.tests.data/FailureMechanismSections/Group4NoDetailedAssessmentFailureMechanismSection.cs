using Assembly.Kernel.Model.AssessmentResultTypes;

namespace assembly.kernel.acceptance.tests.data.FailureMechanismSections
{
    public class Group4NoDetailedAssessmentFailureMechanismSection : FailureMechanismSectionBase
    {
        public EAssessmentResultTypeE1 SimpleAssessmentResult { get; set; }

        // No detailed assessment

        public EAssessmentResultTypeT1 TailorMadeAssessmentResult { get; set; }
    }
}
