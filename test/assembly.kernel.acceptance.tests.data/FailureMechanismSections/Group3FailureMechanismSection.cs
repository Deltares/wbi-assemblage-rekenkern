using Assembly.Kernel.Model.AssessmentResultTypes;
using Assembly.Kernel.Model.FmSectionTypes;

namespace assembly.kernel.acceptance.tests.data.FailureMechanismSections
{
    public class Group3FailureMechanismSection : FailureMechanismSectionBase
    {
        public EAssessmentResultTypeE1 SimpleAssessmentResult { get; set; }

        // 0G6
        public EFmSectionCategory DetailedAssessmentResult { get; set; }

        // 0T4
        public EAssessmentResultTypeT3 TailorMadeAssessmentResult { get; set; }

        public EFmSectionCategory? TailorMadeAssessmentResultCategory { get; set; }
    }
}
