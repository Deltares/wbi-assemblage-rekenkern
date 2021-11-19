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
using System.Globalization;
using assembly.kernel.benchmark.tests.data.Input.FailureMechanisms;
using Assembly.Kernel.Model;
using Assembly.Kernel.Model.FmSectionTypes;

namespace assembly.kernel.benchmark.tests.io
{
    /// <summary>
    /// Extension methods for <see cref="string"/>.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Translate a string value to a specific <see cref="MechanismType"/>.
        /// </summary>
        /// <param name="str">string value to be translated.</param>
        /// <returns>The translated <see cref="MechanismType"/>.</returns>
        public static MechanismType ToMechanismType(this string str)
        {
            MechanismType mechanismType;
            if (!Enum.TryParse(str, true, out mechanismType))
            {
                throw new InvalidEnumArgumentException(str);
            }

            return mechanismType;
        }

        /// <summary>
        /// Translate a string value to a specific <see cref="EFailureMechanismCategory"/>.
        /// </summary>
        /// <param name="str">string value to be translated.</param>
        /// <returns>The translated <see cref="EFailureMechanismCategory"/>.</returns>
        public static EFailureMechanismCategory ToFailureMechanismCategory(this string str)
        {
            EFailureMechanismCategory category;
            if (!Enum.TryParse(str, true, out category))
            {
                switch (str.ToLower())
                {
                    case "niet meegenomen":
                    case "-":
                        category = EFailureMechanismCategory.Nvt;
                        break;
                    case "nog geen oordeel":
                        category = EFailureMechanismCategory.VIIt;
                        break;
                    default:
                        category = (EFailureMechanismCategory) (-2);
                        break;
                }
            }

            if (string.IsNullOrWhiteSpace(str))
            {
                return EFailureMechanismCategory.Gr;
            }

            if ((int) category < -1)
            {
                throw new InvalidEnumArgumentException(str);
            }

            return category;
        }

        /// <summary>
        /// Translate a string value to a specific <see cref="EIndirectAssessmentResult"/>.
        /// </summary>
        /// <param name="str">string value to be translated.</param>
        /// <returns>The translated <see cref="EIndirectAssessmentResult"/>.</returns>
        public static EIndirectAssessmentResult ToIndirectFailureMechanismSectionCategory(this string str)
        {
            switch (str)
            {
                case "GEEN WAARDE":
                    return EIndirectAssessmentResult.Gr;
                case "-":
                    return EIndirectAssessmentResult.Nvt;
                case "*":
                    return EIndirectAssessmentResult.FactoredInOtherFailureMechanism;
                case "FV_ET":
                    // TODO: Remove
                    return EIndirectAssessmentResult.FvGt;
                case "FV_GT":
                    return EIndirectAssessmentResult.FvGt;
                case "FV_TOM":
                    return EIndirectAssessmentResult.FvTom;
                case "NGO":
                    return EIndirectAssessmentResult.Ngo;
                default:
                    if (string.IsNullOrWhiteSpace(str))
                    {
                        return EIndirectAssessmentResult.Gr;
                    }

                    throw new InvalidEnumArgumentException(str);
            }
        }

        /// <summary>
        /// Translate a string value to a specific <see cref="EAssessmentGrade"/>.
        /// </summary>
        /// <param name="str">string value to be translated.</param>
        /// <returns>The translated <see cref="EAssessmentGrade"/>.</returns>
        public static EAssessmentGrade ToAssessmentGrade(this string str)
        {
            EAssessmentGrade sectionCategory;
            if (!Enum.TryParse(str, true, out sectionCategory))
            {
                switch (str.ToLower())
                {
                    case "a+":
                        sectionCategory = EAssessmentGrade.APlus;
                        break;
                    case "niet meegenomen":
                        sectionCategory = EAssessmentGrade.Nvt;
                        break;
                    case "nog geen oordeel":
                        sectionCategory = EAssessmentGrade.Ngo;
                        break;
                    default:
                        sectionCategory = (EAssessmentGrade) (-1);
                        break;
                }
            }

            if (string.IsNullOrWhiteSpace(str))
            {
                return EAssessmentGrade.Gr;
            }

            if (sectionCategory < 0)
            {
                throw new InvalidEnumArgumentException(str);
            }

            return sectionCategory;
        }
    }
}