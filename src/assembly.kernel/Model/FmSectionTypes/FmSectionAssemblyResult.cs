﻿#region Copyright (C) Rijkswaterstaat 2019. All rights reserved
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

namespace Assembly.Kernel.Model.FmSectionTypes
{
    /// <summary>
    /// Base class for the failure mechanism section assembly result.
    /// </summary>
    public interface IFmSectionAssemblyResult
    {
        /// <summary>
        /// Does the assessment result have a result other than Gr.
        /// </summary>
        /// <returns>false if the assessment result is Gr</returns>
        bool HasResult();

        /// <summary>
        /// Returns whether the specified result is not applicable of has a neglectable probability of failure.
        /// </summary>
        /// <returns></returns>
        bool NotApplicableOrNeglectable();
    }
}