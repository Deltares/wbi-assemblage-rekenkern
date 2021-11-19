using Assembly.Kernel.Exceptions;

namespace Assembly.Kernel.Model
{
    /// <summary>
    /// The assessment result for this assessment section
    /// </summary>
    public class AssessmentSectionResult
    {
        /// <summary>
        /// Constructor of AssessmentSectionResult
        /// </summary>
        /// <param name="failureProbability">The estimated probability of flooding of the assessment section</param>
        /// <param name="grade">The grade associated with the probability of flooding</param>
        /// <exception cref="AssemblyException">Thrown when the specified probability is less than 0.0 or greater than 1.0</exception>
        public AssessmentSectionResult(double failureProbability, EAssessmentGrade grade)
        {
            if (failureProbability < 0.0 || failureProbability > 1.0)
            {
                throw new AssemblyException("AssessmentSectionResult", EAssemblyErrors.FailureProbabilityOutOfRange);
            }

            Category = grade;
            FailureProbability = failureProbability;
        }

        /// <summary>
        /// The estimated probability of flooding of the assessment section
        /// </summary>
        public double FailureProbability { get; }

        /// <summary>
        /// The grade associated with the probability of flooding
        /// </summary>
        public EAssessmentGrade Category { get; }
    }
}