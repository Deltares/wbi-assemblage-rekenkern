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

using Assembly.Kernel.Exceptions;

namespace Assembly.Kernel.Model.FmSectionTypes
{
    /// <summary>
    /// Failure mechanism assessment translation result for direct failure mechanisms, including estimated probability of failure.
    /// </summary>
    public class FmSectionAssemblyDirectResultWithProbability : FmSectionAssemblyDirectResult
    {
        /// <summary>
        /// Constructor of the direct failure mechanism assembly result with failure probability.
        /// </summary>
        /// <param name="result">The translated category type of the result</param>
        /// <param name="failureProbability">The failure probability of the failure mechanism section</param>
        /// <exception cref="AssemblyException">Thrown when failure probability is &lt;0 or &gt;1</exception>
        public FmSectionAssemblyDirectResultWithProbability(EFmSectionCategory result, double failureProbability) :
            base(result)
        {
            if (failureProbability < 0.0 || failureProbability > 1.0)
            {
                throw new AssemblyException("FmSectionAssemblyDirectResultWithProbability",
                                            EAssemblyErrors.FailureProbabilityOutOfRange);
            }

            FailureProbability = failureProbability;
        }

        /// <summary>
        /// Optional failure probability originating from the failure mechanism section assessment result.
        /// This field can be null!
        /// </summary>
        public double FailureProbability { get; }

        /// <summary>
        /// Convert to string
        /// </summary>
        /// <returns>String of the object</returns>
        public override string ToString()
        {
            return "FmSectionAssemblyDirectResultWithProbability [" + Result + " P: " + FailureProbability + "]";
        }
    }
}