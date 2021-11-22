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
            IEnumerable<FailurePathAssemblyResult> failurePathProbabilities,
            CategoriesList<AssessmentSectionCategory> categories,
            bool partialAssembly)
        {
            FailurePathAssemblyResult[] failurePathProbabilitiesArray = CheckFailurePathAssemblyResults(failurePathProbabilities);

            if (categories == null)
            {
                throw new AssemblyException("Categories", EAssemblyErrors.ValueMayNotBeNull);
            }

            if (partialAssembly)
            {
                failurePathProbabilitiesArray = failurePathProbabilitiesArray.Where(fmr =>
                        !double.IsNaN(fmr.FailureProbability))
                    .ToArray();
            }

            if (failurePathProbabilitiesArray.All(fmr => double.IsNaN(fmr.FailureProbability)))
            {
                return new AssessmentSectionResult(double.NaN, EAssessmentGrade.Gr);
            }

            var failureProbabilityProduct = 1.0;
            foreach (var failurePathResult in failurePathProbabilitiesArray)
            {
                if (double.IsNaN(failurePathResult.FailureProbability))
                {
                    return new AssessmentSectionResult(double.NaN, EAssessmentGrade.Gr);
                }

                failureProbabilityProduct *= 1.0 - failurePathResult.FailureProbability;
            }

            var probabilityOfFailure = 1 - failureProbabilityProduct;
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

            FailurePathAssemblyResult[] failurePathResults = failurePathAssemblyResults.ToArray();

            if (failurePathResults.Length == 0)
            {
                throw new AssemblyException("AssembleFailurePathResult",
                    EAssemblyErrors.FailurePathAssemblerInputInvalid);
            }

            return failurePathResults;
        }
    }
}