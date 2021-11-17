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
        public AssessmentSectionResult AssembleAssessmentSectionWbi2B1(
            IEnumerable<FailurePathAssemblyResult> failureMechanismAssemblyResults,
            CategoriesList<AssessmentSectionCategory> categories,
            bool partialAssembly)
        {
            FailurePathAssemblyResult[] failureMechanismResults =
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
                return new AssessmentSectionResult(double.NaN, EAssessmentGrade.Gr);
            }

            var failureProbProduct = 1.0;
            foreach (var failureMechanismAssemblyResult in failureMechanismResults)
            {
                if (double.IsNaN(failureMechanismAssemblyResult.FailureProbability))
                {
                    return new AssessmentSectionResult(double.NaN, EAssessmentGrade.Gr);
                }
                failureProbProduct *= 1.0 - failureMechanismAssemblyResult.FailureProbability;
            }

            var probabilityOfFailure = 1 - failureProbProduct;
            var category = categories.GetCategoryForFailureProbability(probabilityOfFailure);
            return new AssessmentSectionResult(probabilityOfFailure, category.Category);
        }

        private static FailurePathAssemblyResult[] CheckFailureMechanismAssemblyResults(
            IEnumerable<FailurePathAssemblyResult> failureMechanismAssemblyResults)
        {
            if (failureMechanismAssemblyResults == null)
            {
                throw new AssemblyException("AssembleFailureMechanismResult", EAssemblyErrors.ValueMayNotBeNull);
            }

            List<FailurePathAssemblyResult> failureMechanismResults = failureMechanismAssemblyResults.ToList();

            if (failureMechanismResults.Count == 0)
            {
                throw new AssemblyException("AssembleFailureMechanismResult",
                                            EAssemblyErrors.FailureMechanismAssemblerInputInvalid);
            }

            return failureMechanismResults.ToArray();
        }
    }
}