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
using Assembly.Kernel.Model.AssessmentSection;
using Assembly.Kernel.Model.Categories;
using Assembly.Kernel.Model.FailurePaths;

namespace Assembly.Kernel.Implementations
{
    /// <inheritdoc />
    public class AssessmentGradeAssembler : IAssessmentGradeAssembler
    {
        /// <inheritdoc />
        public AssessmentSectionResult AssembleAssessmentSectionWbi2B1(
            IEnumerable<FailurePathAssemblyResult> failurePathmAssemblyResults,
            CategoriesList<AssessmentSectionCategory> categories,
            bool partialAssembly)
        {
            FailurePathAssemblyResult[] failurePathAssemblyResults =
                CheckFailurePathAssemblyResults(failurePathmAssemblyResults);

            if (categories == null)
            {
                throw new AssemblyException("Categories", EAssemblyErrors.ValueMayNotBeNull);
            }

            if (partialAssembly)
            {
                failurePathAssemblyResults = failurePathAssemblyResults.Where(fmr =>
                        !double.IsNaN(fmr.FailureProbability))
                    .ToArray();
            }

            if (failurePathAssemblyResults.All(fmr => double.IsNaN(fmr.FailureProbability)))
            {
                return new AssessmentSectionResult(double.NaN, EAssessmentGrade.Gr);
            }

            var failureProbProduct = 1.0;
            foreach (var failurePathAssemblyResult in failurePathAssemblyResults)
            {
                if (double.IsNaN(failurePathAssemblyResult.FailureProbability))
                {
                    return new AssessmentSectionResult(double.NaN, EAssessmentGrade.Gr);
                }

                failureProbProduct *= 1.0 - failurePathAssemblyResult.FailureProbability;
            }

            var probabilityOfFailure = 1 - failureProbProduct;
            var category = categories.GetCategoryForFailureProbability(probabilityOfFailure);
            return new AssessmentSectionResult(probabilityOfFailure, category.Category);
        }

        private static FailurePathAssemblyResult[] CheckFailurePathAssemblyResults(
            IEnumerable<FailurePathAssemblyResult> failurePathAssemblyResults)
        {
            if (failurePathAssemblyResults == null)
            {
                throw new AssemblyException("AssembleFailurePathResult", EAssemblyErrors.ValueMayNotBeNull);
            }

            List<FailurePathAssemblyResult> failurePathResults = failurePathAssemblyResults.ToList();

            if (failurePathResults.Count == 0)
            {
                throw new AssemblyException("AssembleFailurePathResult",
                    EAssemblyErrors.FailurePathAssemblerInputInvalid);
            }

            return failurePathResults.ToArray();
        }
    }
}