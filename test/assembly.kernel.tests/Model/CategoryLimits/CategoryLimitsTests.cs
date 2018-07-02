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
using Assembly.Kernel.Model;
using Assembly.Kernel.Model.CategoryLimits;
using Assembly.Kernel.Model.FmSectionTypes;
using NUnit.Framework;

// ReSharper disable ObjectCreationAsStatement

namespace Assembly.Kernel.Tests.Model.CategoryLimits
{
    [TestFixture]
    public class CategoryLimitsTests
    {
        public void AssessmentSectionCategoryLimitsTest(EAssessmentGrade assessmentGrade, double lowerLimit,
            double upperLimit, bool shouldExceptionOccure)
        {
            try
            {
                new AssessmentSectionCategory(assessmentGrade, lowerLimit, upperLimit);
            }
            catch (AssemblyException e)
            {
                if (!shouldExceptionOccure)
                {
                    Assert.Fail("Exception occured while it should not have.");
                }

                if (e.Errors != null)
                {
                    var errors = e.Errors as List<AssemblyErrorMessage>;

                    Assert.NotNull(errors);
                    Assert.AreEqual(1, errors.Count);
                    var message = errors[0];

                    Assert.AreEqual(EAssemblyErrors.LowerLimitIsAboveUpperLimit, message.ErrorCode);
                    Assert.AreEqual("Category: " + assessmentGrade, message.EntityId);
                }
            }

            Assert.Pass();
        }

        public void FailureMechanismCategoryLimitsTest(EFailureMechanismCategory category, double lowerLimit,
            double upperLimit, bool shouldExceptionOccure)
        {
            try
            {
                new FailureMechanismCategory(category, lowerLimit, upperLimit);
            }
            catch (AssemblyException e)
            {
                if (!shouldExceptionOccure)
                {
                    Assert.Fail("Exception occured while it should not have.");
                }

                if (e.Errors != null)
                {
                    var errors = e.Errors as List<AssemblyErrorMessage>;

                    Assert.NotNull(errors);
                    Assert.AreEqual(1, errors.Count);
                    var message = errors[0];

                    Assert.AreEqual(EAssemblyErrors.LowerLimitIsAboveUpperLimit, message.ErrorCode);
                    Assert.AreEqual("Category: " + category, message.EntityId);
                }
            }

            Assert.Pass();
        }

        public void FmSectionCategoryLimitsTest(EFmSectionCategory category, double lowerLimit,
            double upperLimit, bool shouldExceptionOccure)
        {
            try
            {
                new FmSectionCategory(category, lowerLimit, upperLimit);
            }
            catch (AssemblyException e)
            {
                if (!shouldExceptionOccure)
                {
                    Assert.Fail("Exception occured while it should not have.");
                }

                if (e.Errors != null)
                {
                    var errors = e.Errors as List<AssemblyErrorMessage>;

                    Assert.NotNull(errors);
                    Assert.AreEqual(1, errors.Count);
                    var message = errors[0];

                    Assert.AreEqual(EAssemblyErrors.LowerLimitIsAboveUpperLimit, message.ErrorCode);
                    Assert.AreEqual("Category: " + category, message.EntityId);
                }
            }

            Assert.Pass();
        }

        [Test]
        public void AssessmentSectionCategoryLimitsTests()
        {
            AssessmentSectionCategoryLimitsTest(EAssessmentGrade.A, 0, 0.1, false);
            AssessmentSectionCategoryLimitsTest(EAssessmentGrade.B, 0.5, 0.1, true);
        }

        [Test]
        public void FailureMechanismCategoryLimitsTests()
        {
            FailureMechanismCategoryLimitsTest(EFailureMechanismCategory.It, 0, 0.1, false);
            FailureMechanismCategoryLimitsTest(EFailureMechanismCategory.VIIt, 0.5, 0.1, true);
        }

        [Test]
        public void FmSectionCategoryLimitsTests()
        {
            FmSectionCategoryLimitsTest(EFmSectionCategory.Iv, 0, 0.1, false);
            FmSectionCategoryLimitsTest(EFmSectionCategory.VIv, 0.5, 0.1, true);
        }
    }
}