#region Copyright (C) Rijkswaterstaat 2019. All rights reserved
// Copyright (C) Rijkswaterstaat 2019. All rights reserved.
//
// This file is part of the Assembly kernel.
//
// Assembly kernel is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program. If not, see <http://www.gnu.org/licenses/>.
//
// All names, logos, and references to "Rijkswaterstaat" are registered trademarks of
// Rijkswaterstaat and remain full property of Rijkswaterstaat at all times.
// All rights reserved.
#endregion

using System.Collections.Generic;
using Assembly.Kernel.Exceptions;
using Assembly.Kernel.Interfaces;
using Assembly.Kernel.Model;
using Assembly.Kernel.Model.CategoryLimits;
using Assembly.Kernel.Model.FmSectionTypes;

namespace Assembly.Kernel.Implementations
{
    /// <inheritdoc />
    /// <summary>
    /// Implementation of the category limits interface.
    /// </summary>
    public class CategoryLimitsCalculator : ICategoryLimitsCalculator
    {
        /// <inheritdoc />
        public CategoriesList<AssessmentSectionCategory> CalculateAssessmentSectionCategoryLimitsWbi21(
            AssessmentSection section)
        {
            var sigDiv30 = CapToOne(section.FailureProbabilitySignallingLimit / 30.0);
            var lowTimes30 = CapToOne(section.FailureProbabilityLowerLimit * 30.0);

            return new CategoriesList<AssessmentSectionCategory>(new[]
            {
                new AssessmentSectionCategory(
                    EAssessmentGrade.APlus,
                    0,
                    sigDiv30),
                new AssessmentSectionCategory(
                    EAssessmentGrade.A,
                    sigDiv30,
                    section.FailureProbabilitySignallingLimit),
                new AssessmentSectionCategory(
                    EAssessmentGrade.B,
                    section.FailureProbabilitySignallingLimit,
                    section.FailureProbabilityLowerLimit),
                new AssessmentSectionCategory(
                    EAssessmentGrade.C,
                    section.FailureProbabilityLowerLimit,
                    lowTimes30),
                new AssessmentSectionCategory(
                    EAssessmentGrade.D,
                    lowTimes30,
                    1)
            });
        }

        /// <inheritdoc />
        public CategoriesList<FailureMechanismCategory> CalculateFailureMechanismCategoryLimitsWbi11(
            AssessmentSection section, FailureMechanism failureMechanism)
        {
            var signTimesMargin = CapToOne(section.FailureProbabilitySignallingLimit *
                                           failureMechanism.FailureProbabilityMarginFactor);
            var signTimesMarginDiv30 = CapToOne(signTimesMargin / 30.0);
            var lowTimesMargin = CapToOne(section.FailureProbabilityLowerLimit *
                                          failureMechanism.FailureProbabilityMarginFactor);
            var lowTimes30 = CapToOne(section.FailureProbabilityLowerLimit * 30.0);

            return new CategoriesList<FailureMechanismCategory>(new[]
            {
                new FailureMechanismCategory(
                    EFailureMechanismCategory.It,
                    0,
                    signTimesMarginDiv30),
                new FailureMechanismCategory(
                    EFailureMechanismCategory.IIt,
                    signTimesMarginDiv30,
                    signTimesMargin),
                new FailureMechanismCategory(
                    EFailureMechanismCategory.IIIt,
                    signTimesMargin,
                    lowTimesMargin),
                new FailureMechanismCategory(
                    EFailureMechanismCategory.IVt,
                    lowTimesMargin,
                    section.FailureProbabilityLowerLimit),
                new FailureMechanismCategory(
                    EFailureMechanismCategory.Vt,
                    section.FailureProbabilityLowerLimit,
                    lowTimes30),
                new FailureMechanismCategory(
                    EFailureMechanismCategory.VIt,
                    lowTimes30,
                    1)
            });
        }

        /// <inheritdoc />
        public CategoriesList<FmSectionCategory> CalculateFmSectionCategoryLimitsWbi01(AssessmentSection section,
            FailureMechanism failureMechanism)
        {
            var pSigDsn = failureMechanism.FailureProbabilityMarginFactor * section.FailureProbabilitySignallingLimit /
                          failureMechanism.LengthEffectFactor;
            var pLowDsn = failureMechanism.FailureProbabilityMarginFactor * section.FailureProbabilityLowerLimit /
                          failureMechanism.LengthEffectFactor;

            CheckPdsnValues(section, pSigDsn, pLowDsn);

            return new CategoriesList<FmSectionCategory>(new[]
            {
                new FmSectionCategory(
                    EFmSectionCategory.Iv,
                    0,
                    CapToOne(pSigDsn / 30.0)),
                new FmSectionCategory(
                    EFmSectionCategory.IIv,
                    CapToOne(pSigDsn / 30.0),
                    pSigDsn),
                new FmSectionCategory(
                    EFmSectionCategory.IIIv,
                    pSigDsn,
                    pLowDsn),
                new FmSectionCategory(
                    EFmSectionCategory.IVv,
                    pLowDsn,
                    section.FailureProbabilityLowerLimit),
                new FmSectionCategory(
                    EFmSectionCategory.Vv,
                    section.FailureProbabilityLowerLimit,
                    CapToOne(section.FailureProbabilityLowerLimit * 30.0)),
                new FmSectionCategory(
                    EFmSectionCategory.VIv,
                    CapToOne(section.FailureProbabilityLowerLimit * 30.0),
                    1)
            });
        }

        /// <inheritdoc />
        public CategoriesList<FmSectionCategory> CalculateFmSectionCategoryLimitsWbi02(double assessmentSectionNorm,
            FailureMechanism failureMechanism)
        {
            var pDsn = CapToOne(failureMechanism.FailureProbabilityMarginFactor *
                                (10 * assessmentSectionNorm) /
                                failureMechanism.LengthEffectFactor);

            return new CategoriesList<FmSectionCategory>(new[]
            {
                new FmSectionCategory(
                    EFmSectionCategory.IIv,
                    0,
                    pDsn),
                new FmSectionCategory(
                    EFmSectionCategory.Vv,
                    pDsn,
                    1)
            });
        }


        private static void CheckPdsnValues(AssessmentSection section, double pSigDsn,
            double pLowDsn)
        {
            var errors = new List<AssemblyErrorMessage>();
            if (pSigDsn >= section.FailureProbabilitySignallingLimit)
            {
                errors.Add(new AssemblyErrorMessage("CalculateCategoryLimits",
                    EAssemblyErrors.PsigDsnAbovePsig));
            }

            if (pLowDsn >= section.FailureProbabilityLowerLimit)
            {
                errors.Add(new AssemblyErrorMessage("CalculateCategoryLimits",
                    EAssemblyErrors.PlowDsnAbovePlow));
            }

            if (errors.Count > 0)
            {
                throw new AssemblyException(errors);
            }
        }

        /// <summary>
        /// Caps the input value to one. So every value above one will return one.
        /// </summary>
        /// <param name="d">The value to cap</param>
        /// <returns>The capped value</returns>
        private static double CapToOne(double d)
        {
            return d > 1.0 ? 1.0 : d;
        }
    }
}