using Assembly.Kernel.Model.AssessmentResultTypes;
using Assembly.Kernel.Model.FmSectionTypes;

namespace assembly.kernel.benchmark.tests.data.Input.FailureMechanismSections
{
    public class STBUFailureMechanismSection : FailureMechanismSectionBase<EFmSectionCategory>
    {
        /// <summary>
        /// The result of simple assessment as input for assembly.
        /// </summary>
        public EAssessmentResultTypeE1 SimpleAssessmentResult { get; set; }

        /// <summary>
        /// The result of detailed assessment as input for assembly.
        /// </summary>
        public EAssessmentResultTypeG2 DetailedAssessmentResult { get; set; }

        /// <summary>
        /// The result of detailed assessment as a probability as input for assembly.
        /// </summary>
        public double DetailedAssessmentResultProbability { get; set; }

        /// <summary>
        /// The result of tailor made assessment as input for assembly.
        /// </summary>
        public EAssessmentResultTypeT4 TailorMadeAssessmentResult { get; set; }

        /// <summary>
        /// The result of tailor made assessment as a probability as input for assembly.
        /// </summary>
        public double TailorMadeAssessmentResultProbability { get; set; }
    }
}