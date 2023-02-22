﻿// Copyright (C) Rijkswaterstaat 2022. All rights reserved.
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

using System;

namespace Assembly.Kernel.Exceptions
{
    /// <summary>
    /// Error message containing detailed information of the origin of the error.
    /// </summary>
    [Serializable]
    public class AssemblyErrorMessage
    {
        /// <summary>
        /// Creates a new instance of <see cref="AssemblyErrorMessage"/>.
        /// </summary>
        /// <param name="entityId">The id of the entity on which the error occurred.</param>
        /// <param name="errorCode">The code of the error which occurred.</param>
        internal AssemblyErrorMessage(string entityId, EAssemblyErrors errorCode)
        {
            EntityId = entityId;
            ErrorCode = errorCode;
        }

        /// <summary>
        /// Gets the id of the entity on which the error occurred.
        /// </summary>
        public string EntityId { get; }

        /// <summary>
        /// Gets the code of the error which occurred.
        /// </summary>
        public EAssemblyErrors ErrorCode { get; }
    }
}