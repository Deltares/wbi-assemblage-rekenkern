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

namespace Assembly.Kernel.Implementations.Validators
{
    /// <summary>
    /// Validator for an AssessmentSection object.
    /// </summary>
    public class AssessmentSectionValidator
    {
        /// <summary>
        /// Check all input parameters of an assessment section object.
        /// </summary>
        /// <param name="length">The length of the assessement section in meters, must be &gt; 0</param>
        /// <param name="failureProbabilityLowerLimit">The failure probability lower limit of the assessment section. 
        /// Must be &gt;= 0 &lt;= 1</param>
        /// <param name="failureProbabilitySignallingLimit">The failure probability signalling limit of the assessment 
        /// section. Must be &gt;= 0 &lt;= 1</param>
        /// <exception cref="AssemblyException">Thrown when input is not valid.</exception>
        public static void CheckAssessmentSectionInput(double length,
            double failureProbabilitySignallingLimit, double failureProbabilityLowerLimit)
        {
            var errors = new List<AssemblyErrorMessage>();
            if (length <= 0)
            {
                errors.Add(new AssemblyErrorMessage("AssessmentSection", EAssemblyErrors.SectionLengthOutOfRange));
            }

            if (failureProbabilityLowerLimit < 0 || failureProbabilityLowerLimit > 1)
            {
                errors.Add(new AssemblyErrorMessage("AssessmentSection", EAssemblyErrors.LowerLimitOutOfRange));
            }

            if (failureProbabilitySignallingLimit < 0 || failureProbabilitySignallingLimit > 1)
            {
                errors.Add(new AssemblyErrorMessage("AssessmentSection", EAssemblyErrors.SignallingLimitOutOfRange));
            }

            if (failureProbabilitySignallingLimit > failureProbabilityLowerLimit)
            {
                errors.Add(
                    new AssemblyErrorMessage("AssessmentSection", EAssemblyErrors.SignallingLimitAboveLowerLimit));
            }

            if (errors.Count > 0)
            {
                throw new AssemblyException(errors);
            }
        }
    }
}