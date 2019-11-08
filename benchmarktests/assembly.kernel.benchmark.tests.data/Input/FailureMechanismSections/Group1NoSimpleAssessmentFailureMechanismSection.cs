using Assembly.Kernel.Model.AssessmentResultTypes;
using Assembly.Kernel.Model.FmSectionTypes;

namespace assembly.kernel.benchmark.tests.data.Input.FailureMechanismSections
{
    /// <summary>
    /// GEKB or STKWp
    /// </summary>
    public class Group1NoSimpleAssessmentFailureMechanismSection : FailureMechanismSectionBase<EFmSectionCategory>, IProbabilisticMechanismSection
    {
        /// <summary>
        /// The result of simple assessment as input for assembly.
        /// </summary>
        public EAssessmentResultTypeE2 SimpleAssessmentResult { get; set; }

        public double SimpleAssessmentResultProbability { get; set; }

        /// <summary>
        /// The result of detailed assessment as input for assembly
        /// </summary>
        public EAssessmentResultTypeG2 DetailedAssessmentResult { get; set; }

        public double DetailedAssessmentResultProbability { get; set; }

        /// <summary>
        /// The result of tailor made assessment as input for assembly
        /// </summary>
        public EAssessmentResultTypeT3 TailorMadeAssessmentResult { get; set; }

        public double TailorMadeAssessmentResultProbability { get; set; }

        public double ExpectedCombinedResultProbability { get; set; }
    }
}
