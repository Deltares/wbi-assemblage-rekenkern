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
    /// Failure mechanism assessment translation result for indirect failure mechanisms.
    /// </summary>
    public class FmSectionAssemblyIndirectResult : IFmSectionAssemblyResult
    {
        /// <summary>
        /// Constructor of the indirect failure mechanism assembly result.
        /// </summary>
        /// <param name="result">The translated category type of the result</param>
        public FmSectionAssemblyIndirectResult(EIndirectAssessmentResult result)
        {
            Result = result;
        }

        /// <summary>
        /// The Failure mechanism section category as result of the assessment result translation.
        /// </summary>
        public EIndirectAssessmentResult Result { get; }

        /// <summary>
        /// Convert to string
        /// </summary>
        /// <returns>String of the object</returns>
        public override string ToString()
        {
            return "FmSectionAssemblyIndirectResult [" + Result + "]";
        }

        /// <inheritdoc />
        public bool HasResult()
        {
            return Result != EIndirectAssessmentResult.Gr;
        }

        /// <inheritdoc />
        public bool NotApplicableOrNeglectable()
        {
            return Result == EIndirectAssessmentResult.Nvt || Result == EIndirectAssessmentResult.FvEt;
        }
    }
}