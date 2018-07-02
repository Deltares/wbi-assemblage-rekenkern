#region Copyright (c) 2018 Technolution BV. All Rights Reserved. 

// // Copyright (C) Technolution BV. 2018. All rights reserved.
// //
// // This file is part of the Assembly kernel.
// //
// // Ringtoets is free software: you can redistribute it and/or modify
// // it under the terms of the GNU Lesser General Public License as published by
// // the Free Software Foundation, either version 3 of the License, or
// // (at your option) any later version.
// // 
// // This program is distributed in the hope that it will be useful,
// // but WITHOUT ANY WARRANTY; without even the implied warranty of
// // MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// // GNU Lesser General Public License for more details.
// //
// // You should have received a copy of the GNU Lesser General Public License
// // along with this program. If not, see <http://www.gnu.org/licenses/>.
// //
// // All names, logos, and references to "Technolution BV" are registered trademarks of
// // Technolution BV and remain full property of Technolution BV at all times.
// // All rights reserved.

#endregion

using Assembly.Kernel.Exceptions;
using Assembly.Kernel.Model;
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
        /// Translate an assessment result to an Failure mecahnism category as specified by WBI-0E-1
        /// </summary>
        /// <param name="assessment">The assessement result to translate</param>
        /// <returns>The Failure mechanism category belonging to the assessment result</returns>
        /// <exception cref="AssemblyException">Thrown when input is not valid for this assembly method</exception>
        FmSectionAssemblyDirectResultWithProbability TranslateAssessmentResultWbi0E1(EAssessmentResultTypeE1 assessment);

        /// <summary>
        /// Translate an assessment result to an Failure mecahnism category as specified by WBI-0E-3
        /// </summary>
        /// <param name="assessment">The assessement result to translate</param>
        /// <returns>The Failure mechanism category belonging to the assessment result</returns>
        /// <exception cref="AssemblyException">Thrown when input is not valid for this assembly method</exception>
        FmSectionAssemblyDirectResultWithProbability TranslateAssessmentResultWbi0E3(EAssessmentResultTypeE2 assessment);

        /// <summary>
        /// Translate an assessment result to an Failure mecahnism category as specified by WBI-0G-1
        /// </summary>
        /// <param name="assessment">The assessement result to translate</param>
        /// <returns>The Failure mechanism category belonging to the assessment result</returns>
        /// <exception cref="AssemblyException">Thrown when input is not valid for this assembly method</exception>
        FmSectionAssemblyDirectResult TranslateAssessmentResultWbi0G1(EAssessmentResultTypeG1 assessment);

        /// <summary>
        /// Translate an assessment result to an Failure mecahnism category as specified by WBI-0T-1
        /// </summary>
        /// <param name="assessment">The assessement result to translate</param>
        /// <returns>The Failure mechanism category belonging to the assessment result</returns>
        /// <exception cref="AssemblyException">Thrown when input is not valid for this assembly method</exception>
        FmSectionAssemblyDirectResult TranslateAssessmentResultWbi0T1(EAssessmentResultTypeT1 assessment);

        /// <summary>
        /// Translate an assessment result to an Failure mecahnism category as specified by WBI-0E-2
        /// </summary>
        /// <param name="assessment">The assessment result to translate</param>
        /// <returns>The Failure mechanism category belonging to the assessment result</returns>
        /// <exception cref="AssemblyException">Thrown when input is not valid for this assembly method</exception>
        FmSectionAssemblyIndirectResult TranslateAssessmentResultWbi0E2(EAssessmentResultTypeE1 assessment);

        /// <summary>
        /// Translate an assessment result to an Failure mecahnism category as specified by WBI-0E-4
        /// </summary>
        /// <param name="assessment">The assessment result to translate</param>
        /// <returns>The Failure mechanism category belonging to the assessment result</returns>
        /// <exception cref="AssemblyException">Thrown when input is not valid for this assembly method</exception>
        FmSectionAssemblyIndirectResult TranslateAssessmentResultWbi0E4(EAssessmentResultTypeE2 assessment);

        /// <summary>
        /// Translate an assessment result to an Failure mecahnism category as specified by WBI-0G-2
        /// </summary>
        /// <param name="assessment">The assessment result to translate</param>
        /// <returns>The Failure mechanism category belonging to the assessment result</returns>
        /// <exception cref="AssemblyException">Thrown when input is not valid for this assembly method</exception>
        FmSectionAssemblyIndirectResult TranslateAssessmentResultWbi0G2(EAssessmentResultTypeG1 assessment);

        /// <summary>
        /// Translate an assessment result to an Failure mecahnism category as specified by WBI-0T-2
        /// </summary>
        /// <param name="assessment">The assessment result to translate</param>
        /// <returns>The Failure mechanism category belonging to the assessment result</returns>
        /// <exception cref="AssemblyException">Thrown when input is not valid for this assembly method</exception>
        FmSectionAssemblyIndirectResult TranslateAssessmentResultWbi0T2(EAssessmentResultTypeT2 assessment);

        /// <summary>
        /// Translate an assessment result or failure mechanism category to an Failure mecahnism category 
        /// as specified by WBI-0G-4
        /// </summary>
        /// <param name="assessment">The assessment result to translate</param>
        /// <param name="category">The failure mechanism category to use when 
        /// assessment == AssessmentCategorySpecified otherwise this field is ignored</param>
        /// <returns>The Failure mechanism category belonging to the assessment result</returns>
        /// <exception cref="AssemblyException">Thrown when input is not valid for this assembly method</exception>
        FmSectionAssemblyDirectResult TranslateAssessmentResultWbi0G4(EAssessmentResultTypeG2 assessment,
            EFmSectionCategory? category);

        /// <summary>
        /// Translate an assessment result or failure mechanism category to an Failure mecahnism category 
        /// as specified by WBI-0T-4
        /// </summary>
        /// <param name="assessment">The assessment result to translate</param>
        /// <param name="category">The failure mechanism category to use when 
        /// assessment == AssessmentCategorySpecified otherwise this field is ignored</param>
        /// <returns>The Failure mechanism category belonging to the assessment result</returns>
        /// <exception cref="AssemblyException">Thrown when input is not valid for this assembly method</exception>
        FmSectionAssemblyDirectResult TranslateAssessmentResultWbi0T4(EAssessmentResultTypeT3 assessment,
            EFmSectionCategory? category);

        /// <summary>
        /// Translate a list of category comliancy results to one failure mechanism section result
        ///  as specified in WBI-0G-6.
        /// </summary>
        /// <param name="compliancyResults">The failure mechniasm category limit compliancy results</param>
        /// <returns>The failure mechanism category distilled from the list of compliancy results</returns>
        /// <exception cref="AssemblyException">Thrown when:<br/>
        /// - compliancyResults == null<br/>
        /// - compliancyResults list does not contain 5 entries<br/>
        /// - Not all mandatory categories are present<br/>
        /// - A Does not comply is present for a lower category.</exception>
        FmSectionAssemblyDirectResult TranslateAssessmentResultWbi0G6(
            FmSectionCategoryCompliancyResults compliancyResults);

        /// <summary>
        /// Translate a list of category comliancy results to one failure mechanism section result
        ///  as specified in WBI-0T-6.
        /// </summary>
        /// <param name="compliancyResults">The failure mechniasm category limit compliancy results, 
        /// should be null or an empty list when assessment result is filled</param>
        /// <param name="assessment">The assessment result of the section, will be ignored when 
        /// compliancy results are filled. May be null when compliancyResults are present</param>
        /// <returns>The failure mechanism category distilled from the list of compliancy results</returns>
        /// <exception cref="AssemblyException">Thrown when:<br/>
        /// - compliancyResults == null<br/>
        /// - compliancyResults list does not contain 5 entries<br/>
        /// - Not all mandatory categories are present<br/>
        /// - A Does not comply is present for a lower category.</exception>
        FmSectionAssemblyDirectResult TranslateAssessmentResultWbi0T6(
            FmSectionCategoryCompliancyResults compliancyResults, EAssessmentResultTypeT3 assessment);

        /// <summary>
        /// Translate an assessment result with failure probability to an failure mechanism result
        ///  including failure probability as specified in WBI-0G-3.
        /// </summary>
        /// <param name="section">The assessment section of the assessment</param>
        /// <param name="failureMechanism">The failure mechanism of the assessment</param>
        /// <param name="assessment">The assessment result to check</param>
        /// <param name="failureProbability">The failure probability if assessment == FailureProbabilitySpecified.
        /// This field may be Double.NaN when it is in the state of "No result yet" </param>
        /// <returns>The failure mechanism category belonging to the failure probability or assessment result</returns>
        /// <exception cref="AssemblyException">Thrown when input is not valid for this assembly method</exception>
        FmSectionAssemblyDirectResultWithProbability TranslateAssessmentResultWbi0G3(AssessmentSection section,
            FailureMechanism failureMechanism, EAssessmentResultTypeG2 assessment, double failureProbability);

        /// <summary>
        /// Translate an assessment result with failure probability to an failure mechanism result
        ///  including failure probability as specified in WBI-0G-5.
        /// </summary>
        /// <param name="section">The assessment section of the assessment</param>
        /// <param name="failureMechanism">The failure mechanism of the assessment</param>
        /// <param name="fmSectionLengthEffectFactor">The length effect factor of the failure mechanism section</param>
        /// <param name="assessment">The assessment result to check</param>
        /// <param name="failureProbability">The failure probability if assessment == FailureProbabilitySpecified.
        /// This field may be Double.NaN when it is in the state of "No result yet", if the resulting failure probability 
        /// is greater than 1.0 it will be maximized to 1.0 </param>
        /// <returns>The failure mechanism category belonging to the failure probability or assessment result</returns>
        /// <exception cref="AssemblyException">Thrown when input is not valid for this assembly method</exception>
        FmSectionAssemblyDirectResultWithProbability TranslateAssessmentResultWbi0G5(AssessmentSection section,
            FailureMechanism failureMechanism, double fmSectionLengthEffectFactor, EAssessmentResultTypeG2 assessment,
            double failureProbability);

        /// <summary>
        /// Translate an assessment result with failure probability to an failure mechanism result
        ///  including failure probability as specified in WBI-0T-3.
        /// </summary>
        /// <param name="section">The assessment section of the assessment</param>
        /// <param name="failureMechanism">The failure mechanism of the assessment</param>
        /// <param name="assessment">The assessment result to check</param>
        /// <param name="failureProbability">The failure probability if assessment == FailureProbabilitySpecified.
        /// This field may be Double.NaN when it is in the state of "No result yet" </param>
        /// <returns>The failure mechanism category belonging to the failure probability or assessment result</returns>
        /// <exception cref="AssemblyException">Thrown when input is not valid for this assembly method</exception>
        FmSectionAssemblyDirectResultWithProbability TranslateAssessmentResultWbi0T3(AssessmentSection section,
            FailureMechanism failureMechanism, EAssessmentResultTypeT3 assessment, double failureProbability);

        /// <summary>
        /// Translate an assessment result with failure probability to an failure mechanism result
        ///  including failure probability as specified in WBI-0T-5.
        /// </summary>
        /// <param name="section">The assessment section of the assessment</param>
        /// <param name="failureMechanism">The failure mechanism of the assessment</param>
        /// <param name="fmSectionLengthEffectFactor">The length effect factor of the failure mechanism section</param>
        /// <param name="assessment">The assessment result to check</param>
        /// <param name="failureProbability">The failure probability if assessment == FailureProbabilitySpecified.
        /// This field may be Double.NaN when it is in the state of "No result yet", if the resulting failure probability 
        /// is greater than 1.0 it will be maximized to 1.0 </param>
        /// <returns>The failure mechanism category belonging to the failure probability or assessment result</returns>
        /// <exception cref="AssemblyException">Thrown when input is not valid for this assembly method</exception>
        FmSectionAssemblyDirectResultWithProbability TranslateAssessmentResultWbi0T5(AssessmentSection section,
            FailureMechanism failureMechanism, double fmSectionLengthEffectFactor, EAssessmentResultTypeT3 assessment,
            double failureProbability);

        /// <summary>
        /// Translate an assessment result with failure probability to an failure mechanism result
        ///  including failure probability as specified in WBI-0T-7.
        /// </summary>
        /// <param name="assessment">The assessment result to check</param>
        /// <param name="failureProbability">The failure probability if assessment == FailureProbabilitySpecified.
        /// This field may be Double.NaN when it is in the state of "No result yet" </param>
        /// <param name="categoriesList">The list with categories that should be used when translating a probability to a category.</param>
        /// <returns>The failure mechanism category belonging to the failure probability or assessment result</returns>
        /// <exception cref="AssemblyException">Thrown when input is not valid for this assembly method</exception>
        FmSectionAssemblyDirectResult TranslateAssessmentResultWbi0T7(EAssessmentResultTypeT4 assessment, double failureProbability, CategoriesList<FmSectionCategory> categoriesList);

        /// <summary>
        /// Translate the assessment result of direct failure mechanism section assessments to a 
        /// single normative result. As specified in WBI-0A-1.
        /// </summary>
        /// <param name="simpleAssessmentResult">The test result of a simple assessment. May not be null.</param>
        /// <param name="detailedAssessmentResult">The test result of a detailed assessment. 
        /// May be null when not available</param>
        /// <param name="customAssessmentResult">The test result of a custom assessment.
        /// May be null when not available</param>
        /// <returns>The normative result.</returns>
        /// <exception cref="AssemblyException">Thrown when simpleAssessmentResult == null</exception>
        FmSectionAssemblyDirectResult TranslateAssessmentResultWbi0A1(
            FmSectionAssemblyDirectResult simpleAssessmentResult,
            FmSectionAssemblyDirectResult detailedAssessmentResult,
            FmSectionAssemblyDirectResult customAssessmentResult);

        /// <summary>
        /// Translate the assessment result of direct failure mechanism section assessments including estimation 
        /// of the failure probability to a single normative result. As specified in WBI-0A-1.
        /// </summary>
        /// <param name="simpleAssessmentResult">The test result of a simple assessment. May not be null.</param>
        /// <param name="detailedAssessmentResult">The test result of a detailed assessment. 
        /// May be null when not available</param>
        /// <param name="customAssessmentResult">The test result of a custom assessment.
        /// May be null when not available</param>
        /// <returns>The normative result.</returns>
        /// <exception cref="AssemblyException">Thrown when simpleAssessmentResult == null</exception>
        FmSectionAssemblyDirectResultWithProbability TranslateAssessmentResultWbi0A1(
            FmSectionAssemblyDirectResultWithProbability simpleAssessmentResult,
            FmSectionAssemblyDirectResultWithProbability detailedAssessmentResult,
            FmSectionAssemblyDirectResultWithProbability customAssessmentResult);

        /// <summary>
        /// Translate the assessment result of indirect failure mechanism section assessments to a 
        /// single normative result. As specified in WBI-0A-1.
        /// </summary>
        /// <param name="simpleAssessmentResult">The test result of a simple assessment. May not be null.</param>
        /// <param name="detailedAssessmentResult">The test result of a detailed assessment. 
        /// May be null when not available</param>
        /// <param name="customAssessmentResult">The test result of a custom assessment.
        /// May be null when not available</param>
        /// <returns>The normative result.</returns>
        /// <exception cref="AssemblyException">Thrown when simpleAssessmentResult == null</exception>
        FmSectionAssemblyIndirectResult TranslateAssessmentResultWbi0A1(
            FmSectionAssemblyIndirectResult simpleAssessmentResult,
            FmSectionAssemblyIndirectResult detailedAssessmentResult,
            FmSectionAssemblyIndirectResult customAssessmentResult);
    }
}