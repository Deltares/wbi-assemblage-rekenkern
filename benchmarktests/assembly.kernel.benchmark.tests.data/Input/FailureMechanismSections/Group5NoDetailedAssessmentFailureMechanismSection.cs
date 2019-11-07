using Assembly.Kernel.Model;
using Assembly.Kernel.Model.AssessmentResultTypes;

namespace assembly.kernel.benchmark.tests.data.Input.FailureMechanismSections
{
    public class Group5NoDetailedAssessmentFailureMechanismSection : FailureMechanismSectionBase<EIndirectAssessmentResult>
    {
        public EAssessmentResultTypeE1 SimpleAssessmentResult { get; set; }

        public EAssessmentResultTypeT2 TailorMadeAssessmentResult { get; set; }
    }
}
