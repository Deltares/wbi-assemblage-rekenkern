using Assembly.Kernel.Model.AssessmentResultTypes;
using Assembly.Kernel.Model.FmSectionTypes;

namespace assembly.kernel.benchmark.tests.data.Input.FailureMechanismSections
{
    public class Group3FailureMechanismSection : FailureMechanismSectionBase<EFmSectionCategory>
    {
        public EAssessmentResultTypeE1 SimpleAssessmentResult { get; set; }

        // 0G4 (in Riskeer G6 is used, but this is not included in the benchmark)
        public EAssessmentResultTypeG2 DetailedAssessmentResult { get; set; }

        public EFmSectionCategory DetailedAssessmentResultValue { get; set; }

        // 0T4
        public EAssessmentResultTypeT3 TailorMadeAssessmentResult { get; set; }

        public EFmSectionCategory TailorMadeAssessmentResultCategory { get; set; }
    }
}
