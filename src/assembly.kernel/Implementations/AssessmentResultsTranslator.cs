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

using System.Collections.Generic;
using Assembly.Kernel.Exceptions;
using Assembly.Kernel.Interfaces;
using Assembly.Kernel.Model;
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

        /// <summary>
        /// Translates a list of compliancy results to a single result.
        /// </summary>
        /// <param name="compliancyResults">The list of compliancy results to use for the translation</param>
        /// <returns>The failure mechanism category distilled from the list of compliancy results</returns>
        /// <exception cref="AssemblyException">Thrown when:<br/>
        /// - compliancyResults == null<br/>
        /// - A lower category has "Does not comply" when a higher category has "Comply"</exception>
        private static FmSectionAssemblyDirectResult TranslateCompliancyResultToCategory(
            FmSectionCategoryCompliancyResults compliancyResults)
        {
            if (compliancyResults == null)
            {
                throw new AssemblyException("FmSectionCompliancyResults", EAssemblyErrors.ValueMayNotBeNull);
            }

            var firstComplyFound = false;
            var result = EFmSectionCategory.Gr;

            foreach (KeyValuePair<EFmSectionCategory, ECategoryCompliancy> compliancyResult
                in compliancyResults.GetCompliancyResults())
            {
                switch (compliancyResult.Value)
                {
                    case ECategoryCompliancy.Ngo:
                        return new FmSectionAssemblyDirectResult(EFmSectionCategory.VIIv);
                    case ECategoryCompliancy.Complies:
                        if (!firstComplyFound)
                        {
                            result = compliancyResult.Key;
                            firstComplyFound = true;
                        }

                        break;
                    case ECategoryCompliancy.DoesNotComply:
                        if (firstComplyFound)
                        {
                            throw new AssemblyException("FmSectionCompliancyResults",
                                                        EAssemblyErrors.DoesNotComplyAfterComply);
                        }

                        result = EFmSectionCategory.VIv;
                        break;
                }
            }

            return new FmSectionAssemblyDirectResult(result);
        }

        /// <summary>
        /// Mapper which maps a value of type TInput to a value of type TResult.
        /// And Checks whether the input value is not null and is present in the list of TInput values.
        /// </summary>
        /// <typeparam name="TInput">The allowable input type</typeparam>
        /// <typeparam name="TResult">The resulting output type</typeparam>
        private sealed class ResultMapper<TInput, TResult> : Dictionary<TInput, TResult>
        {
            private readonly string translateMethodName;

            public ResultMapper(string translateMethodName)
            {
                this.translateMethodName = translateMethodName;
            }

            /// <summary>
            /// Get the TResult value for the TInput value
            /// </summary>
            /// <param name="input">The input to translate</param>
            /// <returns>The translated input</returns>
            /// <exception cref="AssemblyException">Thrown when input == null or 
            /// when input value is not present in the list of results</exception>
            public TResult GetResult(TInput input)
            {
                CheckInput(input);
                return this[input];
            }

            private void CheckInput(TInput input)
            {
                if (input == null)
                {
                    throw new AssemblyException("TranslateAssessmentResult: " + translateMethodName,
                                                EAssemblyErrors.ValueMayNotBeNull);
                }

                if (!(Keys as ICollection<TInput>).Contains(input))
                {
                    throw new AssemblyException(translateMethodName + " input: " + input,
                                                EAssemblyErrors.TranslateAssessmentInvalidInput);
                }
            }
        }
    }
}