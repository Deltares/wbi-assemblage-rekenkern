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
        /// <inheritdoc />
        /// <summary>
        /// Creates a new instance of <see cref="AssemblyException"/> with
        /// serialized data.
        /// </summary>
        /// <exception cref="SerializationException">Thrown when <paramref name="info"/> does not contain
        /// <see cref="Errors"/>.</exception>
        protected AssemblyException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            var errorMessages = (IEnumerable<AssemblyErrorMessage>) info.GetValue("Errors", typeof(IEnumerable<AssemblyErrorMessage>));

            if (errorMessages == null)
            {
                throw new SerializationException($"Can't construct {nameof(AssemblyException)} when {nameof(errorMessages)} is null.");
            }

            Errors = errorMessages;
        }

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
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="errorMessages"/>
        /// is <c>null</c>.</exception>
        internal AssemblyException(IEnumerable<AssemblyErrorMessage> errorMessages)
        {
            if (errorMessages == null)
            {
                throw new ArgumentNullException(nameof(errorMessages));
            }

            Errors = errorMessages;
        }

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

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Errors", Errors);

            base.GetObjectData(info, context);
        }
    }
}