﻿// Copyright (C) Stichting Deltares and State of the Netherlands 2023. All rights reserved.
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

using System.ComponentModel;
using Assembly.Kernel.Model.Categories;

namespace Assembly.Kernel.Acceptance.TestUtil.Data.Input
{
    public static class EExpectedAssessmentGradeExtensions
    {
        /// <summary>
        /// Translates an expected assessment grade into the actual <see cref="EAssessmentGrade"/>.
        /// </summary>
        /// <param name="expectedAssessmentGrade">The expected assessment grade to translate.</param>
        /// <returns>The <see cref="EAssessmentGrade"/> that is expected.</returns>
        /// <exception cref="InvalidEnumArgumentException">Thrown in case of an invalid enum value for <paramref name="expectedAssessmentGrade"/>.</exception>
        public static EAssessmentGrade ToEAssessmentGrade(this EExpectedAssessmentGrade expectedAssessmentGrade)
        {
            switch (expectedAssessmentGrade)
            {
                case EExpectedAssessmentGrade.APlus:
                    return EAssessmentGrade.APlus;
                case EExpectedAssessmentGrade.A:
                    return EAssessmentGrade.A;
                case EExpectedAssessmentGrade.B:
                    return EAssessmentGrade.B;
                case EExpectedAssessmentGrade.C:
                    return EAssessmentGrade.C;
                case EExpectedAssessmentGrade.D:
                    return EAssessmentGrade.D;
                default:
                    throw new InvalidEnumArgumentException();
            }
        }
    }
}