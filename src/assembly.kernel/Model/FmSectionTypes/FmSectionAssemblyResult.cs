#region Copyright (c) 2018 Technolution BV. All Rights Reserved. 

// // Copyright (C) Technolution BV. 2018. All rights reserved.
// //
// // This file is part of the Assembly kernel.
// //
// // Ringtoets is free software: you can redistribute it and/or modify
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

namespace Assembly.Kernel.Model.FmSectionTypes
{
    /// <summary>
    /// Base class for the failure mechanism section assembly result.
    /// </summary>
    public abstract class FmSectionAssemblyResult
    {
        /// <summary>
        /// FmSectionAssemblyResult constructor.
        /// </summary>
        /// <param name="resultType">Result type of the assembly result</param>
        protected FmSectionAssemblyResult(EAssembledAssessmentResultType resultType)
        {
            ResultType = resultType;
        }

        /// <summary>
        /// The type of the FmSection assembly result.
        /// </summary>
        public EAssembledAssessmentResultType ResultType { get; }

        /// <summary>
        /// Does the assessment result have a result other than Gr.
        /// </summary>
        /// <returns>false if the assessment result is Gr</returns>
        public abstract bool HasResult();

        /// <summary>
        /// Creates a copy of the current FmSectionAssemblyResult.
        /// </summary>
        /// <returns>The newly created FmSectionAssemblyResult</returns>
        public abstract FmSectionAssemblyResult Clone();
    }
}