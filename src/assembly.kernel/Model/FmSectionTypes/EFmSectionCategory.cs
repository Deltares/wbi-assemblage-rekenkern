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
namespace Assembly.Kernel.Model.FmSectionTypes {
    /// <summary>
    /// Failure mechanism section categories
    /// </summary>
    public enum EFmSectionCategory {
        /// <summary>
        /// Highest rating
        /// Complies with the signalling limit well
        /// </summary>
        Iv,
        /// <summary>
        /// Complies with signalling limit
        /// </summary>
        IIv,
        /// <summary>
        /// Complies with lower limit, an probably complies with signalling limit
        /// </summary>
        IIIv,
        /// <summary>
        /// Probably complies with lower or signalling limit
        /// </summary>
        IVv,
        /// <summary>
        /// Complies with lower limit
        /// </summary>
        Vv,
        /// <summary>
        /// Lowest rating
        /// Does not comply with both lower and signalling limits
        /// </summary>
        VIv,
        /// <summary>
        /// No verdict yet
        /// </summary>
        VIIv,
        /// <summary>
        /// No result
        /// </summary>
        Gr,
        /// <summary>
        /// Result is not applicable.
        /// </summary>
        NotApplicable
    }
}