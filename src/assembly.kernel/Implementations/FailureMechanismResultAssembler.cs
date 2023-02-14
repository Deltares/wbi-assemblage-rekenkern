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

            Probability combinedFailureProbabilityUncorrelated = failureMechanismSectionAssemblyResults.Aggregate(
                (Probability) 1.0, (current, sectionProbability) => current * sectionProbability.Inverse).Inverse;

            double highestProbabilityValue = (double) failureMechanismSectionAssemblyResults.Max(p => p) * lengthEffectFactor;
            
            var correlation = EFailureMechanismAssemblyMethod.Correlated;
            
            if (failureMechanismSectionAssemblyResults.Count() < 2
                || highestProbabilityValue > combinedFailureProbabilityUncorrelated)
            {
                correlation = EFailureMechanismAssemblyMethod.Uncorrelated;
            }

            return new FailureMechanismAssemblyResult(
                new Probability(Math.Min(highestProbabilityValue, combinedFailureProbabilityUncorrelated)),
                correlation);
        }

        public FailureMechanismAssemblyResult CalculateFailureMechanismFailureProbabilityWithLengthEffectBoi1A2(
            double lengthEffectFactor,
            IEnumerable<ResultWithProfileAndSectionProbabilities> failureMechanismSectionAssemblyResults,
            bool partialAssembly)
        {
            CheckInput(failureMechanismSectionAssemblyResults, lengthEffectFactor, partialAssembly);

            if (partialAssembly)
            {
                failureMechanismSectionAssemblyResults = failureMechanismSectionAssemblyResults.Where(r => r.ProbabilitySection.IsDefined && r.ProbabilityProfile.IsDefined);
                if (!failureMechanismSectionAssemblyResults.Any())
                {
                    return new FailureMechanismAssemblyResult(new Probability(0.0), EFailureMechanismAssemblyMethod.Correlated);
                }
            }

            double highestFailureProbabilityProfile = (double) failureMechanismSectionAssemblyResults.Max(p => p.ProbabilityProfile) * lengthEffectFactor;
            Probability combinedFailureProbabilityUncorrelated = failureMechanismSectionAssemblyResults.Aggregate(
                (Probability) 1.0, (current, sectionResult) => current * sectionResult.ProbabilitySection.Inverse).Inverse;

            EFailureMechanismAssemblyMethod correlation = FindCorrelation(failureMechanismSectionAssemblyResults, highestFailureProbabilityProfile,
                                                                          combinedFailureProbabilityUncorrelated);
            var probabilityValue = (Probability) Math.Min(highestFailureProbabilityProfile, combinedFailureProbabilityUncorrelated);

            return new FailureMechanismAssemblyResult(probabilityValue, correlation);
        }

        private static EFailureMechanismAssemblyMethod FindCorrelation(IEnumerable<ResultWithProfileAndSectionProbabilities> sectionResults,
                                                                       double highestFailureProbabilityProfile,
                                                                       Probability combinedFailureProbabilityUncorrelated)
        {
            return sectionResults.Count() < 2
                   || highestFailureProbabilityProfile > combinedFailureProbabilityUncorrelated
                       ? EFailureMechanismAssemblyMethod.Uncorrelated
                       : EFailureMechanismAssemblyMethod.Correlated;
        }

        private static void CheckInput(IEnumerable<ResultWithProfileAndSectionProbabilities> failureMechanismSectionAssemblyResults, double lengthEffectFactor,
                                       bool partialAssembly)
        {
            var errors = new List<AssemblyErrorMessage>();
            
            if (failureMechanismSectionAssemblyResults == null)
            {
                throw new AssemblyException(nameof(failureMechanismSectionAssemblyResults), EAssemblyErrors.ValueMayNotBeNull);
            }
            
            if (!failureMechanismSectionAssemblyResults.Any())
            {
                errors.Add(new AssemblyErrorMessage(nameof(FailureMechanismResultAssembler), EAssemblyErrors.EmptyResultsList));
            }

            if (lengthEffectFactor < 1)
            {
                errors.Add(new AssemblyErrorMessage(nameof(lengthEffectFactor), EAssemblyErrors.LengthEffectFactorOutOfRange));
            }
            
            if(!partialAssembly && failureMechanismSectionAssemblyResults.Any(r => !r.ProbabilityProfile.IsDefined 
                                                                                   || !r.ProbabilitySection.IsDefined))
            {
                throw new AssemblyException(nameof(FailureMechanismResultAssembler), EAssemblyErrors.EncounteredOneOrMoreSectionsWithoutResult);
            }

            if (errors.Count > 0)
            {
                throw new AssemblyException(errors);
            }
        }

        private static void CheckInput(IEnumerable<Probability> failureMechanismSectionAssemblyResults, double lengthEffectFactor, bool partialAssembly)
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

            if (!partialAssembly && failureMechanismSectionAssemblyResults.Any(r => !r.IsDefined))
            {
                throw new AssemblyException(nameof(failureMechanismSectionAssemblyResults), EAssemblyErrors.EncounteredOneOrMoreSectionsWithoutResult);
            }

            if (errors.Count > 0)
            {
                throw new AssemblyException(errors);
            }
        }
    }
}