#region Copyright (c) 2018 Technolution BV. All Rights Reserved. 

// // Copyright (C) Technolution BV. 2018. All rights reserved.
// //
// // This file is part of the Assembly kernel.
// //
// // Assembly kernel is free software: you can redistribute it and/or modify
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

using System;
using System.Collections.Generic;
using System.Linq;
using Assembly.Kernel.Exceptions;
using Assembly.Kernel.Interfaces;
using Assembly.Kernel.Model;
using Assembly.Kernel.Model.CategoryLimits;
using Assembly.Kernel.Model.FmSectionTypes;

namespace Assembly.Kernel.Implementations
{
    /// <inheritdoc />
    public class FailureMechanismResultAssembler : IFailureMechanismResultAssembler
    {
        private readonly ICategoryLimitsCalculator categoryLimitsCalculator = new CategoryLimitsCalculator();

        /// <inheritdoc />
        public EFailureMechanismCategory AssembleFailureMechanismWbi1A1(
            IEnumerable<FmSectionAssemblyDirectResult> fmSectionAssemblyResults, bool partialAssembly)
        {
            if (fmSectionAssemblyResults == null)
            {
                throw new AssemblyException("AssembleFailureMechanismResult", EAssemblyErrors.ValueMayNotBeNull);
            }

            List<FmSectionAssemblyDirectResult> sectionResults = fmSectionAssemblyResults.ToList();

            // result list should not be empty
            if (sectionResults.Count == 0)
            {
                throw new AssemblyException("AssembleFailureMechanismResult",
                    EAssemblyErrors.FailureMechanismAssemblerInputInvalid);
            }

            var returnValue = EFmSectionCategory.NotApplicable;
            foreach (var sectionResult in sectionResults)
            {
                switch (sectionResult.Result)
                {
                    case EFmSectionCategory.Iv:
                    case EFmSectionCategory.IIv:
                    case EFmSectionCategory.IIIv:
                    case EFmSectionCategory.IVv:
                    case EFmSectionCategory.Vv:
                    case EFmSectionCategory.VIv:
                        if (sectionResult.Result.IsLowerCategoryThan(returnValue))
                        {
                            returnValue = sectionResult.Result;
                        }

                        break;
                    case EFmSectionCategory.VIIv:
                        if (!partialAssembly)
                        {
                            return EFailureMechanismCategory.VIIt;
                        }

                        break;
                    case EFmSectionCategory.Gr:
                        return EFailureMechanismCategory.Gr;
                    case EFmSectionCategory.NotApplicable:
                        // ignore not applicable category
                        break;
                    default:
                        throw new AssemblyException(
                            "AssembleFailureMechanismResult: " + sectionResult.Result,
                            EAssemblyErrors.CategoryNotAllowed);
                }
            }

            return returnValue.ToAssessmentGrade();
        }

        /// <inheritdoc />
        public EIndirectAssessmentResult AssembleFailureMechanismWbi1A2(
            IEnumerable<FmSectionAssemblyIndirectResult> fmSectionAssemblyResults, bool partialAssembly)
        {
            if (fmSectionAssemblyResults == null)
            {
                throw new AssemblyException("AssembleFailureMechanismResult", EAssemblyErrors.ValueMayNotBeNull);
            }

            List<FmSectionAssemblyIndirectResult> sectionResults = fmSectionAssemblyResults.ToList();

            if (sectionResults.Count == 0)
            {
                throw new AssemblyException("AssembleFailureMechanismResult",
                    EAssemblyErrors.FailureMechanismAssemblerInputInvalid);
            }

            var returnValue = EIndirectAssessmentResult.Nvt;
            foreach (var sectionResult in sectionResults)
            {
                switch (sectionResult.Result)
                {
                    case EIndirectAssessmentResult.Ngo:
                        if (!partialAssembly)
                        {
                            return EIndirectAssessmentResult.Ngo;
                        }

                        break;
                    case EIndirectAssessmentResult.Nvt:
                    case EIndirectAssessmentResult.FvEt:
                    case EIndirectAssessmentResult.FvGt:
                    case EIndirectAssessmentResult.FvTom:
                    case EIndirectAssessmentResult.FactoredInOtherFailureMechanism:
                        if (sectionResult.Result.IsLowerCategoryThan(returnValue))
                        {
                            returnValue = sectionResult.Result;
                        }

                        break;
                    case EIndirectAssessmentResult.Gr:
                        return EIndirectAssessmentResult.Gr;
                    default:
                        throw new AssemblyException(
                            "AssembleFailureMechanismResult: " + sectionResult.Result,
                            EAssemblyErrors.CategoryNotAllowed);
                }
            }

            return returnValue;
        }

        /// <inheritdoc />
        public FailureMechanismAssemblyResult AssembleFailureMechanismWbi1B1(AssessmentSection section,
            FailureMechanism failureMechanism, IEnumerable<FmSectionAssemblyDirectResultWithProbability> fmSectionAssemblyResults,
            bool partialAssembly)
        {
            // step 1: Ptraject = 1 - Product(1-Pi){i=1 -> N} where N is the number of failure mechanism sections.
            var failureProbProduct = 1.0;
            var highestFailureProbability = 0.0;

            var failureProbFound = false;
            foreach (var fmSectionResult in fmSectionAssemblyResults)
            {
                switch (fmSectionResult.Result)
                {
                    case EFmSectionCategory.Iv:
                    case EFmSectionCategory.IIv:
                    case EFmSectionCategory.IIIv:
                    case EFmSectionCategory.IVv:
                    case EFmSectionCategory.Vv:
                    case EFmSectionCategory.VIv:
                        if (double.IsNaN(fmSectionResult.FailureProbability))
                        {
                            throw new AssemblyException("FailureMechanismAssembler", EAssemblyErrors.ValueMayNotBeNull);
                        }

                        // This failuremechanism section contains a failure probability 
                        failureProbFound = true;

                        var sectionFailureProb = fmSectionResult.FailureProbability;
                        if (sectionFailureProb > highestFailureProbability)
                        {
                            highestFailureProbability = sectionFailureProb;
                        }

                        failureProbProduct *= 1.0 - sectionFailureProb;
                        break;
                    case EFmSectionCategory.VIIv:
                        // If one of the results is VIIv and it isn't a partial result,
                        // the resulting category will also be VIIt. See FO 6.2.1
                        if (!partialAssembly)
                        {
                            return new FailureMechanismAssemblyResult(EFailureMechanismCategory.VIIt);
                        }

                        continue;
                    case EFmSectionCategory.Gr:
                        // if one of the input categories is No result and it isn't a partial result, 
                        // the resulting category will also be no result. See FO 6.2.1
                        if (!partialAssembly)
                        {
                            return new FailureMechanismAssemblyResult(EFailureMechanismCategory.Gr);
                        }

                        continue;
                    case EFmSectionCategory.NotApplicable:
                        // ignore not applicable category
                        continue;
                }
            }

            if (!failureProbFound)
            {
                return new FailureMechanismAssemblyResult(EFailureMechanismCategory.Nvt);
            }

            var failureMechanismFailureProbability = 1 - failureProbProduct;

            // step 2: Get section with largest failure probability and multiply with Assessment section length effect factor.
            highestFailureProbability *= failureMechanism.LengthEffectFactor;
            // step 3: Compare the Failure probabilities from step 1 and 2 and use the lowest of the two.
            var resultFailureProb = Math.Min(highestFailureProbability, failureMechanismFailureProbability);
            // step 4: Get category limits for failure mechanism and return the category + failure probability
            IEnumerable<FailureMechanismCategoryLimits> categoryLimits =
                categoryLimitsCalculator.CalculateFailureMechanismCategoryLimitsWbi11(section, failureMechanism);

            var resultCategory = categoryLimits
                .First(limits => resultFailureProb <= limits.UpperLimit)
                .Category;
            return new FailureMechanismAssemblyResult(resultCategory, resultFailureProb);
        }
    }
}