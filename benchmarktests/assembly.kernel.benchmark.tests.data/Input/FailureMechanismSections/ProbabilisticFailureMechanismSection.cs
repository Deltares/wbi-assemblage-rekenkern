using Assembly.Kernel.Model.AssessmentResultTypes;
using Assembly.Kernel.Model.FmSectionTypes;

namespace assembly.kernel.benchmark.tests.data.Input.FailureMechanismSections
{
    public class ProbabilisticFailureMechanismSection : FailureMechanismSectionBase<EFmSectionCategory>, IProbabilisticMechanismSection
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
        public EAssessmentResultTypeT3 TailorMadeAssessmentResult { get; set; }

        /// <summary>
        /// The result of tailor made assessment as a probability as input for assembly.
        /// </summary>
        public double TailorMadeAssessmentResultProbability { get; set; }

        /// <summary>
        /// The length-effect factor for this failure mechanism section specific (>= 1).
        /// </summary>
        public double LengthEffectFactor { get; set; }

        public double ExpectedCombinedResultProbability { get; set; }
    }
}