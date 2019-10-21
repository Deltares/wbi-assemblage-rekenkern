#region Copyright (C) Rijkswaterstaat 2019. All rights reserved
// Copyright (C) Rijkswaterstaat 2019. All rights reserved.
//
// This file is part of the Assembly kernel.
//
// Assembly kernel is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
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
using Assembly.Kernel.Model.AssessmentResultTypes;
using Assembly.Kernel.Model.CategoryLimits;
using Assembly.Kernel.Model.FmSectionTypes;

namespace Assembly.Kernel.Implementations
{
    /// <inheritdoc />
    public class AssessmentResultsTranslator : IAssessmentResultsTranslator
    {
        private readonly ResultMapper<EAssessmentResultTypeE1, EFmSectionCategory> wbi0E1ResultMap =
            new ResultMapper<EAssessmentResultTypeE1, EFmSectionCategory>("Wbi0E1")
            {
                {EAssessmentResultTypeE1.Nvt, EFmSectionCategory.NotApplicable},
                {EAssessmentResultTypeE1.Fv, EFmSectionCategory.Iv},
                {EAssessmentResultTypeE1.Vb, EFmSectionCategory.VIIv},
                {EAssessmentResultTypeE1.Gr, EFmSectionCategory.Gr}
            };

        private readonly ResultMapper<EAssessmentResultTypeE1, EIndirectAssessmentResult> wbi0E2ResultMap =
            new ResultMapper<EAssessmentResultTypeE1, EIndirectAssessmentResult>("Wbi0E2")
            {
                {EAssessmentResultTypeE1.Nvt, EIndirectAssessmentResult.Nvt},
                {EAssessmentResultTypeE1.Fv, EIndirectAssessmentResult.FvEt},
                {EAssessmentResultTypeE1.Vb, EIndirectAssessmentResult.Ngo},
                {EAssessmentResultTypeE1.Gr, EIndirectAssessmentResult.Gr}
            };

        private readonly ResultMapper<EAssessmentResultTypeE2, EFmSectionCategory> wbi0E3ResultMap =
            new ResultMapper<EAssessmentResultTypeE2, EFmSectionCategory>("Wbi0E3")
            {
                {EAssessmentResultTypeE2.Nvt, EFmSectionCategory.NotApplicable},
                {EAssessmentResultTypeE2.Wvt, EFmSectionCategory.VIIv},
                {EAssessmentResultTypeE2.Gr, EFmSectionCategory.Gr}
            };

        private readonly ResultMapper<EAssessmentResultTypeE2, EIndirectAssessmentResult> wbi0E4ResultMap =
            new ResultMapper<EAssessmentResultTypeE2, EIndirectAssessmentResult>("Wbi0E4")
            {
                {EAssessmentResultTypeE2.Nvt, EIndirectAssessmentResult.Nvt},
                {EAssessmentResultTypeE2.Wvt, EIndirectAssessmentResult.Ngo},
                {EAssessmentResultTypeE2.Gr, EIndirectAssessmentResult.Gr}
            };

        private readonly ResultMapper<EAssessmentResultTypeG1, EFmSectionCategory> wbi0G1ResultMap =
            new ResultMapper<EAssessmentResultTypeG1, EFmSectionCategory>("Wbi0G1")
            {
                {EAssessmentResultTypeG1.V, EFmSectionCategory.IIv},
                {EAssessmentResultTypeG1.Vn, EFmSectionCategory.Vv},
                {EAssessmentResultTypeG1.Ngo, EFmSectionCategory.VIIv},
                {EAssessmentResultTypeG1.Gr, EFmSectionCategory.Gr}
            };

        private readonly ResultMapper<EAssessmentResultTypeG1, EIndirectAssessmentResult> wbi0G2ResultMap =
            new ResultMapper<EAssessmentResultTypeG1, EIndirectAssessmentResult>("Wbi0G2")
            {
                {EAssessmentResultTypeG1.V, EIndirectAssessmentResult.FvGt},
                {EAssessmentResultTypeG1.Vn, EIndirectAssessmentResult.Ngo},
                {EAssessmentResultTypeG1.Ngo, EIndirectAssessmentResult.Ngo},
                {EAssessmentResultTypeG1.Gr, EIndirectAssessmentResult.Gr}
            };

        private readonly ResultMapper<EAssessmentResultTypeG2, EFmSectionCategory> wbi0G4ResultMap =
            new ResultMapper<EAssessmentResultTypeG2, EFmSectionCategory>("Wbi0G4")
            {
                {EAssessmentResultTypeG2.Ngo, EFmSectionCategory.VIIv},
                {EAssessmentResultTypeG2.Gr, EFmSectionCategory.Gr}
            };

        private readonly ResultMapper<EAssessmentResultTypeT2, EIndirectAssessmentResult> wbi0T2ResultMap =
            new ResultMapper<EAssessmentResultTypeT2, EIndirectAssessmentResult>("Wbi0T2")
            {
                {EAssessmentResultTypeT2.V, EIndirectAssessmentResult.FvTom},
                {EAssessmentResultTypeT2.Vn, EIndirectAssessmentResult.Ngo},
                {EAssessmentResultTypeT2.Ngo, EIndirectAssessmentResult.Ngo},
                {EAssessmentResultTypeT2.Fv, EIndirectAssessmentResult.FvTom},
                {EAssessmentResultTypeT2.Verd, EIndirectAssessmentResult.FactoredInOtherFailureMechanism},
                {EAssessmentResultTypeT2.Gr, EIndirectAssessmentResult.Gr}
            };

        private readonly ResultMapper<EAssessmentResultTypeT3, EFmSectionCategory> wbi0T4ResultMap =
            new ResultMapper<EAssessmentResultTypeT3, EFmSectionCategory>("Wbi0T4")
            {
                {EAssessmentResultTypeT3.Ngo, EFmSectionCategory.VIIv},
                {EAssessmentResultTypeT3.Fv, EFmSectionCategory.Iv},
                {EAssessmentResultTypeT3.Gr, EFmSectionCategory.Gr}
            };

        private readonly ResultMapper<EAssessmentResultTypeT1, EFmSectionCategory> wbiT1ResultMap =
            new ResultMapper<EAssessmentResultTypeT1, EFmSectionCategory>("Wbi0T1")
            {
                {EAssessmentResultTypeT1.V, EFmSectionCategory.IIv},
                {EAssessmentResultTypeT1.Vn, EFmSectionCategory.Vv},
                {EAssessmentResultTypeT1.Ngo, EFmSectionCategory.VIIv},
                {EAssessmentResultTypeT1.Fv, EFmSectionCategory.Iv},
                {EAssessmentResultTypeT1.Gr, EFmSectionCategory.Gr}
            };

        /*
         *direct failure mechnism methods.
         */
        /// <inheritdoc />
        public FmSectionAssemblyDirectResultWithProbability TranslateAssessmentResultWbi0E1(
            EAssessmentResultTypeE1 assessment)
        {
            var fmSectionCategory = wbi0E1ResultMap.GetResult(assessment);
            return new FmSectionAssemblyDirectResultWithProbability(fmSectionCategory,
                ResultCategoryToProbabilityEstimation(fmSectionCategory));
        }

        /// <inheritdoc />
        public FmSectionAssemblyDirectResultWithProbability TranslateAssessmentResultWbi0E3(
            EAssessmentResultTypeE2 assessment)
        {
            var fmSectionCategory = wbi0E3ResultMap.GetResult(assessment);
            return new FmSectionAssemblyDirectResultWithProbability(fmSectionCategory,
                ResultCategoryToProbabilityEstimation(fmSectionCategory));
        }

        /// <inheritdoc />
        public FmSectionAssemblyDirectResult TranslateAssessmentResultWbi0G1(EAssessmentResultTypeG1 assessment)
        {
            return new FmSectionAssemblyDirectResult(wbi0G1ResultMap.GetResult(assessment));
        }

        /// <inheritdoc />
        public FmSectionAssemblyDirectResult TranslateAssessmentResultWbi0T1(EAssessmentResultTypeT1 assessment)
        {
            return new FmSectionAssemblyDirectResult(wbiT1ResultMap.GetResult(assessment));
        }

        /*
         * indirect Failure mechanism methods
         */
        /// <inheritdoc />
        public FmSectionAssemblyIndirectResult TranslateAssessmentResultWbi0E2(EAssessmentResultTypeE1 assessment)
        {
            return new FmSectionAssemblyIndirectResult(wbi0E2ResultMap.GetResult(assessment));
        }

        /// <inheritdoc />
        public FmSectionAssemblyIndirectResult TranslateAssessmentResultWbi0E4(EAssessmentResultTypeE2 assessment)
        {
            return new FmSectionAssemblyIndirectResult(wbi0E4ResultMap.GetResult(assessment));
        }

        /// <inheritdoc />
        public FmSectionAssemblyIndirectResult TranslateAssessmentResultWbi0G2(EAssessmentResultTypeG1 assessment)
        {
            return new FmSectionAssemblyIndirectResult(wbi0G2ResultMap.GetResult(assessment));
        }

        /// <inheritdoc />
        public FmSectionAssemblyIndirectResult TranslateAssessmentResultWbi0T2(EAssessmentResultTypeT2 assessment)
        {
            return new FmSectionAssemblyIndirectResult(wbi0T2ResultMap.GetResult(assessment));
        }

        /*
         * Methods with supplied categories
         */
        /// <inheritdoc />
        public FmSectionAssemblyDirectResult TranslateAssessmentResultWbi0G4(EAssessmentResultTypeG2 assessment,
            EFmSectionCategory? category)
        {
            if (assessment != EAssessmentResultTypeG2.ResultSpecified)
            {
                return new FmSectionAssemblyDirectResult(wbi0G4ResultMap.GetResult(assessment));
            }

            switch (category)
            {
                case null:
                    throw new AssemblyException(
                        "Wbi0G4 input: " + assessment + " - null",
                        EAssemblyErrors.ValueMayNotBeNull);
                case EFmSectionCategory.Gr:
                case EFmSectionCategory.NotApplicable:
                    throw new AssemblyException("Wbi0G4 input: " + category,
                        EAssemblyErrors.TranslateAssessmentInvalidInput);
            }

            return new FmSectionAssemblyDirectResult(category.Value);
        }

        /// <inheritdoc />
        public FmSectionAssemblyDirectResult TranslateAssessmentResultWbi0T4(EAssessmentResultTypeT3 assessment,
            EFmSectionCategory? category)
        {
            if (assessment != EAssessmentResultTypeT3.ResultSpecified)
            {
                return new FmSectionAssemblyDirectResult(wbi0T4ResultMap.GetResult(assessment));
            }

            switch (category)
            {
                case null:
                    throw new AssemblyException(
                        "Wbi0T4 input: " + assessment + " - null",
                        EAssemblyErrors.ValueMayNotBeNull);
                case EFmSectionCategory.Gr:
                case EFmSectionCategory.NotApplicable:
                    throw new AssemblyException("Wbi0T4 input: " + category,
                        EAssemblyErrors.TranslateAssessmentInvalidInput);
            }

            return new FmSectionAssemblyDirectResult(category.Value);
        }

        /*
         * Methods for assessment result determination based of category limit compliancy.
         */

        /// <inheritdoc />
        public FmSectionAssemblyDirectResult TranslateAssessmentResultWbi0G6(
            FmSectionCategoryCompliancyResults compliancyResults)
        {
            return TranslateCompliancyResultToCategory(compliancyResults);
        }

        /// <inheritdoc />
        public FmSectionAssemblyDirectResult TranslateAssessmentResultWbi0T6(
            FmSectionCategoryCompliancyResults compliancyResults, EAssessmentResultTypeT3 assessment)
        {
            if (assessment != EAssessmentResultTypeT3.ResultSpecified && compliancyResults != null)
            {
                throw new AssemblyException("FmSectionCompliancyResults: compliancy & assessmentResult",
                    EAssemblyErrors.TranslateAssessmentInvalidInput);
            }

            switch (assessment)
            {
                case EAssessmentResultTypeT3.Fv:
                    return new FmSectionAssemblyDirectResult(EFmSectionCategory.Iv);
                case EAssessmentResultTypeT3.Gr:
                    return new FmSectionAssemblyDirectResult(EFmSectionCategory.Gr);
                case EAssessmentResultTypeT3.Ngo:
                    return new FmSectionAssemblyDirectResult(EFmSectionCategory.VIIv);
                case EAssessmentResultTypeT3.ResultSpecified:
                    return TranslateCompliancyResultToCategory(compliancyResults);
                default:
                    throw new AssemblyException(
                        "FmSectionCompliancyResults",
                        EAssemblyErrors.TranslateAssessmentInvalidInput);
            }
        }

        /*
         * Methods with supplied failure probability
         */
        /// <inheritdoc/>
        public FmSectionAssemblyDirectResultWithProbability TranslateAssessmentResultWbi0G3(
            EAssessmentResultTypeG2 assessment, double failureProbability, CategoriesList<FmSectionCategory> categories)
        {
            switch (assessment)
            {
                case EAssessmentResultTypeG2.Ngo:
                    return new FmSectionAssemblyDirectResultWithProbability(EFmSectionCategory.VIIv, double.NaN);
                case EAssessmentResultTypeG2.Gr:
                    return new FmSectionAssemblyDirectResultWithProbability(EFmSectionCategory.Gr, double.NaN);
                case EAssessmentResultTypeG2.ResultSpecified:
                    if (double.IsNaN(failureProbability))
                    {
                        throw new AssemblyException(
                            "TranslateAssessmentResult with Failure probability: " + assessment,
                            EAssemblyErrors.ValueMayNotBeNull);
                    }

                    var category = categories.GetCategoryForFailureProbability(failureProbability);

                    return new FmSectionAssemblyDirectResultWithProbability(category.Category, failureProbability);

                default:
                    throw new AssemblyException("TranslateAssessmentResult with Failure probability: " + assessment,
                        EAssemblyErrors.TranslateAssessmentInvalidInput);
            }
        }

        /// <inheritdoc />
        public FmSectionAssemblyDirectResultWithProbability TranslateAssessmentResultWbi0G5(
            double fmSectionLengthEffectFactor,
            EAssessmentResultTypeG2 assessment,
            double failureProbability,
            CategoriesList<FmSectionCategory> categories)
        {
            switch (assessment)
            {
                case EAssessmentResultTypeG2.Ngo:
                    return new FmSectionAssemblyDirectResultWithProbability(EFmSectionCategory.VIIv, double.NaN);
                case EAssessmentResultTypeG2.Gr:
                    return new FmSectionAssemblyDirectResultWithProbability(EFmSectionCategory.Gr, double.NaN);
                case EAssessmentResultTypeG2.ResultSpecified:
                    if (double.IsNaN(failureProbability))
                    {
                        throw new AssemblyException("TranslateAssessmentResult with Failure probability: " + assessment,
                            EAssemblyErrors.ValueMayNotBeNull);
                    }

                    var category = categories.GetCategoryForFailureProbability(failureProbability).Category;

                    var failureProbValue = failureProbability * fmSectionLengthEffectFactor;
                    if (failureProbValue > 1)
                    {
                        failureProbValue = 1.0;
                    }

                    return new FmSectionAssemblyDirectResultWithProbability(category, failureProbValue);

                default:
                    throw new AssemblyException("TranslateAssessmentResult with Failure probability: " + assessment,
                        EAssemblyErrors.TranslateAssessmentInvalidInput);
            }
        }

        /// <inheritdoc />
        public FmSectionAssemblyDirectResultWithProbability TranslateAssessmentResultWbi0T3(
            EAssessmentResultTypeT3 assessment,
            double failureProbability,
            CategoriesList<FmSectionCategory> categories)
        {
            switch (assessment)
            {
                case EAssessmentResultTypeT3.Ngo:
                    return new FmSectionAssemblyDirectResultWithProbability(EFmSectionCategory.VIIv, double.NaN);
                case EAssessmentResultTypeT3.Fv:
                    return new FmSectionAssemblyDirectResultWithProbability(EFmSectionCategory.Iv, 0.0);
                case EAssessmentResultTypeT3.Gr:
                    return new FmSectionAssemblyDirectResultWithProbability(EFmSectionCategory.Gr, double.NaN);
                case EAssessmentResultTypeT3.ResultSpecified:
                    if (double.IsNaN(failureProbability))
                    {
                        throw new AssemblyException("TranslateAssessmentResult with Failure probability: " + assessment,
                            EAssemblyErrors.ValueMayNotBeNull);
                    }

                    var category = categories.GetCategoryForFailureProbability(failureProbability).Category;

                    return new FmSectionAssemblyDirectResultWithProbability(category, failureProbability);

                default:
                    throw new AssemblyException("TranslateAssessmentResult with Failure probability: " + assessment,
                        EAssemblyErrors.TranslateAssessmentInvalidInput);
            }
        }

        /// <inheritdoc />
        public FmSectionAssemblyDirectResultWithProbability TranslateAssessmentResultWbi0T5(
            double fmSectionLengthEffectFactor,
            EAssessmentResultTypeT3 assessment,
            double failureProbability, CategoriesList<FmSectionCategory> categories)
        {
            switch (assessment)
            {
                case EAssessmentResultTypeT3.Ngo:
                    return new FmSectionAssemblyDirectResultWithProbability(EFmSectionCategory.VIIv, double.NaN);
                case EAssessmentResultTypeT3.Fv:
                    return new FmSectionAssemblyDirectResultWithProbability(EFmSectionCategory.Iv, 0.0);
                case EAssessmentResultTypeT3.Gr:
                    return new FmSectionAssemblyDirectResultWithProbability(EFmSectionCategory.Gr, double.NaN);
                case EAssessmentResultTypeT3.ResultSpecified:
                    if (double.IsNaN(failureProbability))
                    {
                        throw new AssemblyException("TranslateAssessmentResult with Failure probability: " + assessment,
                            EAssemblyErrors.ValueMayNotBeNull);
                    }

                    var category = categories.GetCategoryForFailureProbability(failureProbability).Category;

                    var failureProbValue = failureProbability * fmSectionLengthEffectFactor;
                    if (failureProbValue > 1)
                    {
                        failureProbValue = 1.0;
                    }

                    return new FmSectionAssemblyDirectResultWithProbability(category, failureProbValue);

                default:
                    throw new AssemblyException("TranslateAssessmentResult with Failure probability: " + assessment,
                        EAssemblyErrors.TranslateAssessmentInvalidInput);
            }
        }

        /// <inheritdoc />
        public FmSectionAssemblyDirectResult TranslateAssessmentResultWbi0T7(EAssessmentResultTypeT4 assessment,
            double failureProbability, CategoriesList<FmSectionCategory> categories)
        {
            switch (assessment)
            {
                case EAssessmentResultTypeT4.V:
                    return new FmSectionAssemblyDirectResult(EFmSectionCategory.IIv);
                case EAssessmentResultTypeT4.Vn:
                    return new FmSectionAssemblyDirectResult(EFmSectionCategory.Vv);
                case EAssessmentResultTypeT4.Ngo:
                    return new FmSectionAssemblyDirectResult(EFmSectionCategory.VIIv);
                case EAssessmentResultTypeT4.Fv:
                    return new FmSectionAssemblyDirectResult(EFmSectionCategory.Iv);
                case EAssessmentResultTypeT4.Gr:
                    return new FmSectionAssemblyDirectResult(EFmSectionCategory.Gr);
                case EAssessmentResultTypeT4.ResultSpecified:
                    if (double.IsNaN(failureProbability))
                    {
                        throw new AssemblyException("TranslateAssessmentResult with Failure probability: " + assessment,
                            EAssemblyErrors.ValueMayNotBeNull);
                    }

                    var category = categories.GetCategoryForFailureProbability(failureProbability).Category;

                    return new FmSectionAssemblyDirectResult(category);

                default:
                    throw new AssemblyException("TranslateAssessmentResult with Failure probability: " + assessment,
                        EAssemblyErrors.TranslateAssessmentInvalidInput);
            }
        }

        /*
         * Methods for translation to a normative result result.
         */

        /// <inheritdoc />
        public FmSectionAssemblyDirectResult TranslateAssessmentResultWbi0A1(
            FmSectionAssemblyDirectResult simpleAssessmentResult,
            FmSectionAssemblyDirectResult detailedAssessmentResult,
            FmSectionAssemblyDirectResult customAssessmentResult)
        {
            var result = TranslateAssessmentResultWbi0A1Internal(simpleAssessmentResult, detailedAssessmentResult,
                customAssessmentResult);
            return new FmSectionAssemblyDirectResult(result.Result);
        }

        /// <inheritdoc />
        public FmSectionAssemblyDirectResultWithProbability TranslateAssessmentResultWbi0A1(
        FmSectionAssemblyDirectResultWithProbability simpleAssessmentResult,
            FmSectionAssemblyDirectResultWithProbability detailedAssessmentResult,
            FmSectionAssemblyDirectResultWithProbability customAssessmentResult)
        {
            var result = TranslateAssessmentResultWbi0A1Internal(simpleAssessmentResult, detailedAssessmentResult,
                customAssessmentResult);
            return new FmSectionAssemblyDirectResultWithProbability(result.Result, result.FailureProbability);
        }

        /// <inheritdoc />
        public FmSectionAssemblyIndirectResult TranslateAssessmentResultWbi0A1(
            FmSectionAssemblyIndirectResult simpleAssessmentResult,
            FmSectionAssemblyIndirectResult detailedAssessmentResult,
            FmSectionAssemblyIndirectResult customAssessmentResult)
        {
            var result = TranslateAssessmentResultWbi0A1Internal(simpleAssessmentResult, detailedAssessmentResult, customAssessmentResult);
            return new FmSectionAssemblyIndirectResult(result.Result);
        }

        /*
        * Private methods and classes.
        */
        /// <summary>
        /// Internal method to calculate WBI-0A-1 (normative results based on a simple assessment, detailed assessment and custom assessment result).
        /// </summary>
        /// <typeparam name="TResult">Generic assessment result.</typeparam>
        /// <param name="simpleAssessmentResult">The test result of a simple assessment. May not be null.</param>
        /// <param name="detailedAssessmentResult">The test result of a detailed assessment. 
        /// May be null when not available</param>
        /// <param name="customAssessmentResult">The test result of a custom assessment.
        /// May be null when not available</param>
        /// <returns>The normative result.</returns>
        /// <exception cref="AssemblyException">Thrown when simpleAssessmentResult == null</exception>
        private TResult TranslateAssessmentResultWbi0A1Internal<TResult>(
            TResult simpleAssessmentResult,
            TResult detailedAssessmentResult,
            TResult customAssessmentResult) where TResult : IFmSectionAssemblyResult
        {
            if (simpleAssessmentResult == null)
            {
                throw new AssemblyException(
                    "TranslateNormativeAssessmentResult: simple assessment result",
                    EAssemblyErrors.ValueMayNotBeNull);
            }

            if (simpleAssessmentResult.NotApplicableOrNeglectable())
            {
                return simpleAssessmentResult;
            }

            if (customAssessmentResult != null && customAssessmentResult.HasResult())
            {
                return customAssessmentResult;
            }

            if (detailedAssessmentResult != null && detailedAssessmentResult.HasResult())
            {
                return detailedAssessmentResult;
            }

            return simpleAssessmentResult;
        }

        /// <summary>
        /// Translates a section result category to the associated estimated probability of failure
        /// </summary>
        /// <param name="fmSectionCategory">The section result category that needs to be translated</param>
        /// <returns>Either 0.0 in case the category equals EFmSectionCategory.Iv or EFmSectionCategory.NotApplicable</returns>
        private static double ResultCategoryToProbabilityEstimation(EFmSectionCategory fmSectionCategory)
        {
            return fmSectionCategory == EFmSectionCategory.Iv || fmSectionCategory == EFmSectionCategory.NotApplicable
                ? 0.0
                : double.NaN;
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
        }
    }
}