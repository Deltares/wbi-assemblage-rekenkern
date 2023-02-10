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
using Assembly.Kernel.Model;
using Assembly.Kernel.Model.Categories;
using Assembly.Kernel.Model.FailureMechanismSections;

namespace Assembly.Kernel.Interfaces
{
    /// <summary>
    /// Interface to translate assessment results to a failure mechanism section assembly result.
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
        /// <param name="categories">List of <see cref="InterpretationCategory"/> to translate a probability to
        /// an <see cref="EInterpretationCategory"/>.</param>
        /// <returns>A new result resembling the normative result of the input parameters.</returns>
        /// <exception cref="AssemblyException">Thrown when <paramref name="categories"/> equals null.</exception>
        /// <exception cref="AssemblyException">Thrown when <paramref name="refinementStatus"/> equals
        /// <see cref="ERefinementStatus.Performed"/> and <see cref="Probability.Undefined"/> of
        /// <paramref name="refinedProbabilitySection"/> is true.</exception>
        /// <exception cref="AssemblyException">Thrown when <paramref name="relevance"/> equals
        /// <see cref="ESectionInitialMechanismProbabilitySpecification.RelevantWithProbabilitySpecification"/>
        /// and <see cref="Probability.Undefined"/> of <paramref name="probabilityInitialMechanismSection"/> is true.</exception>
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
        /// <param name="categories">List of <see cref="InterpretationCategory"/> to translate a probability to an
        /// <see cref="EInterpretationCategory"/>.</param>
        /// <returns>A new result resembling the normative result of the input parameters.</returns>
        /// <exception cref="AssemblyException">Thrown when <paramref name="categories"/> equals null.</exception>
        /// <exception cref="AssemblyException">Thrown when <paramref name="refinementStatus"/> equals
        /// <see cref="ERefinementStatus.Performed"/> in combination with <paramref name="refinedProbabilitySection"/> equals
        /// <see cref="Probability.Undefined"/> or when <paramref name="refinedProbabilityProfile"/> equals true.</exception>
        /// <exception cref="AssemblyException">Thrown when <paramref name="relevance"/> equals
        /// <see cref="ESectionInitialMechanismProbabilitySpecification.RelevantWithProbabilitySpecification"/>
        /// in combination with <paramref name="probabilityInitialMechanismSection"/> equals <see cref="Probability.Undefined"/> or when
        /// <paramref name="probabilityInitialMechanismProfile"/> equalsfixed true.</exception>
        FailureMechanismSectionAssemblyResultWithLengthEffect TranslateAssessmentResultWithLengthEffectAggregatedMethod(
            ESectionInitialMechanismProbabilitySpecification relevance,
            Probability probabilityInitialMechanismProfile,
            Probability probabilityInitialMechanismSection,
            ERefinementStatus refinementStatus,
            Probability refinedProbabilityProfile,
            Probability refinedProbabilitySection,
            CategoriesList<InterpretationCategory> categories);

        /// <summary>
        /// Determines the representative probability for a section.
        /// </summary>
        /// <param name="refinementNecessary">Indicator whether refinement is necessary.</param>
        /// <param name="probabilityInitialMechanismSection">The probability estimation of the initial mechanism.</param>
        /// <param name="refinedProbabilitySection">The refined probability estimation.</param>
        /// <returns>The representative probability.</returns>
        Probability DetermineRepresentativeProbabilityBoi0A1(
            bool refinementNecessary,
            Probability probabilityInitialMechanismSection,
            Probability refinedProbabilitySection);

        /// <summary>
        /// Determines the representative probabilities of a failure mechanism section.
        /// </summary>
        /// <param name="refinementNecessary">Indicator whether refinement is necessary.</param>
        /// <param name="probabilityInitialMechanismProfile">The probability estimation of the initial mechanism for a profile.</param>
        /// <param name="probabilityInitialMechanismSection">The probability estimation of the initial mechanism for a section.</param>
        /// <param name="refinedProbabilityProfile">The refined probability estimation for a profile.</param>
        /// <param name="refinedProbabilitySection">The refined probability estimation for a section.</param>
        /// <returns>A <see cref="ResultWithProfileAndSectionProbabilities"/>.</returns>
        /// <exception cref="AssemblyException">Thrown when:
        /// <list type="bullet">
        /// <item><paramref name="probabilityInitialMechanismProfile"/> is <see cref="Probability.Undefined"/>;</item>
        /// <item><paramref name="probabilityInitialMechanismSection"/> is <see cref="Probability.Undefined"/>;</item>
        /// <item><paramref name="refinedProbabilityProfile"/> is <see cref="Probability.Undefined"/>;</item>
        /// <item><paramref name="refinedProbabilityProfile"/> is <see cref="Probability.Undefined"/>;</item>
        /// <item><paramref name="refinedProbabilityProfile"/> &gt; <paramref name="refinedProbabilitySection"/>;</item>
        /// <item><paramref name="probabilityInitialMechanismProfile"/> &gt; <paramref name="probabilityInitialMechanismSection"/>.</item>
        /// </list>
        /// </exception>
        ResultWithProfileAndSectionProbabilities DetermineRepresentativeProbabilitiesBoi0A2(
            bool refinementNecessary,
            Probability probabilityInitialMechanismProfile,
            Probability probabilityInitialMechanismSection,
            Probability refinedProbabilityProfile,
            Probability refinedProbabilitySection);

        /// <summary>
        /// Determines the interpretation category that is associated with the section probability (based on the specified categories).
        /// </summary>
        /// <param name="sectionProbability">The section probability.</param>
        /// <param name="categories">The categories to use.</param>
        /// <returns>The <see cref="EInterpretationCategory"/> of the failure mechanism section.</returns>
        /// <exception cref="AssemblyException">Thrown when <paramref name="sectionProbability"/> is
        /// <see cref="Probability.Undefined"/> or <paramref name="categories"/> is <c>null</c>.</exception>
        EInterpretationCategory DetermineInterpretationCategoryFromFailureMechanismSectionProbabilityBoi0B1(
            Probability sectionProbability,
            CategoriesList<InterpretationCategory> categories);

        /// <summary>
        /// Determines the interpretation category.
        /// </summary>
        /// <param name="analysisState">The state of the analysis.</param>
        /// <returns>An <see cref="EInterpretationCategory"/>.</returns>
        /// <exception cref="AssemblyException">Thrown when <paramref name="analysisState"/> is not
        /// <see cref="EAnalysisState.NotRelevant"/>, <see cref="EAnalysisState.ProbabilityEstimationNecessary"/>
        /// or <see cref="EAnalysisState.NoProbabilityEstimationNecessary"/>.</exception>
        /// <remarks>
        /// Use method <c>DetermineRepresentativeProbabilityBoi0A1</c> or <c>DetermineRepresentativeProbabilitiesBoi0A2</c>
        /// when <paramref name="analysisState"/> is <see cref="EAnalysisState.ProbabilityEstimated"/>.
        /// </remarks>
        /// <seealso cref="DetermineRepresentativeProbabilityBoi0A1"/>
        /// <seealso cref="DetermineRepresentativeProbabilitiesBoi0A2"/>
        EInterpretationCategory DetermineInterpretationCategoryWithoutProbabilityEstimationBoi0C1(
            EAnalysisState analysisState);

        /// <summary>
        /// Translates interpretation categories to a probability.
        /// </summary>
        /// <param name="category">The interpretation category.</param>
        /// <returns>The failure probability associated with the interpretation category.</returns>
        /// <exception cref="AssemblyException">Thrown when <paramref name="category"/> is invalid or unsupported.exception>
        Probability TranslateInterpretationCategoryToProbabilityBoi0C2(EInterpretationCategory category);

        /// <summary>
        /// Calculates a section failure probability.
        /// </summary>
        /// <param name="profileProbability">The failure probability of a representative profile.</param>
        /// <param name="lengthEffectFactor">The length effect factor.</param>
        /// <returns>The failure probability of the failure mechanism section.</returns>
        /// <exception cref="AssemblyException">Thrown when <paramref name="lengthEffectFactor"/> &lt; 1.</exception>
        Probability CalculateProfileProbabilityToSectionProbabilityBoi0D1(Probability profileProbability, double lengthEffectFactor);

        /// <summary>
        /// Calculates a profile failure probability.
        /// </summary>
        /// <param name="sectionProbability">The failure probability of a failure mechanism section.</param>
        /// <param name="lengthEffectFactor">The length effect factor.</param>
        /// <returns>The failure probability of the representative profile for a failure mechanism section.</returns>
        /// <exception cref="AssemblyException">Thrown when <paramref name="lengthEffectFactor"/> &lt; 1.</exception>
        Probability CalculateSectionProbabilityToProfileProbabilityBoi0D2(Probability sectionProbability, double lengthEffectFactor);
    }
}