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

using System.Collections.Generic;
using assembly.kernel.benchmark.tests.data.Input.FailureMechanismSections;
using Assembly.Kernel.Model;

namespace assembly.kernel.benchmark.tests.data.Input.FailureMechanisms
{
    /// <summary>
    /// Base class for an expected failure mechanism result.
    /// </summary>
    public class ExpectedFailureMechanismResult
    {
        /// <summary>
        /// Creates a new instance of <see cref="ExpectedFailureMechanismResult"/>.
        /// </summary>
        /// <param name="name">The name of the failure mechanism result.</param>
        /// <param name="mechanismId">Unique identifier of the mechanism.</param>
        /// <param name="hasLengthEffect">Specifies whether there is length-effect within a section in this mechanism.</param>
        public ExpectedFailureMechanismResult(string name, string mechanismId, bool hasLengthEffect)
        {
            Name = name;
            MechanismId = mechanismId;
            HasLengthEffect = hasLengthEffect;
            Sections = new List<IExpectedFailureMechanismSection>();
        }

        /// <summary>
        /// Name of the failure mechanism.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// MechanismId of the failure mechanism.
        /// </summary>
        public string MechanismId { get; }

        /// <summary>
        /// Indicates whether there is a length-effect within a single section.
        /// </summary>
        public bool HasLengthEffect { get; }

        /// <summary>
        /// The expected combined probability.
        /// </summary>
        public Probability ExpectedCombinedProbability { get; set; }

        /// <summary>
        /// Indicates whether we expect correlated sections or not when determining the combined probability.
        /// </summary>
        public EFailureMechanismAssemblyMethod ExpectedIsSectionsCorrelated { get; set; }

        /// <summary>
        /// The expected result while performing partial assembly.
        /// </summary>
        public Probability ExpectedCombinedProbabilityPartial { get; set; }

        /// <summary>
        /// Indicates whether we expect correlated sections or not when determining the combined probability.
        /// </summary>
        public EFailureMechanismAssemblyMethod ExpectedIsSectionsCorrelatedPartial { get; set; }

        /// <summary>
        /// A listing of all sections within the failure mechanism.
        /// </summary>
        public IEnumerable<IExpectedFailureMechanismSection> Sections { get; set; }

        /// <summary>
        /// Length-effect factor for this failure mechanism.
        /// </summary>
        public double LengthEffectFactor { get; set; }

    }
}