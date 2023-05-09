// Copyright (C) Stichting Deltares and State of the Netherlands 2023. All rights reserved.
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
// All names, logos, and references to "Deltares" are registered trademarks of
// Stichting Deltares and remain full property of Stichting Deltares at all times.
// All rights reserved.

using System;
using Assembly.Kernel.Exceptions;
using Assembly.Kernel.Interfaces;
using Assembly.Kernel.Model;
using Assembly.Kernel.Model.Categories;
using Assembly.Kernel.Model.FailureMechanismSections;

namespace Assembly.Kernel.Implementations
{
    /// <summary>
    /// Translate assessment results to a failure mechanism section assembly result.
    /// </summary>
    public class AssessmentResultsTranslator : IAssessmentResultsTranslator
    {
        /// <inheritdoc />
        public Probability DetermineRepresentativeProbabilityBoi0A1(
            bool refinementNecessary,
            Probability probabilityInitialMechanismSection,
            Probability refinedProbabilitySection)
        {
            return refinementNecessary
                       ? refinedProbabilitySection
                       : probabilityInitialMechanismSection;
        }

        /// <inheritdoc />
        public ResultWithProfileAndSectionProbabilities DetermineRepresentativeProbabilitiesBoi0A2(
            bool refinementNecessary,
            Probability probabilityInitialMechanismProfile,
            Probability probabilityInitialMechanismSection,
            Probability refinedProbabilityProfile,
            Probability refinedProbabilitySection)
        {
            return refinementNecessary
                       ? new ResultWithProfileAndSectionProbabilities(refinedProbabilityProfile, refinedProbabilitySection)
                       : new ResultWithProfileAndSectionProbabilities(probabilityInitialMechanismProfile, probabilityInitialMechanismSection);
        }

        /// <inheritdoc />
        public EInterpretationCategory DetermineInterpretationCategoryFromFailureMechanismSectionProbabilityBoi0B1(
            Probability sectionProbability,
            CategoriesList<InterpretationCategory> categories)
        {
            if (categories == null)
            {
                throw new ArgumentNullException(nameof(categories));
            }

            return categories.GetCategoryForFailureProbability(sectionProbability).Category;
        }

        /// <inheritdoc />
        public EInterpretationCategory DetermineInterpretationCategoryWithoutProbabilityEstimationBoi0C1(
            EAnalysisState analysisState)
        {
            switch (analysisState)
            {
                case EAnalysisState.NotRelevant:
                    return EInterpretationCategory.NotRelevant;
                case EAnalysisState.ProbabilityEstimationNecessary:
                    return EInterpretationCategory.Dominant;
                case EAnalysisState.NoProbabilityEstimationNecessary:
                    return EInterpretationCategory.NotDominant;
                default:
                    throw new AssemblyException(nameof(analysisState), EAssemblyErrors.InvalidEnumValue);
            }
        }

        /// <inheritdoc />
        public Probability TranslateInterpretationCategoryToProbabilityBoi0C2(EInterpretationCategory category)
        {
            switch (category)
            {
                case EInterpretationCategory.NotDominant:
                case EInterpretationCategory.NotRelevant:
                    return new Probability(0.0);
                case EInterpretationCategory.Dominant:
                case EInterpretationCategory.NoResult:
                    return Probability.Undefined;
                default:
                    throw new AssemblyException(nameof(category), EAssemblyErrors.InvalidCategoryValue);
            }
        }

        /// <inheritdoc />
        public Probability CalculateProfileProbabilityToSectionProbabilityBoi0D1(Probability profileProbability, double lengthEffectFactor)
        {
            CheckValidLengthEffectFactor(lengthEffectFactor);

            double sectionProbabilityValue = (double) profileProbability * lengthEffectFactor;
            return new Probability(Math.Min(sectionProbabilityValue, 1.0));
        }

        /// <inheritdoc />
        public Probability CalculateSectionProbabilityToProfileProbabilityBoi0D2(Probability sectionProbability, double lengthEffectFactor)
        {
            CheckValidLengthEffectFactor(lengthEffectFactor);

            return sectionProbability / lengthEffectFactor;
        }

        private static void CheckValidLengthEffectFactor(double lengthEffectFactor)
        {
            if (lengthEffectFactor < 1.0)
            {
                throw new AssemblyException(nameof(lengthEffectFactor), EAssemblyErrors.LengthEffectFactorOutOfRange);
            }
        }
    }
}