using System;
using System.ComponentModel;
using assembly.kernel.acceptance.tests.data;
using Assembly.Kernel.Model;
using Assembly.Kernel.Model.FmSectionTypes;

namespace assembly.kernel.acceptance.tests.io
{
    public static class StringExtensions
    {
        public static MechanismType ToMechanismType(this string str)
        {
            MechanismType mechanismType;
            if (!Enum.TryParse(str, true, out mechanismType))
            {
                throw new InvalidEnumArgumentException(str);
            }

            return mechanismType;
        }

        public static EFailureMechanismCategory ToFailureMechanismCategory(this string str)
        {
            EFailureMechanismCategory category;
            if (!Enum.TryParse(str, true, out category))
            {
                switch (str)
                {
                    case "NIET MEEGENOMEN":
                        category = EFailureMechanismCategory.Nvt;
                        break;
                    case "NOG GEEN OORDEEL":
                        category = EFailureMechanismCategory.VIIt;
                        break;
                    default:
                        category = (EFailureMechanismCategory) (-1);
                        break;
                }
            }

            if (string.IsNullOrWhiteSpace(str))
            {
                return EFailureMechanismCategory.Gr;
            }

            if (category < 0)
            {
                throw new InvalidEnumArgumentException(str);
            }

            return category;
        }

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
                    throw new InvalidEnumArgumentException(str);
            }
        }

        public static EFmSectionCategory ToFailureMechanismSectionCategory(this string str)
        {
            EFmSectionCategory sectionCategory;
            if (!Enum.TryParse(str, true, out sectionCategory))
            {
                switch (str)
                {
                    case "NIET MEEGENOMEN":
                    case "-":
                        sectionCategory = EFmSectionCategory.NotApplicable;
                        break;
                    case "NOG GEEN OORDEEL":
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

        public static EAssessmentGrade ToAssessmentGrade(this string str)
        {
            EAssessmentGrade sectionCategory;
            if (!Enum.TryParse(str, true, out sectionCategory))
            {
                switch (str)
                {
                    case "A+":
                        sectionCategory = EAssessmentGrade.APlus;
                        break;
                    case "NIET MEEGENOMEN":
                        sectionCategory = EAssessmentGrade.Nvt;
                        break;
                    case "NOG GEEN OORDEEL":
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
    }
}
