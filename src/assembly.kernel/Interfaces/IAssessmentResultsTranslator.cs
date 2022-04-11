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

using Assembly.Kernel.Exceptions;
using Assembly.Kernel.Model;
using Assembly.Kernel.Model.Categories;
using Assembly.Kernel.Model.FailureMechanismSections;

namespace Assembly.Kernel.Interfaces
{
    /// <summary>
    /// Translate assessment results to a failure mechanism section assessment result.
    /// </summary>
    public interface IAssessmentResultsTranslator
    {
        /// <summary>
        /// Translate the assessment result of failure mechanism section assessments to a 
        /// single normative result.
        /// </summary>
        /// <param name="relevance">Relevance state for the failure mechanism section that is being analyzed.</param>
        /// <param name="probabilityInitialMechanismSection">Probability of failure estimation of the initial mechanism
        /// for the failure mechanism section that is being analyzed.</param>
        /// <param name="refinementStatus">Refinement status of the failure mechanism section that is being analyzed.</param>
        /// <param name="refinedProbabilitySection">Probability of failure estimation after refinement for the failure
        /// mechanism section that is being analyzed.</param>
        /// <param name="categories">List of <seealso cref="InterpretationCategory"/> to translate a probability to
        /// an <seealso cref="EInterpretationCategory"/>.</param>
        /// <returns>A new result resembling the normative result of the input parameters.</returns>
        /// <exception cref="AssemblyException">Thrown when <paramref name="categories"/> equals null.</exception>
        /// <exception cref="AssemblyException">Thrown when <paramref name="refinementStatus"/> equals
        /// <seealso cref="ERefinementStatus.Performed"/> and <seealso cref="Probability.Undefined"/> of
        /// <paramref name="refinedProbabilitySection"/> is true.</exception>
        /// <exception cref="AssemblyException">Thrown when <paramref name="relevance"/> equals
        /// <seealso cref="ESectionInitialMechanismProbabilitySpecification.RelevantWithProbabilitySpecification"/>
        /// and <seealso cref="Probability.Undefined"/> of <paramref name="probabilityInitialMechanismSection"/> is true.</exception>
        FailureMechanismSectionAssemblyResult TranslateAssessmentResultAggregatedMethod(
            ESectionInitialMechanismProbabilitySpecification relevance,
            Probability probabilityInitialMechanismSection,
            ERefinementStatus refinementStatus,
            Probability refinedProbabilitySection,
            CategoriesList<InterpretationCategory> categories);

        /// <summary>
        /// Translate the assessment result of failure mechanism section assessments to a 
        /// single normative result.
        /// </summary>
        /// <param name="relevance">Relevance state for the failure mechanism section that is being analyzed.</param>
        /// <param name="probabilityInitialMechanismProfile">Probability of failure estimation of the initial mechanism
        /// for a representative profile in the failure mechanism section that is being analyzed.</param>
        /// <param name="probabilityInitialMechanismSection">Probability of failure estimation of the initial mechanism
        /// for the failure mechanism section that is being analyzed.</param>
        /// <param name="refinementStatus">Refinement status of the failure mechanism section that is being analyzed.</param>
        /// <param name="refinedProbabilityProfile">Probability of failure estimation after refinement for a representative
        /// profile in the failure mechanism section that is being analyzed.</param>
        /// <param name="refinedProbabilitySection">Probability of failure estimation after refinement for the failure
        /// mechanism section that is being analyzed.</param>
        /// <param name="categories">List of <seealso cref="InterpretationCategory"/> to translate a probability to an
        /// <seealso cref="EInterpretationCategory"/>.</param>
        /// <returns>A new result resembling the normative result of the input parameters.</returns>
        /// <exception cref="AssemblyException">Thrown when <paramref name="categories"/> equals null.</exception>
        /// <exception cref="AssemblyException">Thrown when <paramref name="refinementStatus"/> equals
        /// <seealso cref="ERefinementStatus.Performed"/> and <seealso cref="Probability.Undefined"/> of
        /// <paramref name="refinedProbabilitySection"/> or <paramref name="refinedProbabilityProfile"/> is true.</exception>
        /// <exception cref="AssemblyException">Thrown when <paramref name="relevance"/> equals
        /// <seealso cref="ESectionInitialMechanismProbabilitySpecification.RelevantWithProbabilitySpecification"/>
        /// and <seealso cref="Probability.Undefined"/> of <paramref name="probabilityInitialMechanismSection"/> or
        /// <paramref name="probabilityInitialMechanismProfile"/> is true.</exception>
        FailureMechanismSectionAssemblyResultWithLengthEffect TranslateAssessmentResultWithLengthEffectAggregatedMethod(
            ESectionInitialMechanismProbabilitySpecification relevance,
            Probability probabilityInitialMechanismProfile,
            Probability probabilityInitialMechanismSection,
            ERefinementStatus refinementStatus,
            Probability refinedProbabilityProfile,
            Probability refinedProbabilitySection,
            CategoriesList<InterpretationCategory> categories);

        /// <summary>
        /// Determines the representative probability for a section based on estimations
        /// of the initial mechanism and further analysis.
        /// </summary>
        /// <param name="refinementNecessary">Indicates whether further refinement was necessary.</param>
        /// <param name="probabilityInitialMechanismSection">The probability estimation of the initial mechanism.</param>
        /// <param name="refinedProbabilitySection">The probability estimation after further refinement.</param>
        /// <returns>The representative probability.</returns>
        Probability DetermineRepresentativeProbabilityBoi0A1(
            bool refinementNecessary,
            Probability probabilityInitialMechanismSection,
            Probability refinedProbabilitySection
            );

        /// <summary>
        /// This method determines the representative probabilities of a failure mechanism section.
        /// </summary>
        /// <param name="refinementNecessary">Indicates whether refinement of the failure probability was/is necessary.</param>
        /// <param name="probabilityInitialMechanismProfile">The probability estimation based on the initial mechanism for a profile.</param>
        /// <param name="probabilityInitialMechanismSection">The probability estimation based on the initial mechanism for a section.</param>
        /// <param name="refinedProbabilityProfile">The refined probability estimation for a profile.</param>
        /// <param name="refinedProbabilitySection">The refined probability estimation for a profile.</param>
        /// <returns>The representative probabilities for a profile and section.</returns>
        /// <exception cref="AssemblyException">Thrown in case just one of <paramref name="probabilityInitialMechanismProfile"/> and
        /// <paramref name="probabilityInitialMechanismSection"/> is undefined.</exception>
        /// <exception cref="AssemblyException">Thrown in case just one of <paramref name="refinedProbabilityProfile"/> or
        /// <paramref name="refinedProbabilitySection"/> is undefined.</exception>
        /// <exception cref="AssemblyException">Thrown in case <paramref name="refinedProbabilityProfile"/> &gt; <paramref name="refinedProbabilitySection"/>.</exception>
        /// <exception cref="AssemblyException">Thrown in case <paramref name="probabilityInitialMechanismProfile"/> &gt; <paramref name="probabilityInitialMechanismSection"/>.</exception>
        IProfileAndSectionProbabilities DetermineRepresentativeProbabilitiesBoi0A2(
            bool refinementNecessary,
            Probability probabilityInitialMechanismProfile,
            Probability probabilityInitialMechanismSection,
            Probability refinedProbabilityProfile,
            Probability refinedProbabilitySection
            );

        /// <summary>
        /// Returns the correct interpretation category that is associated with the specified probability (based on the specified categories).
        /// </summary>
        /// <param name="sectionProbability">The probability for which an interpretation category needs to be found.</param>
        /// <param name="categories">The list of categories and category boundaries.</param>
        /// <returns>The interpretation category associated with the specified probability.</returns>
        /// <exception cref="AssemblyException">Thrown if <paramref name="categories"/> equals null.</exception>
        EInterpretationCategory DetermineInterpretationCategoryFromFailureMechanismSectionProbabilityBoi0B1(
            Probability sectionProbability, 
            CategoriesList<InterpretationCategory> categories
            );

        /// <summary>
        /// Translates the analysis state to the associated interpretation category.
        /// </summary>
        /// <param name="analysisState">The state of the analysis.</param>
        /// <returns>The associated interpretation category.</returns>
        /// <exception cref="AssemblyException">Thrown in case of an invalid or unsupported enum value for <paramref name="analysisState"/>.</exception>
        EInterpretationCategory DetermineInterpretationCategoryWithoutProbabilityEstimationBoi0C1(
            EAnalysisState analysisState
            );

        /// <summary>
        /// Translates interpretation categories without association with a probability range to default probabilities.
        /// </summary>
        /// <param name="category">The interpretation category to translate.</param>
        /// <returns>The failure probability associated with the interpretation category.</returns>
        /// <exception cref="AssemblyException">Thrown in case of an unsupported value for <paramref name="category"/>
        /// (a category that is associated with a probability range or an invalid enum value).</exception>
        Probability TranslateInterpretationCategoryToProbabilityBoi0C2(EInterpretationCategory category);

        /// <summary>
        /// Calculates a section failure probability given the failure probability of a representative profile and the length effect factor.
        /// </summary>
        /// <param name="profileProbability">The failure probability of a representative profile.</param>
        /// <param name="lengthEffectFactor">Length effect factor.</param>
        /// <returns>The calculated probability of failure of the failure mechanism section.</returns>
        Probability CalculateProfileProbabilityToSectionProbabilityBoi0D1(Probability profileProbability, double lengthEffectFactor);

        /// <summary>
        /// Calculates a failure probability of a representative profile given the failure probability of a failure mechanism section and the length effect factor.
        /// </summary>
        /// <param name="sectionProbability">The failure probability of a failure mechanism section.</param>
        /// <param name="lengthEffectFactor">Length effect factor.</param>
        /// <returns>The calculated probability of failure of the representative profile for a failure mechanism section.</returns>
        Probability CalculateSectionProbabilityToProfileProbabilityBoi0D2(Probability sectionProbability, double lengthEffectFactor);

    }
}