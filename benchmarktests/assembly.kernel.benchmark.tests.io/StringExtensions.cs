﻿#region Copyright (C) Rijkswaterstaat 2019. All rights reserved
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
using Assembly.Kernel.Model.AssessmentResultTypes;
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
                    return EIndirectAssessmentResult.FvEt;
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
        /// Translate a string value to a specific <see cref="EFmSectionCategory"/>.
        /// </summary>
        /// <param name="str">string value to be translated.</param>
        /// <returns>The translated <see cref="EFmSectionCategory"/>.</returns>
        public static EFmSectionCategory ToFailureMechanismSectionCategory(this string str)
        {
            EFmSectionCategory sectionCategory;
            if (!Enum.TryParse(str, true, out sectionCategory))
            {
                switch (str.ToLower())
                {
                    case "niet meegenomen":
                    case "-":
                        sectionCategory = EFmSectionCategory.NotApplicable;
                        break;
                    case "nog geen oordeel":
                    case "ngo":
                        sectionCategory = EFmSectionCategory.VIIv;
                        break;
                    default:
                        sectionCategory = (EFmSectionCategory) (-2);
                        break;
                }
            }

            if (string.IsNullOrWhiteSpace(str))
            {
                return EFmSectionCategory.Gr;
            }

            if ((int) sectionCategory < -1)
            {
                throw new InvalidEnumArgumentException(str);
            }

            return sectionCategory;
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

        /// <summary>
        /// Translate a string value to a specific <see cref="EAssessmentResultTypeE1"/>.
        /// </summary>
        /// <param name="str">string value to be translated.</param>
        /// <returns>The translated <see cref="EAssessmentResultTypeE1"/>.</returns>
        public static EAssessmentResultTypeE1 ToEAssessmentResultTypeE1(this string str)
        {
            EAssessmentResultTypeE1 assessmentResultType;
            if (!Enum.TryParse(str, true, out assessmentResultType))
            {
                if (string.IsNullOrWhiteSpace(str))
                {
                    return EAssessmentResultTypeE1.Gr;
                }
            }

            return assessmentResultType;
        }

        /// <summary>
        /// Translate a string value to a specific <see cref="EAssessmentResultTypeE2"/>.
        /// </summary>
        /// <param name="str">string value to be translated.</param>
        /// <returns>The translated <see cref="EAssessmentResultTypeE2"/>.</returns>
        public static EAssessmentResultTypeE2 ToEAssessmentResultTypeE2(this string str)
        {
            EAssessmentResultTypeE2 assessmentResultType;
            if (!Enum.TryParse(str, true, out assessmentResultType))
            {
                if (string.IsNullOrWhiteSpace(str))
                {
                    return EAssessmentResultTypeE2.Gr;
                }
            }

            return assessmentResultType;
        }

        /// <summary>
        /// Translate a string value to a specific <see cref="EAssessmentResultTypeG1"/>..
        /// </summary>
        /// <param name="str">string value to be translated.</param>
        /// <returns>The translated <see cref="EAssessmentResultTypeG1"/>.</returns>
        public static EAssessmentResultTypeG1 ToEAssessmentResultTypeG1(this string str)
        {
            EAssessmentResultTypeG1 assessmentResultType;
            if (!Enum.TryParse(str, true, out assessmentResultType))
            {
                if (string.IsNullOrWhiteSpace(str))
                {
                    return EAssessmentResultTypeG1.Gr;
                }
            }

            return assessmentResultType;
        }

        /// <summary>
        /// Translate a string value to a specific <see cref="EAssessmentResultTypeG2"/>.
        /// </summary>
        /// <param name="str">string value to be translated.</param>
        /// <returns>The translated <see cref="EAssessmentResultTypeG2"/>.</returns>
        public static EAssessmentResultTypeG2 ToEAssessmentResultTypeG2(this string str, bool probabilistic)
        {
            if (probabilistic)
            {
                var culture = str.Contains(",") ? CultureInfo.CurrentCulture : CultureInfo.InvariantCulture;
                double cellValueAsDouble;
                if (double.TryParse(str, NumberStyles.Any, culture, out cellValueAsDouble))
                {
                    return EAssessmentResultTypeG2.ResultSpecified;
                }
            }
            else
            {
                try
                {
                    var result = str.ToFailureMechanismSectionCategory();
                    if (result > 0 && (int) result < 8)
                    {
                        return EAssessmentResultTypeG2.ResultSpecified;
                    }
                }
                catch (InvalidEnumArgumentException)
                {
                    // Do nothing, return direct parsing result.
                }
            }

            EAssessmentResultTypeG2 assessmentResultType;
            if (!Enum.TryParse(str, true, out assessmentResultType))
            {
                return EAssessmentResultTypeG2.Gr;
            }

            return assessmentResultType;
        }

        /// <summary>
        /// Translate a string value to a specific <see cref="EAssessmentResultTypeT1"/>.
        /// </summary>
        /// <param name="str">string value to be translated.</param>
        /// <returns>The translated <see cref="EAssessmentResultTypeT1"/>.</returns>
        public static EAssessmentResultTypeT1 ToEAssessmentResultTypeT1(this string str)
        {
            EAssessmentResultTypeT1 assessmentResultType;
            if (!Enum.TryParse(str, true, out assessmentResultType))
            {
                if (string.IsNullOrWhiteSpace(str))
                {
                    return EAssessmentResultTypeT1.Gr;
                }
            }

            return assessmentResultType;
        }

        /// <summary>
        /// Translate a string value to a specific <see cref="EAssessmentResultTypeT2"/>.
        /// </summary>
        /// <param name="str">string value to be translated.</param>
        /// <returns>The translated <see cref="EAssessmentResultTypeT2"/>.</returns>
        public static EAssessmentResultTypeT2 ToEAssessmentResultTypeT2(this string str)
        {
            EAssessmentResultTypeT2 assessmentResultType;
            if (!Enum.TryParse(str, true, out assessmentResultType))
            {
                if (string.IsNullOrWhiteSpace(str))
                {
                    return EAssessmentResultTypeT2.Gr;
                }
            }

            return assessmentResultType;
        }

        /// <summary>
        /// Translate a string value to a specific <see cref="EAssessmentResultTypeT3"/>.
        /// </summary>
        /// <param name="str">string value to be translated.</param>
        /// <returns>The translated <see cref="EAssessmentResultTypeT3"/>.</returns>
        public static EAssessmentResultTypeT3 ToEAssessmentResultTypeT3(this string str, bool probabilistic)
        {
            if (probabilistic)
            {
                var culture = str.Contains(",") ? CultureInfo.CurrentCulture : CultureInfo.InvariantCulture;
                double cellValueAsDouble;
                if (double.TryParse(str, NumberStyles.Any, culture, out cellValueAsDouble))
                {
                    return EAssessmentResultTypeT3.ResultSpecified;
                }
            }
            else
            {
                try
                {
                    var result = str.ToFailureMechanismSectionCategory();
                    if (result > 0 && (int) result < 8)
                    {
                        return EAssessmentResultTypeT3.ResultSpecified;
                    }
                }
                catch (InvalidEnumArgumentException)
                {
                    // Do nothing, return direct parsing result.
                }
            }

            EAssessmentResultTypeT3 assessmentResultType;
            if (!Enum.TryParse(str, true, out assessmentResultType))
            {
                return EAssessmentResultTypeT3.Gr;
            }

            return assessmentResultType;
        }

        /// <summary>
        /// Translate a string value to a specific <see cref="EAssessmentResultTypeT4"/>.
        /// </summary>
        /// <param name="str">string value to be translated.</param>
        /// <returns>The translated <see cref="EAssessmentResultTypeT4"/>.</returns>
        public static EAssessmentResultTypeT4 ToEAssessmentResultTypeT4(this string str)
        {
            var culture = str.Contains(",") ? CultureInfo.CurrentCulture : CultureInfo.InvariantCulture;
            double cellValueAsDouble;
            if (double.TryParse(str, NumberStyles.Any, culture, out cellValueAsDouble))
            {
                return EAssessmentResultTypeT4.ResultSpecified;
            }

            EAssessmentResultTypeT4 assessmentResultType;
            if (!Enum.TryParse(str, true, out assessmentResultType))
            {
                return EAssessmentResultTypeT4.Gr;
            }

            return assessmentResultType;
        }
    }
}