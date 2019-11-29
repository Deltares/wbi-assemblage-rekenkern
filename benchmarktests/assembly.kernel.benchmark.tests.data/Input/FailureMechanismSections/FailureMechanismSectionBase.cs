using Assembly.Kernel.Model.FmSectionTypes;

namespace assembly.kernel.benchmark.tests.data.Input.FailureMechanismSections
{
    public class FailureMechanismSectionBase<TCombinedResult> : IFailureMechanismSection
    {
        /// <summary>
        /// The name of the section.
        /// </summary>
        /// TODO: Use this in messages in case of an assertion error of failing test
        public string SectionName { get; set; }

        /// <summary>
        /// The expected combined result for the specific section as a result of method WBI-0A-1.
        /// </summary>
        public TCombinedResult ExpectedCombinedResult { get; set; }

        public double Start { get; set; }

        public double End { get; set; }

        public IFmSectionAssemblyResult ExpectedSimpleAssessmentAssemblyResult { get; set; }

        public IFmSectionAssemblyResult ExpectedDetailedAssessmentAssemblyResult { get; set; }

        public IFmSectionAssemblyResult ExpectedTailorMadeAssessmentAssemblyResult { get; set; }
    }
}