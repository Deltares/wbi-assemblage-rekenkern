using Assembly.Kernel.Model.AssessmentResultTypes;
using Assembly.Kernel.Model.FmSectionTypes;

namespace assembly.kernel.acceptance.tests.data.FailureMechanismSections
{
    /// <summary>
    /// GEKB or STKWp
    /// </summary>
    public class Group1NoSimpleAssessmentFailureMechanismSection : FailureMechanismSectionBase<EFmSectionCategory>
    {
        public EAssessmentResultTypeE2 SimpleAssessmentResult { get; set; }

        public EAssessmentResultTypeG2 DetailedAssessmentResult { get; set; }

        public double DetailedAssessmentResultProbability { get; set; }

        public EAssessmentResultTypeT3 TailorMadeAssessmentResult { get; set; }

        public double TailorMadeAssessmentResultProbability { get; set; }

        public double ExpectedCombinedResultProbability { get; set; }
    }
}
