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
            FailureMechanismAssemblyResult[] failureMechanismResults =
                CheckFailureMechanismAssemblyResults(failureMechanismAssemblyResults);

            if (partialAssembly)
            {
                failureMechanismResults = failureMechanismResults.Where(fmr =>
                                                                            fmr.Category != EFailureMechanismCategory.Gr &&
                                                                            fmr.Category != EFailureMechanismCategory.VIIt)
                                                                 .ToArray();
            }

            if (failureMechanismResults.All(fmr => fmr.Category == EFailureMechanismCategory.Gr))
            {
                return EFailureMechanismCategory.Gr;
            }

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
                    case EFailureMechanismCategory.Gr:
                        return EFailureMechanismCategory.VIIt;
                    default:
                        throw new AssemblyException(
                            "AssembleFailureMechanismResult: " + failureMechanismResult.Category,
                            EAssemblyErrors.CategoryNotAllowed);
                }
            }

            return categoryViiFound ? EFailureMechanismCategory.VIIt : resultCategory;
        }

        /// <inheritdoc />
        public AssessmentSectionResult AssembleAssessmentSectionWbi2B1(
            IEnumerable<FailureMechanismAssemblyResult> failureMechanismAssemblyResults,
            CategoriesList<AssessmentSectionCategory> categories,
            bool partialAssembly)
        {
            FailureMechanismAssemblyResult[] failureMechanismResults =
                CheckFailureMechanismAssemblyResults(failureMechanismAssemblyResults);

            if (categories == null)
            {
                throw new AssemblyException("Categories", EAssemblyErrors.ValueMayNotBeNull);
            }

            if (partialAssembly)
            {
                failureMechanismResults = failureMechanismResults.Where(fmr =>
                                                                            !double.IsNaN(fmr.FailureProbability))
                                                                 .ToArray();
            }

            if (failureMechanismResults.All(fmr => double.IsNaN(fmr.FailureProbability)))
            {
                return new AssessmentSectionResult(EAssessmentGrade.Gr, double.NaN);
            }

            var failureProbProduct = 1.0;
            foreach (var failureMechanismAssemblyResult in failureMechanismResults)
            {
                if (double.IsNaN(failureMechanismAssemblyResult.FailureProbability))
                {
                    return new AssessmentSectionResult(EAssessmentGrade.Gr, double.NaN);
                }
                failureProbProduct *= 1.0 - failureMechanismAssemblyResult.FailureProbability;
            }

            var probabilityOfFailure = 1 - failureProbProduct;
            var category = categories.GetCategoryForFailureProbability(probabilityOfFailure);
            return new AssessmentSectionResult(category.Category, probabilityOfFailure);
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

            if (assemblyResultNoFailureProbability == EFailureMechanismCategory.Gr ||
                assemblyResultWithFailureProbability.Category == EFailureMechanismCategory.Gr)
            {
                if (assemblyResultNoFailureProbability == EFailureMechanismCategory.Gr &&
                    assemblyResultWithFailureProbability.Category == EFailureMechanismCategory.Gr)
                {
                    return EAssessmentGrade.Gr;
                }

                return EAssessmentGrade.Ngo;
            }

            return assemblyResultNoFailureProbability > assemblyResultWithFailureProbability.Category
                       ? assemblyResultNoFailureProbability.ToAssessmentGrade()
                       : assemblyResultWithFailureProbability.Category.ToAssessmentGrade();
        }

        private static FailureMechanismAssemblyResult[] CheckFailureMechanismAssemblyResults(
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

            return failureMechanismResults.ToArray();
        }
    }
}