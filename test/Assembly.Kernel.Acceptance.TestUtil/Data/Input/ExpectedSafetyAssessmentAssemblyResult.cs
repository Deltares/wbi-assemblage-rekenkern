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

using Assembly.Kernel.Model;

namespace Assembly.Kernel.Acceptance.TestUtil.Data.Input
{
    /// <summary>
    /// The safety assessment assembly result.
    /// </summary>
    public class ExpectedSafetyAssessmentAssemblyResult
    {
        /// <summary>
        /// The expected estimated probability of flooding for the combined
        /// failure mechanisms.
        /// </summary>
        public Probability CombinedProbability { get; set; }

        /// <summary>
        /// The expected assessment grade.
        /// </summary>
        public EExpectedAssessmentGrade CombinedAssessmentGrade { get; set; }

        /// <summary>
        /// The expected estimated probability of flooding for the combined
        /// failure mechanisms as a result of partial assessment.
        /// </summary>
        public Probability CombinedProbabilityPartial { get; set; }

        /// <summary>
        /// The expected assessment grade as a result of partial assessment.
        /// </summary>
        public EExpectedAssessmentGrade CombinedAssessmentGradePartial { get; set; }
    }
}