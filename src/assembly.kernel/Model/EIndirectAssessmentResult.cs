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

namespace Assembly.Kernel.Model
{
    /// <summary>
    /// Assessment results for indirect failure mechanisms.
    /// </summary>
    public enum EIndirectAssessmentResult
    {
        /// <summary>
        /// Does not apply
        /// </summary>
        Nvt = 1,

        /// <summary>
        /// Failure probability negligible for a simple assessment
        /// </summary>
        FvEt = 2,

        /// <summary>
        /// Failure probability negligible for a detailed assessment
        /// </summary>
        FvGt = 3,

        /// <summary>
        /// Failure probability negligible for a custom assessment
        /// </summary>
        FvTom = 4,

        /// <summary>
        /// Discounted with relevant failure mechanisms
        /// </summary>
        FactoredInOtherFailureMechanism = 5,

        /// <summary>
        /// No judgement yet
        /// </summary>
        Ngo = 6,

        /// <summary>
        /// No result
        /// </summary>
        Gr = 7
    }
}