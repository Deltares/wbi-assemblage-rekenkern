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

using System.Collections.Generic;
using Assembly.Kernel.Exceptions;
using Assembly.Kernel.Model;
using Assembly.Kernel.Model.Categories;

namespace Assembly.Kernel.Interfaces
{
    /// <summary>
    /// Assemble Failure mechanism assembly results into one AssessmentResult.
    /// </summary>
    public interface IAssessmentGradeAssembler
    {
        /// <summary>
        /// Assembles Failure mechanism results with failure probability into one assembly section result.
        /// </summary>
        /// <param name="failureMechanismProbabilities">failure mechanism assembly result with failure probability.</param>
        /// <param name="partialAssembly">true if partial assembly is required (ignoring all undefined probabilities).</param>
        /// <returns>An assembled assessment section result.</returns>
        /// <exception cref="AssemblyException">Thrown when the list of probabilities is either null, or the list is empty
        /// (after filtering out undefined probabilities when <paramref name="partialAssembly"/> is true).</exception>
        Probability CalculateAssessmentSectionFailureProbabilityBoi2A1(
            IEnumerable<Probability> failureMechanismProbabilities,
            bool partialAssembly);

        /// <summary>
        /// Determine the assessment grade given the failure probability of an assessment section.
        /// </summary>
        /// <param name="failureProbability">The failure probability of the assessment section.</param>
        /// <param name="categories">Categories list that should be used to translate the combined probability of failure to the correct category.</param>
        /// <returns>An <see cref="EAssessmentGrade"/> representing the assessment grade for the assessment section.</returns>
        /// <exception cref="AssemblyException">In case the probability was undefined.</exception>
        /// <exception cref="AssemblyException">In case the categories equal null.</exception>
        EAssessmentGrade DetermineAssessmentGradeBoi2B1(
            Probability failureProbability,
            CategoriesList<AssessmentSectionCategory> categories);
    }
}