#region Copyright (C) Rijkswaterstaat 2022. All rights reserved

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

#endregion

namespace Assembly.Kernel.Model.Categories
{
    /// <summary>
    /// Failure mechanism section categories
    /// </summary>
    public enum EInterpretationCategory
    {
        /// <summary>
        /// Not relevant
        /// </summary>
        NotRelevant = 0,

        /// <summary>
        /// Relevant but not dominant without probability estimation
        /// </summary>
        NotDominant = 1,

        /// <summary>
        /// probability &lt; 1/1000 of the signal flooding probability.
        /// </summary>
        III = 2,

        /// <summary>
        /// probability &lt; 1/100 of the signal flooding probability.
        /// </summary>
        II = 3,

        /// <summary>
        /// probability &lt; 1/10 of the signal flooding probability.
        /// </summary>
        I = 4,

        /// <summary>
        /// probability &lt; the signal flooding probability.
        /// </summary>
        Zero = 5,

        /// <summary>
        /// probability &lt; the maximum allowable flooding probability.
        /// </summary>
        IMin = 6,

        /// <summary>
        /// probability $lt; 10 times the maximum allowable flooding probability.
        /// </summary>
        IIMin = 7,

        /// <summary>
        /// probability $gt; 10 times the maximum allowable flooding probability.
        /// </summary>
        IIIMin = 8,

        /// <summary>
        /// Dominant without probability estimation.
        /// </summary>
        Dominant = 9,

        /// <summary>
        /// No result.
        /// </summary>
        NoResult = 10
    }
}