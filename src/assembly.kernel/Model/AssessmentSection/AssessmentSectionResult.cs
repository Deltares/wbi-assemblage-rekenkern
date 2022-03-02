#region Copyright (C) Rijkswaterstaat 2022. All rights reserved

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
using Assembly.Kernel.Model.Categories;

namespace Assembly.Kernel.Model.AssessmentSection
{
    /// <summary>
    /// The assessment result for this assessment section
    /// </summary>
    public class AssessmentSectionResult
    {
        /// <summary>
        /// Constructor of AssessmentSectionResult
        /// </summary>
        /// <param name="failureProbability">The estimated probability of flooding of the assessment section</param>
        /// <param name="grade">The grade associated with the probability of flooding</param>
        /// <exception cref="AssemblyException">Thrown when the specified probability is less than 0.0 or greater than 1.0</exception>
        public AssessmentSectionResult(Probability failureProbability, EAssessmentGrade grade)
        {
            Category = grade;
            FailureProbability = failureProbability;
        }

        /// <summary>
        /// The estimated probability of flooding of the assessment section
        /// </summary>
        public double FailureProbability { get; }

        /// <summary>
        /// The grade associated with the probability of flooding
        /// </summary>
        public EAssessmentGrade Category { get; }
    }
}