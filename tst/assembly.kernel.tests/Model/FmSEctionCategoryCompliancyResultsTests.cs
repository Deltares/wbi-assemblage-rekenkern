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

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assembly.Kernel.Exceptions;
using Assembly.Kernel.Model;
using Assembly.Kernel.Model.FmSectionTypes;
using NUnit.Framework;

namespace Assembly.Kernel.Tests.Model {
    [TestFixture]
    public class FmSEctionCategoryCompliancyResultsTests {

        private sealed class CompliancyTestCases {
            public static IEnumerable NotAllowedCategories {
                // ReSharper disable once UnusedMember.Local
                get {
                    yield return new TestCaseData(EFmSectionCategory.VIv);
                    yield return new TestCaseData(EFmSectionCategory.VIIv);
                    yield return new TestCaseData(EFmSectionCategory.Gr);
                    yield return new TestCaseData(EFmSectionCategory.NotApplicable);

                }
            }
        }

        [Test, TestCaseSource(
                 typeof(CompliancyTestCases),
                 nameof(CompliancyTestCases.NotAllowedCategories))]
        public void CategoryNotAllowed(EFmSectionCategory category) {
            try {
                new FmSectionCategoryCompliancyResults().Set(category, ECategoryCompliancy.Complies);
            } catch (AssemblyException e) {
                Assert.NotNull(e.Errors);
                var message = e.Errors.FirstOrDefault();
                Assert.NotNull(message);
                Assert.AreEqual(EAssemblyErrors.CategoryNotAllowed, message.ErrorCode);
                Assert.Pass();
            }
            Assert.Fail("Expected exception was not thrown");
        }        
    }
}