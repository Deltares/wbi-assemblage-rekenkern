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
        public IEnumerable<AssessmentSectionCategoryLimits> CalculateAssessmentSectionCategoryLimitsWbi21(
            AssessmentSection section)
        {
            var sigDiv30 = CapToOne(section.FailureProbabilitySignallingLimit / 30.0);
            var lowTimes30 = CapToOne(section.FailureProbabilityLowerLimit * 30.0);

            var limits = new List<AssessmentSectionCategoryLimits>
            {
                new AssessmentSectionCategoryLimits(
                    EAssessmentGrade.APlus,
                    0,
                    sigDiv30),
                new AssessmentSectionCategoryLimits(
                    EAssessmentGrade.A,
                    sigDiv30,
                    section.FailureProbabilitySignallingLimit),
                new AssessmentSectionCategoryLimits(
                    EAssessmentGrade.B,
                    section.FailureProbabilitySignallingLimit,
                    section.FailureProbabilityLowerLimit),
                new AssessmentSectionCategoryLimits(
                    EAssessmentGrade.C,
                    section.FailureProbabilityLowerLimit,
                    lowTimes30),
                new AssessmentSectionCategoryLimits(
                    EAssessmentGrade.D,
                    lowTimes30,
                    1)
            };

            return limits;
        }

        /// <inheritdoc />
        public IEnumerable<FailureMechanismCategoryLimits> CalculateFailureMechanismCategoryLimitsWbi11(
            AssessmentSection section, FailureMechanism failureMechanism)
        {
            var signTimesMargin = CapToOne(section.FailureProbabilitySignallingLimit *
                                           failureMechanism.FailureProbabilityMarginFactor);
            var signTimesMarginDiv30 = CapToOne(signTimesMargin / 30.0);
            var lowTimesMargin = CapToOne(section.FailureProbabilityLowerLimit *
                                          failureMechanism.FailureProbabilityMarginFactor);
            var lowTimes30 = CapToOne(section.FailureProbabilityLowerLimit * 30.0);

            var limits = new List<FailureMechanismCategoryLimits>
            {
                new FailureMechanismCategoryLimits(
                    EFailureMechanismCategory.It,
                    0,
                    signTimesMarginDiv30),
                new FailureMechanismCategoryLimits(
                    EFailureMechanismCategory.IIt,
                    signTimesMarginDiv30,
                    signTimesMargin),
                new FailureMechanismCategoryLimits(
                    EFailureMechanismCategory.IIIt,
                    signTimesMargin,
                    lowTimesMargin),
                new FailureMechanismCategoryLimits(
                    EFailureMechanismCategory.IVt,
                    lowTimesMargin,
                    section.FailureProbabilityLowerLimit),
                new FailureMechanismCategoryLimits(
                    EFailureMechanismCategory.Vt,
                    section.FailureProbabilityLowerLimit,
                    lowTimes30),
                new FailureMechanismCategoryLimits(
                    EFailureMechanismCategory.VIt,
                    lowTimes30,
                    1)
            };

            return limits;
        }

        /// <inheritdoc />
        public IEnumerable<FmSectionCategoryLimits> CalculateFmSectionCategoryLimitsWbi01(AssessmentSection section,
            FailureMechanism failureMechanism)
        {
            var pSigDsn =
                failureMechanism.FailureProbabilityMarginFactor * section.FailureProbabilitySignallingLimit /
                failureMechanism.LengthEffectFactor;
            var pLowDsn = failureMechanism.FailureProbabilityMarginFactor * section.FailureProbabilityLowerLimit /
                          failureMechanism.LengthEffectFactor;

            return CalucalteLimitsPdsn(section, pSigDsn, pLowDsn);
        }

        /// <inheritdoc />
        public IEnumerable<FmSectionCategoryLimits> CalculateFmSectionCategoryLimitsWbi02(AssessmentSection section,
            FailureMechanism failureMechanism)
        {
            var pSigDsn =
                CapToOne(failureMechanism.FailureProbabilityMarginFactor *
                         (10 * section.FailureProbabilitySignallingLimit) /
                         failureMechanism.LengthEffectFactor);
            var pLowDsn =
                CapToOne(failureMechanism.FailureProbabilityMarginFactor * (10 * section.FailureProbabilityLowerLimit) /
                         failureMechanism.LengthEffectFactor);

            return CalucalteLimitsPdsn(section, pSigDsn, pLowDsn);
        }


        private static IEnumerable<FmSectionCategoryLimits> CalucalteLimitsPdsn(AssessmentSection section,
            double pSigDsn, double pLowDsn)
        {
            CheckPdsnValues(section, pSigDsn, pLowDsn);

            var pSigDsnDiv30 = CapToOne(pSigDsn / 30.0);
            var lowTimes30 = CapToOne(section.FailureProbabilityLowerLimit * 30.0);

            var limits = new List<FmSectionCategoryLimits>
            {
                new FmSectionCategoryLimits(
                    EFmSectionCategory.Iv,
                    0,
                    pSigDsnDiv30),
                new FmSectionCategoryLimits(
                    EFmSectionCategory.IIv,
                    pSigDsnDiv30,
                    pSigDsn),
                new FmSectionCategoryLimits(
                    EFmSectionCategory.IIIv,
                    pSigDsn,
                    pLowDsn),
                new FmSectionCategoryLimits(
                    EFmSectionCategory.IVv,
                    pLowDsn,
                    section.FailureProbabilityLowerLimit),
                new FmSectionCategoryLimits(
                    EFmSectionCategory.Vv,
                    section.FailureProbabilityLowerLimit,
                    lowTimes30),
                new FmSectionCategoryLimits(
                    EFmSectionCategory.VIv,
                    lowTimes30,
                    1)
            };

            return limits;
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