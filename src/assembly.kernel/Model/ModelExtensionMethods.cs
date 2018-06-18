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
using Assembly.Kernel.Model.FmSectionTypes;

namespace Assembly.Kernel.Model {
    /// <summary>
    /// Extension methods used in the application.
    /// </summary>
    public static class ModelExtensionMethods {
        private static readonly Dictionary<EFmSectionCategory, int> FmSectionDirectCategoryOrderMap =
            new Dictionary<EFmSectionCategory, int> {
                { EFmSectionCategory.NotApplicable, 1 },
                { EFmSectionCategory.Iv, 2 },
                { EFmSectionCategory.IIv, 3 },
                { EFmSectionCategory.IIIv, 4 },
                { EFmSectionCategory.IVv, 5 },
                { EFmSectionCategory.Vv, 6 },
                { EFmSectionCategory.VIv, 7 },
                { EFmSectionCategory.VIIv, 8 },
                { EFmSectionCategory.Gr, 9 },
            };

        private static readonly Dictionary<EIndirectAssessmentResult, int> IndirectCategoryOrderMap =
            new Dictionary<EIndirectAssessmentResult, int> {
                { EIndirectAssessmentResult.Nvt, 1 },
                { EIndirectAssessmentResult.FvEt, 2 },
                { EIndirectAssessmentResult.FvGt, 3 },
                { EIndirectAssessmentResult.FvTom, 4 },
                { EIndirectAssessmentResult.FactoredInOtherFailureMechanism, 5 },
                { EIndirectAssessmentResult.Ngo, 6 },
                { EIndirectAssessmentResult.Gr, 7 },
            };

        private static readonly Dictionary<EFailureMechanismCategory, int> FailureMechanismCategoryOrderMap =
            new Dictionary<EFailureMechanismCategory, int> {
                { EFailureMechanismCategory.Nvt, 1 },
                { EFailureMechanismCategory.It, 2 },
                { EFailureMechanismCategory.IIt, 3 },
                { EFailureMechanismCategory.IIIt, 4 },
                { EFailureMechanismCategory.IVt, 5 },
                { EFailureMechanismCategory.Vt, 6 },
                { EFailureMechanismCategory.VIt, 7 },
                { EFailureMechanismCategory.VIIt, 8 },
                { EFailureMechanismCategory.Gr, 9 },
            };

        private static readonly Dictionary<EAssessmentGrade, int> AssessmentGradeOrderMap =
            new Dictionary<EAssessmentGrade, int> {
                { EAssessmentGrade.Nvt, 1 },
                { EAssessmentGrade.APlus, 2 },
                { EAssessmentGrade.A, 3 },
                { EAssessmentGrade.B, 4 },
                { EAssessmentGrade.C, 5 },
                { EAssessmentGrade.D, 6 },
                { EAssessmentGrade.Ngo, 7 },
                { EAssessmentGrade.Gr, 8 },
            };

        /// <summary>
        /// Determines if the current category is lower in rank than the supplied category.
        /// </summary>
        /// <param name="thisCategory">The category on which this method is called</param>
        /// <param name="category">The category to compare to</param>
        /// <returns>True if this category is lower in rank than \"category\"</returns>
        public static bool IsLowerCategoryThan(this EFmSectionCategory thisCategory, EFmSectionCategory category) {
            return FmSectionDirectCategoryOrderMap[thisCategory] > FmSectionDirectCategoryOrderMap[category];
        }

        /// <summary>
        /// Determines if the current category is lower in rank than the supplied category.
        /// </summary>
        /// <param name="thisCategory">The category on which this method is called</param>
        /// <param name="category">The category to compare to</param>
        /// <returns>True if the category is lower in rank than \"category\"</returns>
        public static bool IsLowerCategoryThan(this EIndirectAssessmentResult thisCategory,
            EIndirectAssessmentResult category) {
            return IndirectCategoryOrderMap[thisCategory] > IndirectCategoryOrderMap[category];
        }

        /// <summary>
        /// Determines if the current category is lower in rank than the supplied category.
        /// </summary>
        /// <param name="thisCategory">The category on which this method is called</param>
        /// <param name="category">The category to compare to</param>
        /// <returns>True if the category is lower in rank than \"category\"</returns>
        public static bool IsLowerCategoryThan(this EAssessmentGrade thisCategory,
            EAssessmentGrade category) {
            return AssessmentGradeOrderMap[thisCategory] > AssessmentGradeOrderMap[category];
        }

        /// <summary>
        /// Determines if the current category is lower in rank than the supplied category.
        /// </summary>
        /// <param name="thisCategory">The category on which this method is called</param>
        /// <param name="category">The category to compare to</param>
        /// <returns>True if the category is lower in rank than \"category\"</returns>
        public static bool IsLowerCategoryThan(this EFailureMechanismCategory thisCategory,
            EFailureMechanismCategory category) {
            return FailureMechanismCategoryOrderMap[thisCategory] > FailureMechanismCategoryOrderMap[category];
        }

        /// <summary>
        /// Translates an EFmSectionCategory to an EFailureMechanismCategory, as specified in WBI-1A-1
        /// </summary>
        /// <param name="failureMechanismCategory">The failure mechanism section category</param>
        /// <returns>The failure mechanism category belonging to the section category</returns>
        /// <exception cref="AssemblyException">Thrown when a category is present which is unkown to the translator
        /// </exception>
        public static EFailureMechanismCategory ToAssessmentGrade(
            this EFmSectionCategory failureMechanismCategory) {
            switch (failureMechanismCategory) {
            case EFmSectionCategory.Iv:
                return EFailureMechanismCategory.It;
            case EFmSectionCategory.IIv:
                return EFailureMechanismCategory.IIt;
            case EFmSectionCategory.IIIv:
                return EFailureMechanismCategory.IIIt;
            case EFmSectionCategory.IVv:
                return EFailureMechanismCategory.IVt;
            case EFmSectionCategory.Vv:
                return EFailureMechanismCategory.Vt;
            case EFmSectionCategory.VIv:
                return EFailureMechanismCategory.VIt;
            case EFmSectionCategory.VIIv:
                return EFailureMechanismCategory.VIIt;
            case EFmSectionCategory.Gr:
                return EFailureMechanismCategory.Gr;
            case EFmSectionCategory.NotApplicable:
                return EFailureMechanismCategory.Nvt;
            default:
                throw new AssemblyException("FailureMechanismAssembler: " + failureMechanismCategory,
                    EAssemblyErrors.FailureMechanismAssemblerInputInvalid);
            }
        }

        /// <summary>
        /// Translates an EFailureMechanismCategory to an EAssessmentGrade, as specified in Wbi-2A-1
        /// </summary>
        /// <param name="failureMechanismCategory">The failure mechanism section category</param>
        /// <returns>The failure mechanism category belonging to the section category</returns>
        /// <exception cref="AssemblyException">Thrown when a category is present which is unkown to the translator
        /// </exception>
        public static EAssessmentGrade ToAssessmentGrade(
            this EFailureMechanismCategory failureMechanismCategory) {
            switch (failureMechanismCategory) {
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