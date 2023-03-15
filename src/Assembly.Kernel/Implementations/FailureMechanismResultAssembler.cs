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
        /// <inheritdoc />
        public Probability CalculateFailureMechanismFailureProbabilityBoi1A1(
            IEnumerable<Probability> failureMechanismSectionAssemblyResults,
            bool partialAssembly)
        {
            if (failureMechanismSectionAssemblyResults == null)
            {
                throw new ArgumentNullException(nameof(failureMechanismSectionAssemblyResults));
            }

            return CalculateFailureMechanismFailureProbabilityP1(failureMechanismSectionAssemblyResults, partialAssembly);
        }

        /// <inheritdoc />
        public Probability CalculateFailureMechanismFailureProbabilityBoi1A2(
            IEnumerable<Probability> failureMechanismSectionAssemblyResults,
            double lengthEffectFactor, bool partialAssembly)
        {
            if (failureMechanismSectionAssemblyResults == null)
            {
                throw new ArgumentNullException(nameof(failureMechanismSectionAssemblyResults));
            }

            return CalculateFailureMechanismFailureProbabilityP2(failureMechanismSectionAssemblyResults, lengthEffectFactor, partialAssembly);
        }

        /// <inheritdoc />
        public Probability CalculateFailureMechanismFailureProbabilityBoi1A3(
            IEnumerable<ResultWithProfileAndSectionProbabilities> failureMechanismSectionAssemblyResults,
            bool partialAssembly)
        {
            if (failureMechanismSectionAssemblyResults == null)
            {
                throw new ArgumentNullException(nameof(failureMechanismSectionAssemblyResults));
            }

            return CalculateFailureMechanismFailureProbabilityP1(failureMechanismSectionAssemblyResults.Select(fmsar => fmsar.ProbabilitySection),
                                                                 partialAssembly);
        }

        /// <inheritdoc />
        public Probability CalculateFailureMechanismFailureProbabilityBoi1A4(
            IEnumerable<ResultWithProfileAndSectionProbabilities> failureMechanismSectionAssemblyResults,
            double lengthEffectFactor, bool partialAssembly)
        {
            if (failureMechanismSectionAssemblyResults == null)
            {
                throw new ArgumentNullException(nameof(failureMechanismSectionAssemblyResults));
            }

            return CalculateFailureMechanismFailureProbabilityP2(failureMechanismSectionAssemblyResults.Select(fmsar => fmsar.ProbabilityProfile),
                                                                 lengthEffectFactor, partialAssembly);
        }

        /// <inheritdoc />
        public BoundaryLimits CalculateFailureMechanismBoundariesBoi1B1(
            IEnumerable<Probability> failureMechanismSectionAssemblyResults,
            bool partialAssembly)
        {
            if (failureMechanismSectionAssemblyResults == null)
            {
                throw new ArgumentNullException(nameof(failureMechanismSectionAssemblyResults));
            }

            return CalculateFailureMechanismBoundaries(failureMechanismSectionAssemblyResults, partialAssembly);
        }

        /// <inheritdoc />
        public BoundaryLimits CalculateFailureMechanismBoundariesBoi1B2(
            IEnumerable<ResultWithProfileAndSectionProbabilities> failureMechanismSectionAssemblyResults,
            bool partialAssembly)
        {
            if (failureMechanismSectionAssemblyResults == null)
            {
                throw new ArgumentNullException(nameof(failureMechanismSectionAssemblyResults));
            }

            return CalculateFailureMechanismBoundaries(failureMechanismSectionAssemblyResults.Select(fmsar => fmsar.ProbabilitySection),
                                                       partialAssembly);
        }

        /// <summary>
        /// Calculates a <see cref="Probability"/> from <paramref name="failureMechanismSectionAssemblyResults"/>.
        /// </summary>
        /// <param name="failureMechanismSectionAssemblyResults">The list of failure mechanism section assembly results.</param>
        /// <param name="partialAssembly">Indicator whether partial assembly is required.</param>
        /// <returns>The calculated <see cref="Probability"/>.</returns>
        /// <exception cref="AssemblyException">Thrown when:
        /// <list type="bullet">
        /// <item><paramref name="failureMechanismSectionAssemblyResults"/> is <c>empty</c>;</item>
        /// <item><paramref name="failureMechanismSectionAssemblyResults"/> contains <see cref="Probability.Undefined"/> probabilities
        /// when <paramref name="partialAssembly"/> is <c>false</c>.</item>
        /// </list>
        /// </exception>
        /// <remarks>When <paramref name="partialAssembly"/> is <c>true</c>, all <see cref="Probability.Undefined"/> probabilities are ignored.</remarks>
        private static Probability CalculateFailureMechanismFailureProbabilityP1(
            IEnumerable<Probability> failureMechanismSectionAssemblyResults,
            bool partialAssembly)
        {
            ValidateInput(failureMechanismSectionAssemblyResults, partialAssembly);

            if (partialAssembly)
            {
                failureMechanismSectionAssemblyResults = failureMechanismSectionAssemblyResults.Where(r => r.IsDefined);

                if (!failureMechanismSectionAssemblyResults.Any())
                {
                    return new Probability(0.0);
                }
            }

            return failureMechanismSectionAssemblyResults.Aggregate(
                (Probability) 1.0,
                (current, sectionProbability) => current * sectionProbability.Inverse).Inverse;
        }

        /// <summary>
        /// Calculates a <see cref="Probability"/> from <paramref name="failureMechanismSectionAssemblyResults"/>.
        /// </summary>
        /// <param name="failureMechanismSectionAssemblyResults">The list of failure mechanism section assembly results.</param>
        /// <param name="lengthEffectFactor">The length effect factor.</param>
        /// <param name="partialAssembly">Indicator whether partial assembly is required.</param>
        /// <returns>The calculated <see cref="Probability"/>.</returns>
        /// <exception cref="AssemblyException">Thrown when:
        /// <list type="bullet">
        /// <item><paramref name="failureMechanismSectionAssemblyResults"/> is <c>empty</c>;</item>
        /// <item><paramref name="failureMechanismSectionAssemblyResults"/> contains <see cref="Probability.Undefined"/> probabilities
        /// when <paramref name="partialAssembly"/> is <c>false</c>;</item>
        /// <item><paramref name="lengthEffectFactor"/> is &lt; 1.</item>
        /// </list>
        /// </exception>
        /// <remarks>When <paramref name="partialAssembly"/> is <c>true</c>, all <see cref="Probability.Undefined"/> probabilities are ignored.</remarks>
        private static Probability CalculateFailureMechanismFailureProbabilityP2(
            IEnumerable<Probability> failureMechanismSectionAssemblyResults,
            double lengthEffectFactor, bool partialAssembly)
        {
            ValidateInput(failureMechanismSectionAssemblyResults, lengthEffectFactor, partialAssembly);

            if (partialAssembly)
            {
                failureMechanismSectionAssemblyResults = failureMechanismSectionAssemblyResults.Where(
                    r => r.IsDefined);

                if (!failureMechanismSectionAssemblyResults.Any())
                {
                    return new Probability(0.0);
                }
            }

            double assemblyResult = (double) failureMechanismSectionAssemblyResults.Max(ar => ar) * lengthEffectFactor;
            return new Probability(Math.Min(1.0, assemblyResult));
        }

        /// <summary>
        /// Calculates <see cref="BoundaryLimits"/> from <paramref name="failureMechanismSectionAssemblyResults"/>.
        /// </summary>
        /// <param name="failureMechanismSectionAssemblyResults">The list of failure mechanism section assembly results.</param>
        /// <param name="partialAssembly">Indicator whether partial assembly is required.</param>
        /// <returns>The calculated <see cref="BoundaryLimits"/>.</returns>
        /// <exception cref="AssemblyException">Thrown when:
        /// <list type="bullet">
        /// <item><paramref name="failureMechanismSectionAssemblyResults"/> is <c>empty</c>;</item>
        /// <item><paramref name="failureMechanismSectionAssemblyResults"/> contains <see cref="Probability.Undefined"/> probabilities
        /// when <paramref name="partialAssembly"/> is <c>false</c>.</item>
        /// </list>
        /// </exception>
        /// <remarks>When <paramref name="partialAssembly"/> is <c>true</c>, all <see cref="Probability.Undefined"/> probabilities are ignored.</remarks>
        private static BoundaryLimits CalculateFailureMechanismBoundaries(IEnumerable<Probability> failureMechanismSectionAssemblyResults,
                                                                          bool partialAssembly)
        {
            ValidateInput(failureMechanismSectionAssemblyResults, partialAssembly);

            if (partialAssembly)
            {
                failureMechanismSectionAssemblyResults = failureMechanismSectionAssemblyResults.Where(p => p.IsDefined);

                if (!failureMechanismSectionAssemblyResults.Any())
                {
                    return new BoundaryLimits(new Probability(0.0), new Probability(0.0));
                }
            }

            return new BoundaryLimits(
                failureMechanismSectionAssemblyResults.Max(p => p),
                failureMechanismSectionAssemblyResults.Aggregate(
                    (Probability) 1.0,
                    (current, sectionProbability) => current * sectionProbability.Inverse).Inverse);
        }

        /// <summary>
        /// Validates the input.
        /// </summary>
        /// <param name="failureMechanismSectionAssemblyResults">The list of failure mechanism section assembly results.</param>
        /// <param name="partialAssembly">Indicator whether partial assembly is required.</param>
        /// <exception cref="AssemblyException">Thrown when:
        /// <list type="bullet">
        /// <item><paramref name="failureMechanismSectionAssemblyResults"/> is <c>null</c> or <c>empty</c>;</item>
        /// <item><paramref name="failureMechanismSectionAssemblyResults"/> contains <see cref="Probability.Undefined"/> probabilities
        /// when <paramref name="partialAssembly"/> is <c>false</c>.</item>
        /// </list>
        /// </exception>
        private static void ValidateInput(IEnumerable<Probability> failureMechanismSectionAssemblyResults,
                                          bool partialAssembly)
        {
            var errors = new List<AssemblyErrorMessage>();

            if (!failureMechanismSectionAssemblyResults.Any())
            {
                errors.Add(new AssemblyErrorMessage(nameof(failureMechanismSectionAssemblyResults), EAssemblyErrors.EmptyResultsList));
            }

            if (!errors.Any() && !partialAssembly && failureMechanismSectionAssemblyResults.Any(fmsar => !fmsar.IsDefined))
            {
                errors.Add(new AssemblyErrorMessage(nameof(failureMechanismSectionAssemblyResults),
                                                    EAssemblyErrors.EncounteredOneOrMoreSectionsWithoutResult));
            }

            if (errors.Any())
            {
                throw new AssemblyException(errors);
            }
        }

        /// <summary>
        /// Validates the input.
        /// </summary>
        /// <param name="failureMechanismSectionAssemblyResults">The list of failure mechanism section assembly results.</param>
        /// <param name="lengthEffectFactor">The length effect factor.</param>
        /// <param name="partialAssembly">Indicator whether partial assembly is required.</param>
        /// <exception cref="AssemblyException">Thrown when:
        /// <list type="bullet">
        /// <item><paramref name="failureMechanismSectionAssemblyResults"/> is <c>null</c> or <c>empty</c>;</item>
        /// <item><paramref name="failureMechanismSectionAssemblyResults"/> contains <see cref="Probability.Undefined"/> probabilities
        /// when <paramref name="partialAssembly"/> is <c>false</c>;</item>
        /// <item><paramref name="lengthEffectFactor"/> is &lt; 1.</item>
        /// </list>
        /// </exception>
        private static void ValidateInput(IEnumerable<Probability> failureMechanismSectionAssemblyResults,
                                          double lengthEffectFactor, bool partialAssembly)
        {
            var errors = new List<AssemblyErrorMessage>();

            if (!failureMechanismSectionAssemblyResults.Any())
            {
                errors.Add(new AssemblyErrorMessage(nameof(failureMechanismSectionAssemblyResults), EAssemblyErrors.EmptyResultsList));
            }

            if (lengthEffectFactor < 1.0)
            {
                errors.Add(new AssemblyErrorMessage(nameof(lengthEffectFactor), EAssemblyErrors.LengthEffectFactorOutOfRange));
            }

            if (!errors.Any() && !partialAssembly && failureMechanismSectionAssemblyResults.Any(fmsar => !fmsar.IsDefined))
            {
                errors.Add(new AssemblyErrorMessage(nameof(failureMechanismSectionAssemblyResults),
                                                    EAssemblyErrors.EncounteredOneOrMoreSectionsWithoutResult));
            }

            if (errors.Any())
            {
                throw new AssemblyException(errors);
            }
        }
    }
}