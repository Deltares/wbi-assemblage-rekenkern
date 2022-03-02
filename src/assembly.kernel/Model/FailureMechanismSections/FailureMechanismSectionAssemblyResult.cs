using System.Globalization;
using Assembly.Kernel.Exceptions;
using Assembly.Kernel.Model.Categories;

namespace Assembly.Kernel.Model.FailureMechanismSections
{
    /// <summary>
    /// Class that holds the results for a section of a failure mechanism
    /// </summary>
    public class FailureMechanismSectionAssemblyResult
    {
        /// <summary>
        /// Constructor for the FailureMechanismSectionAssemblyResult class
        /// </summary>
        /// <param name="probabilityProfile">Estimated probability of failure for a representative profile in the section</param>
        /// <param name="probabilitySection">Estimated probability of failure of the section</param>
        /// <param name="category">The resulting interpretation category</param>
        /// <exception cref="AssemblyException">In case probabilityProfile or probabilitySection is not within the range 0.0 - 1.0 (or exactly 0.0 or 1.0)</exception>
        public FailureMechanismSectionAssemblyResult(Probability probabilityProfile, Probability probabilitySection,
            EInterpretationCategory category)
        {
            switch (category)
            {
                case EInterpretationCategory.Dominant:
                case EInterpretationCategory.NotDominant:
                case EInterpretationCategory.Gr:
                    if (!double.IsNaN(probabilityProfile) || !double.IsNaN(probabilitySection))
                    {
                        throw new AssemblyException("FailureMechanismSectionAssemblyResult", EAssemblyErrors.NonMatchingProbabilityValues);
                    }
                    break;
                case EInterpretationCategory.III:
                case EInterpretationCategory.II:
                case EInterpretationCategory.I:
                case EInterpretationCategory.Zero:
                case EInterpretationCategory.IMin:
                case EInterpretationCategory.IIMin:
                case EInterpretationCategory.IIIMin:
                    if (double.IsNaN(probabilitySection.Value) || double.IsNaN(probabilityProfile.Value))
                    {
                        throw new AssemblyException("FailureMechanismSectionAssemblyResult", EAssemblyErrors.ValueMayNotBeNaN);
                    }

                    if (probabilitySection < probabilityProfile)
                    {
                        throw new AssemblyException("FailureMechanismSectionAssemblyResult", EAssemblyErrors.ProfileProbabilityGreaterThanSectionProbability);
                    }
                    break;
                default:
                    throw new AssemblyException("FailureMechanismSectionAssemblyResult", EAssemblyErrors.InvalidCategoryValue);
            }
            
            InterpretationCategory = category;
            ProbabilityProfile = probabilityProfile;
            ProbabilitySection = probabilitySection;

            NSection = double.IsNaN(probabilitySection.Value) || double.IsNaN(probabilityProfile.Value) || probabilitySection == probabilityProfile
                ? 1.0
                : probabilitySection.Value / probabilityProfile.Value;
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
            return "FailureMechanismSectionAssemblyResult [" + InterpretationCategory + " Pprofile:" +
                   ProbabilityProfile.Value.ToString(CultureInfo.InvariantCulture) + ", Psection:" +
                   ProbabilitySection.Value.ToString(CultureInfo.InvariantCulture) + "]";
        }
    }
}