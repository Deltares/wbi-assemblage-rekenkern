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

using System;
using System.Collections.Generic;
using System.Linq;
using Assembly.Kernel.Exceptions;
using Assembly.Kernel.Interfaces;
using Assembly.Kernel.Model;
using Assembly.Kernel.Model.Categories;
using Assembly.Kernel.Model.FailureMechanismSections;

namespace Assembly.Kernel.Implementations
{
    /// <inheritdoc />
    public class FailureMechanismResultAssembler : IFailureMechanismResultAssembler
    {
        /// <inheritdoc />
        public FailureMechanismAssemblyResult CalculateFailureMechanismFailureProbabilityBoi1A1(
            double lengthEffectFactor,
            IEnumerable<FailureMechanismSectionAssemblyResult> failureMechanismSectionAssemblyResults,
            bool partialAssembly)
        {
            var sectionResults = CheckInput(failureMechanismSectionAssemblyResults, lengthEffectFactor);

            if (partialAssembly)
            {
                sectionResults = sectionResults.Where(r =>
                    r.InterpretationCategory != EInterpretationCategory.Dominant &&
                    r.InterpretationCategory != EInterpretationCategory.Gr).ToArray();
                if (sectionResults.Length == 0)
                {
                    return new FailureMechanismAssemblyResult(new Probability(0),
                        EFailureMechanismAssemblyMethod.Correlated);
                }
            }
            else
            {
                CheckForValidResults(sectionResults);
            }

            var noFailureProbProduct = (Probability)1.0;
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

                noFailureProbProduct *= sectionResult.ProbabilitySection.Complement;
            }

            highestFailureProbabilityProfile *= lengthEffectFactor;
            var combinedFailureProbabilityUnCorrelated = noFailureProbProduct.Complement;

            var correlation = sectionResults.Length < 2
                ? EFailureMechanismAssemblyMethod.UnCorrelated
                : highestFailureProbabilityProfile <= combinedFailureProbabilityUnCorrelated
                    ? EFailureMechanismAssemblyMethod.Correlated
                    : EFailureMechanismAssemblyMethod.UnCorrelated;
            var probabilityValue = (Probability)Math.Min(highestFailureProbabilityProfile,combinedFailureProbabilityUnCorrelated);

            return new FailureMechanismAssemblyResult(probabilityValue, correlation);
        }

        private static void CheckForValidResults(FailureMechanismSectionAssemblyResult[] sectionResults)
        {
            var errors = new List<AssemblyErrorMessage>();
            if (sectionResults.Any(r => r.InterpretationCategory == EInterpretationCategory.Gr))
            {
                errors.Add(new AssemblyErrorMessage("failureMechanismSectionAssemblyResults",
                    EAssemblyErrors.EncounteredOneOrMoreSectionsWithoutResult));
            }

            if (sectionResults.Any(r => r.InterpretationCategory == EInterpretationCategory.Dominant))
            {
                errors.Add(new AssemblyErrorMessage("failureMechanismSectionAssemblyResults",
                    EAssemblyErrors.DominantSectionCannotBeAssembled));
            }

            if (errors.Any())
            {
                throw new AssemblyException(errors);
            }
        }

        private static FailureMechanismSectionAssemblyResult[] CheckInput(
            IEnumerable<FailureMechanismSectionAssemblyResult> results, double lengthEffectFactor)
        {
            if (results == null)
            {
                throw new AssemblyException("results", EAssemblyErrors.ValueMayNotBeNull);
            }

            var sectionResults = results.ToArray();

            // result list should not be empty
            if (sectionResults.Length == 0)
            {
                throw new AssemblyException("AssembleFailureMechanismResult", EAssemblyErrors.EmptyResultsList);
            }

            if (lengthEffectFactor < 1)
            {
                throw new AssemblyException("FailureMechanism", EAssemblyErrors.LengthEffectFactorOutOfRange);
            }

            return sectionResults;
        }
    }
}