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

namespace assembly.kernel.benchmark.tests.data.Input.FailureMechanismSections
{
    /// <summary>
    /// Base class for a failure mechanism section.
    /// </summary>
    /// <typeparam name="TCombinedResult">The type of the combined result.</typeparam>
    public class FailureMechanismSectionBase<TCombinedResult> : IFailureMechanismSection<TCombinedResult>
    {
        /// <summary>
        /// The name of the section.
        /// </summary>
        /// TODO: Use this in messages in case of an assertion error of failing test
        public string SectionName { get; set; }

        public TCombinedResult ExpectedCombinedResult { get; set; }

        public double Start { get; set; }

        public double End { get; set; }

        /*
        public IFmSectionAssemblyResult ExpectedDetailedAssessmentAssemblyResult { get; set; }

        public IFmSectionAssemblyResult ExpectedTailorMadeAssessmentAssemblyResult { get; set; }
    */
    }
}