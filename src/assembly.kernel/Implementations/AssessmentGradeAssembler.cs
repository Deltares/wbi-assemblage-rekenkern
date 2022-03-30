#region Copyright (C) Rijkswaterstaat 2022. All rights reserved

// Copyright (C) Rijkswaterstaat 2022. All rights reserved.
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
            IEnumerable<Probability> failureMechanismProbabilities,
            CategoriesList<AssessmentSectionCategory> categories,
            bool partialAssembly)
        {
            Probability[] failureMechanismProbabilitiesArray = CheckFailureMechanismAssemblyResults(failureMechanismProbabilities, categories);

            if (partialAssembly)
            {
                failureMechanismProbabilitiesArray = failureMechanismProbabilitiesArray.Where(probability => probability.IsDefined).ToArray();

                if (failureMechanismProbabilitiesArray.Length == 0)
                {
                    throw new AssemblyException("failureMechanismProbabilities", EAssemblyErrors.EmptyResultsList);
                }
            }

            var failureProbabilityProduct = 1.0;
            foreach (var probability in failureMechanismProbabilitiesArray)
            {
                if (!probability.IsDefined)
                {
                    throw new AssemblyException("failureMechanismProbabilities", EAssemblyErrors.ProbabilityMayNotBeUndefined);
                }

                failureProbabilityProduct *= 1.0 - probability;
            }

            var probabilityOfFailure = 1.0 - failureProbabilityProduct;
            var category = categories.GetCategoryForFailureProbability(new Probability(probabilityOfFailure));
            return new AssessmentSectionResult(new Probability(probabilityOfFailure), category.Category);
        }

        private static Probability[] CheckFailureMechanismAssemblyResults(IEnumerable<Probability> probabilities,
            CategoriesList<AssessmentSectionCategory> categories)
        {
            var errors = new List<AssemblyErrorMessage>();

            Probability[] probabilitiesArray = null;
            if (probabilities == null)
            {
                errors.Add(new AssemblyErrorMessage("AssembleFailureMechanismResult", EAssemblyErrors.ValueMayNotBeNull));
            }
            else
            {
                probabilitiesArray = probabilities as Probability[] ?? probabilities.ToArray();
                if (probabilitiesArray.Length == 0)
                {
                    errors.Add(new AssemblyErrorMessage("AssembleFailureMechanismResult", EAssemblyErrors.EmptyResultsList));
                }
            }

            if (categories == null)
            {
                errors.Add(new AssemblyErrorMessage("Categories", EAssemblyErrors.ValueMayNotBeNull));
            }

            if (errors.Count > 0)
            {
                throw new AssemblyException(errors);
            }

            return probabilitiesArray;
        }
    }
}