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

using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace assembly.kernel.benchmark.tests.data.Input.FailureMechanisms
{
    /// <summary>
    /// Factory for creating instances of <see cref="IExpectedFailureMechanismResult"/>.
    /// </summary>
    public static class FailureMechanismResultFactory
    {
        /// <summary>
        /// Creates an empty IExpectedFailureMechanismResult based on the specified MechanismType.
        /// </summary>
        /// <param name="type">The mechanism type of the mechanism for which an empty expected result needs to be created</param>
        /// <returns>The created <see cref="IExpectedFailureMechanismResult"/>.</returns>
        public static IExpectedFailureMechanismResult CreateFailureMechanism(string mechanismId)
        {
            throw new NotImplementedException();
        }

        private static StbuExpectedFailureMechanismResult CreateSTBUFailureMechanism()
        {
            return new StbuExpectedFailureMechanismResult();
        }
    }
}