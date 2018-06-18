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
    /// The assembly result class of a direct failure mechanism.
    /// </summary>
    public class FailureMechanismAssemblyResult
    {
        /// <summary>
        /// Failure mechanism assembly direct result constructor, without failure probability. 
        /// The failure probability will be null when using this constructor.
        /// </summary>
        /// <param name="category">The resulting category of the failure mechanism assembly step</param>
        public FailureMechanismAssemblyResult(
            EFailureMechanismCategory category)
        {
            Category = category;
            FailureProbability = double.NaN;
        }

        /// <summary>
        /// Failure mechanism assembly direct result constructor, with failure probability.
        /// </summary>
        /// <param name="category">The resulting category of the failure mechanism assembly step</param>
        /// <param name="failureProbability">The assembled failure probability of the failure mechanism</param>
        public FailureMechanismAssemblyResult(EFailureMechanismCategory category, double failureProbability)
        {
            Category = category;
            FailureProbability = failureProbability;
        }

        /// <summary>
        /// The failure mechanism category.
        /// </summary>
        public EFailureMechanismCategory Category { get; }

        /// <summary>
        /// The failure probability of the failure mechanism. 
        /// If no failure probability is present this value is null.
        /// </summary>
        public double FailureProbability { get; }
    }
}