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

using System.Globalization;
using Assembly.Kernel.Exceptions;
using Assembly.Kernel.Model.Categories;

namespace Assembly.Kernel.Model.FailureMechanismSections
{
    /// <summary>
    /// Class that holds the results for a section of a failure mechanism.
    /// </summary>
    public class FailureMechanismSectionAssemblyResult
    {
        /// <summary>
        /// Constructor for the FailureMechanismSectionWithAssemblyResult class.
        /// </summary>
        /// <param name="probabilitySection">Estimated probability of failure of the section.</param>
        /// <param name="category">The resulting interpretation category.</param>
        /// <exception cref="AssemblyException">In case <paramref name="category"/> equals <seealso cref="EInterpretationCategory.NotRelevant"/> or <seealso cref="EInterpretationCategory.NotDominant"/>
        /// and <paramref name="probabilitySection"/> does not equal 0.0.</exception>
        /// <exception cref="AssemblyException">In case <paramref name="category"/> equals <seealso cref="EInterpretationCategory.Dominant"/> or <seealso cref="EInterpretationCategory.NoResult"/>
        /// and <paramref name="probabilitySection"/> is defined (<seealso cref="Probability.Undefined"/> equals False).</exception>
        ///<exception cref="AssemblyException">Thrown when <paramref name="category"/> equals one of the categories associated with a probability range (<seealso cref="EInterpretationCategory.III"/>
        /// to <seealso cref="EInterpretationCategory.IIIMin"/>) and <paramref name="probabilitySection"/> is undefined (<seealso cref="Probability.IsDefined"/> equals false).</exception>
        /// <exception cref="AssemblyException">In case of an invalid value for <paramref name="category"/>.</exception>
        /// <inheritdoc cref="ResultWithProfileAndSectionProbabilities"/>
        public FailureMechanismSectionAssemblyResult(Probability probabilitySection,
            EInterpretationCategory category)
        {
            switch (category)
            {
                case EInterpretationCategory.NotDominant:
                case EInterpretationCategory.NotRelevant:
                    if (!probabilitySection.IsNegligibleDifference((Probability)0))
                    {
                        throw new AssemblyException(nameof(FailureMechanismSectionAssemblyResultWithLengthEffect), EAssemblyErrors.NonMatchingProbabilityValues);
                    }
                    break;
                case EInterpretationCategory.Dominant:
                case EInterpretationCategory.NoResult:
                    if (probabilitySection.IsDefined)
                    {
                        throw new AssemblyException(nameof(FailureMechanismSectionAssemblyResultWithLengthEffect), EAssemblyErrors.NonMatchingProbabilityValues);
                    }
                    break;
                case EInterpretationCategory.III:
                case EInterpretationCategory.II:
                case EInterpretationCategory.I:
                case EInterpretationCategory.Zero:
                case EInterpretationCategory.IMin:
                case EInterpretationCategory.IIMin:
                case EInterpretationCategory.IIIMin:
                    if (!probabilitySection.IsDefined)
                    {
                        throw new AssemblyException(nameof(FailureMechanismSectionAssemblyResultWithLengthEffect), EAssemblyErrors.ProbabilityMayNotBeUndefined);
                    }
                    break;
                default:
                    throw new AssemblyException(nameof(category), EAssemblyErrors.InvalidCategoryValue);
            }

            InterpretationCategory = category;
            ProbabilitySection = probabilitySection;
        }

        /// <summary>
        /// The resulting interpretation category.
        /// </summary>
        public EInterpretationCategory InterpretationCategory { get; }

        /// <summary>
        /// Estimated probability of failure of the section.
        /// </summary>
        public Probability ProbabilitySection { get; }

        /// <inheritdoc />
        public override string ToString()
        {
            return nameof(FailureMechanismSectionAssemblyResult) + " [" + InterpretationCategory + ", Psection:" +
                   ProbabilitySection.ToString(CultureInfo.InvariantCulture) + "]";
        }
    }
}
