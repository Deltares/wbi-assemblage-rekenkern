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

namespace Assembly.Kernel.Model.Categories
{
    /// <summary>
    /// Failure mechanism section categories
    /// </summary>
    public enum EInterpretationCategory
    {
        /// <summary>
        /// Relevant but not dominant without probability estimation
        /// </summary>
        ND = 1,

        /// <summary>
        /// probability less than 1/30 of the signalling norm
        /// </summary>
        III = 2,

        /// <summary>
        /// probability less than 1/10 of the signalling norm
        /// </summary>
        II = 3,

        /// <summary>
        /// probability less than 1/3 of the signalling norm
        /// </summary>
        I = 4,

        /// <summary>
        /// probability less than the signalling norm
        /// </summary>
        ZeroPlus = 5,

        /// <summary>
        /// probability less than the lower boundary norm
        /// </summary>
        Zero = 6,

        /// <summary>
        /// probability less than the lower boundary norm
        /// </summary>
        IMin = 7,

        /// <summary>
        /// probability less than the 3 times the lower boundary norm
        /// </summary>
        IIMin = 8,

        /// <summary>
        /// probability less than the 10 times the lower boundary norm
        /// </summary>
        IIIMin = 9,

        /// <summary>
        /// Dominant without probability estimation
        /// </summary>
        D = 10,

        /// <summary>
        /// No result
        /// </summary>
        Gr = 11
    }
}