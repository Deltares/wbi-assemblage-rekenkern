#region Copyright (c) 2018 Technolution BV. All Rights Reserved. 

// // Copyright (C) Technolution BV. 2018. All rights reserved.
// //
// // This file is part of the Assembly kernel.
// //
// // Assembly kernel is free software: you can redistribute it and/or modify
// // it under the terms of the GNU Lesser General Public License as published by
// // the Free Software Foundation, either version 3 of the License, or
// // (at your option) any later version.
// // 
// // This program is distributed in the hope that it will be useful,
// // but WITHOUT ANY WARRANTY; without even the implied warranty of
// // MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// // GNU Lesser General Public License for more details.
// //
// // You should have received a copy of the GNU Lesser General Public License
// // along with this program. If not, see <http://www.gnu.org/licenses/>.
// //
// // All names, logos, and references to "Technolution BV" are registered trademarks of
// // Technolution BV and remain full property of Technolution BV at all times.
// // All rights reserved.

#endregion

// ReSharper disable InconsistentNaming
namespace Assembly.Kernel.Model
{
    /// <summary>
    /// Failure mechanism categories.
    /// </summary>
    public enum EFailureMechanismCategory
    {
        /// <summary>
        /// Highest rating
        /// Well above signalling limit
        /// </summary>
        It,

        /// <summary>
        /// Complies with signalling limit
        /// </summary>
        IIt,

        /// <summary>
        /// Complies with lower limit and probably with signalling limit
        /// </summary>
        IIIt,

        /// <summary>
        /// Complies with lower limit
        /// </summary>
        IVt,

        /// <summary>
        /// Does not comply with lower limit
        /// </summary>
        Vt,

        /// <summary>
        /// Well below lower limit
        /// </summary>
        VIt,

        /// <summary>
        /// No verdict yet
        /// </summary>
        VIIt,

        /// <summary>
        /// Does not apply
        /// </summary>
        Nvt,

        /// <summary>
        /// No result
        /// </summary>
        Gr
    }
}