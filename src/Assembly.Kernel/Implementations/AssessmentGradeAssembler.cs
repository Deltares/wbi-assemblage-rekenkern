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

using System;
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
                throw new ArgumentNullException(nameof(failureMechanismProbabilities));
            }

            if (partialAssembly)
            {
                failureMechanismProbabilities = failureMechanismProbabilities.Where(p => p.IsDefined);
            }

            ValidateProbabilities(failureMechanismProbabilities);
            return CalculateFailureProbability(failureMechanismProbabilities);
        }

        /// <inheritdoc />
        public Probability CalculateAssessmentSectionFailureProbabilityBoi2A2(
            IEnumerable<Probability> correlatedFailureMechanismProbabilities,
            IEnumerable<Probability> uncorrelatedFailureMechanismProbabilities,
            bool partialAssembly)
        {
            if (correlatedFailureMechanismProbabilities == null)
            {
                throw new ArgumentNullException(nameof(correlatedFailureMechanismProbabilities));
            }

            if (uncorrelatedFailureMechanismProbabilities == null)
            {
                throw new ArgumentNullException(nameof(uncorrelatedFailureMechanismProbabilities));
            }

            if (partialAssembly)
            {
                correlatedFailureMechanismProbabilities = correlatedFailureMechanismProbabilities.Where(p => p.IsDefined);
                uncorrelatedFailureMechanismProbabilities = uncorrelatedFailureMechanismProbabilities.Where(p => p.IsDefined);
            }

            ValidateProbabilities(correlatedFailureMechanismProbabilities, uncorrelatedFailureMechanismProbabilities);

            IEnumerable<Probability> failureMechanismProbabilities = uncorrelatedFailureMechanismProbabilities.Concat(new[]
            {
                correlatedFailureMechanismProbabilities.Max(p => p)
            });

            return CalculateFailureProbability(failureMechanismProbabilities);
        }

        /// <inheritdoc />
        public EAssessmentGrade DetermineAssessmentGradeBoi2B1(
            Probability failureProbability, CategoriesList<AssessmentSectionCategory> categories)
        {
            if (categories == null)
            {
                throw new ArgumentNullException(nameof(categories));
            }

            if (!failureProbability.IsDefined)
            {
                throw new AssemblyException(nameof(failureProbability), EAssemblyErrors.UndefinedProbability);
            }

            return categories.GetCategoryForFailureProbability(failureProbability).Category;
        }

        /// <summary>
        /// Validates the <paramref name="failureMechanismProbabilities"/>.
        /// </summary>
        /// <param name="failureMechanismProbabilities">The failure mechanism assembly results.</param>
        /// <exception cref="AssemblyException">Thrown when <paramref name="failureMechanismProbabilities"/> is <c>empty</c>
        /// or has undefined probabilities.</exception>
        private static void ValidateProbabilities(IEnumerable<Probability> failureMechanismProbabilities)
        {
            if (!failureMechanismProbabilities.Any())
            {
                throw new AssemblyException(nameof(failureMechanismProbabilities), EAssemblyErrors.EmptyResultsList);
            }

            if (!failureMechanismProbabilities.All(failureMechanismProbability => failureMechanismProbability.IsDefined))
            {
                throw new AssemblyException(nameof(failureMechanismProbabilities), EAssemblyErrors.UndefinedProbability);
            }
        }

        /// <summary>
        /// Validates the <paramref name="correlatedFailureMechanismProbabilities"/> and
        /// <paramref name="uncorrelatedFailureMechanismProbabilities"/>.
        /// </summary>
        /// <param name="correlatedFailureMechanismProbabilities">The correlated failure mechanism assembly results.</param>
        /// <param name="uncorrelatedFailureMechanismProbabilities">The uncorrelated failure mechanism assembly results.</param>
        /// <exception cref="AssemblyException">Thrown when:
        /// <list type="bullet">
        /// <item><paramref name="correlatedFailureMechanismProbabilities"/> is <c>empty</c>;</item>
        /// <item><paramref name="correlatedFailureMechanismProbabilities"/> or
        /// <paramref name="uncorrelatedFailureMechanismProbabilities"/> contains <see cref="Probability.Undefined"/> probabilities.</item>
        /// </list>
        /// </exception>
        private static void ValidateProbabilities(IEnumerable<Probability> correlatedFailureMechanismProbabilities,
                                                  IEnumerable<Probability> uncorrelatedFailureMechanismProbabilities)
        {
            if (!correlatedFailureMechanismProbabilities.Any())
            {
                throw new AssemblyException(nameof(correlatedFailureMechanismProbabilities), EAssemblyErrors.EmptyResultsList);
            }

            if (!correlatedFailureMechanismProbabilities.All(failureMechanismProbability => failureMechanismProbability.IsDefined))
            {
                throw new AssemblyException(nameof(correlatedFailureMechanismProbabilities), EAssemblyErrors.UndefinedProbability);
            }

            if (!uncorrelatedFailureMechanismProbabilities.All(failureMechanismProbability => failureMechanismProbability.IsDefined))
            {
                throw new AssemblyException(nameof(uncorrelatedFailureMechanismProbabilities), EAssemblyErrors.UndefinedProbability);
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