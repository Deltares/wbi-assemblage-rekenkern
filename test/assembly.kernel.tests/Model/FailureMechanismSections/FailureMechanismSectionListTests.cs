// Copyright (C) Rijkswaterstaat 2022. All rights reserved.
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

using System.Collections.Generic;
using Assembly.Kernel.Exceptions;
using Assembly.Kernel.Model.Categories;
using Assembly.Kernel.Model.FailureMechanismSections;
using NUnit.Framework;

namespace Assembly.Kernel.Tests.Model.FailureMechanismSections
{
    [TestFixture]
    public class FailureMechanismSectionListTests
    {
        [Test]
        public void ResultListNullInputTest()
        {
            TestHelper.AssertExpectedErrorMessage(() =>
            {
                new FailureMechanismSectionList(null);
            }, EAssemblyErrors.ValueMayNotBeNull);
        }

        [Test]
        public void EmptyListInputTest()
        {
            TestHelper.AssertExpectedErrorMessage(() =>
            {
                new FailureMechanismSectionList(new List<FailureMechanismSection>());
            }, EAssemblyErrors.CommonFailureMechanismSectionsInvalid);
        }

        [Test]
        public void DifferentSectionTypesInputTest()
        {
            TestHelper.AssertExpectedErrorMessage(() =>
            {
                new FailureMechanismSectionList(
                    new List<FailureMechanismSection>
                    {
                        new FailureMechanismSection(1, 5),
                        new FailureMechanismSectionWithCategory(5, 15, EInterpretationCategory.I)
                    });
            }, EAssemblyErrors.InputNotTheSameType);
        }

        [Test]
        public void FirstSectionStartNotZeroInputTest()
        {
            TestHelper.AssertExpectedErrorMessage(() =>
            {
                new FailureMechanismSectionList(
                    new List<FailureMechanismSection>
                    {
                        new FailureMechanismSectionWithCategory(1, 5, EInterpretationCategory.I),
                        new FailureMechanismSectionWithCategory(10, 15, EInterpretationCategory.I)
                    });
            }, EAssemblyErrors.CommonFailureMechanismSectionsInvalid);
        }

        [Test]
        public void GapBetweenSectionsInputTest()
        {
            TestHelper.AssertExpectedErrorMessage(() =>
            {
                new FailureMechanismSectionList(
                    new List<FailureMechanismSection>
                    {
                        new FailureMechanismSectionWithCategory(0, 5, EInterpretationCategory.I),
                        new FailureMechanismSectionWithCategory(10, 15, EInterpretationCategory.I)
                    });
            }, EAssemblyErrors.CommonFailureMechanismSectionsNotConsecutive);
        }

        [Test]
        public void GetCategoryOfSectionOutsideOfRange()
        {
            TestHelper.AssertExpectedErrorMessage(() =>
            {
                var failureMechanismSectionList = new FailureMechanismSectionList(
                    new List<FailureMechanismSection>
                    {
                        new FailureMechanismSectionWithCategory(0, 10, EInterpretationCategory.I),
                        new FailureMechanismSectionWithCategory(10, 20, EInterpretationCategory.I)
                    });
                failureMechanismSectionList.GetSectionAtPoint(25.0);
            }, EAssemblyErrors.RequestedPointOutOfRange);
        }

        [Test]
        public void GetCategoryOfSectionReturnsCategory()
        {
            var failureMechanismSectionList = new FailureMechanismSectionList(
                new List<FailureMechanismSection>
                {
                    new FailureMechanismSectionWithCategory(0, 10, EInterpretationCategory.I),
                    new FailureMechanismSectionWithCategory(10, 20, EInterpretationCategory.II)
                });
            var s = failureMechanismSectionList.GetSectionAtPoint(15.0);
            Assert.AreEqual(10,s.Start);
            Assert.AreEqual(20, s.End);
            var sectionWithCategory = s as FailureMechanismSectionWithCategory;
            Assert.IsNotNull(sectionWithCategory);
            Assert.AreEqual(EInterpretationCategory.II, sectionWithCategory.Category);
        }

        [Test]
        public void OverlappingSectionsInputTest()
        {
            TestHelper.AssertExpectedErrorMessage(() =>
            {
                new FailureMechanismSectionList(
                    new List<FailureMechanismSection>
                    {
                        new FailureMechanismSectionWithCategory(0, 10, EInterpretationCategory.I),
                        new FailureMechanismSectionWithCategory(5, 15, EInterpretationCategory.I)
                    });
            }, EAssemblyErrors.CommonFailureMechanismSectionsNotConsecutive);
        }

        [Test]
        public void ZeroLengthSectionInputTest()
        {
            TestHelper.AssertExpectedErrorMessage(() =>
            {
                new FailureMechanismSectionList(
                    new List<FailureMechanismSection>
                    {
                        new FailureMechanismSectionWithCategory(0, 10, EInterpretationCategory.I),
                        new FailureMechanismSectionWithCategory(10, 10, EInterpretationCategory.I)
                    });
            }, EAssemblyErrors.FailureMechanismSectionSectionStartEndInvalid);
        }
    }
}