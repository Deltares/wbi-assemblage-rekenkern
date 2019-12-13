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

using System.Collections.Generic;
using assembly.kernel.benchmark.tests.data.Input.FailureMechanismSections;

namespace assembly.kernel.benchmark.tests.data.Input.FailureMechanisms
{
    /// <summary>
    /// Interface for expected failure mechanism result.
    /// </summary>
    public interface IExpectedFailureMechanismResult
    {
        /// <summary>
        /// Name of the failure mechanism
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Type of the failure mechanism
        /// </summary>
        MechanismType Type { get; }

        /// <summary>
        /// Assembly group (1, 2, 3, 4 or 5)
        /// </summary>
        int Group { get; }

        /// <summary>
        /// Denotes whether the failure mechanism should be taken into account while performing assembly
        /// </summary>
        /// TODO: Currently not used. Remove this parameter?
        bool AccountForDuringAssembly { get; set; }

        /// <summary>
        /// The expected result (EIndirectAssessmentResult in case of group 5, EFailureMechanismCategory in other cases)
        /// </summary>
        object ExpectedAssessmentResult { get; set; }

        /// <summary>
        /// The expected result while performing partial assembly (EIndirectAssessmentResult in case of group 5, EFailureMechanismCategory in other cases)
        /// </summary>
        object ExpectedAssessmentResultTemporal { get; set; }

        /// <summary>
        /// A listing of all sections within the failure mechanism
        /// </summary>
        IEnumerable<IFailureMechanismSection> Sections { get; set; }
    }
}