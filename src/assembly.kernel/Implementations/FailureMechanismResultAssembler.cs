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

            return CreateFailureMechanismAssemblyResult(failureMechanismSectionAssemblyResults, p => p, p => p, lengthEffectFactor);
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

            return CreateFailureMechanismAssemblyResult(failureMechanismSectionAssemblyResults, sectionResult => sectionResult.ProbabilityProfile,
                                                        sectionResult => sectionResult.ProbabilitySection, lengthEffectFactor);
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

        private static double GetHighestProbabilityValue<T>(IEnumerable<T> failureMechanismSectionAssemblyResults,
                                                            Func<T, Probability> getProbabilityFunc,
                                                            double lengthEffectFactor)
        {
            return (double) failureMechanismSectionAssemblyResults.Max(getProbabilityFunc) * lengthEffectFactor;
        }

        private static Probability GetCombinedFailureProbabilityUncorrelated<T>(IEnumerable<T> failureMechanismSectionAssemblyResults,
                                                                                Func<T, Probability> getProbabilityFunc)
        {
            return failureMechanismSectionAssemblyResults.Aggregate(
                (Probability) 1.0, (current, sectionProbability) => current * getProbabilityFunc(sectionProbability).Inverse).Inverse;
        }

        private static FailureMechanismAssemblyResult CreateFailureMechanismAssemblyResult<T>(
            IEnumerable<T> failureMechanismSectionAssemblyResults,
            Func<T, Probability> getHighestProbabilityValueFunc,
            Func<T, Probability> getCombinedFailureProbabilityUncorrelatedFunc,
            double lengthEffectFactor)
        {
            double highestProbabilityValue = GetHighestProbabilityValue(
                failureMechanismSectionAssemblyResults, getHighestProbabilityValueFunc, lengthEffectFactor);

            Probability combinedFailureProbabilityUncorrelated = GetCombinedFailureProbabilityUncorrelated(
                failureMechanismSectionAssemblyResults, getCombinedFailureProbabilityUncorrelatedFunc);

            EFailureMechanismAssemblyMethod assemblyMethod =
                failureMechanismSectionAssemblyResults.Count() < 2 || highestProbabilityValue > combinedFailureProbabilityUncorrelated
                    ? EFailureMechanismAssemblyMethod.Uncorrelated
                    : EFailureMechanismAssemblyMethod.Correlated;

            return new FailureMechanismAssemblyResult(
                new Probability(Math.Min(highestProbabilityValue, combinedFailureProbabilityUncorrelated)),
                assemblyMethod);
        }
    }
}