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

using Assembly.Kernel.Model.CategoryLimits;

namespace assembly.kernel.benchmark.tests.data.Input.FailureMechanisms
{
    /// <summary>
    /// Expected failure mechanism result for group 3.
    /// </summary>
    public class Group3ExpectedFailureMechanismResult : ExpectedFailureMechanismResultBase, IGroup3ExpectedFailureMechanismResult
    {
        /// <summary>
        /// Creates a new instance of <see cref="Group3ExpectedFailureMechanismResult"/>.
        /// </summary>
        /// <param name="name">The name of the failure mechanism.</param>
        /// <param name="type">The type of the failure mechanism.</param>
        public Group3ExpectedFailureMechanismResult(string name, MechanismType type) : base(name)
        {
            Type = type;
        }

        public override MechanismType Type { get; }

        public override int Group => 3;

        public double FailureMechanismProbabilitySpace { get; set; }

        public double LengthEffectFactor { get; set; }

        public CategoriesList<FmSectionCategory> ExpectedFailureMechanismSectionCategories { get; set; }
    }
}