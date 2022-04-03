#region Copyright (C) Rijkswaterstaat 2022. All rights reserved

// Copyright (C) Rijkswaterstaat 2022. All rights reserved.
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

using System;
using System.Collections.Generic;
using System.Linq;

namespace Assembly.Kernel.Exceptions
{
    /// <summary>
    /// This exception is returned when an exception occurred during the execution of a method in the Assembly kernel.
    /// </summary>
    public class AssemblyException : Exception
    {
        /// <summary>
        /// Assembly exception constructor for a single error message.
        /// </summary>
        /// <param name="entityId">The id of the entity on which the error occurred.</param>
        /// <param name="error">The code of the error which occurred.</param>
        internal AssemblyException(string entityId, EAssemblyErrors error)
        {
            Errors = new List<AssemblyErrorMessage>
            {
                new AssemblyErrorMessage(entityId, error)
            };
        }

        /// <summary>
        /// Assembly exception constructor for multiple error messages.
        /// </summary>
        /// <param name="errorMessages">A list of error messages.</param>
        internal AssemblyException(IEnumerable<AssemblyErrorMessage> errorMessages)
        {
            if (errorMessages == null)
            {
                errorMessages = new List<AssemblyErrorMessage>
                {
                    new AssemblyErrorMessage(nameof(AssemblyException),EAssemblyErrors.ErrorConstructingErrorMessage)
                };
            }

            Errors = errorMessages;
        }

        /// <summary>
        /// The exception text.
        /// </summary>
        public override string Message
        {
            get
            {
                return Errors.Aggregate("One or more errors occured during the assembly process:\n", (current, error) => current + (error.ErrorCode + "\n"));
            }
        }

        /// <summary>
        /// This property contains one or more error messages containing more detail of the occured error(s).
        /// </summary>
        public IEnumerable<AssemblyErrorMessage> Errors { get; }
    }
}