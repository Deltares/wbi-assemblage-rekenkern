#region Copyright (C) Rijkswaterstaat 2019. All rights reserved
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

using Assembly.Kernel.Exceptions;

namespace Assembly.Kernel.Model
{
    /// <summary>
    /// Extension methods used in the application.
    /// </summary>
    public static class ModelExtensionMethods
    {
        /// <summary>
        /// Translates an EFailureMechanismCategory to an EAssessmentGrade, as specified in Wbi-2A-1
        /// </summary>
        /// <param name="failureMechanismCategory">The failure mechanism section category</param>
        /// <returns>The failure mechanism category belonging to the section category</returns>
        /// <exception cref="AssemblyException">Thrown when a category is present which is unkown to the translator
        /// </exception>
        public static EAssessmentGrade ToAssessmentGrade(
            this EFailureMechanismCategory failureMechanismCategory)
        {
            switch (failureMechanismCategory)
            {
                case EFailureMechanismCategory.It:
                    return EAssessmentGrade.APlus;
                case EFailureMechanismCategory.IIt:
                    return EAssessmentGrade.A;
                case EFailureMechanismCategory.IIIt:
                    return EAssessmentGrade.B;
                case EFailureMechanismCategory.IVt:
                    return EAssessmentGrade.C;
                case EFailureMechanismCategory.Vt:
                    return EAssessmentGrade.C;
                case EFailureMechanismCategory.VIt:
                    return EAssessmentGrade.D;
                case EFailureMechanismCategory.VIIt:
                    return EAssessmentGrade.Ngo;
                case EFailureMechanismCategory.Nvt:
                    return EAssessmentGrade.Nvt;
                case EFailureMechanismCategory.Gr:
                    return EAssessmentGrade.Gr;
                default:
                    throw new AssemblyException("FailureMechanismAssembler: " + failureMechanismCategory,
                                                EAssemblyErrors.FailureMechanismAssemblerInputInvalid);
            }
        }
    }
}