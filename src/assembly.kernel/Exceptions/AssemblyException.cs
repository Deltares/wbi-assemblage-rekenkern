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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Assembly.Kernel.Exceptions
{
    /// <summary>
    /// Exception that is thrown when performing an assembly has failed.
    /// </summary>
    [Serializable]
    public class AssemblyException : Exception
    {
        /// <summary>
        /// Creates a new instance of <see cref="AssemblyException"/> with a single error message.
        /// </summary>
        /// <param name="entityId">The id of the entity on which the error occurred.</param>
        /// <param name="error">The code of the error which occurred.</param>
        internal AssemblyException(string entityId, EAssemblyErrors error) : this(new[]
        {
            new AssemblyErrorMessage(entityId, error)
        }) {}

        /// <summary>
        /// Creates a new instance of <see cref="AssemblyException"/> with multiple error messages.
        /// </summary>
        /// <param name="errorMessages">A list of error messages.</param>
        internal AssemblyException(IEnumerable<AssemblyErrorMessage> errorMessages)
        {
            if (errorMessages == null)
            {
                errorMessages = new List<AssemblyErrorMessage>
                {
                    new AssemblyErrorMessage(nameof(AssemblyException), EAssemblyErrors.ErrorConstructingErrorMessage)
                };
            }

            Errors = errorMessages;
        }
        
        /// <summary>
        /// Creates a new instance of <see cref="AssemblyFactoryException"/> with
        /// serialized data.</summary>
        /// <param name="info">The <see cref="SerializationInfo"/> that holds the serialized
        /// object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="StreamingContext"/> that contains contextual
        /// information about the source or destination.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="info"/> parameter is
        /// <c>null</c>.</exception>
        /// <exception cref="SerializationException">The class name is <c>null</c> or
        /// <see cref="Exception.HResult" /> is zero (0).</exception>
        protected AssemblyException(SerializationInfo info, StreamingContext context) 
            : base(info, context) {}

        /// <summary>
        /// Gets the exception message.
        /// </summary>
        public override string Message
        {
            get
            {
                return Errors.Aggregate("One or more errors occured during the assembly process:",
                                        (current, error) => current + (Environment.NewLine + error.ErrorCode));
            }
        }

        /// <summary>
        /// Gets the list of error messages.
        /// </summary>
        public IEnumerable<AssemblyErrorMessage> Errors { get; }
    }
}