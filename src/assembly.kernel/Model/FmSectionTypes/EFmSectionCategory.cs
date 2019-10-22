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

// ReSharper disable InconsistentNaming
namespace Assembly.Kernel.Model.FmSectionTypes
{
    /// <summary>
    /// Failure mechanism section categories
    /// </summary>
    public enum EFmSectionCategory
    {
        /// <summary>
        /// Result is not applicable.
        /// </summary>
        NotApplicable = -1,

        /// <summary>
        /// Highest rating
        /// Complies with the signalling limit well
        /// </summary>
        Iv = 1,

        /// <summary>
        /// Complies with signalling limit
        /// </summary>
        IIv = 2,

        /// <summary>
        /// Complies with lower limit, an probably complies with signalling limit
        /// </summary>
        IIIv = 3,

        /// <summary>
        /// Probably complies with lower or signalling limit
        /// </summary>
        IVv = 4,

        /// <summary>
        /// Complies with lower limit
        /// </summary>
        Vv = 5,

        /// <summary>
        /// Lowest rating
        /// Does not comply with both lower and signalling limits
        /// </summary>
        VIv = 6,

        /// <summary>
        /// No verdict yet
        /// </summary>
        VIIv = 7,

        /// <summary>
        /// No result
        /// </summary>
        Gr = 8
    }
}