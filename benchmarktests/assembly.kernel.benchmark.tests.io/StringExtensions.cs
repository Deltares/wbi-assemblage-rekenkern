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

using System;
using System.ComponentModel;
using assembly.kernel.benchmark.tests.data.Input;
using Assembly.Kernel.Model.Categories;

namespace assembly.kernel.benchmark.tests.io
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
            EExpectedAssessmentGrade sectionCategory;
            if (!Enum.TryParse(str, true, out sectionCategory))
            {
                switch (str.ToLower())
                {
                    case "a+":
                        sectionCategory = EExpectedAssessmentGrade.APlus;
                        break;
                    case "-":
                    case "gr":
                        sectionCategory = EExpectedAssessmentGrade.Exception;
                        break;
                    default:
                        sectionCategory = (EExpectedAssessmentGrade) (-1);
                        break;
                }
            }

            if (sectionCategory < 0)
            {
                throw new InvalidEnumArgumentException(str);
            }

            return sectionCategory;
        }

        /// <summary>
        /// Translate a string value to a specific <see cref="EInterpretationCategory"/>.
        /// </summary>
        /// <param name="str">string value to be translated.</param>
        /// <returns>The translated <see cref="EInterpretationCategory"/>.</returns>
        public static EInterpretationCategory ToInterpretationCategory(this string str)
        {
            // TODO: Test
            EInterpretationCategory interpretationCategory;
            switch (str.ToLower())
            {
                case "d":
                    interpretationCategory = EInterpretationCategory.Dominant;
                    break;
                case "nd":
                    interpretationCategory = EInterpretationCategory.NotDominant;
                    break;
                case "+iii":
                    interpretationCategory = EInterpretationCategory.III;
                    break;
                case "+ii":
                    interpretationCategory = EInterpretationCategory.II;
                    break;
                case "+i":
                    interpretationCategory = EInterpretationCategory.I;
                    break;
                case "+0":
                    interpretationCategory = EInterpretationCategory.Zero;
                    break;
                case "-i":
                    interpretationCategory = EInterpretationCategory.IMin;
                    break;
                case "-ii":
                    interpretationCategory = EInterpretationCategory.IIMin;
                    break;
                case "-iii":
                    interpretationCategory = EInterpretationCategory.IIIMin;
                    break;
                default:
                    interpretationCategory = (EInterpretationCategory)(-1);
                    break;
            }

            if (interpretationCategory < 0)
            {
                throw new InvalidEnumArgumentException(str);
            }

            return interpretationCategory;
        }
    }
}