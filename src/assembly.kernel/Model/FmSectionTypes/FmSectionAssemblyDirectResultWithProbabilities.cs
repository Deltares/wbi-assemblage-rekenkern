using Assembly.Kernel.Exceptions;

namespace Assembly.Kernel.Model.FmSectionTypes
{
    /// <summary>
    /// Failure mechanism assessment translation result for direct failure mechanisms, including estimated probability of failure for profile and section.
    /// </summary>
    public class FmSectionAssemblyDirectResultWithProbabilities : FmSectionAssemblyDirectResultWithProbability
    {
        /// <summary>
        /// Constructor of the direct failure mechanism assembly result with failure probability.
        /// </summary>
        /// <param name="result">The translated category type of the result</param>
        /// <param name="failureProbabilitySection">The failure probability of the failure mechanism section</param>
        /// <exception cref="AssemblyException">Thrown when failure probability is &lt;0 or &gt;1</exception>
        public FmSectionAssemblyDirectResultWithProbabilities(EFmSectionCategory result, double failureProbabilitySection, double failureProbabilityProfile) : base(result, failureProbabilitySection)
        {
            if (failureProbabilityProfile < 0.0 || failureProbabilityProfile > 1.0)
            {
                throw new AssemblyException("FmSectionAssemblyDirectResultWithProbabilities",
                    EAssemblyErrors.FailureProbabilityOutOfRange);
            }

            FailureProbabilityProfile = failureProbabilityProfile;
        }

        /// <summary>
        /// Optional failure probability of the profile originating from the failure mechanism section assessment result.
        /// This field can be null!
        /// </summary>
        public double FailureProbabilityProfile { get; }

        /// <summary>
        /// Convert to string
        /// </summary>
        /// <returns>String of the object</returns>
        public override string ToString()
        {
            return "FmSectionAssemblyDirectResultWithProbabilities [" + Result + " Psection: " + FailureProbability + " Pprofile:" + FailureProbabilityProfile + "]";
        }
    }
}
