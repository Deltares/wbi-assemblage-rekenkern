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

using System.Collections.Generic;
using Assembly.Kernel.Exceptions;
using Assembly.Kernel.Interfaces;
using Assembly.Kernel.Model;
using Assembly.Kernel.Model.Categories;
using Assembly.Kernel.Model.FailurePaths;

namespace Assembly.Kernel.Implementations
{
    /// <inheritdoc />
    public class AssessmentResultsTranslator : IAssessmentResultsTranslator
    {
        /// <inheritdoc />
        public FailurePathSectionAssemblyResult TranslateAssessmentResultWbi0A2(bool isRelevant,
            Probability probabilityInitialMechanismProfile,
            Probability probabilityInitialMechanismSection, 
            bool needsRefinement, 
            Probability refinedProbabilityProfile,
            Probability refinedProbabilitySection, 
            CategoriesList<InterpretationCategory> categories)
        {
            CheckInput(probabilityInitialMechanismProfile, probabilityInitialMechanismSection, refinedProbabilityProfile, refinedProbabilitySection, categories);

            if (!isRelevant)
            {
                return new FailurePathSectionAssemblyResult(new Probability(0.0), new Probability(0.0), EInterpretationCategory.III);
            }

            if (needsRefinement)
            {
                // Check whether a refined probability is given
                if (!double.IsNaN(refinedProbabilitySection.Value))
                {
                    var category = categories.GetCategoryForFailureProbability(refinedProbabilitySection).Category;
                    // TODO: Write tests for these situations
                    var probabilityProfile = double.IsNaN(refinedProbabilityProfile.Value) ? refinedProbabilitySection : refinedProbabilityProfile;
                    return new FailurePathSectionAssemblyResult(probabilityProfile, refinedProbabilitySection, category);
                }
                return new FailurePathSectionAssemblyResult(Probability.NaN, Probability.NaN, EInterpretationCategory.D);
            }

            if (!double.IsNaN(probabilityInitialMechanismSection))
            {
                var category = categories.GetCategoryForFailureProbability(probabilityInitialMechanismSection).Category;
                // TODO: Write tests for these situations
                var initialMechanismProfile = double.IsNaN(probabilityInitialMechanismProfile.Value)
                    ? probabilityInitialMechanismSection
                    : probabilityInitialMechanismProfile;
                return new FailurePathSectionAssemblyResult(initialMechanismProfile, probabilityInitialMechanismSection, category);
            }

            return new FailurePathSectionAssemblyResult(Probability.NaN, Probability.NaN, EInterpretationCategory.ND);
        }

        private static void CheckInput(Probability probabilityInitialMechanismProfile,
            Probability probabilityInitialMechanismSection, Probability refinedProbabilityProfile,
            Probability refinedProbabilitySection, CategoriesList<InterpretationCategory> categories)
        {
            var errors = new List<AssemblyErrorMessage>();

            if (probabilityInitialMechanismSection < probabilityInitialMechanismProfile)
            {
                errors.Add(new AssemblyErrorMessage("AssemblyResultsTranslator",
                    EAssemblyErrors.ProfileProbabilityGreaterThanSectionProbability));
            }

            if (refinedProbabilitySection < refinedProbabilityProfile)
            {
                errors.Add(new AssemblyErrorMessage("AssemblyResultsTranslator",
                    EAssemblyErrors.ProfileProbabilityGreaterThanSectionProbability));
            }

            if (categories == null)
            {
                errors.Add(new AssemblyErrorMessage("AssemblyResultsTranslator", EAssemblyErrors.ValueMayNotBeNull));
            }

            if (errors.Count > 0)
            {
                throw new AssemblyException(errors);
            }
        }
    }
}