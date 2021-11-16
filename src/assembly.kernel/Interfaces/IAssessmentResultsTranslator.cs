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
using Assembly.Kernel.Model.AssessmentResultTypes;
using Assembly.Kernel.Model.CategoryLimits;
using Assembly.Kernel.Model.FmSectionTypes;

namespace Assembly.Kernel.Interfaces
{
    /// <summary>
    /// Translate assessment results to an failure mechanism section assessment result.
    /// </summary>
    public interface IAssessmentResultsTranslator
    {
        /// <summary>
        /// Translate an assessment result to an Failure mechanism category as specified by WBI-0G-1.
        /// </summary>
        /// <param name="assessment">The assessment result to translate.</param>
        /// <returns>The Failure mechanism category belonging to the assessment result.</returns>
        /// <exception cref="AssemblyException">Thrown when input is not valid for this assembly method.</exception>
        FmSectionAssemblyDirectResult TranslateAssessmentResultWbi0G1(EAssessmentResultTypeG1 assessment);

        /// <summary>
        /// Translate an assessment result to an Failure mechanism category as specified by WBI-0T-1.
        /// </summary>
        /// <param name="assessment">The assessment result to translate.</param>
        /// <returns>The Failure mechanism category belonging to the assessment result.</returns>
        /// <exception cref="AssemblyException">Thrown when input is not valid for this assembly method.</exception>
        FmSectionAssemblyDirectResult TranslateAssessmentResultWbi0T1(EAssessmentResultTypeT1 assessment);

        /// <summary>
        /// Translate an assessment result to an Failure mechanism category as specified by WBI-0G-2.
        /// </summary>
        /// <param name="assessment">The assessment result to translate.</param>
        /// <returns>The Failure mechanism category belonging to the assessment result.</returns>
        /// <exception cref="AssemblyException">Thrown when input is not valid for this assembly method.</exception>
        FmSectionAssemblyIndirectResult TranslateAssessmentResultWbi0G2(EAssessmentResultTypeG1 assessment);

        /// <summary>
        /// Translate an assessment result to an Failure mechanism category as specified by WBI-0T-2.
        /// </summary>
        /// <param name="assessment">The assessment result to translate.</param>
        /// <returns>The Failure mechanism category belonging to the assessment result.</returns>
        /// <exception cref="AssemblyException">Thrown when input is not valid for this assembly method.</exception>
        FmSectionAssemblyIndirectResult TranslateAssessmentResultWbi0T2(EAssessmentResultTypeT2 assessment);

        /// <summary>
        /// Translate an assessment result or failure mechanism category to an Failure mechanism category
        /// as specified by WBI-0G-4.
        /// </summary>
        /// <param name="assessment">The assessment result to translate.</param>
        /// <param name="category">The failure mechanism category to use when 
        /// assessment == AssessmentCategorySpecified otherwise this field is ignored.</param>
        /// <returns>The Failure mechanism category belonging to the assessment result.</returns>
        /// <exception cref="AssemblyException">Thrown when input is not valid for this assembly method.</exception>
        FmSectionAssemblyDirectResult TranslateAssessmentResultWbi0G4(EAssessmentResultTypeG2 assessment,
                                                                      EFmSectionCategory? category);

        /// <summary>
        /// Translate an assessment result or failure mechanism category to an Failure mechanism category 
        /// as specified by WBI-0T-4.
        /// </summary>
        /// <param name="assessment">The assessment result to translate.</param>
        /// <param name="category">The failure mechanism category to use when 
        /// assessment == AssessmentCategorySpecified otherwise this field is ignored.</param>
        /// <returns>The Failure mechanism category belonging to the assessment result.</returns>
        /// <exception cref="AssemblyException">Thrown when input is not valid for this assembly method.</exception>
        FmSectionAssemblyDirectResult TranslateAssessmentResultWbi0T4(EAssessmentResultTypeT3 assessment,
                                                                      EFmSectionCategory? category);

        /// <summary>
        /// Translate a list of category compliancy results to one failure mechanism section result
        /// as specified in WBI-0G-6.
        /// </summary>
        /// <param name="compliancyResults">The failure mechanism category limit compliancy results.</param>
        /// <returns>The failure mechanism category distilled from the list of compliancy results.</returns>
        /// <exception cref="AssemblyException">Thrown when:<br/>
        /// - compliancyResults == null<br/>
        /// - compliancyResults list does not contain 5 entries<br/>
        /// - Not all mandatory categories are present<br/>
        /// - A Does not comply is present for a lower category.</exception>
        FmSectionAssemblyDirectResult TranslateAssessmentResultWbi0G6(
            FmSectionCategoryCompliancyResults compliancyResults);

        /// <summary>
        /// Translate a list of category compliancy results to one failure mechanism section result
        /// as specified in WBI-0T-6.
        /// </summary>
        /// <param name="compliancyResults">The failure mechanism category limit compliancy results, 
        /// should be null or an empty list when assessment result is filled.</param>
        /// <param name="assessment">The assessment result of the section, will be ignored when 
        /// compliancy results are filled. May be null when <paramref name="compliancyResults"/> are present.</param>
        /// <returns>The failure mechanism category distilled from the list of compliancy results.</returns>
        /// <exception cref="AssemblyException">Thrown when:<br/>
        /// - compliancyResults == null<br/>
        /// - compliancyResults list does not contain 5 entries<br/>
        /// - Not all mandatory categories are present<br/>
        /// - A Does not comply is present for a lower category.</exception>
        FmSectionAssemblyDirectResult TranslateAssessmentResultWbi0T6(
            FmSectionCategoryCompliancyResults compliancyResults, EAssessmentResultTypeT3 assessment);

        /// <summary>
        /// Translate an assessment result with failure probability to an failure mechanism result
        /// including failure probability as specified in WBI-0G-3.
        /// </summary>
        /// <param name="assessment">The assessment result to check.</param>
        /// <param name="failureProbability">The failure probability if assessment == FailureProbabilitySpecified.
        /// This field may be Double.NaN when it is in the state of "No result yet".</param>
        /// <param name="categories">Categories list that should be used when determining the category based on the entered failureProbability.</param>
        /// <returns>The failure mechanism category belonging to the failure probability or assessment result.</returns>
        /// <exception cref="AssemblyException">Thrown when input is not valid for this assembly method.</exception>
        FmSectionAssemblyDirectResultWithProbability TranslateAssessmentResultWbi0G3(
            EAssessmentResultTypeG2 assessment,
            double failureProbability,
            CategoriesList<FmSectionCategory> categories);

        /// <summary>
        /// Translate an assessment result with failure probability to an failure mechanism result
        /// including failure probability as specified in WBI-0G-5.
        /// </summary>
        /// <param name="fmSectionLengthEffectFactor">The length effect factor of the failure mechanism section.</param>
        /// <param name="assessment">The assessment result to check.</param>
        /// <param name="failureProbability">The failure probability if assessment == FailureProbabilitySpecified.
        ///     This field may be Double.NaN when it is in the state of "No result yet", if the resulting failure probability 
        ///     is greater than 1.0 it will be maximized to 1.0 </param>
        /// <param name="categories">Categories list that should be used when determining the category based on the entered failureProbability.</param>
        /// <returns>The failure mechanism category belonging to the failure probability or assessment result.</returns>
        /// <exception cref="AssemblyException">Thrown when input is not valid for this assembly method.</exception>
        FmSectionAssemblyDirectResultWithProbability TranslateAssessmentResultWbi0G5(
            double fmSectionLengthEffectFactor,
            EAssessmentResultTypeG2 assessment,
            double failureProbability,
            CategoriesList<FmSectionCategory> categories);

        /// <summary>
        /// Translate an assessment result with failure probability to an failure mechanism result
        /// including failure probability as specified in WBI-0T-3.
        /// </summary>
        /// <param name="assessment">The assessment result to check.</param>
        /// <param name="failureProbability">The failure probability if assessment == FailureProbabilitySpecified.
        ///     This field may be Double.NaN when it is in the state of "No result yet" </param>
        /// <param name="categories">Categories list that should be used when determining the category based on the entered failureProbability.</param>
        /// <returns>The failure mechanism category belonging to the failure probability or assessment result.</returns>
        /// <exception cref="AssemblyException">Thrown when input is not valid for this assembly method.</exception>
        FmSectionAssemblyDirectResultWithProbability TranslateAssessmentResultWbi0T3(
            EAssessmentResultTypeT3 assessment,
            double failureProbability,
            CategoriesList<FmSectionCategory> categories);

        /// <summary>
        /// Translate an assessment result with failure probability to an failure mechanism result
        /// including failure probability as specified in WBI-0T-5.
        /// </summary>
        /// <param name="fmSectionLengthEffectFactor">The length effect factor of the failure mechanism section.</param>
        /// <param name="assessment">The assessment result to check.</param>
        /// <param name="failureProbability">The failure probability if assessment == FailureProbabilitySpecified.
        ///     This field may be Double.NaN when it is in the state of "No result yet", if the resulting failure probability 
        ///     is greater than 1.0 it will be maximized to 1.0 </param>
        /// <param name="categories">Categories list that should be used when determining the category based on the entered failureProbability.</param>
        /// <returns>The failure mechanism category belonging to the failure probability or assessment result.</returns>
        /// <exception cref="AssemblyException">Thrown when input is not valid for this assembly method.</exception>
        FmSectionAssemblyDirectResultWithProbability TranslateAssessmentResultWbi0T5(
            double fmSectionLengthEffectFactor,
            EAssessmentResultTypeT3 assessment,
            double failureProbability,
            CategoriesList<FmSectionCategory> categories);

        /// <summary>
        /// Translate an assessment result with failure probability to an failure mechanism result
        /// including failure probability as specified in WBI-0T-7.
        /// </summary>
        /// <param name="assessment">The assessment result to check.</param>
        /// <param name="failureProbability">The failure probability if assessment == FailureProbabilitySpecified.
        /// This field may be Double.NaN when it is in the state of "No result yet" </param>
        /// <param name="categories">The list with categories that should be used when translating a probability to a category.</param>
        /// <returns>The failure mechanism category belonging to the failure probability or assessment result.</returns>
        /// <exception cref="AssemblyException">Thrown when input is not valid for this assembly method.</exception>
        FmSectionAssemblyDirectResult TranslateAssessmentResultWbi0T7(
            EAssessmentResultTypeT4 assessment,
            double failureProbability,
            CategoriesList<FmSectionCategory> categories);

        /// <summary>
        /// Translate the assessment result of failure mechanism section assessments (direct without probability) to a 
        /// single normative result. As specified in WBI-0A-1.
        /// </summary>
        /// <param name="detailedAssessmentResult">The test result of a detailed assessment. 
        /// May be null when not available.</param>
        /// <param name="customAssessmentResult">The test result of a custom assessment.
        /// May be null when not available.</param>
        /// <returns>A new result resambling the normative result of the three input parameters.</returns>
        /// <exception cref="AssemblyException">Thrown when simpleAssessmentResult == null.</exception>
        FmSectionAssemblyDirectResult TranslateAssessmentResultWbi0A1(
            FmSectionAssemblyDirectResult detailedAssessmentResult,
            FmSectionAssemblyDirectResult customAssessmentResult);

        /// <summary>
        /// Translate the assessment result of failure mechanism section assessments (direct with probability) to a 
        /// single normative result. As specified in WBI-0A-1.
        /// </summary>
        /// <param name="detailedAssessmentResult">The test result of a detailed assessment. 
        /// May be null when not available.</param>
        /// <param name="customAssessmentResult">The test result of a custom assessment.
        /// May be null when not available.</param>
        /// <returns>A new result resambling the normative result of the three input parameters.</returns>
        /// <exception cref="AssemblyException">Thrown when simpleAssessmentResult == null</exception>
        FmSectionAssemblyDirectResultWithProbability TranslateAssessmentResultWbi0A1(
            FmSectionAssemblyDirectResultWithProbability detailedAssessmentResult,
            FmSectionAssemblyDirectResultWithProbability customAssessmentResult);

        /// <summary>
        /// Translate the assessment result of failure mechanism section assessments (indirect) to a 
        /// single normative result. As specified in WBI-0A-1.
        /// </summary>
        /// <param name="detailedAssessmentResult">The test result of a detailed assessment. 
        /// May be null when not available.</param>
        /// <param name="customAssessmentResult">The test result of a custom assessment.
        /// May be null when not available.</param>
        /// <returns>A new result resambling the normative result of the three input parameters.</returns>
        /// <exception cref="AssemblyException">Thrown when simpleAssessmentResult == null</exception>
        FmSectionAssemblyIndirectResult TranslateAssessmentResultWbi0A1(
            FmSectionAssemblyIndirectResult detailedAssessmentResult,
            FmSectionAssemblyIndirectResult customAssessmentResult);
    }
}