using Assembly.Kernel.Model.AssessmentResultTypes;
using Assembly.Kernel.Model.FmSectionTypes;

namespace assembly.kernel.benchmark.tests.data.Input.FailureMechanismSections
{
    public class Group4NoDetailedAssessmentFailureMechanismSection : FailureMechanismSectionBase<EFmSectionCategory>
    {
        /// <summary>
        /// The result of simple assessment as input for assembly.
        /// </summary>
        public EAssessmentResultTypeE1 SimpleAssessmentResult { get; set; }

        /// <summary>
        /// The result of tailor made assessment as input for assembly.
        /// </summary>
        public EAssessmentResultTypeT1 TailorMadeAssessmentResult { get; set; }
    }
}