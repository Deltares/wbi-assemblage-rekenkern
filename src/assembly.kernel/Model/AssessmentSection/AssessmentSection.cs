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
using Assembly.Kernel.Exceptions;

namespace Assembly.Kernel.Model.AssessmentSection
{
    /// <summary>
    /// Assessment Section data object.
    /// </summary>
    public class AssessmentSection
    {
        /// <summary>
        /// <see cref="AssessmentSection"/> Constructor.
        /// </summary>
        /// <param name="signalFloodingProbability">Signal flooding probability of the section in 1/years. Has to be between 0 and 1.</param>
        /// <param name="maximumAllowableFloodingProbability">Maximum allowable flooding probability of the section in 1/years. 
        ///     Has to be between 0 and 1.</param>
        /// <exception cref="AssemblyException">Thrown when one of the input values is not valid.</exception>
        public AssessmentSection(Probability signalFloodingProbability,
                                 Probability maximumAllowableFloodingProbability)
        {
            if (signalFloodingProbability > maximumAllowableFloodingProbability)
            {
                throw new AssemblyException(nameof(AssessmentSection), EAssemblyErrors.SignalFloodingProbabilityAboveMaximumAllowableFloodingProbability);
            }

            SignalFloodingProbability = signalFloodingProbability;
            MaximumAllowableFloodingProbability = maximumAllowableFloodingProbability;
        }

        /// <summary>
        /// Signal flooding probability of the section in 1/years.
        /// </summary>
        public Probability SignalFloodingProbability { get; }

        /// <summary>
        /// Maximum allowable flooding probability of the section in 1/years. 
        /// </summary>
        public Probability MaximumAllowableFloodingProbability { get; }

        /// <summary>
        /// Generates string from assessment section object.
        /// </summary>
        /// <returns>Text representation of the assessment section object.</returns>
        public override string ToString()
        {
            return
                $"Signal flooding probability: {SignalFloodingProbability}, " + Environment.NewLine +
                $"Maximum allowable flooding probability: {MaximumAllowableFloodingProbability}";
        }
    }
}