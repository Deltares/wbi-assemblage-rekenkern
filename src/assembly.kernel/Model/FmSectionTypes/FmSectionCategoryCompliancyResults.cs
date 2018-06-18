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

using System.Collections.Generic;
using Assembly.Kernel.Exceptions;

namespace Assembly.Kernel.Model.FmSectionTypes
{
    /// <summary>
    /// Container for Failure mechanism compliancy results.
    /// Default all categories are set to the compliancy result: NoResult
    /// </summary>
    public class FmSectionCategoryCompliancyResults
    {
        private readonly Dictionary<EFmSectionCategory, ECategoryCompliancy> compliancyResultMap =
            new Dictionary<EFmSectionCategory, ECategoryCompliancy>
            {
                {EFmSectionCategory.Iv, ECategoryCompliancy.NoResult},
                {EFmSectionCategory.IIv, ECategoryCompliancy.NoResult},
                {EFmSectionCategory.IIIv, ECategoryCompliancy.NoResult},
                {EFmSectionCategory.IVv, ECategoryCompliancy.NoResult},
                {EFmSectionCategory.Vv, ECategoryCompliancy.NoResult}
            };

        /// <summary>
        /// Set the compliancy result for a Failure mechanism category.
        /// </summary>
        /// <param name="category">The category to set the compliancy result for</param>
        /// <param name="compliancyResult">The compliancy result to add to the category.</param>
        /// <returns>The updated FmSectionCategoryCompliancyResults object (self)</returns>
        /// <exception cref="AssemblyException">Thrown when a category is supplied which is not allowed</exception>
        public FmSectionCategoryCompliancyResults Set(EFmSectionCategory category,
            ECategoryCompliancy compliancyResult)
        {
            if (!compliancyResultMap.ContainsKey(category))
            {
                throw new AssemblyException("CompliancyResults: " + category, EAssemblyErrors.CategoryNotAllowed);
            }

            compliancyResultMap[category] = compliancyResult;

            return this;
        }

        /// <summary>
        /// Gets the compliancy result per failure mechanism section category.
        /// </summary>
        /// <returns>Dictonary containing the compliancy result per failure mechanism section category</returns>
        public Dictionary<EFmSectionCategory, ECategoryCompliancy> GetCompliancyResults()
        {
            return compliancyResultMap;
        }
    }
}