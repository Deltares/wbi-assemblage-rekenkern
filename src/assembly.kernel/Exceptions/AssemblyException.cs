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

using System;
using System.Collections.Generic;

namespace Assembly.Kernel.Exceptions
{
    /// <summary>
    /// This exception is returned when an exception occured during the execution of a method in the Assembly kernel.
    /// </summary>
    public class AssemblyException : Exception
    {
        private readonly List<AssemblyErrorMessage> errors;

        /// <summary>
        /// Assembly exception constructor for a single error message
        /// </summary>
        /// <param name="entityId">The id of the entity on which the error occurred</param>
        /// <param name="error">The code of the error which occurred</param>
        public AssemblyException(string entityId, EAssemblyErrors error)
        {
            errors = new List<AssemblyErrorMessage> {new AssemblyErrorMessage(entityId, error)};
        }

        /// <summary>
        /// Assembly exception constructor for multiple error messages
        /// </summary>
        /// <param name="errorMessages">A list of error messages</param>
        public AssemblyException(IEnumerable<AssemblyErrorMessage> errorMessages)
        {
            errors = errorMessages as List<AssemblyErrorMessage>;
        }

        /// <summary>
        /// The default exception text.
        /// </summary>
        public override string Message => "One or more errors occured during the assembly process!" +
                                          " See containing error message objects for more details.";

        /// <summary>
        /// This property contains one or more error messages containing more detail of the occured error(s).
        /// </summary>
        public IEnumerable<AssemblyErrorMessage> Errors => errors;
    }
}