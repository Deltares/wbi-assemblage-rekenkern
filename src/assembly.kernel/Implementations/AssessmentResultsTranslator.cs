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
using Assembly.Kernel.Model.CategoryLimits;
using Assembly.Kernel.Model.FmSectionTypes;

namespace Assembly.Kernel.Implementations
{
    /// <inheritdoc />
    public class AssessmentResultsTranslator : IAssessmentResultsTranslator
    {
        /// <inheritdoc />
        public FpSectionAssemblyResult TranslateAssessmentResultWbi0A2(bool isRelevant, double probabilityInitialMechanismProfile,
            double probabilityInitialMechanismSection, bool needsRefinement, double refinedProbabilityProfile,
            double refinedProbabilitySection, CategoriesList<InterpretationCategory> categories)
        {
            CheckInputProbability(probabilityInitialMechanismProfile);
            CheckInputProbability(probabilityInitialMechanismSection);
            CheckInputProbability(refinedProbabilityProfile);
            CheckInputProbability(refinedProbabilitySection);

            if (categories == null)
            {
                throw new AssemblyException("AssemblyResultsTranslator", EAssemblyErrors.ValueMayNotBeNull);
            }

            if (!isRelevant)
            {
                return new FpSectionAssemblyResult(0.0,0.0,EInterpretationCategory.III);
            }

            if (needsRefinement)
            {
                // Check whether a refined probability is given
                if (!double.IsNaN(refinedProbabilitySection))
                {
                    var interpretationCategory = categories.GetCategoryForFailureProbability(refinedProbabilitySection).Category;
                    return new FpSectionAssemblyResult(
                        double.IsNaN(refinedProbabilityProfile) ? refinedProbabilitySection : refinedProbabilityProfile,
                        refinedProbabilitySection, interpretationCategory);
                }

                return new FpSectionAssemblyResult(double.NaN, double.NaN, EInterpretationCategory.D);
            }
            else
            {
                if (!double.IsNaN(probabilityInitialMechanismSection))
                {
                    var interpretationCategory = categories.GetCategoryForFailureProbability(probabilityInitialMechanismSection).Category;
                    return new FpSectionAssemblyResult(double.IsNaN(probabilityInitialMechanismProfile) ? probabilityInitialMechanismSection : probabilityInitialMechanismProfile, probabilityInitialMechanismSection, interpretationCategory);
                }

                return new FpSectionAssemblyResult(double.NaN, double.NaN, EInterpretationCategory.ND);
            }
        }

        private static void CheckInputProbability(double probability)
        {
            if (probability < 0.0 || probability > 1.0)
            {
                throw new AssemblyException("AssemblyResultsTranslator", EAssemblyErrors.FailureProbabilityOutOfRange);
            }
        }
    }
}