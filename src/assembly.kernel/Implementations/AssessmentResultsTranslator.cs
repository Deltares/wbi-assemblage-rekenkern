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
using Assembly.Kernel.Interfaces;
using Assembly.Kernel.Model;
using Assembly.Kernel.Model.Categories;
using Assembly.Kernel.Model.FailurePathSections;

namespace Assembly.Kernel.Implementations
{
    /// <inheritdoc />
    public class AssessmentResultsTranslator : IAssessmentResultsTranslator
    {
        /// <inheritdoc />
        public FailurePathSectionAssemblyResult TranslateAssessmentResultWbi0A2(
            ESectionInitialMechanismProbabilitySpecification isRelevant,
            Probability probabilityInitialMechanismProfile,
            Probability probabilityInitialMechanismSection,
            bool needsRefinement,
            Probability refinedProbabilityProfile,
            Probability refinedProbabilitySection,
            CategoriesList<InterpretationCategory> categories)
        {
            if (categories == null)
            {
                throw new AssemblyException("AssemblyResultsTranslator", EAssemblyErrors.ValueMayNotBeNull);
            }

            if (isRelevant == ESectionInitialMechanismProbabilitySpecification.NotRelevant)
            {
                return new FailurePathSectionAssemblyResult(new Probability(0.0), new Probability(0.0),
                    EInterpretationCategory.III);
            }

            if (needsRefinement)
            {
                CheckProbabilityRatio(refinedProbabilityProfile, refinedProbabilitySection);

                // Check whether a refined probability is given
                if (double.IsNaN(refinedProbabilitySection.Value))
                {
                    return new FailurePathSectionAssemblyResult(Probability.NaN, Probability.NaN,
                        EInterpretationCategory.D);
                }

                var probabilityProfile = double.IsNaN(refinedProbabilityProfile.Value)
                    ? refinedProbabilitySection
                    : refinedProbabilityProfile;
                var refinedCategory = categories.GetCategoryForFailureProbability(refinedProbabilitySection).Category;
                return new FailurePathSectionAssemblyResult(probabilityProfile, refinedProbabilitySection, refinedCategory);
            }

            if (isRelevant == ESectionInitialMechanismProbabilitySpecification.RelevantNoProbabilitySpecification)
            {
                return new FailurePathSectionAssemblyResult(Probability.NaN, Probability.NaN,
                    EInterpretationCategory.ND);
            }

            if (double.IsNaN(probabilityInitialMechanismSection))
            {
                throw new AssemblyException("probabilityInitialMechanismSection", EAssemblyErrors.ValueMayNotBeNaN);
            }

            // No refinement necessary. Look at probabilities for the initial mechanism
            CheckProbabilityRatio(probabilityInitialMechanismProfile, probabilityInitialMechanismSection);

            var categoryInitialMechanism = categories.GetCategoryForFailureProbability(probabilityInitialMechanismSection).Category;
            var initialMechanismProfile = double.IsNaN(probabilityInitialMechanismProfile.Value)
                ? probabilityInitialMechanismSection
                : probabilityInitialMechanismProfile;
            return new FailurePathSectionAssemblyResult(initialMechanismProfile, probabilityInitialMechanismSection, categoryInitialMechanism);
        }

        private static void CheckProbabilityRatio(Probability profileProbability, Probability sectionProbability)
        {
            if (sectionProbability < profileProbability)
            {
                throw new AssemblyException("AssemblyResultsTranslator", EAssemblyErrors.ProfileProbabilityGreaterThanSectionProbability);
            }
        }
    }
}