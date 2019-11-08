using Assembly.Kernel.Model.AssessmentResultTypes;
using Assembly.Kernel.Model.FmSectionTypes;

namespace assembly.kernel.benchmark.tests.data.Input.FailureMechanismSections
{
    /// <summary>
    /// ZST or DA
    /// </summary>
    public class Group3NoSimpleAssessmentFailureMechanismSection : FailureMechanismSectionBase<EFmSectionCategory>
    {
        /// <summary>
        /// The result of simple assessment as input for assembly
        /// </summary>
        public EAssessmentResultTypeE2 SimpleAssessmentResult { get; set; }

        /// <summary>
        /// The result of detailed assessment as input for assembly. This result can be used in WBI-0G-4. In Riskeer the other method for group 3 failure mechanisms is used (WBI-0G-6).
        /// </summary>
        public EAssessmentResultTypeG2 DetailedAssessmentResult { get; set; }

        /// <summary>
        /// The result of detailed assessment translated to an EFmSectionCategory as input for assembly. This result can be used in WBI-0G-4. In Riskeer the other method for group 3 failure mechanisms is used (WBI-0G-6).
        /// </summary>
        public EFmSectionCategory DetailedAssessmentResultValue { get; set; }

        /// <summary>
        /// The result of tailor made assessment as input for assembly
        /// </summary>
        public EAssessmentResultTypeT3 TailorMadeAssessmentResult { get; set; }

        /// <summary>
        /// The result of simple assessment translated to an EFmSectionCategory as input for assembly.
        /// </summary>
        public EFmSectionCategory TailorMadeAssessmentResultCategory { get; set; }
    }
}
