#region Copyright (C) Rijkswaterstaat 2022. All rights reserved.

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

#endregion

using Assembly.Kernel.Exceptions;

namespace Assembly.Kernel.Model.FailureMechanismSections
{
    /// <summary>
    /// Class that holds two probabilities for a profile and section. As both
    /// probabilities are connected they should be both defined or both undefined.
    /// </summary>
    public class ResultWithProfileAndSectionProbabilities
    {
        /// <summary>
        /// Constructs the result with profile and section probability.
        /// </summary>
        /// <param name="probabilityProfile">Probability of failure of a profile.</param>
        /// <param name="probabilitySection">Probability of failure of a section.</param>
        /// <exception cref="AssemblyException">Thrown in case just one of <paramref name="probabilityProfile"/> and <paramref name="probabilitySection"/> is defined.</exception>
        /// <exception cref="AssemblyException">Thrown in case <paramref name="probabilityProfile"/> &gt; <paramref name="probabilitySection"/>.</exception>
        public ResultWithProfileAndSectionProbabilities(Probability probabilityProfile, Probability probabilitySection)
        {
            if (probabilitySection.IsDefined != probabilityProfile.IsDefined)
            {
                throw new AssemblyException(nameof(ResultWithProfileAndSectionProbabilities), EAssemblyErrors.ProbabilitiesNotBothDefinedOrUndefined);
            }

            if (probabilitySection.IsDefined && probabilitySection < probabilityProfile)
            {
                throw new AssemblyException(nameof(ResultWithProfileAndSectionProbabilities), EAssemblyErrors.ProfileProbabilityGreaterThanSectionProbability);
            }

            ProbabilityProfile = probabilityProfile;
            ProbabilitySection = probabilitySection;

            LengthEffectFactor = !probabilitySection.IsDefined || !probabilityProfile.IsDefined || probabilitySection == probabilityProfile
                                     ? 1.0
                                     : (double) probabilitySection / (double) probabilityProfile;
        }

        /// <summary>
        /// Estimated probability of failure for a representative profile in the section.
        /// </summary>
        public Probability ProbabilityProfile { get; }

        /// <summary>
        /// Estimated probability of failure of the section.
        /// </summary>
        public Probability ProbabilitySection { get; }

        /// <summary>
        /// The length-effect factor.
        /// </summary>
        public double LengthEffectFactor { get; }
    }
}