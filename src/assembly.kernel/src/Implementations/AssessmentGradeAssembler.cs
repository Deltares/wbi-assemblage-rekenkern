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

using System.Collections.Generic;
using System.Linq;
using Assembly.Kernel.Exceptions;
using Assembly.Kernel.Interfaces;
using Assembly.Kernel.Model;
using Assembly.Kernel.Model.CategoryLimits;

namespace Assembly.Kernel.Implementations {
    /// <inheritdoc />
    public class AssessmentGradeAssembler : IAssessmentGradeAssembler {
        private readonly ICategoryLimitsCalculator categoryLimitsCalculator = new CategoryLimitsCalculator();

        /// <inheritdoc />
        public EAssessmentGrade AssembleAssessmentSectionWbi2A1(
            IEnumerable<FailureMechanismAssemblyResult> failureMechanismAssemblyResults,
            bool partialAssembly) {
            if (failureMechanismAssemblyResults == null) {
                throw new AssemblyException("AssembleFailureMechanismResult", EAssemblyErrors.ValueMayNotBeNull);
            }

            List<FailureMechanismAssemblyResult> failureMechanismResults = failureMechanismAssemblyResults.ToList();

            if (failureMechanismResults.Count == 0) {
                throw new AssemblyException("AssembleFailureMechanismResult",
                    EAssemblyErrors.FailureMechanismAssemblerInputInvalid);
            }

            var resultCategory = EFailureMechanismCategory.Nvt;
            foreach (var failureMechanismResult in failureMechanismResults) {
                switch (failureMechanismResult.Category) {
                case EFailureMechanismCategory.It:
                case EFailureMechanismCategory.IIt:
                case EFailureMechanismCategory.IIIt:
                case EFailureMechanismCategory.IVt:
                case EFailureMechanismCategory.Vt:
                case EFailureMechanismCategory.VIt:
                    if (failureMechanismResult.Category.IsLowerCategoryThan(resultCategory)) {
                        resultCategory = failureMechanismResult.Category;
                    }

                    break;
                case EFailureMechanismCategory.VIIt:
                    if (!partialAssembly) {
                        return EAssessmentGrade.Ngo;
                    }

                    break;
                case EFailureMechanismCategory.Nvt:
                    // ignore does not apply category
                    break;
                case EFailureMechanismCategory.Gr:
                    return EAssessmentGrade.Gr;
                default:
                    throw new AssemblyException(
                        "AssembleFailureMechanismResult: " + failureMechanismResult.Category,
                        EAssemblyErrors.CategoryNotAllowed);
                }
            }

            return resultCategory.ToAssessmentGrade();
        }

        /// <inheritdoc />
        public AssessmentSectionAssemblyResult AssembleAssessmentSectionWbi2B1(AssessmentSection section,
            IEnumerable<FailureMechanismAssemblyResult> failureMechanismAssemblyResults,
            bool partialAssembly) {
            // step 1: Ptraject = 1 - Product(1-Pi){i=1 -> N} where N is the number of failure mechanisms.
            var failureProbProduct = 1.0;
            var failureProbFound = false;

            foreach (var failureMechanismAssemblyResult in failureMechanismAssemblyResults) {
                switch (failureMechanismAssemblyResult.Category) {
                case EFailureMechanismCategory.It:
                case EFailureMechanismCategory.IIt:
                case EFailureMechanismCategory.IIIt:
                case EFailureMechanismCategory.IVt:
                case EFailureMechanismCategory.Vt:
                case EFailureMechanismCategory.VIt:
                    if (double.IsNaN(failureMechanismAssemblyResult.FailureProbability)) {
                        throw new AssemblyException("FailureMechanismAssembler", EAssemblyErrors.ValueMayNotBeNull);
                    }

                    // This failuremechanism section contains a failure probability 
                    failureProbFound = true;

                    var sectionFailureProb = failureMechanismAssemblyResult.FailureProbability;
                    failureProbProduct *= 1.0 - sectionFailureProb;
                    break;
                case EFailureMechanismCategory.VIIt:
                    // If one of the results is VIIv and it isn't a partial result,
                    // the resulting category will also be VIIt. See FO 7.2.1
                    if (!partialAssembly) {
                        return new AssessmentSectionAssemblyResult(EAssessmentGrade.Ngo);
                    }

                    continue;
                case EFailureMechanismCategory.Gr:
                    // If one of the results is No result and it isn't a partial result,
                    // the resulting category will also be VIIt. See FO 7.2.1
                    if (!partialAssembly) {
                        return new AssessmentSectionAssemblyResult(EAssessmentGrade.Gr);
                    }

                    continue;
                case EFailureMechanismCategory.Nvt:
                    // ignore "does not apply" category
                    continue;
                }
            }

            if (!failureProbFound) {
                return new AssessmentSectionAssemblyResult(EAssessmentGrade.Nvt);
            }

            var assessmentSectionFailureProb = 1 - failureProbProduct;

            // step 2: Get category limits for the assessment section and return the category + failure probability
            IEnumerable<AssessmentSectionCategoryLimits> categoryLimits =
                categoryLimitsCalculator.CalculateAssessmentSectionCategoryLimitsWbi21(section);

            var resultCategory = categoryLimits
                .First(limits => assessmentSectionFailureProb <= limits.UpperLimit)
                .Category;

            return new AssessmentSectionAssemblyResult(resultCategory, assessmentSectionFailureProb);
        }

        /// <inheritdoc />
        public AssessmentSectionAssemblyResult AssembleAssessmentSectionWbi2C1(
            AssessmentSectionAssemblyResult assemblyResultNoFailureProbability,
            AssessmentSectionAssemblyResult assemblyResultWithFailureProbability) {
            if (assemblyResultNoFailureProbability == null || assemblyResultWithFailureProbability == null) {
                throw new AssemblyException("AssessmentGradeAssembler", EAssemblyErrors.ValueMayNotBeNull);
            }

            // Return the result with failure probability when the assembly result 
            // without failure probability does not apply
            if (assemblyResultNoFailureProbability.Category == EAssessmentGrade.Nvt) {
                return assemblyResultWithFailureProbability.CreateNewFrom();
            }

            if (assemblyResultNoFailureProbability.Category.IsLowerCategoryThan(assemblyResultWithFailureProbability
                .Category)) {
                return assemblyResultNoFailureProbability.CreateNewFrom();
            } 
            return assemblyResultWithFailureProbability.CreateNewFrom();    
        }
    }
}