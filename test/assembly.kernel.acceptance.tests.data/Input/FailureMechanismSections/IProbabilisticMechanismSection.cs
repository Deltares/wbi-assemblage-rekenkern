using assembly.kernel.acceptance.tests.data.Input.FailureMechanisms;
using Assembly.Kernel.Model.FmSectionTypes;

namespace assembly.kernel.acceptance.tests.data.Input.FailureMechanismSections
{
    public interface IProbabilisticMechanismSection : IFailureMechanismSection
    {
        double SimpleAssessmentResultProbability { get; set; }

        double DetailedAssessmentResultProbability { get; set; }

        double TailorMadeAssessmentResultProbability { get; set; }

        EFmSectionCategory ExpectedCombinedResult { get; set; }

        double ExpectedCombinedResultProbability { get; set; }
    }
}