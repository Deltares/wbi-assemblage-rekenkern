﻿// Copyright (C) Rijkswaterstaat 2022. All rights reserved.
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
using Assembly.Kernel.Exceptions;
using Assembly.Kernel.Model;
using Assembly.Kernel.Model.Categories;

namespace Assembly.Kernel.Interfaces
{
    /// <summary>
    /// Interface to assemble failure mechanism assembly results into one assessment result.
    /// </summary>
    public interface IAssessmentGradeAssembler
    {
        /// <summary>
        /// Assembles failure mechanism results with failure probability into one assessment section result.
        /// </summary>
        /// <param name="failureMechanismProbabilities">The failure mechanism assembly results with failure probability.</param>
        /// <param name="partialAssembly">Indicator whether partial assembly is required.</param>
        /// <returns>An assembled assessment section result.</returns>
        /// <exception cref="AssemblyException">Thrown when the list of probabilities is either <c>null</c>, or the list is <c>empty</c>.</exception>
        /// <remarks>When <paramref name="partialAssembly"/> is <c>true</c>, all <c>Undefined</c> probabilities are ignored.</remarks>
        /// <seealso cref="Probability.Undefined"/>
        Probability CalculateAssessmentSectionFailureProbabilityBoi2A1(
            IEnumerable<Probability> failureMechanismProbabilities, bool partialAssembly);

        /// <summary>
        /// Determine the assessment grade given the failure probability of an assessment section.
        /// </summary>
        /// <param name="failureProbability">The failure probability of the assessment section.</param>
        /// <param name="categories">The categories to use.</param>
        /// <returns>The <see cref="EAssessmentGrade"/> of the assessment section.</returns>
        /// <exception cref="AssemblyException">Thrown when <paramref name="categories"/> is <c>null</c>
        /// or <paramref name="failureProbability"/> is undefined.</exception>
        EAssessmentGrade DetermineAssessmentGradeBoi2B1(
            Probability failureProbability, CategoriesList<AssessmentSectionCategory> categories);
    }
}