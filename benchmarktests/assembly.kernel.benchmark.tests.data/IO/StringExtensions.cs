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
using System.ComponentModel;
using assembly.kernel.benchmark.tests.data.Data.Input;
using Assembly.Kernel.Model.Categories;

namespace assembly.kernel.benchmark.tests.data.IO
{
    /// <summary>
    /// Extension methods for <see cref="string"/>.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Translate a string value to a specific <see cref="EAssessmentGrade"/>.
        /// </summary>
        /// <param name="str">string value to be translated.</param>
        /// <returns>The translated <see cref="EAssessmentGrade"/>.</returns>
        public static EExpectedAssessmentGrade ToExpectedAssessmentGrade(this string str)
        {
            if (Enum.TryParse(str, true, out EExpectedAssessmentGrade sectionCategory))
            {
                return sectionCategory;
            }

            switch (str.ToLower())
            {
                case "a+":
                    return EExpectedAssessmentGrade.APlus;
                case "-":
                case "gr":
                    return EExpectedAssessmentGrade.Exception;
                default:
                    throw new InvalidEnumArgumentException(str);
            }
        }

        /// <summary>
        /// Translate a string value to a specific <see cref="EInterpretationCategory"/>.
        /// </summary>
        /// <param name="str">string value to be translated.</param>
        /// <returns>The translated <see cref="EInterpretationCategory"/>.</returns>
        public static EInterpretationCategory ToInterpretationCategory(this string str)
        {
            switch (str.ToLower())
            {
                case "nr":
                    return EInterpretationCategory.NotRelevant;
                case "d":
                    return EInterpretationCategory.Dominant;
                case "nd":
                    return EInterpretationCategory.NotDominant;
                case "+iii":
                    return EInterpretationCategory.III;
                case "+ii":
                    return EInterpretationCategory.II;
                case "+i":
                    return EInterpretationCategory.I;
                case "+0":
                case "0":
                    return EInterpretationCategory.Zero;
                case "-i":
                    return EInterpretationCategory.IMin;
                case "-ii":
                    return EInterpretationCategory.IIMin;
                case "-iii":
                    return EInterpretationCategory.IIIMin;
                default:
                    throw new InvalidEnumArgumentException(str);
            }
        }
    }
}