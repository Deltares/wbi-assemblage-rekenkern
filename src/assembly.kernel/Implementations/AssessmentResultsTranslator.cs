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
using Assembly.Kernel.Model.FailurePaths;

namespace Assembly.Kernel.Implementations
{
    /// <inheritdoc />
    public class AssessmentResultsTranslator : IAssessmentResultsTranslator
    {
        /// <inheritdoc />
        public FailurePathSectionAssemblyResult TranslateAssessmentResultWbi0A2(bool isRelevant,
            double probabilityInitialMechanismProfile,
            double probabilityInitialMechanismSection, bool needsRefinement, double refinedProbabilityProfile,
            double refinedProbabilitySection, CategoriesList<InterpretationCategory> categories)
        {
            CheckInputProbabilities(probabilityInitialMechanismProfile, probabilityInitialMechanismSection);
            CheckInputProbabilities(refinedProbabilityProfile, refinedProbabilitySection);

            if (categories == null)
            {
                throw new AssemblyException("AssemblyResultsTranslator", EAssemblyErrors.ValueMayNotBeNull);
            }

            if (!isRelevant)
            {
                return new FailurePathSectionAssemblyResult(0.0, 0.0, EInterpretationCategory.III);
            }

            if (needsRefinement)
            {
                // Check whether a refined probability is given
                if (!double.IsNaN(refinedProbabilitySection))
                {
                    var interpretationCategory =
                        categories.GetCategoryForFailureProbability(refinedProbabilitySection).Category;
                    return new FailurePathSectionAssemblyResult(
                        double.IsNaN(refinedProbabilityProfile) ? refinedProbabilitySection : refinedProbabilityProfile,
                        refinedProbabilitySection, interpretationCategory);
                }

                return new FailurePathSectionAssemblyResult(double.NaN, double.NaN, EInterpretationCategory.D);
            }

            if (!double.IsNaN(probabilityInitialMechanismSection))
            {
                var interpretationCategory =
                    categories.GetCategoryForFailureProbability(probabilityInitialMechanismSection).Category;
                return new FailurePathSectionAssemblyResult(
                    double.IsNaN(probabilityInitialMechanismProfile)
                        ? probabilityInitialMechanismSection
                        : probabilityInitialMechanismProfile, probabilityInitialMechanismSection,
                    interpretationCategory);
            }

            return new FailurePathSectionAssemblyResult(double.NaN, double.NaN, EInterpretationCategory.ND);
        }

        private static void CheckInputProbabilities(double probabilityProfile, double probabilitySection)
        {
            if (probabilityProfile < 0.0 || probabilityProfile > 1.0)
            {
                throw new AssemblyException("AssemblyResultsTranslator", EAssemblyErrors.FailureProbabilityOutOfRange);
            }
            if (probabilitySection < 0.0 || probabilitySection > 1.0)
            {
                throw new AssemblyException("AssemblyResultsTranslator", EAssemblyErrors.FailureProbabilityOutOfRange);
            }

            if (probabilitySection < probabilityProfile)
            {
                throw new AssemblyException("AssemblyResultsTranslator", EAssemblyErrors.ProfileProbabilityGreaterThanSectionProbability);
            }
        }
    }
}