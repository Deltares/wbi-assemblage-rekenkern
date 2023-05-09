// Copyright (C) Stichting Deltares and State of the Netherlands 2023. All rights reserved.
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
// All names, logos, and references to "Deltares" are registered trademarks of
// Stichting Deltares and remain full property of Stichting Deltares at all times.
// All rights reserved.

using Assembly.Kernel.Exceptions;

namespace Assembly.Kernel.Model
{
    /// <summary>
    /// Assessment Section data object.
    /// </summary>
    public class AssessmentSection
    {
        /// <summary>
        /// Create a new instance of <see cref="AssessmentSection"/>.
        /// </summary>
        /// <param name="signalFloodingProbability">Signal flooding probability of the section in 1/years.</param>
        /// <param name="maximumAllowableFloodingProbability">Maximum allowable flooding probability of the section in 1/years.</param>
        /// <exception cref="AssemblyException">Thrown when <paramref name="signalFloodingProbability"/>
        /// &gt; <paramref name="maximumAllowableFloodingProbability"/></exception>.
        public AssessmentSection(Probability signalFloodingProbability, Probability maximumAllowableFloodingProbability)
        {
            if (signalFloodingProbability > maximumAllowableFloodingProbability)
            {
                throw new AssemblyException(nameof(AssessmentSection),
                                            EAssemblyErrors.SignalFloodingProbabilityAboveMaximumAllowableFloodingProbability);
            }

            SignalFloodingProbability = signalFloodingProbability;
            MaximumAllowableFloodingProbability = maximumAllowableFloodingProbability;
        }

        /// <summary>
        /// Gets the signal flooding probability of the section in 1/years.
        /// </summary>
        public Probability SignalFloodingProbability { get; }

        /// <summary>
        /// Gets the maximum allowable flooding probability of the section in 1/years. 
        /// </summary>
        public Probability MaximumAllowableFloodingProbability { get; }
    }
}