namespace Assembly.Kernel.Model.FailureMechanismSections
{
    /// <summary>
    /// Interface that defines the profile and section probabilities of a failure mechanism section.
    /// </summary>
    public interface IFailureMechanismSectionWithProbabilities
    {
        /// <summary>
        /// Estimated probability of failure for a representative profile in the section
        /// </summary>
        Probability ProbabilityProfile { get; }

        /// <summary>
        /// Estimated probability of failure of the section
        /// </summary>
        Probability ProbabilitySection { get; }
    }
}
