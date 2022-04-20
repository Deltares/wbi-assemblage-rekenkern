#region Copyright (C) Rijkswaterstaat 2022. All rights reserved.

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
using Assembly.Kernel.Model.Categories;

namespace Assembly.Kernel.Implementations
{
    /// <inheritdoc />
    public class AssessmentGradeAssembler : IAssessmentGradeAssembler
    {
        /// <inheritdoc />
        public Probability CalculateAssessmentSectionFailureProbabilityBoi2A1(
            IEnumerable<Probability> failureMechanismProbabilities,
            bool partialAssembly)
        {
            if (failureMechanismProbabilities == null)
            {
                throw new AssemblyException(nameof(failureMechanismProbabilities), EAssemblyErrors.ValueMayNotBeNull);
            }

            var failureProbabilitiesArray = PrepareResultsList(failureMechanismProbabilities, partialAssembly);
            var failureProbability = CalculateFailureProbabilityFromListOfProbabilities(failureProbabilitiesArray);

            return failureProbability;
        }

        /// <inheritdoc />
        public EAssessmentGrade DetermineAssessmentGradeBoi2B1(Probability failureProbability, CategoriesList<AssessmentSectionCategory> categories)
        {
            CheckForDefinedProbabilityAndCategories(failureProbability, categories);
            var category = categories.GetCategoryForFailureProbability(failureProbability);
            return category.Category;
        }

        private static Probability[] PrepareResultsList(IEnumerable<Probability> failureMechanismProbabilities, bool partialAssembly)
        {
            var failureProbabilitiesArray = failureMechanismProbabilities as Probability[] ??
                                            failureMechanismProbabilities.ToArray();

            CheckForProbabilities(failureProbabilitiesArray);
            if (partialAssembly)
            {
                failureProbabilitiesArray = failureProbabilitiesArray.Where(probability => probability.IsDefined).ToArray();
                CheckForProbabilities(failureProbabilitiesArray);
            }

            return failureProbabilitiesArray;
        }

        private static Probability CalculateFailureProbabilityFromListOfProbabilities(Probability[] failureProbabilitiesArray)
        {
            var failureProbabilityProduct = (Probability) 1.0;
            foreach (var probability in failureProbabilitiesArray)
            {
                if (!probability.IsDefined)
                {
                    throw new AssemblyException(nameof(Probability), EAssemblyErrors.UndefinedProbability);
                }

                failureProbabilityProduct *= probability.Inverse;
            }

            var failureProbability = failureProbabilityProduct.Inverse;
            return failureProbability;
        }

        private static void CheckForProbabilities(Probability[] probabilities)
        {
            if (probabilities.Length == 0)
            {
                throw new AssemblyException(nameof(probabilities), EAssemblyErrors.EmptyResultsList);
            }
        }

        private static void CheckForDefinedProbabilityAndCategories(Probability failureProbability, CategoriesList<AssessmentSectionCategory> categories)
        {
            var errors = new List<AssemblyErrorMessage>();
            if (!failureProbability.IsDefined)
            {
                errors.Add(new AssemblyErrorMessage(nameof(Probability), EAssemblyErrors.UndefinedProbability));
            }

            if (categories == null)
            {
                errors.Add(new AssemblyErrorMessage(nameof(categories), EAssemblyErrors.ValueMayNotBeNull));
            }

            if (errors.Count > 0)
            {
                throw new AssemblyException(errors);
            }
        }
    }
}