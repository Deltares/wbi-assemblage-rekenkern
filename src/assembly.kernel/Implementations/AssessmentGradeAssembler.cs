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

namespace Assembly.Kernel.Implementations
{
    /// <inheritdoc />
    public class AssessmentGradeAssembler : IAssessmentGradeAssembler
    {
        /// <inheritdoc />
        public AssessmentSectionResult AssembleAssessmentSectionWbi2B1(
            IEnumerable<Probability> failurePathProbabilities,
            CategoriesList<AssessmentSectionCategory> categories,
            bool partialAssembly)
        {
            Probability[] failurePathProbabilitiesArray = CheckFailurePathAssemblyResults(failurePathProbabilities);

            if (categories == null)
            {
                throw new AssemblyException("Categories", EAssemblyErrors.ValueMayNotBeNull);
            }

            if (partialAssembly)
            {
                failurePathProbabilitiesArray = failurePathProbabilitiesArray.Where(probability =>
                        !double.IsNaN(probability.Value))
                    .ToArray();
            }

            if (failurePathProbabilitiesArray.All(probability => double.IsNaN(probability.Value)))
            {
                return new AssessmentSectionResult(Probability.NaN, EAssessmentGrade.Gr);
            }

            var failureProbabilityProduct = 1.0;
            foreach (var probability in failurePathProbabilitiesArray)
            {
                if (double.IsNaN(probability.Value))
                {
                    return new AssessmentSectionResult(Probability.NaN, EAssessmentGrade.Gr);
                }

                failureProbabilityProduct *= 1.0 - probability;
            }

            var probabilityOfFailure = 1.0 - failureProbabilityProduct;
            var category = categories.GetCategoryForFailureProbability(new Probability(probabilityOfFailure));
            return new AssessmentSectionResult(new Probability(probabilityOfFailure), category.Category);
        }

        private static Probability[] CheckFailurePathAssemblyResults(
            IEnumerable<Probability> probabilities)
        {
            if (probabilities == null)
            {
                throw new AssemblyException("AssembleFailurePathResult", EAssemblyErrors.ValueMayNotBeNull);
            }

            Probability[] probabilitiesArray = probabilities.ToArray();

            if (probabilitiesArray.Length == 0)
            {
                throw new AssemblyException("AssembleFailurePathResult",
                    EAssemblyErrors.EmptyResultsList);
            }

            return probabilitiesArray;
        }
    }
}