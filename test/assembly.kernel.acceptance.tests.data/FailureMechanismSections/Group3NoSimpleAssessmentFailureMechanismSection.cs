using Assembly.Kernel.Model.AssessmentResultTypes;
using Assembly.Kernel.Model.FmSectionTypes;

namespace assembly.kernel.acceptance.tests.data.FailureMechanismSections
{
    /// <summary>
    /// ZST or DA
    /// </summary>
    public class Group3NoSimpleAssessmentFailureMechanismSection : FailureMechanismSectionBase<EFmSectionCategory>
    {
        public EAssessmentResultTypeE2 SimpleAssessmentResult { get; set; }

        // 0G6
        public EFmSectionCategory DetailedAssessmentResult { get; set; }

        // 0T4
        public EAssessmentResultTypeT3 TailorMadeAssessmentResult { get; set; }

        public EFmSectionCategory TailorMadeAssessmentResultCategory { get; set; }
    }
}
