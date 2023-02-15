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

using System.Collections.Generic;
using System.Linq;
using Assembly.Kernel.Exceptions;
using Assembly.Kernel.Interfaces;
using Assembly.Kernel.Model;
using Assembly.Kernel.Model.Categories;

namespace Assembly.Kernel.Implementations
{
    /// <summary>
    /// Assemble failure mechanism assembly results into one assessment result.
    /// </summary>
    public class AssessmentGradeAssembler : IAssessmentGradeAssembler
    {
        /// <inheritdoc />
        public Probability CalculateAssessmentSectionFailureProbabilityBoi2A1(
            IEnumerable<Probability> failureMechanismProbabilities, bool partialAssembly)
        {
            if (failureMechanismProbabilities == null)
            {
                throw new AssemblyException(nameof(failureMechanismProbabilities), EAssemblyErrors.ValueMayNotBeNull);
            }

            if (partialAssembly)
            {
                failureMechanismProbabilities = failureMechanismProbabilities.Where(p => p.IsDefined);
            }

            ValidateProbabilities(failureMechanismProbabilities);
            return CalculateFailureProbability(failureMechanismProbabilities);
        }

        /// <inheritdoc />
        public EAssessmentGrade DetermineAssessmentGradeBoi2B1(
            Probability failureProbability, CategoriesList<AssessmentSectionCategory> categories)
        {
            AssemblyErrorMessage[] validationErrors = ValidateProbabilityAndCategories(failureProbability, categories).ToArray();

            if (validationErrors.Any())
            {
                throw new AssemblyException(validationErrors);
            }

            return categories.GetCategoryForFailureProbability(failureProbability).Category;
        }

        /// <summary>
        /// Validates the <paramref name="failureMechanismProbabilities"/>.
        /// </summary>
        /// <param name="failureMechanismProbabilities">The <see cref="IEnumerable{T}"/> of <see cref="Probability"/> to validate.</param>
        /// <exception cref="AssemblyException">Thrown when <paramref name="failureMechanismProbabilities"/> is <c>empty</c>
        /// or has undefined probabilities.</exception>
        private static void ValidateProbabilities(IEnumerable<Probability> failureMechanismProbabilities)
        {
            if (!failureMechanismProbabilities.Any())
            {
                throw new AssemblyException(nameof(failureMechanismProbabilities), EAssemblyErrors.EmptyResultsList);
            }

            foreach (Probability failureMechanismProbability in failureMechanismProbabilities)
            {
                if (!failureMechanismProbability.IsDefined)
                {
                    throw new AssemblyException(nameof(failureMechanismProbability), EAssemblyErrors.UndefinedProbability);
                }
            }
        }

        private static IEnumerable<AssemblyErrorMessage> ValidateProbabilityAndCategories(
            Probability failureProbability, CategoriesList<AssessmentSectionCategory> categories)
        {
            if (!failureProbability.IsDefined)
            {
                yield return new AssemblyErrorMessage(nameof(failureProbability), EAssemblyErrors.UndefinedProbability);
            }

            if (categories == null)
            {
                yield return new AssemblyErrorMessage(nameof(categories), EAssemblyErrors.ValueMayNotBeNull);
            }
        }

        private static Probability CalculateFailureProbability(IEnumerable<Probability> failureMechanismProbabilities)
        {
            var assessmentSectionFailureProbability = (Probability) 1.0;

            assessmentSectionFailureProbability = failureMechanismProbabilities.Aggregate(
                assessmentSectionFailureProbability, (current, fmp) => current * fmp.Inverse);

            return assessmentSectionFailureProbability.Inverse;
        }
    }
}