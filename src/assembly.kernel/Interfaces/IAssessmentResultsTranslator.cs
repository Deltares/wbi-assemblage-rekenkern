#region Copyright (C) Rijkswaterstaat 2019. All rights reserved

// Copyright (C) Rijkswaterstaat 2019. All rights reserved.
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
using Assembly.Kernel.Model;
using Assembly.Kernel.Model.Categories;
using Assembly.Kernel.Model.FailurePathSections;

namespace Assembly.Kernel.Interfaces
{
    /// <summary>
    /// Translate assessment results to an failure path section assessment result.
    /// </summary>
    public interface IAssessmentResultsTranslator
    {
        /// <summary>
        /// Translate the assessment result of failure path section assessments to a 
        /// single normative result. As specified in WBI-0A-2.
        /// </summary>
        /// <param name="isRelevant"></param>
        /// <param name="probabilityInitialMechanismProfile"></param>
        /// <param name="probabilityInitialMechanismSection"></param>
        /// <param name="needsRefinement"></param>
        /// <param name="refinedProbabilityProfile"></param>
        /// <param name="refinedProbabilitySection"></param>
        /// <param name="categories"></param>
        /// <returns>A new result resembling the normative result of the input parameters.</returns>
        /// <exception cref="AssemblyException">Thrown when probabilityInitialMechanismProfile is either smaller than 0.0 or greater than 1.0</exception>
        /// <exception cref="AssemblyException">Thrown when probabilityInitialMechanismSection is either smaller than 0.0 or greater than 1.0</exception>
        /// <exception cref="AssemblyException">Thrown when refinedProbabilityProfile is either smaller than 0.0 or greater than 1.0</exception>
        /// <exception cref="AssemblyException">Thrown when refinedProbabilitySection is either smaller than 0.0 or greater than 1.0</exception>
        FailurePathSectionAssemblyResult TranslateAssessmentResultWbi0A2(
            ESectionInitialMechanismProbabilitySpecification isRelevant,
            Probability probabilityInitialMechanismProfile,
            Probability probabilityInitialMechanismSection,
            bool needsRefinement,
            Probability refinedProbabilityProfile,
            Probability refinedProbabilitySection,
            CategoriesList<InterpretationCategory> categories);
    }
}