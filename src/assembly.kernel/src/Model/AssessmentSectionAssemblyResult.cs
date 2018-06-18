#region Copyright (c) 2018 Technolution BV. All Rights Reserved. 
// // Copyright (C) Technolution BV. 2018. All rights reserved.
// //
// // This file is part of the Assembly kernel.
// //
// // Assembly kernel is free software: you can redistribute it and/or modify
// // it under the terms of the GNU Lesser General Public License as published by
// // the Free Software Foundation, either version 3 of the License, or
// // (at your option) any later version.
// // 
// // This program is distributed in the hope that it will be useful,
// // but WITHOUT ANY WARRANTY; without even the implied warranty of
// // MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// // GNU Lesser General Public License for more details.
// //
// // You should have received a copy of the GNU Lesser General Public License
// // along with this program. If not, see <http://www.gnu.org/licenses/>.
// //
// // All names, logos, and references to "Technolution BV" are registered trademarks of
// // Technolution BV and remain full property of Technolution BV at all times.
// // All rights reserved.
#endregion

namespace Assembly.Kernel.Model {
    /// <summary>
    /// Assessment section assembly result.
    /// </summary>
    public class AssessmentSectionAssemblyResult {
        /// <summary>
        /// The assessment grade.
        /// </summary>
        public EAssessmentGrade Category { get; }
        /// <summary>
        /// The failure probability belonging to the result.
        /// If the failure mechanism is without failure probability this property will be null.
        /// </summary>
        public double FailureProbability { get; }

        /// <summary>
        /// AssessmentSectionAssemblyResult constructor without a failure probability.
        /// </summary>
        /// <param name="category">The assembly assessment grade</param>
        public AssessmentSectionAssemblyResult(EAssessmentGrade category) {
            Category = category;
            FailureProbability = double.NaN;
        }

        /// <summary>
        /// AssessmentSectionAssemblyResult constructor with a failure probability.
        /// </summary>
        /// <param name="category">The assembly assessment grade</param>
        /// <param name="failureProbability">The failure probability of the assessment</param>
        public AssessmentSectionAssemblyResult(EAssessmentGrade category, double failureProbability) {
            Category = category;
            FailureProbability = failureProbability;
        }

        /// <summary>
        /// Creates a new Assessment section assembly result from the current result.
        /// </summary>
        /// <returns>The newly created assembly result</returns>
        public AssessmentSectionAssemblyResult CreateNewFrom() {
            return double.IsNaN(FailureProbability) ?
                new AssessmentSectionAssemblyResult(Category) :
                new AssessmentSectionAssemblyResult(Category, FailureProbability);
        }
    }
}