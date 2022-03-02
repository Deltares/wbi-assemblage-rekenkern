﻿#region Copyright (C) Rijkswaterstaat 2019. All rights reserved
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
    /// Base class for an expected failure mechanism result.
    /// </summary>
    public abstract class ExpectedFailureMechanismResultBase : IExpectedFailureMechanismResult
    {
        /// <summary>
        /// Creates a new instance of <see cref="ExpectedFailureMechanismResultBase"/>.
        /// </summary>
        /// <param name="name">The name of the failure mechanism result.</param>
        protected ExpectedFailureMechanismResultBase(string name, string mechanismId)
        {
            Name = name;
            MechanismId = mechanismId;
            Sections = new List<IFailureMechanismSection>();
        }

        /// <summary>
        /// Name of the failure mechanism
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// MechanismId of the failure mechanism
        /// </summary>
        public string MechanismId { get; }

        public bool HasLengthEffect { get; }

        /// <summary>
        /// Denotes whether the failure mechanism should be taken into account while performing assembly
        /// </summary>
        public bool AccountForDuringAssembly { get; set; }

        /// <summary>
        /// The expected result (EIndirectAssessmentResult in case of group 5, EFailureMechanismCategory in other cases)
        /// </summary>
        public object ExpectedAssessmentResult { get; set; }

        /// <summary>
        /// The expected result while performing partial assembly (EIndirectAssessmentResult in case of group 5, EFailureMechanismCategory in other cases)
        /// </summary>
        public object ExpectedAssessmentResultTemporal { get; set; }

        /// <summary>
        /// A listing of all sections within the failure mechanism
        /// </summary>
        public IEnumerable<IFailureMechanismSection> Sections { get; set; }
    }
}