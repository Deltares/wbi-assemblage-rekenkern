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

using Assembly.Kernel.Model;
using Assembly.Kernel.Model.Categories;

namespace assembly.kernel.benchmark.tests.data.Input
{
    /// <summary>
    /// The safety assessment assembly result.
    /// </summary>
    public class SafetyAssessmentAssemblyResult
    {
        /// <summary>
        /// The combined probability space (faalkansruimte) for all relevant failure mechanisms
        /// in groups 1 and 2 (the probabilistic mechanisms).
        /// </summary>
        public double CombinedFailureMechanismProbabilitySpace { get; set; }

        /// <summary>
        /// The expected section categories (A+ to D) on the highest (assessment section) level.
        /// </summary>
        public CategoriesList<AssessmentSectionCategory> ExpectedAssessmentSectionCategories { get; set; }

        /*/// <summary>
        /// The expected result (toetsoordeel) for the combined failure mechanisms
        /// in group 1 and 2 (the probabilistic mechanisms).
        /// </summary>
        public EFailureMechanismCategory ExpectedAssemblyResultGroups1and2 { get; set; }

        /// <summary>
        /// The expected result (toetsoordeel) for the combined failure mechanisms
        /// in group 1 and 2 (the probabilistic mechanisms) as a result of temporal assessment.
        /// </summary>
        public EFailureMechanismCategory ExpectedAssemblyResultGroups1and2Temporal { get; set; }
        */

        /// <summary>
        /// The expected estimated probability of flooding for the combined
        /// failure mechanisms in group 1 and 2 (the probabilistic mechanisms).
        /// </summary>
        public double ExpectedAssemblyResultGroups1and2Probability { get; set; }

        /// <summary>
        /// The expected estimated probability of flooding for the combined
        /// failure mechanisms in group 1 and 2 (the probabilistic mechanisms) as a result of temporal assessment.
        /// </summary>
        public double ExpectedAssemblyResultGroups1and2ProbabilityTemporal { get; set; }

        /*/// <summary>
        /// The expected result (toetsoordeel) for the combined failure mechanisms
        /// in group 3 and 4 (the non-probabilistic direct failure mechanisms)
        /// </summary>
        public EFailureMechanismCategory ExpectedAssemblyResultGroups3and4 { get; set; }

        /// <summary>
        /// The expected result (toetsoordeel) for the combined failure mechanisms
        /// in group 3 and 4 (the non-probabilistic direct failure mechanisms) as a result of temporal assessment.
        /// </summary>
        public EFailureMechanismCategory ExpectedAssemblyResultGroups3and4Temporal { get; set; }*/

        /// <summary>
        /// The expected safety assessment verdict (A+ to D) for the assessment
        /// section (final result).
        /// </summary>
        public EAssessmentGrade ExpectedSafetyAssessmentAssemblyResult { get; set; }

        /// <summary>
        /// The expected safety assessment verdict (A+ to D) for the assessment
        /// section (final result) as a result of temporal assessment.
        /// </summary>
        public EAssessmentGrade ExpectedSafetyAssessmentAssemblyResultTemporal { get; set; }
    }
}