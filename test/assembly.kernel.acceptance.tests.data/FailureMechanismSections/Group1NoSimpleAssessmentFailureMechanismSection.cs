using Assembly.Kernel.Model.AssessmentResultTypes;

namespace assembly.kernel.acceptance.tests.data.FailureMechanismSections
{
    /// <summary>
    /// GEKB or STKWp
    /// </summary>
    public class Group1NoSimpleAssessmentFailureMechanismSection : FailureMechanismSectionBase
    {
        public EAssessmentResultTypeE2 SimpleAssessmentResult { get; set; }

        public EAssessmentResultTypeG2 DetailedAssessmentResult { get; set; }

        public double DetailedAssessmentResultProbability { get; set; }

        public EAssessmentResultTypeT3 TailorMadeAssessmentResult { get; set; }

        public double TailorMadeAssessmentResultProbability { get; set; }
    }
}
