using Assembly.Kernel.Model;
using Assembly.Kernel.Model.AssessmentResultTypes;

namespace assembly.kernel.benchmark.tests.data.Input.FailureMechanismSections
{
    public class Group5FailureMechanismSection : FailureMechanismSectionBase<EIndirectAssessmentResult>
    {
        /// <summary>
        /// The result of simple assessment as input for assembly.
        /// </summary>
        public EAssessmentResultTypeE1 SimpleAssessmentResult { get; set; }

        /// <summary>
        /// The result of detailed assessment as input for assembly.
        /// </summary>
        public EAssessmentResultTypeG1 DetailedAssessmentResult { get; set; }

        /// <summary>
        /// The result of tailor made assessment as input for assembly.
        /// </summary>
        public EAssessmentResultTypeT2 TailorMadeAssessmentResult { get; set; }
    }
}