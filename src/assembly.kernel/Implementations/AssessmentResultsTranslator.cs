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

using Assembly.Kernel.Exceptions;
using Assembly.Kernel.Interfaces;
using Assembly.Kernel.Model;
using Assembly.Kernel.Model.Categories;
using Assembly.Kernel.Model.FailureMechanismSections;

namespace Assembly.Kernel.Implementations
{
    /// <inheritdoc />
    public class AssessmentResultsTranslator : IAssessmentResultsTranslator
    {
        /// <inheritdoc />
        public FailureMechanismSectionAssemblyResult TranslateAssessmentResultAggregatedMethod(
            ESectionInitialMechanismProbabilitySpecification relevance,
            Probability probabilityInitialMechanismSection,
            ERefinementStatus refinementStatus,
            Probability refinedProbabilitySection,
            CategoriesList<InterpretationCategory> categories)
        {
            var translator = new AssessmentResultsTranslator();
            var analysisState = GetAnalysisState(relevance, refinementStatus);
            if (analysisState == EAnalysisState.ProbabilityEstimated)
            {
                var probability = translator.DetermineRepresentativeProbabilityBoi0A1(refinementStatus == ERefinementStatus.Performed,
                                                                                      probabilityInitialMechanismSection,
                                                                                      refinedProbabilitySection);
                var category =
                    translator.DetermineInterpretationCategoryFromFailureMechanismSectionProbabilityBoi0B1(
                        probability, categories);
                return new FailureMechanismSectionAssemblyResult(probability, category);
            }
            else
            {
                var category = translator.DetermineInterpretationCategoryWithoutProbabilityEstimationBoi0C1(analysisState);
                var probability = translator.TranslateInterpretationCategoryToProbabilityBoi0C2(category);
                return new FailureMechanismSectionAssemblyResult(probability, category);
            }
        }

        /// <inheritdoc />
        public FailureMechanismSectionAssemblyResultWithLengthEffect TranslateAssessmentResultWithLengthEffectAggregatedMethod(
            ESectionInitialMechanismProbabilitySpecification relevance,
            Probability probabilityInitialMechanismProfile,
            Probability probabilityInitialMechanismSection,
            ERefinementStatus refinementStatus,
            Probability refinedProbabilityProfile,
            Probability refinedProbabilitySection,
            CategoriesList<InterpretationCategory> categories)
        {
            var translator = new AssessmentResultsTranslator();
            var analysisState = GetAnalysisState(relevance, refinementStatus);
            if (analysisState == EAnalysisState.ProbabilityEstimated)
            {
                var probabilities = translator.DetermineRepresentativeProbabilitiesBoi0A2(refinementStatus == ERefinementStatus.Performed,
                                                                                          probabilityInitialMechanismProfile,
                                                                                          probabilityInitialMechanismSection,
                                                                                          refinedProbabilityProfile,
                                                                                          refinedProbabilitySection);
                var category =
                    translator.DetermineInterpretationCategoryFromFailureMechanismSectionProbabilityBoi0B1(
                        probabilities.ProbabilitySection, categories);
                return new FailureMechanismSectionAssemblyResultWithLengthEffect(probabilities.ProbabilityProfile, probabilities.ProbabilitySection, category);
            }
            else
            {
                var category = translator.DetermineInterpretationCategoryWithoutProbabilityEstimationBoi0C1(analysisState);
                var probability = translator.TranslateInterpretationCategoryToProbabilityBoi0C2(category);
                return new FailureMechanismSectionAssemblyResultWithLengthEffect(probability, probability, category);
            }
        }

        /// <inheritdoc />
        public Probability DetermineRepresentativeProbabilityBoi0A1(bool refinementNecessary,
                                                                    Probability probabilityInitialMechanismSection, Probability refinedProbabilitySection)
        {
            return refinementNecessary ? refinedProbabilitySection : probabilityInitialMechanismSection;
        }

        /// <inheritdoc />
        public ResultWithProfileAndSectionProbabilities DetermineRepresentativeProbabilitiesBoi0A2(bool refinementNecessary,
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
            Probability sectionProbability, CategoriesList<InterpretationCategory> categories)
        {
            if (categories == null)
            {
                throw new AssemblyException(nameof(categories), EAssemblyErrors.ValueMayNotBeNull);
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

            var sectionProbabilityValue = (double) profileProbability * lengthEffectFactor;
            return new Probability(sectionProbabilityValue > 1 ? 1 : sectionProbabilityValue);
        }

        /// <inheritdoc />
        public Probability CalculateSectionProbabilityToProfileProbabilityBoi0D2(Probability sectionProbability,
                                                                                 double lengthEffectFactor)
        {
            CheckValidLengthEffectFactor(lengthEffectFactor);
            return sectionProbability / lengthEffectFactor;
        }

        private EAnalysisState GetAnalysisState(ESectionInitialMechanismProbabilitySpecification relevance, ERefinementStatus refinementStatus)
        {
            switch (relevance)
            {
                case ESectionInitialMechanismProbabilitySpecification.NotRelevant:
                    return EAnalysisState.NotRelevant;
                case ESectionInitialMechanismProbabilitySpecification.RelevantNoProbabilitySpecification:
                case ESectionInitialMechanismProbabilitySpecification.RelevantWithProbabilitySpecification:
                    switch (refinementStatus)
                    {
                        case ERefinementStatus.NotNecessary:
                            return relevance == ESectionInitialMechanismProbabilitySpecification
                                       .RelevantNoProbabilitySpecification
                                       ? EAnalysisState.NoProbabilityEstimationNecessary
                                       : EAnalysisState.ProbabilityEstimated;
                        case ERefinementStatus.Necessary:
                            return EAnalysisState.ProbabilityEstimationNecessary;
                        case ERefinementStatus.Performed:
                            return EAnalysisState.ProbabilityEstimated;
                        default:
                            throw new AssemblyException(nameof(refinementStatus), EAssemblyErrors.InvalidEnumValue);
                    }
                default:
                    throw new AssemblyException(nameof(refinementStatus), EAssemblyErrors.InvalidEnumValue);
            }
        }

        private static void CheckValidLengthEffectFactor(double lengthEffectFactor)
        {
            if (lengthEffectFactor < 1)
            {
                throw new AssemblyException(nameof(lengthEffectFactor), EAssemblyErrors.LengthEffectFactorOutOfRange);
            }
        }
    }
}