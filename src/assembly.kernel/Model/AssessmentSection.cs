#region Copyright (C) Rijkswaterstaat 2019. All rights reserved
// Copyright (C) Rijkswaterstaat 2019. All rights reserved.
//
// This file is part of the Assembly kernel.
//
// Assembly kernel is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program. If not, see <http://www.gnu.org/licenses/>.
//
// All names, logos, and references to "Rijkswaterstaat" are registered trademarks of
// Rijkswaterstaat and remain full property of Rijkswaterstaat at all times.
// All rights reserved.
#endregion

using Assembly.Kernel.Exceptions;
using Assembly.Kernel.Implementations.Validators;

namespace Assembly.Kernel.Model
{
    /// <summary>
    /// Assessment Section data object
    /// </summary>
    public class AssessmentSection
    {
        /// <summary>
        /// AssessmentSection Constructor
        /// </summary>
        /// <param name="length"> length of the section in meters</param>
        /// <param name="failureProbabilitySignallingLimit">signalling limit for failure 
        /// probability of the section in 1/years. Has to be between 0 and 1</param>
        /// <param name="failureProbabilityLowerLimit">lower limit for failure probability of the section in 1/years. 
        /// Has to be between 0 and 1</param>
        /// <exception cref="AssemblyException">Thrown when one of the input values is not valid</exception>
        public AssessmentSection(double length, double failureProbabilitySignallingLimit,
            double failureProbabilityLowerLimit)
        {
            AssessmentSectionValidator.CheckAssessmentSectionInput(length, failureProbabilitySignallingLimit,
                failureProbabilityLowerLimit);

            Length = length;
            FailureProbabilitySignallingLimit = failureProbabilitySignallingLimit;
            FailureProbabilityLowerLimit = failureProbabilityLowerLimit;
        }

        /// <summary>
        /// Length of the assesment section in meters.
        /// </summary>
        public double Length { get; }

        /// <summary>
        /// signalling limit for failure probability of the section in 1/years.
        /// </summary>
        public double FailureProbabilitySignallingLimit { get; }

        /// <summary>
        /// lower limit for failure probability of the section in 1/years. 
        /// </summary>
        public double FailureProbabilityLowerLimit { get; }

        /// <summary>
        /// Generates string from assessment section object.
        /// </summary>
        /// <returns>Text representation of the assessment section object</returns>
        public override string ToString()
        {
            return
                $"Failure prob signalling limit: {FailureProbabilitySignallingLimit}, " +
                $"Failure prob lower limit: {FailureProbabilityLowerLimit}";
        }
    }
}