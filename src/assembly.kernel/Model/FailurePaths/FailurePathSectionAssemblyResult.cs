using System.Globalization;
using Assembly.Kernel.Exceptions;
using Assembly.Kernel.Model.Categories;

namespace Assembly.Kernel.Model.FailurePaths
{
    /// <summary>
    /// Class that holds the results for a section of a failure path
    /// </summary>
    public class FailurePathSectionAssemblyResult
    {
        /// <summary>
        /// Constructor for the FailurePathSectionAssemblyResult class
        /// </summary>
        /// <param name="probabilityProfile">Estimated probability of failure for a representative profile in the section</param>
        /// <param name="probabilitySection">Estimated probability of failure of the section</param>
        /// <param name="category">The resulting interpretation category</param>
        /// <exception cref="AssemblyException">In case probabilityProfile or probabilitySection is not within the range 0.0 - 1.0 (or exactly 0.0 or 1.0)</exception>
        public FailurePathSectionAssemblyResult(Probability probabilityProfile, Probability probabilitySection,
            EInterpretationCategory category)
        {
            InterpretationCategory = category;
            ProbabilityProfile = probabilityProfile;
            ProbabilitySection = probabilitySection;
            if (double.IsNaN(probabilitySection) || double.IsNaN(probabilityProfile))
            {
                NSection = 1.0;
            }
            else
            {
                NSection = probabilitySection / probabilityProfile;
            }
        }

        /// <summary>
        /// The resulting interpretation category
        /// </summary>
        public EInterpretationCategory InterpretationCategory { get; }

        /// <summary>
        /// The length-effect factor
        /// </summary>
        public double NSection { get; }

        /// <summary>
        /// Estimated probability of failure for a representative profile in the section
        /// </summary>
        public Probability ProbabilityProfile { get; }

        /// <summary>
        /// Estimated probability of failure of the section
        /// </summary>
        public Probability ProbabilitySection { get; }

        /// <inheritdoc />
        public override string ToString()
        {
            return "FailurePathSectionAssemblyResult [" + InterpretationCategory + " Pprofile:" +
                   ProbabilityProfile.Value.ToString(CultureInfo.InvariantCulture) + ", Psection:" +
                   ProbabilitySection.Value.ToString(CultureInfo.InvariantCulture) + "]";
        }
    }
}