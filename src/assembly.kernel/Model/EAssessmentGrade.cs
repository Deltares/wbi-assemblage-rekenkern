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
    /// The different kind of ratings for an assessment.
    /// </summary>
    public enum EAssessmentGrade
    {
        /// <summary>
        /// Highest rating
        /// Assessment section is well above standard
        /// </summary>
        APlus,

        /// <summary>
        /// Assessment section complies with standard
        /// </summary>
        A,

        /// <summary>
        /// Assessment section complies with lower limit but fails signalling limit
        /// </summary>
        B,

        /// <summary>
        /// Assessment section fails both signalling and lower limits
        /// </summary>
        C,

        /// <summary>
        /// Lowest rating
        /// Assessment section fails both limits well
        /// </summary>
        D,

        /// <summary>
        /// No verdict yet
        /// </summary>
        Ngo,

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