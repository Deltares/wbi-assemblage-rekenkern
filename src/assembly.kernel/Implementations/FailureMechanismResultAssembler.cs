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

using System;
using System.Collections.Generic;
using System.Linq;
using Assembly.Kernel.Exceptions;
using Assembly.Kernel.Interfaces;
using Assembly.Kernel.Model;
using Assembly.Kernel.Model.FailureMechanismSections;

namespace Assembly.Kernel.Implementations
{
    /// <summary>
    /// Assemble failure mechanism section results to a failure mechanism result.
    /// </summary>
    public class FailureMechanismResultAssembler : IFailureMechanismResultAssembler
    {
        public FailureMechanismAssemblyResult CalculateFailureMechanismFailureProbabilityBoi1A1(
            double lengthEffectFactor,
            IEnumerable<Probability> failureMechanismSectionAssemblyResults,
            bool partialAssembly)
        {
            CheckInput(failureMechanismSectionAssemblyResults, lengthEffectFactor, partialAssembly);

            if (partialAssembly)
            {
                failureMechanismSectionAssemblyResults = failureMechanismSectionAssemblyResults.Where(r => r.IsDefined);

                if (!failureMechanismSectionAssemblyResults.Any())
                {
                    return new FailureMechanismAssemblyResult(new Probability(0.0), EFailureMechanismAssemblyMethod.Correlated);
                }
            }

            double highestProbabilityValue = (double) failureMechanismSectionAssemblyResults.Max(p => p) * lengthEffectFactor;
            Probability combinedFailureProbabilityUncorrelated = failureMechanismSectionAssemblyResults.Aggregate(
                (Probability) 1.0, (current, sectionProbability) => current * sectionProbability.Inverse).Inverse;

            return new FailureMechanismAssemblyResult(
                new Probability(Math.Min(highestProbabilityValue, combinedFailureProbabilityUncorrelated)),
                FindCorrelation(failureMechanismSectionAssemblyResults, highestProbabilityValue, combinedFailureProbabilityUncorrelated));
        }

        public FailureMechanismAssemblyResult CalculateFailureMechanismFailureProbabilityWithLengthEffectBoi1A2(
            double lengthEffectFactor,
            IEnumerable<ResultWithProfileAndSectionProbabilities> failureMechanismSectionAssemblyResults,
            bool partialAssembly)
        {
            CheckInput(failureMechanismSectionAssemblyResults, lengthEffectFactor, partialAssembly);

            if (partialAssembly)
            {
                failureMechanismSectionAssemblyResults = failureMechanismSectionAssemblyResults.Where(r => r.ProbabilitySection.IsDefined
                                                                                                           && r.ProbabilityProfile.IsDefined);
                if (!failureMechanismSectionAssemblyResults.Any())
                {
                    return new FailureMechanismAssemblyResult(new Probability(0.0), EFailureMechanismAssemblyMethod.Correlated);
                }
            }

            double highestFailureProbabilityProfile = (double) failureMechanismSectionAssemblyResults.Max(sectionResult => sectionResult.ProbabilityProfile)
                                                      * lengthEffectFactor;
            Probability combinedFailureProbabilityUncorrelated = failureMechanismSectionAssemblyResults.Aggregate(
                (Probability) 1.0, (current, sectionResult) => current * sectionResult.ProbabilitySection.Inverse).Inverse;

            return new FailureMechanismAssemblyResult(
                new Probability(Math.Min(highestFailureProbabilityProfile, combinedFailureProbabilityUncorrelated)),
                FindCorrelation(failureMechanismSectionAssemblyResults, highestFailureProbabilityProfile, combinedFailureProbabilityUncorrelated));
        }

        private static void CheckInput(IEnumerable<Probability> failureMechanismSectionAssemblyResults,
                                       double lengthEffectFactor,
                                       bool partialAssembly)
        {
            List<AssemblyErrorMessage> errors = CheckInput(failureMechanismSectionAssemblyResults, lengthEffectFactor).ToList();

            if (!errors.Any() && !partialAssembly)
            {
                foreach (Probability failureMechanismSectionAssemblyResult in failureMechanismSectionAssemblyResults)
                {
                    if (!failureMechanismSectionAssemblyResult.IsDefined)
                    {
                        errors.Add(new AssemblyErrorMessage(nameof(failureMechanismSectionAssemblyResult),
                                                            EAssemblyErrors.EncounteredOneOrMoreSectionsWithoutResult));
                        break;
                    }
                }
            }

            if (errors.Any())
            {
                throw new AssemblyException(errors);
            }
        }

        private static void CheckInput(IEnumerable<ResultWithProfileAndSectionProbabilities> failureMechanismSectionAssemblyResults,
                                       double lengthEffectFactor,
                                       bool partialAssembly)
        {
            List<AssemblyErrorMessage> errors = CheckInput(failureMechanismSectionAssemblyResults, lengthEffectFactor).ToList();

            if (!errors.Any() && !partialAssembly)
            {
                foreach (ResultWithProfileAndSectionProbabilities failureMechanismSectionAssemblyResult in failureMechanismSectionAssemblyResults)
                {
                    if (!failureMechanismSectionAssemblyResult.ProbabilitySection.IsDefined
                        || !failureMechanismSectionAssemblyResult.ProbabilityProfile.IsDefined)
                    {
                        errors.Add(new AssemblyErrorMessage(nameof(failureMechanismSectionAssemblyResult),
                                                            EAssemblyErrors.EncounteredOneOrMoreSectionsWithoutResult));
                        break;
                    }
                }
            }

            if (errors.Any())
            {
                throw new AssemblyException(errors);
            }
        }

        private static IEnumerable<AssemblyErrorMessage> CheckInput<T>(IEnumerable<T> failureMechanismSectionAssemblyResults, double lengthEffectFactor)
        {
            var errors = new List<AssemblyErrorMessage>();

            if (failureMechanismSectionAssemblyResults == null)
            {
                throw new AssemblyException(nameof(failureMechanismSectionAssemblyResults), EAssemblyErrors.ValueMayNotBeNull);
            }

            if (!failureMechanismSectionAssemblyResults.Any())
            {
                errors.Add(new AssemblyErrorMessage(nameof(failureMechanismSectionAssemblyResults), EAssemblyErrors.EmptyResultsList));
            }

            if (lengthEffectFactor < 1)
            {
                errors.Add(new AssemblyErrorMessage(nameof(lengthEffectFactor), EAssemblyErrors.LengthEffectFactorOutOfRange));
            }

            return errors;
        }
        
        private static EFailureMechanismAssemblyMethod FindCorrelation<T>(IEnumerable<T> failureMechanismSectionAssemblyResults,
                                                                          double highestProbabilityValue,
                                                                          Probability combinedFailureProbabilityUncorrelated)
        {
            return failureMechanismSectionAssemblyResults.Count() < 2
                   || highestProbabilityValue > combinedFailureProbabilityUncorrelated
                       ? EFailureMechanismAssemblyMethod.Uncorrelated
                       : EFailureMechanismAssemblyMethod.Correlated;
        }
    }
}