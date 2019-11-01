using assembly.kernel.acceptance.tests.data.Input.FailureMechanisms;
using Assembly.Kernel.Model.FmSectionTypes;

namespace assembly.kernel.acceptance.tests.data.Input.FailureMechanismSections
{
    public class FailureMechanismSectionBase<TCombinedResult> : IFailureMechanismSection
    {
        public double Start { get; set; }

        public double End { get; set; }

        public string SectionName { get; set; }

        public TCombinedResult ExpectedCombinedResult { get; set; }

        public IFmSectionAssemblyResult ExpectedSimpleAssessmentAssemblyResult { get; set; }

        public IFmSectionAssemblyResult ExpectedDetailedAssessmentAssemblyResult { get; set; }

        public IFmSectionAssemblyResult ExpectedTailorMadeAssessmentAssemblyResult { get; set; }
    }
}