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

using System.Collections.Generic;
using Assembly.Kernel.Exceptions;
using Assembly.Kernel.Model.Categories;
using NUnit.Framework;

// ReSharper disable ObjectCreationAsStatement

namespace Assembly.Kernel.Tests.Model.Categories
{
    [TestFixture]
    public class CategoryLimitsTests
    {
        [Test]
        public void AssessmentSectionCategoryLimitsTests()
        {
            AssessmentSectionCategoryLimitsTest(EAssessmentGrade.A, 0, 0.1, false);
            AssessmentSectionCategoryLimitsTest(EAssessmentGrade.B, 0.5, 0.1, true);
        }

        private void AssessmentSectionCategoryLimitsTest(EAssessmentGrade assessmentGrade, double lowerLimit,
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
    }
}