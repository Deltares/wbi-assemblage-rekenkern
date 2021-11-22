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
using Assembly.Kernel.Model.FailurePaths;

namespace Assembly.Kernel.Implementations
{
    /// <inheritdoc />
    public class FailurePathResultAssembler : IFailurePathResultAssembler
    {
        /// <inheritdoc />
        public Probability AssembleFailurePathWbi1B1(
            FailurePath failurePath,
            IEnumerable<FailurePathSectionAssemblyResult> fmSectionAssemblyResults,
            bool partialAssembly)
        {
            FailurePathSectionAssemblyResult[] sectionResults = CheckInput(fmSectionAssemblyResults);

            if (partialAssembly)
            {
                sectionResults = sectionResults.Where(r => !double.IsNaN(r.ProbabilitySection.Value)).ToArray();
            }

            if (sectionResults.All(r => double.IsNaN(r.ProbabilitySection.Value)) || sectionResults.Length == 0)
            {
                return Probability.NaN;
            }

            // step 1: Ptraject = 1 - Product(1-Pi){i=1 -> N} where N is the number of failure path sections.
            // TODO: Merge this with method WBI-0A-2 which contains the same logic.
            var noFailureProbProduct = 1.0;
            var highestFailureProbability = 0.0;

            foreach (var fpSectionResult in sectionResults)
            {
                if (double.IsNaN(fpSectionResult.ProbabilitySection.Value))
                {
                    return Probability.NaN;
                }

                var sectionFailureProb = fpSectionResult.ProbabilitySection;
                var profileFailureProb = fpSectionResult.ProbabilityProfile;
                if (sectionFailureProb > highestFailureProbability)
                {
                    highestFailureProbability = profileFailureProb;
                }

                noFailureProbProduct *= 1.0 - sectionFailureProb;
            }

            var failurePathFailureProbability = 1 - noFailureProbProduct;

            // step 2: Get section with largest failure probability and multiply with Assessment section length effect factor.
            highestFailureProbability *= failurePath.LengthEffectFactor;
            // step 3: Compare the Failure probabilities from step 1 and 2 and use the lowest of the two.
            var resultFailureProb = Math.Min(highestFailureProbability, failurePathFailureProbability);
            // step 4: Return the category + failure probability
            return new Probability(resultFailureProb);
        }

        private static FailurePathSectionAssemblyResult[] CheckInput(
            IEnumerable<FailurePathSectionAssemblyResult> results)
        {
            if (results == null)
            {
                throw new AssemblyException("AssembleFailurePathResult", EAssemblyErrors.ValueMayNotBeNull);
            }

            var sectionResults = results.ToArray();

            // result list should not be empty
            if (sectionResults.Length == 0)
            {
                throw new AssemblyException("AssembleFailurePathResult",
                    EAssemblyErrors.EmptyResultsList);
            }

            return sectionResults;
        }
    }
}