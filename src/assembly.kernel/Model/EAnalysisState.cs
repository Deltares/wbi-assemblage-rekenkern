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

using Assembly.Kernel.Interfaces;

namespace Assembly.Kernel.Model
{
    /// <summary>
    /// Input for <seealso cref="IAssessmentResultsTranslator.DetermineInterpretationCategoryWithoutProbabilityEstimationBoi0C1"/>.
    /// This enum describes the state of the failure probability analysis.
    /// </summary>
    public enum EAnalysisState
    {
        /// <summary>
        /// The section is not relevant for the failure probability of the assessment section.
        /// </summary>
        NotRelevant,

        /// <summary>
        /// The section is relevant, but not dominant for the failure probability of the
        /// assessment section. No further probability estimation is needed. 
        /// </summary>
        NoProbabilityEstimationNecessary,

        /// <summary>
        /// The section is dominant for the failure probability of the assessment section.
        /// Further analysis is necessary, but has not (yet) lead to a probability estimation.
        /// </summary>
        ProbabilityEstimationNecessary,

        /// <summary>
        /// A probability estimation has been made (either based on the initial mechanism or
        /// after refinement.
        /// </summary>
        ProbabilityEstimated
    }
}
