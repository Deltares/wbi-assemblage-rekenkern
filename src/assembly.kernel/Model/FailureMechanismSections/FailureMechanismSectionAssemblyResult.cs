﻿#region Copyright (C) Rijkswaterstaat 2022. All rights reserved

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