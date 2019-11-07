using Assembly.Kernel.Model.AssessmentResultTypes;
using Assembly.Kernel.Model.FmSectionTypes;

namespace assembly.kernel.benchmark.tests.data.Input.FailureMechanismSections
{
    /// <summary>
    /// ZST or DA
    /// </summary>
    public class Group3NoSimpleAssessmentFailureMechanismSection : FailureMechanismSectionBase<EFmSectionCategory>
    {
        public EAssessmentResultTypeE2 SimpleAssessmentResult { get; set; }

        // 0G4
        public EAssessmentResultTypeG2 DetailedAssessmentResult { get; set; }

        public EFmSectionCategory DetailedAssessmentResultValue { get; set; }

        // 0T4
        public EAssessmentResultTypeT3 TailorMadeAssessmentResult { get; set; }

        public EFmSectionCategory TailorMadeAssessmentResultCategory { get; set; }
    }
}
