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

namespace assembly.kernel.benchmark.tests.data.Input
{
    /// <summary>
    /// The expected assessment grades.
    /// </summary>
    public enum EExpectedAssessmentGrade
    {
        /// <summary>
        /// A+ expected.
        /// </summary>
        APlus = 0,

        /// <summary>
        /// A expected.
        /// </summary>
        A = 1,

        /// <summary>
        /// B expected.
        /// </summary>
        B = 2,

        /// <summary>
        /// C expected.
        /// </summary>
        C = 3,

        /// <summary>
        /// D expected.
        /// </summary>
        D = 4,

        /// <summary>
        /// Exception expected.
        /// </summary>
        Exception = 5
    }
}