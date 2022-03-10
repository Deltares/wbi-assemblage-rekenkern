using Assembly.Kernel.Model;
using Assembly.Kernel.Model.Categories;

namespace assembly.kernel.benchmark.tests.data.Input.FailureMechanismSections
{
    /// <summary>
    /// Expected failure mechanism section that includes the length effect within the section.
    /// </summary>
    public class ExpectedFailureMechanismSectionWithLengthEffect :ExpectedFailureMechanismSection
    {
        /// <summary>
        /// Constructor for the <seealso cref="ExpectedFailureMechanismSectionWithLengthEffect"/>.
        /// </summary>
        /// <param name="sectionName">Custom of the section.</param>
        /// <param name="start">Start of the section in meters along the assessment section.</param>
        /// <param name="end">End of the section in meters along the assessment section.</param>
        /// <param name="isRelevant">Relevance of the section for a specific failure mechanism.</param>
        /// <param name="initialMechanismProbabilityProfile">Probability estimation for the initial mechanism for a single profile.</param>
        /// <param name="initialMechanismProbabilitySection">Probability estimation for the initial mechanism for the complete section.</param>
        /// <param name="refinementStatus">Refinement status of the probability estimation.</param>
        /// <param name="refinedProbabilityProfile">Refined probability estimation for a single profile.</param>
        /// <param name="refinedProbabilitySection">Refined probability estimation for the complete section.</param>
        /// <param name="expectedCombinedProbabilityProfile">The expected combined probability for a single profile.</param>
        /// <param name="expectedCombinedProbabilitySection">The expected combined probability for the complete section.</param>
        /// <param name="expectedInterpretationCategory">The expected interpretation category for the section.</param>
        public ExpectedFailureMechanismSectionWithLengthEffect(string sectionName, 
            double start, 
            double end, 
            bool isRelevant, 
            Probability initialMechanismProbabilityProfile, 
            Probability initialMechanismProbabilitySection, 
            ERefinementStatus refinementStatus,
            Probability refinedProbabilityProfile,
            Probability refinedProbabilitySection,
            Probability expectedCombinedProbabilityProfile,
            Probability expectedCombinedProbabilitySection, 
            EInterpretationCategory expectedInterpretationCategory) 
            : base(sectionName, start, end, isRelevant, initialMechanismProbabilitySection, refinementStatus, refinedProbabilitySection, expectedCombinedProbabilitySection, expectedInterpretationCategory)
        {
            InitialMechanismProbabilityProfile = initialMechanismProbabilityProfile;
            RefinedProbabilityProfile = refinedProbabilityProfile;
            ExpectedCombinedProbabilityProfile = expectedCombinedProbabilityProfile;
        }

        /// <summary>
        /// Probability estimation for the initial mechanism for a single profile.
        /// </summary>
        public Probability InitialMechanismProbabilityProfile { get; }

        /// <summary>
        /// Refined probability estimation for a single profile.
        /// </summary>
        public Probability RefinedProbabilityProfile { get; }

        /// <summary>
        /// The expected combined probability for a single profile.
        /// </summary>
        public Probability ExpectedCombinedProbabilityProfile { get; }
    }
}