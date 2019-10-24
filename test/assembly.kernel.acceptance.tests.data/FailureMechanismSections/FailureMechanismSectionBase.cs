using assembly.kernel.acceptance.tests.data.FailureMechanisms;
using Assembly.Kernel.Model.FmSectionTypes;

namespace assembly.kernel.acceptance.tests.data.FailureMechanismSections
{
    public class FailureMechanismSectionBase : IFailureMechanismSection
    {
        public double Start { get; set; }

        public double End { get; set; }

        public string SectionName { get; set; }

        public EFmSectionCategory ExpectedCombinedResult { get; set; }

        public IFmSectionAssemblyResult ExpectedSimpleAssessmentAssemblyResult { get; set; }

        public IFmSectionAssemblyResult ExpectedDetailedAssessmentAssemblyResult { get; set; }

        public IFmSectionAssemblyResult ExpectedCustomAssessmentAssemblyResult { get; set; }
    }
}