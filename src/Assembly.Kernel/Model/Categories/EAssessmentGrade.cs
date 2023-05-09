// Copyright (C) Stichting Deltares and State of the Netherlands 2023. All rights reserved.
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
// All names, logos, and references to "Deltares" are registered trademarks of
// Stichting Deltares and remain full property of Stichting Deltares at all times.
// All rights reserved.

namespace Assembly.Kernel.Model.Categories
{
    /// <summary>
    /// The different kind of ratings for an assessment.
    /// </summary>
    public enum EAssessmentGrade
    {
        /// <summary>
        /// Highest rating
        /// Assessment section is well above the signal flooding probability.
        /// </summary>
        APlus = 0,

        /// <summary>
        /// Assessment section complies with the signal flooding probability.
        /// </summary>
        A = 1,

        /// <summary>
        /// Assessment section complies with maximum flooding probability but fails the signal flooding probability.
        /// </summary>
        B = 2,

        /// <summary>
        /// Assessment section fails both the signal flooding probability and maximum flooding probability.
        /// </summary>
        C = 3,

        /// <summary>
        /// Lowest rating.
        /// Assessment section fails and has a much higher probability than both the signal flooding probability and maximum flooding probability.
        /// </summary>
        D = 4
    }
}