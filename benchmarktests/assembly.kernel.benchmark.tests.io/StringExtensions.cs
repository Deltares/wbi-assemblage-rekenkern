using System;
using System.ComponentModel;
using assembly.kernel.benchmark.tests.data.Input.FailureMechanisms;
using Assembly.Kernel.Model;
using Assembly.Kernel.Model.AssessmentResultTypes;
using Assembly.Kernel.Model.FmSectionTypes;

namespace assembly.kernel.benchmark.tests.io
{
    public static class StringExtensions
    {
        /// <summary>
        /// Translate a string value to a specific MechanismType
        /// </summary>
        /// <param name="str">string value to be translated</param>
        /// <returns></returns>
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
        /// Translate a string value to a specific EFailureMechanismCategory
        /// </summary>
        /// <param name="str">string value to be translated</param>
        /// <returns></returns>
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

            if ((int)category < -1)
            {
                throw new InvalidEnumArgumentException(str);
            }

            return category;
        }

        /// <summary>
        /// Translate a string value to a specific EIndirectAssessmentResult
        /// </summary>
        /// <param name="str">string value to be translated</param>
        /// <returns></returns>
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
        /// Translate a string value to a specific EFmSectionCategory
        /// </summary>
        /// <param name="str">string value to be translated</param>
        /// <returns></returns>
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
                        sectionCategory = (EFmSectionCategory)(-2);
                        break;
                }
            }

            if (string.IsNullOrWhiteSpace(str))
            {
                return EFmSectionCategory.Gr;
            }

            if ((int)sectionCategory < -1)
            {
                throw new InvalidEnumArgumentException(str);
            }

            return sectionCategory;
        }

        /// <summary>
        /// Translate a string value to a specific EAssessmentGrade
        /// </summary>
        /// <param name="str">string value to be translated</param>
        /// <returns></returns>
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
                        sectionCategory = (EAssessmentGrade)(-1);
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
        /// Translate a string value to a specific EAssessmentResultTypeE1
        /// </summary>
        /// <param name="str">string value to be translated</param>
        /// <returns></returns>
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
        /// Translate a string value to a specific EAssessmentResultTypeE2
        /// </summary>
        /// <param name="str">string value to be translated</param>
        /// <returns></returns>
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
        /// Translate a string value to a specific EAssessmentResultTypeG1
        /// </summary>
        /// <param name="str">string value to be translated</param>
        /// <returns></returns>
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
        /// Translate a string value to a specific EAssessmentResultTypeG2
        /// </summary>
        /// <param name="str">string value to be translated</param>
        /// <returns></returns>
        public static EAssessmentResultTypeG2 ToEAssessmentResultTypeG2(this string str, bool probabilistic)
        {
            EAssessmentResultTypeG2 assessmentResultType;
            if (!Enum.TryParse(str, true, out assessmentResultType))
            {
                if (probabilistic)
                {
                    double value;
                    if (double.TryParse(str, out value))
                    {
                        return EAssessmentResultTypeG2.ResultSpecified;
                    }
                }

                if (!probabilistic)
                {
                    try
                    {
                        var result = str.ToFailureMechanismSectionCategory();
                        if (result > 0 && (int)result < 8)
                        {
                            return EAssessmentResultTypeG2.ResultSpecified;
                        }
                    }
                    catch (InvalidEnumArgumentException)
                    {
                        // Do nothing, return Gr.
                    }
                }

                return EAssessmentResultTypeG2.Gr;
            }

            return assessmentResultType;
        }

        /// <summary>
        /// Translate a string value to a specific EAssessmentResultTypeT1
        /// </summary>
        /// <param name="str">string value to be translated</param>
        /// <returns></returns>
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
        /// Translate a string value to a specific EAssessmentResultTypeT2
        /// </summary>
        /// <param name="str">string value to be translated</param>
        /// <returns></returns>
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
        /// Translate a string value to a specific EAssessmentResultTypeT3
        /// </summary>
        /// <param name="str">string value to be translated</param>
        /// <returns></returns>
        public static EAssessmentResultTypeT3 ToEAssessmentResultTypeT3(this string str, bool probabilistic)
        {
            EAssessmentResultTypeT3 assessmentResultType;
            if (!Enum.TryParse(str, true, out assessmentResultType))
            {
                if (probabilistic)
                {
                    double value;
                    if (double.TryParse(str, out value))
                    {
                        return EAssessmentResultTypeT3.ResultSpecified;
                    }
                }

                if (!probabilistic)
                {
                    try
                    {
                        var result = str.ToFailureMechanismSectionCategory();
                        if (result > 0 && (int)result < 8)
                        {
                            return EAssessmentResultTypeT3.ResultSpecified;
                        }
                    }
                    catch (InvalidEnumArgumentException)
                    {
                        // Do nothing, return Gr.
                    }
                }

                return EAssessmentResultTypeT3.Gr;
            }

            return assessmentResultType;
        }

        /// <summary>
        /// Translate a string value to a specific EAssessmentResultTypeT4
        /// </summary>
        /// <param name="str">string value to be translated</param>
        /// <returns></returns>
        public static EAssessmentResultTypeT4 ToEAssessmentResultTypeT4(this string str)
        {
            EAssessmentResultTypeT4 assessmentResultType;
            if (!Enum.TryParse(str, true, out assessmentResultType))
            {
                double value;
                if (double.TryParse(str, out value))
                {
                    return EAssessmentResultTypeT4.ResultSpecified;
                }

                return EAssessmentResultTypeT4.Gr;
            }

            return assessmentResultType;
        }
    }
}
