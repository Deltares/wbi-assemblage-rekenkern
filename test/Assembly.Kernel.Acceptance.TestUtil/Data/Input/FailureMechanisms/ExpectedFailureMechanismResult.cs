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
using Assembly.Kernel.Acceptance.TestUtil.Data.Input.FailureMechanismSections;
using Assembly.Kernel.Model;

namespace Assembly.Kernel.Acceptance.TestUtil.Data.Input.FailureMechanisms
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
        /// <param name="assemblyMethod">The assembly method to use.</param>
        /// <param name="isCorrelated">Specifies whether the failure mechanism is correlated.</param>
        public ExpectedFailureMechanismResult(string name, string mechanismId, bool hasLengthEffect,
                                              string assemblyMethod, bool isCorrelated)
        {
            Name = name;
            MechanismId = mechanismId;
            HasLengthEffect = hasLengthEffect;
            AssemblyMethod = assemblyMethod;
            IsCorrelated = isCorrelated;
            Sections = new List<IExpectedFailureMechanismSection>();
        }

        /// <summary>
        /// Gets the name of the failure mechanism.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the mechanismId of the failure mechanism.
        /// </summary>
        public string MechanismId { get; }

        /// <summary>
        /// Gets whether there is a length-effect within a single section.
        /// </summary>
        public bool HasLengthEffect { get; }

        /// <summary>
        /// Gets the assembly method to use.
        /// </summary>
        public string AssemblyMethod { get; }

        /// <summary>
        /// Gets whether the failure mechanism is correlated.
        /// </summary>
        public bool IsCorrelated { get; }

        /// <summary>
        /// Gets or sets the expected combined probability.
        /// </summary>
        public Probability ExpectedCombinedProbability { get; set; }

        /// <summary>
        /// Gets or sets the expected result while performing partial assembly.
        /// </summary>
        public Probability ExpectedCombinedProbabilityPartial { get; set; }

        /// <summary>
        /// Gets or sets a listing of all sections within the failure mechanism.
        /// </summary>
        public IEnumerable<IExpectedFailureMechanismSection> Sections { get; set; }

        /// <summary>
        /// Gets or sets the length-effect factor for this failure mechanism.
        /// </summary>
        public double LengthEffectFactor { get; set; }

        /// <summary>
        /// Gets or sets the expected theoretical boundaries.
        /// </summary>
        public BoundaryLimits ExpectedTheoreticalBoundaries { get; set; }

        /// <summary>
        /// Gets or sets the expected theoretical boundaries while performing partial assembly.
        /// </summary>
        public BoundaryLimits ExpectedTheoreticalBoundariesPartial { get; set; }
    }
}