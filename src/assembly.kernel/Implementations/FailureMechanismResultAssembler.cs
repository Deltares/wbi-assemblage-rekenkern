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

using System;
using System.Collections.Generic;
using System.Linq;
using Assembly.Kernel.Exceptions;
using Assembly.Kernel.Interfaces;
using Assembly.Kernel.Model;
using Assembly.Kernel.Model.FailureMechanismSections;

namespace Assembly.Kernel.Implementations
{
    /// <inheritdoc />
    public class FailureMechanismResultAssembler : IFailureMechanismResultAssembler
    {
        /// <inheritdoc />
        public FailureMechanismAssemblyResult CalculateFailureMechanismFailureProbabilityWithLengthEffectBoi1A2(
            double lengthEffectFactor,
            IEnumerable<ResultWithProfileAndSectionProbabilities> failureMechanismSectionAssemblyResults,
            bool partialAssembly)
        {
            var sectionResults = FailureMechanismSectionAssemblyResultsToArray(failureMechanismSectionAssemblyResults);
            CheckInput(sectionResults, lengthEffectFactor);

            if (partialAssembly)
            {
                sectionResults = sectionResults.Where(r => r.ProbabilitySection.IsDefined && r.ProbabilityProfile.IsDefined).ToArray();
                if (sectionResults.Length == 0)
                {
                    return new FailureMechanismAssemblyResult(new Probability(0), EFailureMechanismAssemblyMethod.Correlated);
                }
            }
            else
            {
                CheckForDefinedProbabilities(sectionResults);
            }

            var noFailureProbProduct = (Probability) 1.0;
            var highestFailureProbabilityProfile = 0.0;
            foreach (var sectionResult in sectionResults)
            {
                if (sectionResult.ProbabilityProfile > highestFailureProbabilityProfile)
                {
                    highestFailureProbabilityProfile = sectionResult.ProbabilityProfile;
                }

                noFailureProbProduct *= sectionResult.ProbabilitySection.Inverse;
            }

            highestFailureProbabilityProfile *= lengthEffectFactor;
            var combinedFailureProbabilityUncorrelated = noFailureProbProduct.Inverse;

            var correlation = FindCorrelation(sectionResults, highestFailureProbabilityProfile, combinedFailureProbabilityUncorrelated);
            var probabilityValue = (Probability) Math.Min(highestFailureProbabilityProfile, combinedFailureProbabilityUncorrelated);

            return new FailureMechanismAssemblyResult(probabilityValue, correlation);
        }

        /// <inheritdoc />
        public FailureMechanismAssemblyResult CalculateFailureMechanismFailureProbabilityBoi1A1(double lengthEffectFactor,
                                                                                                IEnumerable<Probability> failureMechanismSectionProbabilities, bool partialAssembly)
        {
            var sectionResults = FailureMechanismSectionProbabilitiesToProbabilityArray(failureMechanismSectionProbabilities);
            CheckInput(sectionResults, lengthEffectFactor);

            if (partialAssembly)
            {
                sectionResults = sectionResults.Where(r => r.IsDefined).ToArray();
                if (sectionResults.Length == 0)
                {
                    return new FailureMechanismAssemblyResult(new Probability(0),
                                                              EFailureMechanismAssemblyMethod.Correlated);
                }
            }
            else
            {
                CheckForDefinedProbabilities(sectionResults);
            }

            var noFailureProbProduct = (Probability) 1.0;
            var highestProbabilityValue = 0.0;

            foreach (var sectionProbability in sectionResults)
            {
                if (sectionProbability > highestProbabilityValue)
                {
                    highestProbabilityValue = sectionProbability;
                }

                noFailureProbProduct *= sectionProbability.Inverse;
            }

            highestProbabilityValue *= lengthEffectFactor;
            var combinedFailureProbabilityUncorrelated = noFailureProbProduct.Inverse;

            var correlation = sectionResults.Length < 2
                                  ? EFailureMechanismAssemblyMethod.Uncorrelated
                                  : highestProbabilityValue <= combinedFailureProbabilityUncorrelated
                                      ? EFailureMechanismAssemblyMethod.Correlated
                                      : EFailureMechanismAssemblyMethod.Uncorrelated;
            var probabilityValue = (Probability) Math.Min(highestProbabilityValue, combinedFailureProbabilityUncorrelated);

            return new FailureMechanismAssemblyResult(probabilityValue, correlation);
        }

        private static EFailureMechanismAssemblyMethod FindCorrelation(ResultWithProfileAndSectionProbabilities[] sectionResults,
                                                                       double highestFailureProbabilityProfile, Probability combinedFailureProbabilityUncorrelated)
        {
            var correlation = sectionResults.Length < 2 || highestFailureProbabilityProfile > combinedFailureProbabilityUncorrelated
                                  ? EFailureMechanismAssemblyMethod.Uncorrelated
                                  : EFailureMechanismAssemblyMethod.Correlated;
            return correlation;
        }

        private static ResultWithProfileAndSectionProbabilities[] FailureMechanismSectionAssemblyResultsToArray(
            IEnumerable<ResultWithProfileAndSectionProbabilities> failureMechanismSectionAssemblyResults)
        {
            if (failureMechanismSectionAssemblyResults == null)
            {
                throw new AssemblyException(nameof(failureMechanismSectionAssemblyResults), EAssemblyErrors.ValueMayNotBeNull);
            }

            return failureMechanismSectionAssemblyResults as ResultWithProfileAndSectionProbabilities[] ??
                   failureMechanismSectionAssemblyResults.ToArray();
        }

        private static Probability[] FailureMechanismSectionProbabilitiesToProbabilityArray(
            IEnumerable<Probability> failureMechanismSectionProbabilities)
        {
            if (failureMechanismSectionProbabilities == null)
            {
                throw new AssemblyException(nameof(failureMechanismSectionProbabilities), EAssemblyErrors.ValueMayNotBeNull);
            }

            return failureMechanismSectionProbabilities as Probability[] ??
                   failureMechanismSectionProbabilities.ToArray();
        }

        private static void CheckForDefinedProbabilities(ResultWithProfileAndSectionProbabilities[] sectionResults)
        {
            if (sectionResults.Any(r => !r.ProbabilityProfile.IsDefined || !r.ProbabilitySection.IsDefined))
            {
                throw new AssemblyException(nameof(FailureMechanismResultAssembler), EAssemblyErrors.EncounteredOneOrMoreSectionsWithoutResult);
            }
        }

        private static void CheckForDefinedProbabilities(Probability[] sectionResults)
        {
            if (sectionResults.Any(r => !r.IsDefined))
            {
                throw new AssemblyException(nameof(sectionResults), EAssemblyErrors.EncounteredOneOrMoreSectionsWithoutResult);
            }
        }

        private static void CheckInput(ResultWithProfileAndSectionProbabilities[] results, double lengthEffectFactor)
        {
            var errors = new List<AssemblyErrorMessage>();
            if (results.Length == 0)
            {
                errors.Add(new AssemblyErrorMessage(nameof(FailureMechanismResultAssembler), EAssemblyErrors.EmptyResultsList));
            }

            if (lengthEffectFactor < 1)
            {
                errors.Add(new AssemblyErrorMessage(nameof(lengthEffectFactor), EAssemblyErrors.LengthEffectFactorOutOfRange));
            }

            if (errors.Count > 0)
            {
                throw new AssemblyException(errors);
            }
        }

        private static void CheckInput(Probability[] results, double lengthEffectFactor)
        {
            var errors = new List<AssemblyErrorMessage>();
            if (results.Length == 0)
            {
                errors.Add(new AssemblyErrorMessage(nameof(FailureMechanismResultAssembler), EAssemblyErrors.EmptyResultsList));
            }

            if (lengthEffectFactor < 1)
            {
                errors.Add(new AssemblyErrorMessage(nameof(lengthEffectFactor), EAssemblyErrors.LengthEffectFactorOutOfRange));
            }

            if (errors.Count > 0)
            {
                throw new AssemblyException(errors);
            }
        }
    }
}