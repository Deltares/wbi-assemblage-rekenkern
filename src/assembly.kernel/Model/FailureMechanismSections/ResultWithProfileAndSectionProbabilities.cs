// Copyright (C) Rijkswaterstaat 2022. All rights reserved.
//
// This file is part of the Assembly kernel.
//
// Assembly kernel is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU Lesser General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public License
// along with this program. If not, see <http://www.gnu.org/licenses/>.
//
// All names, logos, and references to "Rijkswaterstaat" are registered trademarks of
// Rijkswaterstaat and remain full property of Rijkswaterstaat at all times.
// All rights reserved.

using Assembly.Kernel.Exceptions;

namespace Assembly.Kernel.Model.FailureMechanismSections
{
    /// <summary>
    /// Class that holds two probabilities for a profile and section.
    /// </summary>
    public class ResultWithProfileAndSectionProbabilities
    {
        /// <summary>
        /// Creates a new instance of <see cref="ResultWithProfileAndSectionProbabilities"/>.
        /// </summary>
        /// <param name="probabilityProfile">The probability of failure of a profile.</param>
        /// <param name="probabilitySection">The probability of failure of a section.</param>
        /// <exception cref="AssemblyException">Thrown when:
        /// <list type="bullet">
        /// <item><paramref name="probabilityProfile"/> and <paramref name="probabilitySection"/> are not both <c>Defined</c> or <c>Undefined</c>;</item>
        /// <item><paramref name="probabilityProfile"/> &gt; <paramref name="probabilitySection"/>.</item>
        /// </list>
        /// </exception>
        /// <seealso cref="Probability.IsDefined"/>
        public ResultWithProfileAndSectionProbabilities(Probability probabilityProfile, Probability probabilitySection)
        {
            ValidateProbabilities(probabilityProfile, probabilitySection);

            ProbabilityProfile = probabilityProfile;
            ProbabilitySection = probabilitySection;

            LengthEffectFactor = probabilitySection.IsDefined && !probabilitySection.IsNegligibleDifference(probabilityProfile)
                                     ? (double) probabilitySection / (double) probabilityProfile
                                     : 1.0;
        }

        /// <summary>
        /// Gets the probability of failure of a profile.
        /// </summary>
        public Probability ProbabilityProfile { get; }

        /// <summary>
        /// Gets the probability of failure of a section.
        /// </summary>
        public Probability ProbabilitySection { get; }

        /// <summary>
        /// Gets the length-effect factor.
        /// </summary>
        public double LengthEffectFactor { get; }

        /// <summary>
        /// Validates the probabilities.
        /// </summary>
        /// <param name="probabilityProfile">The probability of failure of a profile.</param>
        /// <param name="probabilitySection">The probability of failure of a section.</param>
        /// <exception cref="AssemblyException">Thrown when:
        /// <list type="bullet">
        /// <item><paramref name="probabilityProfile"/> and <paramref name="probabilitySection"/> are not both <c>Defined</c> or <c>Undefined</c>;</item>
        /// <item><paramref name="probabilityProfile"/> &gt; <paramref name="probabilitySection"/>.</item>
        /// </list>
        /// </exception>
        /// <seealso cref="Probability.IsDefined"/>
        private static void ValidateProbabilities(Probability probabilityProfile, Probability probabilitySection)
        {
            if (probabilitySection.IsDefined != probabilityProfile.IsDefined)
            {
                throw new AssemblyException(nameof(probabilityProfile), EAssemblyErrors.ProbabilitiesNotBothDefinedOrUndefined);
            }

            if (probabilitySection.IsDefined && probabilitySection < probabilityProfile)
            {
                throw new AssemblyException(nameof(probabilityProfile), EAssemblyErrors.ProfileProbabilityGreaterThanSectionProbability);
            }
        }
    }
}