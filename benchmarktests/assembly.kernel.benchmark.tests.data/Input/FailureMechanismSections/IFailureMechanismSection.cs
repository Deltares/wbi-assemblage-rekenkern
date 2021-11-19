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

using Assembly.Kernel.Model.FmSectionTypes;

namespace assembly.kernel.benchmark.tests.data.Input.FailureMechanismSections
{
    /// <summary>
    /// Interface for a failure mechanism section.
    /// </summary>
    public interface IFailureMechanismSection
    {
        /// <summary>
        /// The start position of the section as a length along the assessment section in meters.
        /// </summary>
        double Start { get; }

        /// <summary>
        /// The end position of the section as a length along the assessment section in meters.
        /// </summary>
        double End { get; }

        /*
        /// <summary>
        /// The expected result of the detailed assessment
        /// </summary>
        IFmSectionAssemblyResult ExpectedDetailedAssessmentAssemblyResult { get; }

        /// <summary>
        /// The expected result of the tailor made assessment
        /// </summary>
        IFmSectionAssemblyResult ExpectedTailorMadeAssessmentAssemblyResult { get; }
    */
    }

    /// <summary>
    /// Interface for a failure mechanism section.
    /// </summary>
    /// <typeparam name="TCombinedResult">The type of the combined result.</typeparam>
    public interface IFailureMechanismSection<out TCombinedResult> : IFailureMechanismSection
    {
        /// <summary>
        /// The expected combined result for the specific section as a result of method WBI-0A-1.
        /// </summary>
        TCombinedResult ExpectedCombinedResult { get; }
    }
}