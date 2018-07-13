using Assembly.Kernel.Exceptions;

namespace Assembly.Kernel.Model.FmSectionTypes
{
    /// <summary>
    /// Failure mechanism assessment translation result for direct failure mechanisms, including estimated probability of failure.
    /// </summary>
    public class FmSectionAssemblyDirectResultWithProbability : FmSectionAssemblyDirectResult
    {
        /// <summary>
        /// Constructor of the direct failure mechanism assembly result with failure probability.
        /// </summary>
        /// <param name="result">The translated category type of the result</param>
        /// <param name="failureProbability">The failure probability of the failure mechanism section</param>
        /// <exception cref="AssemblyException">Thrown when failure probability is &lt;0 or &gt;1</exception>
        public FmSectionAssemblyDirectResultWithProbability(EFmSectionCategory result, double failureProbability) :
            base(result)
        {
            if (failureProbability < 0.0 || failureProbability > 1.0)
            {
                throw new AssemblyException("FmSectionAssemblyDirectResultWithProbability",
                    EAssemblyErrors.FailureProbabilityOutOfRange);
            }

            FailureProbability = failureProbability;
        }

        /// <summary>
        /// Optional failure probability originating from the failure mechanism section assessment result.
        /// This field can be null!
        /// </summary>
        public double FailureProbability { get; }

        /// <summary>
        /// Convert to string
        /// </summary>
        /// <returns>String of the object</returns>
        public override string ToString()
        {
            return "FmSectionAssemblyDirectResultWithProbability [" + Result + " P: " + FailureProbability + "]";
        }
    }
}