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
        public FailureMechanismSectionAssemblyResult TranslateAssessmentResultWbi0A2(
            ESectionInitialMechanismProbabilitySpecification relevance, 
            Probability probabilityInitialMechanismSection,
            ERefinementStatus refinementStatus, 
            Probability refinedProbabilitySection, 
            CategoriesList<InterpretationCategory> categories)
        {
            return TranslateAssessmentResultWbi0A2(
                relevance,
                probabilityInitialMechanismSection,
                probabilityInitialMechanismSection,
                refinementStatus,
                refinedProbabilitySection,
                refinedProbabilitySection,
                categories);
        }

        /// <inheritdoc />
        public FailureMechanismSectionAssemblyResult TranslateAssessmentResultWbi0A2(
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
                throw new AssemblyException("AssemblyResultsTranslator", EAssemblyErrors.ValueMayNotBeNull);
            }

            if (relevance == ESectionInitialMechanismProbabilitySpecification.NotRelevant)
            {
                return new FailureMechanismSectionAssemblyResult(new Probability(0.0), new Probability(0.0), EInterpretationCategory.NotRelevant);
            }

            switch (refinementStatus)
            {
                case ERefinementStatus.Performed:
                    if (!refinedProbabilitySection.IsDefined || !refinedProbabilityProfile.IsDefined)
                    {
                        throw new AssemblyException("refinedProbability", EAssemblyErrors.ProbabilityMayNotBeUndefined);
                    }
                    CheckProbabilityRatio(refinedProbabilityProfile, refinedProbabilitySection);

                    var refinedCategory = categories.GetCategoryForFailureProbability(refinedProbabilitySection).Category;
                    return new FailureMechanismSectionAssemblyResult(refinedProbabilityProfile, refinedProbabilitySection, refinedCategory);
                case ERefinementStatus.Necessary:
                    return new FailureMechanismSectionAssemblyResult(Probability.UndefinedProbability, Probability.UndefinedProbability, EInterpretationCategory.Dominant);
                default:
                    switch (relevance)
                    {
                        case ESectionInitialMechanismProbabilitySpecification.RelevantNoProbabilitySpecification:
                            return new FailureMechanismSectionAssemblyResult(Probability.UndefinedProbability, Probability.UndefinedProbability, EInterpretationCategory.NotDominant);
                        default:
                            if (double.IsNaN(probabilityInitialMechanismSection) || double.IsNaN(probabilityInitialMechanismProfile))
                            {
                                throw new AssemblyException("probabilityInitialMechanism", EAssemblyErrors.ProbabilityMayNotBeUndefined);
                            }

                            CheckProbabilityRatio(probabilityInitialMechanismProfile, probabilityInitialMechanismSection);

                            var categoryInitialMechanism = categories.GetCategoryForFailureProbability(probabilityInitialMechanismSection).Category;
                            return new FailureMechanismSectionAssemblyResult(probabilityInitialMechanismProfile, probabilityInitialMechanismSection, categoryInitialMechanism);
                    }
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