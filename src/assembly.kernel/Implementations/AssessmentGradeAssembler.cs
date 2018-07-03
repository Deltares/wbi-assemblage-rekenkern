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

namespace Assembly.Kernel.Implementations
{
    /// <inheritdoc />
    public class AssessmentGradeAssembler : IAssessmentGradeAssembler
    {
        /// <inheritdoc />
        public EFailureMechanismCategory AssembleAssessmentSectionWbi2A1(
            IEnumerable<FailureMechanismAssemblyResult> failureMechanismAssemblyResults,
            bool partialAssembly)
        {
            var failureMechanismResults = CheckFailureMechanismAssemblyResults(failureMechanismAssemblyResults);

            var categoryViiFound = false;
            var resultCategory = EFailureMechanismCategory.Nvt;
            foreach (var failureMechanismResult in failureMechanismResults)
            {
                switch (failureMechanismResult.Category)
                {
                    case EFailureMechanismCategory.Nvt:
                        // ignore does not apply category

                        break;
                    case EFailureMechanismCategory.It:
                    case EFailureMechanismCategory.IIt:
                    case EFailureMechanismCategory.IIIt:
                    case EFailureMechanismCategory.IVt:
                    case EFailureMechanismCategory.Vt:
                    case EFailureMechanismCategory.VIt:
                        if (failureMechanismResult.Category > resultCategory)
                        {
                            resultCategory = failureMechanismResult.Category;
                        }

                        break;
                    case EFailureMechanismCategory.VIIt:
                        if (!partialAssembly)
                        {
                            categoryViiFound = true;
                        }

                        break;
                    case EFailureMechanismCategory.Gr:
                        return EFailureMechanismCategory.Gr;
                    default:
                        throw new AssemblyException(
                            "AssembleFailureMechanismResult: " + failureMechanismResult.Category,
                            EAssemblyErrors.CategoryNotAllowed);
                }
            }

            return categoryViiFound ? EFailureMechanismCategory.VIIt : resultCategory;
        }

        /// <inheritdoc />
        public FailureMechanismAssemblyResult AssembleAssessmentSectionWbi2B1(
            IEnumerable<FailureMechanismAssemblyResult> failureMechanismAssemblyResults,
            CategoriesList<FailureMechanismCategory> categories,
            bool partialAssembly)
        {
            var failureMechanismResults = CheckFailureMechanismAssemblyResults(failureMechanismAssemblyResults);

            if (categories == null)
            {
                throw new AssemblyException("Categories", EAssemblyErrors.ValueMayNotBeNull);
            }

            // step 1: Ptraject = 1 - Product(1-Pi){i=1 -> N} where N is the number of failure mechanisms.
            var failureProbProduct = 1.0;
            var failureProbFound = false;
            var categoryViiFound = false;

            foreach (var failureMechanismAssemblyResult in failureMechanismResults)
            {
                switch (failureMechanismAssemblyResult.Category)
                {
                    case EFailureMechanismCategory.Nvt:
                        // ignore "does not apply" category
                        continue;
                    case EFailureMechanismCategory.It:
                    case EFailureMechanismCategory.IIt:
                    case EFailureMechanismCategory.IIIt:
                    case EFailureMechanismCategory.IVt:
                    case EFailureMechanismCategory.Vt:
                    case EFailureMechanismCategory.VIt:
                        if (double.IsNaN(failureMechanismAssemblyResult.FailureProbability))
                        {
                            throw new AssemblyException("FailureMechanismAssembler", EAssemblyErrors.ValueMayNotBeNull);
                        }

                        failureProbFound = true;

                        failureProbProduct *= 1.0 - failureMechanismAssemblyResult.FailureProbability;
                        break;
                    case EFailureMechanismCategory.VIIt:
                        // If one of the results is VIIv and it isn't a partial assembly, register this. The result should 
                        // always be VIIt if there is no other result of type GR. See FO 7.2.1
                        if (!partialAssembly)
                        {
                            categoryViiFound = true;
                        }

                        continue;
                    case EFailureMechanismCategory.Gr:
                        return new FailureMechanismAssemblyResult(EFailureMechanismCategory.Gr, double.NaN);
                    default:
                        throw new AssemblyException(
                            "AssembleFailureMechanismResult: " + failureMechanismAssemblyResult.Category,
                            EAssemblyErrors.CategoryNotAllowed);
                }
            }

            if (categoryViiFound)
            {
                return new FailureMechanismAssemblyResult(EFailureMechanismCategory.VIIt, double.NaN);
            }

            if (!failureProbFound)
            {
                return new FailureMechanismAssemblyResult(EFailureMechanismCategory.Nvt, 0.0);
            }

            var assessmentSectionFailureProb = 1 - failureProbProduct;

            // step 2: Get category limits for the assessment section and return the category + failure probability
            var resultCategory = categories.GetCategoryForFailureProbability(assessmentSectionFailureProb);
            return new FailureMechanismAssemblyResult(resultCategory.Category, assessmentSectionFailureProb);
        }

        /// <inheritdoc />
        public EAssessmentGrade AssembleAssessmentSectionWbi2C1(
            EFailureMechanismCategory assemblyResultNoFailureProbability,
            FailureMechanismAssemblyResult assemblyResultWithFailureProbability)
        {
            if (assemblyResultWithFailureProbability == null)
            {
                throw new AssemblyException("AssessmentGradeAssembler", EAssemblyErrors.ValueMayNotBeNull);
            }

            return assemblyResultNoFailureProbability > assemblyResultWithFailureProbability.Category
                ? assemblyResultNoFailureProbability.ToAssessmentGrade()
                : assemblyResultWithFailureProbability.Category.ToAssessmentGrade();
        }

        private static List<FailureMechanismAssemblyResult> CheckFailureMechanismAssemblyResults(
            IEnumerable<FailureMechanismAssemblyResult> failureMechanismAssemblyResults)
        {
            if (failureMechanismAssemblyResults == null)
            {
                throw new AssemblyException("AssembleFailureMechanismResult", EAssemblyErrors.ValueMayNotBeNull);
            }

            List<FailureMechanismAssemblyResult> failureMechanismResults = failureMechanismAssemblyResults.ToList();

            if (failureMechanismResults.Count == 0)
            {
                throw new AssemblyException("AssembleFailureMechanismResult",
                    EAssemblyErrors.FailureMechanismAssemblerInputInvalid);
            }

            return failureMechanismResults;
        }
    }
}