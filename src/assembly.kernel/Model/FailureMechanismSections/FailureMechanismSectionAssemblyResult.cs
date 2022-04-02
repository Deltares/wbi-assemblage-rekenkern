#region Copyright (C) Rijkswaterstaat 2022. All rights reserved

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

using System;
using System.Globalization;
using Assembly.Kernel.Exceptions;
using Assembly.Kernel.Model.Categories;

namespace Assembly.Kernel.Model.FailureMechanismSections
{
    /// <summary>
    /// Class that holds the results for a section of a failure mechanism.
    /// </summary>
    public class FailureMechanismSectionWithAssemblyResult : ResultWithProfileAndSectionProbabilities
    {
        /// <summary>
        /// Constructor for the FailureMechanismSectionWithAssemblyResult class.
        /// </summary>
        /// <param name="probabilityProfile">Estimated probability of failure for a representative profile in the section.</param>
        /// <param name="probabilitySection">Estimated probability of failure of the section.</param>
        /// <param name="category">The resulting interpretation category.</param>
        /// <exception cref="AssemblyException">In case <paramref name="category"/> equals <seealso cref="EInterpretationCategory.NotRelevant"/> or <seealso cref="EInterpretationCategory.NotDominant"/>
        /// and <paramref name="probabilityProfile"/> or <paramref name="probabilitySection"/> do not equal 0.0.</exception>
        /// <exception cref="AssemblyException">In case <paramref name="category"/> equals <seealso cref="EInterpretationCategory.Dominant"/> or <seealso cref="EInterpretationCategory.NoResult"/>
        /// and <paramref name="probabilityProfile"/> or <paramref name="probabilitySection"/> are defined (<seealso cref="Probability.Undefined"/> equals False).</exception>
        ///<exception cref="AssemblyException">Thrown when <paramref name="category"/> equals one of the categories associated with a probability range (<seealso cref="EInterpretationCategory.III"/>
        /// to <seealso cref="EInterpretationCategory.IIIMin"/>) and either <paramref name="probabilityProfile"/> or <paramref name="probabilitySection"/> is undefined (<seealso cref="Probability.IsDefined"/> equals false).</exception>
        /// <exception cref="AssemblyException">In case of an invalid value for <paramref name="category"/>.</exception>
        /// <inheritdoc cref="ResultWithProfileAndSectionProbabilities"/>
        public FailureMechanismSectionWithAssemblyResult(Probability probabilityProfile, Probability probabilitySection,
            EInterpretationCategory category) : base(probabilityProfile, probabilitySection)
        {
            switch (category)
            {
                case EInterpretationCategory.NotDominant:
                case EInterpretationCategory.NotRelevant:
                    if (Math.Abs(probabilityProfile - 0.0) > 1E-8 || Math.Abs(probabilitySection - 0.0) > 1E-8)
                    {
                        throw new AssemblyException(nameof(FailureMechanismSectionWithAssemblyResult), EAssemblyErrors.NonMatchingProbabilityValues);
                    }
                    break;
                case EInterpretationCategory.Dominant:
                case EInterpretationCategory.NoResult:
                    if (probabilityProfile.IsDefined || probabilitySection.IsDefined)
                    {
                        throw new AssemblyException(nameof(FailureMechanismSectionWithAssemblyResult), EAssemblyErrors.NonMatchingProbabilityValues);
                    }
                    break;
                case EInterpretationCategory.III:
                case EInterpretationCategory.II:
                case EInterpretationCategory.I:
                case EInterpretationCategory.Zero:
                case EInterpretationCategory.IMin:
                case EInterpretationCategory.IIMin:
                case EInterpretationCategory.IIIMin:
                    if (!probabilitySection.IsDefined || !probabilityProfile.IsDefined)
                    {
                        throw new AssemblyException(nameof(FailureMechanismSectionWithAssemblyResult), EAssemblyErrors.ProbabilityMayNotBeUndefined);
                    }
                    break;
                default:
                    throw new AssemblyException(nameof(category), EAssemblyErrors.InvalidCategoryValue);
            }

            InterpretationCategory = category;
            NSection = !probabilitySection.IsDefined || !probabilityProfile.IsDefined || probabilitySection == probabilityProfile
                ? 1.0
                : (double)probabilitySection / (double)probabilityProfile;
        }

        /// <summary>
        /// The resulting interpretation category.
        /// </summary>
        public EInterpretationCategory InterpretationCategory { get; }

        /// <summary>
        /// The length-effect factor.
        /// </summary>
        public double NSection { get; }

        /// <inheritdoc />
        public override string ToString()
        {
            return "FailureMechanismSectionWithAssemblyResult [" + InterpretationCategory + " Pprofile:" +
                   ProbabilityProfile.ToString(CultureInfo.InvariantCulture) + ", Psection:" +
                   ProbabilitySection.ToString(CultureInfo.InvariantCulture) + "]";
        }
    }
}