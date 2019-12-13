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

namespace assembly.kernel.benchmark.tests.TestHelpers.FailureMechanism
{
    /// <summary>
    /// Interface describing the failure mechanism result tester.
    /// </summary>
    public interface IFailureMechanismResultTester
    {
        /// <summary>
        /// Tests the simple assessment.
        /// </summary>
        /// <returns><c>true</c> when the simple assessment is valid;
        /// <c>false</c> otherwise.</returns>
        bool TestSimpleAssessment();

        /// <summary>
        /// Tests the detailed assessment.
        /// </summary>
        /// <returns><c>true</c> when the simple assessment is valid;
        /// <c>false</c> otherwise or <c>null</c> when there is no
        /// detailed assessment.</returns>
        bool? TestDetailedAssessment();

        /// <summary>
        /// Tests the tailor made assessment.
        /// </summary>
        /// <returns><c>true</c> when the tailor made assessment is valid;
        /// <c>false</c> otherwise.</returns>
        bool TestTailorMadeAssessment();

        /// <summary>
        /// Tests the combined assessment.
        /// </summary>
        /// <returns><c>true</c> when the combined assessment is valid;
        /// <c>false</c> otherwise.</returns>
        bool TestCombinedAssessment();

        /// <summary>
        /// Tests the assessment section result.
        /// </summary>
        /// <returns><c>true</c> when the assessment section result is valid;
        /// <c>false</c> otherwise.</returns>
        bool TestAssessmentSectionResult();

        bool TestAssessmentSectionResultTemporal();
    }
}