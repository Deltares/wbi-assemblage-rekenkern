#region Copyright (C) Rijkswaterstaat 2022. All rights reserved

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
using System.Linq;
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
        public FailureMechanismSectionWithAssemblyResult TranslateAssessmentResultAggregatedMethod(
            ESectionInitialMechanismProbabilitySpecification relevance, 
            Probability probabilityInitialMechanismSection,
            ERefinementStatus refinementStatus, 
            Probability refinedProbabilitySection, 
            CategoriesList<InterpretationCategory> categories)
        {
            return TranslateAssessmentResultAggregatedMethod(
                relevance,
                probabilityInitialMechanismSection,
                probabilityInitialMechanismSection,
                refinementStatus,
                refinedProbabilitySection,
                refinedProbabilitySection,
                categories);
        }

        /// <inheritdoc />
        public FailureMechanismSectionWithAssemblyResult TranslateAssessmentResultAggregatedMethod(
            ESectionInitialMechanismProbabilitySpecification relevance,
            Probability probabilityInitialMechanismProfile,
            Probability probabilityInitialMechanismSection,
            ERefinementStatus refinementStatus,
            Probability refinedProbabilityProfile,
            Probability refinedProbabilitySection,
            CategoriesList<InterpretationCategory> categories)
        {
            if (categories == null)
            {
                throw new AssemblyException(nameof(categories), EAssemblyErrors.ValueMayNotBeNull);
            }

            if (relevance == ESectionInitialMechanismProbabilitySpecification.NotRelevant)
            {
                return new FailureMechanismSectionWithAssemblyResult(new Probability(0.0), new Probability(0.0), EInterpretationCategory.NotRelevant);
            }

            switch (refinementStatus)
            {
                case ERefinementStatus.Performed:
                    if (!refinedProbabilitySection.IsDefined)
                    {
                        throw new AssemblyException(nameof(refinedProbabilitySection), EAssemblyErrors.ProbabilityMayNotBeUndefined);
                    }
                    if (!refinedProbabilityProfile.IsDefined)
                    {
                        throw new AssemblyException(nameof(refinedProbabilityProfile), EAssemblyErrors.ProbabilityMayNotBeUndefined);
                    }

                    CheckProbabilityRatio(refinedProbabilityProfile, refinedProbabilitySection);

                    var refinedCategory = categories.GetCategoryForFailureProbability(refinedProbabilitySection).Category;
                    return new FailureMechanismSectionWithAssemblyResult(refinedProbabilityProfile, refinedProbabilitySection, refinedCategory);
                case ERefinementStatus.Necessary:
                    return new FailureMechanismSectionWithAssemblyResult(Probability.Undefined, Probability.Undefined, EInterpretationCategory.Dominant);
                default:
                    switch (relevance)
                    {
                        case ESectionInitialMechanismProbabilitySpecification.RelevantNoProbabilitySpecification:
                            return new FailureMechanismSectionWithAssemblyResult((Probability) 0.0, (Probability) 0.0, EInterpretationCategory.NotDominant);
                        default:
                            if (!probabilityInitialMechanismSection.IsDefined || !probabilityInitialMechanismProfile.IsDefined)
                            {
                                throw new AssemblyException("probabilityInitialMechanism", EAssemblyErrors.ProbabilityMayNotBeUndefined);
                            }

                            CheckProbabilityRatio(probabilityInitialMechanismProfile, probabilityInitialMechanismSection);

                            var categoryInitialMechanism = categories.GetCategoryForFailureProbability(probabilityInitialMechanismSection).Category;
                            return new FailureMechanismSectionWithAssemblyResult(probabilityInitialMechanismProfile, probabilityInitialMechanismSection, categoryInitialMechanism);
                    }
            }
        }

        /// <inheritdoc />
        public Probability DetermineRepresentativeProbabilityBoi0A1(bool refinementNecessary,
            Probability probabilityInitialMechanismSection, Probability refinedProbabilitySection)
        {
            return refinementNecessary ? refinedProbabilitySection : probabilityInitialMechanismSection;
        }

        /// <inheritdoc />
        public IProfileAndSectionProbabilities DetermineRepresentativeProbabilitiesBoi0A2(bool refinementNecessary,
            Probability probabilityInitialMechanismProfile, 
            Probability probabilityInitialMechanismSection,
            Probability refinedProbabilityProfile, 
            Probability refinedProbabilitySection)
        {
            var errors = new List<AssemblyErrorMessage>();
            if (probabilityInitialMechanismProfile.IsDefined != probabilityInitialMechanismSection.IsDefined)
            {
                errors.Add(new AssemblyErrorMessage("probabilitiesInitialMechanism", EAssemblyErrors.ProbabilitiesShouldEitherBothBeDefinedOrUndefined));
            }

            if (probabilityInitialMechanismProfile.IsDefined &&
                probabilityInitialMechanismProfile > probabilityInitialMechanismSection)
            {
                errors.Add(new AssemblyErrorMessage("probabilitiesInitialMechanism", EAssemblyErrors.ProfileProbabilityGreaterThanSectionProbability));
            }

            if (refinedProbabilityProfile.IsDefined != refinedProbabilitySection.IsDefined)
            {
                errors.Add(new AssemblyErrorMessage("refinedProbabilities", EAssemblyErrors.ProbabilitiesShouldEitherBothBeDefinedOrUndefined));
            }

            if (refinedProbabilityProfile.IsDefined &&
                refinedProbabilityProfile > refinedProbabilitySection)
            {
                errors.Add(new AssemblyErrorMessage("refinedProbabilities", EAssemblyErrors.ProfileProbabilityGreaterThanSectionProbability));
            }

            if (errors.Any())
            {
                throw new AssemblyException(errors);
            }

            return refinementNecessary ? 
                new ResultWithProfileAndSectionProbabilities(refinedProbabilityProfile, refinedProbabilitySection) :
                new ResultWithProfileAndSectionProbabilities(probabilityInitialMechanismProfile, probabilityInitialMechanismSection);
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

        private static void CheckProbabilityRatio(Probability profileProbability, Probability sectionProbability)
        {
            if (sectionProbability < profileProbability)
            {
                throw new AssemblyException("AssemblyResultsTranslator", EAssemblyErrors.ProfileProbabilityGreaterThanSectionProbability);
            }
        }
    }
}