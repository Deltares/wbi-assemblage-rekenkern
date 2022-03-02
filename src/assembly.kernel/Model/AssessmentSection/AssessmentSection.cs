﻿#region Copyright (C) Rijkswaterstaat 2022. All rights reserved

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

using Assembly.Kernel.Exceptions;

namespace Assembly.Kernel.Model.AssessmentSection
{
    /// <summary>
    /// Assessment Section data object
    /// </summary>
    public class AssessmentSection
    {
        /// <summary>
        /// AssessmentSection Constructor
        /// </summary>
        /// <param name="failureProbabilitySignalingLimit">signaling limit for failure 
        ///     probability of the section in 1/years. Has to be between 0 and 1</param>
        /// <param name="failureProbabilityLowerLimit">lower limit for failure probability of the section in 1/years. 
        ///     Has to be between 0 and 1</param>
        /// <exception cref="AssemblyException">Thrown when one of the input values is not valid</exception>
        public AssessmentSection(Probability failureProbabilitySignalingLimit,
            Probability failureProbabilityLowerLimit)
        {
            if (failureProbabilitySignalingLimit > failureProbabilityLowerLimit)
            {
                throw new AssemblyException("AssessmentSection", EAssemblyErrors.SignalingLimitAboveLowerLimit);
            }
            
            FailureProbabilitySignalingLimit = failureProbabilitySignalingLimit;
            FailureProbabilityLowerLimit = failureProbabilityLowerLimit;
        }

        /// <summary>
        /// signaling limit for failure probability of the section in 1/years.
        /// </summary>
        public Probability FailureProbabilitySignalingLimit { get; }

        /// <summary>
        /// lower limit for failure probability of the section in 1/years. 
        /// </summary>
        public Probability FailureProbabilityLowerLimit { get; }

        /// <summary>
        /// Generates string from assessment section object.
        /// </summary>
        /// <returns>Text representation of the assessment section object</returns>
        public override string ToString()
        {
            return
                $"Failure prob signaling limit: {FailureProbabilitySignalingLimit}, " +
                $"Failure prob lower limit: {FailureProbabilityLowerLimit}";
        }
    }
}