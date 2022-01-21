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

using System;
using System.Collections.Generic;
using System.Linq;
using Assembly.Kernel.Exceptions;
using Assembly.Kernel.Interfaces;
using Assembly.Kernel.Model;
using Assembly.Kernel.Model.Categories;
using Assembly.Kernel.Model.FailurePathSections;

namespace Assembly.Kernel.Implementations
{
    /// <inheritdoc />
    public class FailurePathResultAssembler : IFailurePathResultAssembler
    {
        /// <inheritdoc />
        public Probability AssembleFailurePathWbi1B1(
            double lengthEffectFactor,
            IEnumerable<FailurePathSectionAssemblyResult> failurePathSectionAssemblyResults,
            bool partialAssembly)
        {
            var sectionResults = CheckInput(failurePathSectionAssemblyResults, lengthEffectFactor);

            if (partialAssembly)
            {
                sectionResults = sectionResults.Where(r => !double.IsNaN(r.ProbabilitySection.Value) && r.InterpretationCategory != EInterpretationCategory.Dominant).ToArray();
                if (sectionResults.Length == 0)
                {
                    return new Probability(0);
                }
            }

            var errors = new List<AssemblyErrorMessage>();
            if (sectionResults.Any(r => r.InterpretationCategory != EInterpretationCategory.Dominant && r.InterpretationCategory != EInterpretationCategory.NotDominant &&
                                        (double.IsNaN(r.ProbabilityProfile) || double.IsNaN(r.ProbabilitySection))))
            {
                errors.Add(new AssemblyErrorMessage("failurePathSectionAssemblyResults", EAssemblyErrors.EncounteredOneOrMoreSectionsWithoutResult));
            }

            if (sectionResults.Any(r => r.InterpretationCategory == EInterpretationCategory.Dominant))
            {
                errors.Add(new AssemblyErrorMessage("failurePathSectionAssemblyResults", EAssemblyErrors.DominantSectionCannotBeAssembled));
            }

            if (errors.Any())
            {
                throw new AssemblyException(errors);
            }

            var noFailureProbProduct = 1.0;
            var highestFailureProbabilityProfile = 0.0;

            foreach (var sectionResult in sectionResults)
            {
                if (sectionResult.InterpretationCategory == EInterpretationCategory.NotDominant)
                {
                    continue;
                }

                if (sectionResult.ProbabilityProfile > highestFailureProbabilityProfile)
                {
                    highestFailureProbabilityProfile = sectionResult.ProbabilityProfile;
                }

                noFailureProbProduct *= 1.0 - sectionResult.ProbabilitySection;
            }

            highestFailureProbabilityProfile *= lengthEffectFactor;
            var resultFailureProb = Math.Min(highestFailureProbabilityProfile, 1 - noFailureProbProduct);
            return new Probability(resultFailureProb);
        }

        private static FailurePathSectionAssemblyResult[] CheckInput(
            IEnumerable<FailurePathSectionAssemblyResult> results, double lengthEffectFactor)
        {
            if (results == null)
            {
                throw new AssemblyException("results", EAssemblyErrors.ValueMayNotBeNull);
            }

            var sectionResults = results.ToArray();

            // result list should not be empty
            if (sectionResults.Length == 0)
            {
                throw new AssemblyException("AssembleFailurePathResult", EAssemblyErrors.EmptyResultsList);
            }

            if (lengthEffectFactor < 1)
            {
                throw new AssemblyException("FailurePath", EAssemblyErrors.LengthEffectFactorOutOfRange);
            }

            return sectionResults;
        }
    }
}