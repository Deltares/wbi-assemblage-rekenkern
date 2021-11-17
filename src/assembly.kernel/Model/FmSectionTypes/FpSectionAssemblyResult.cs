using System.Globalization;
using Assembly.Kernel.Exceptions;

namespace Assembly.Kernel.Model.FmSectionTypes
{
    /// <inheritdoc />
    public class FpSectionAssemblyResult : IFmSectionAssemblyResult
    {
        /// <summary>
        /// Constructor for the FpSectionAssemblyResult class
        /// </summary>
        /// <param name="probabilityProfile">Estimated probability of failure for a representative profile in the section</param>
        /// <param name="probabilitySection">Estimated probability of failure of the section</param>
        /// <param name="category">The resulting interpretation category</param>
        /// <exception cref="AssemblyException">In case probabilityProfile or probabilitySection is not within the range 0.0 - 1.0 (or exactly 0.0 or 1.0)</exception>
        public FpSectionAssemblyResult(double probabilityProfile, double probabilitySection, EInterpretationCategory category)
        {
            if (probabilityProfile < 0.0 || probabilityProfile > 1.0)
            {
                throw new AssemblyException("FpSectionAssemblyResult",
                    EAssemblyErrors.FailureProbabilityOutOfRange);
            }
            if (probabilitySection < 0.0 || probabilitySection > 1.0)
            {
                throw new AssemblyException("FpSectionAssemblyResult",
                    EAssemblyErrors.FailureProbabilityOutOfRange);
            }

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
        public double ProbabilityProfile { get; }

        /// <summary>
        /// Estimated probability of failure of the section
        /// </summary>
        public double ProbabilitySection { get; }

        /// <inheritdoc />
        public bool HasResult()
        {
            return InterpretationCategory != EInterpretationCategory.Gr;
        }

        /// <inheritdoc />
        public bool NotApplicableOrNeglectable()
        {
            return false;
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return "FpSectionAssemblyResult [" + InterpretationCategory + " Pprofile:" +
                   ProbabilityProfile.ToString(CultureInfo.InvariantCulture) + ", Psection:" + ProbabilitySection.ToString(CultureInfo.InvariantCulture) + "]";
        }
    }
}
