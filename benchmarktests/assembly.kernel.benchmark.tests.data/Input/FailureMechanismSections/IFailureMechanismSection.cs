using Assembly.Kernel.Model.FmSectionTypes;

namespace assembly.kernel.benchmark.tests.data.Input.FailureMechanismSections
{
    public interface IFailureMechanismSection
    {
        /// <summary>
        /// The start position of the section as a length along the assessment section in meters.
        /// </summary>
        double Start { get; set; }

        /// <summary>
        /// The end position of the section as a length along the assessment section in meters.
        /// </summary>
        double End { get; set; }

        /// <summary>
        /// The expected result of the simple assessment
        /// </summary>
        IFmSectionAssemblyResult ExpectedSimpleAssessmentAssemblyResult { get; set; }

        /// <summary>
        /// The expected result of the detailed assessment
        /// </summary>
        IFmSectionAssemblyResult ExpectedDetailedAssessmentAssemblyResult { get; set; }

        /// <summary>
        /// The expected result of the tailor made assessment
        /// </summary>
        IFmSectionAssemblyResult ExpectedTailorMadeAssessmentAssemblyResult { get; set; }
    }
}