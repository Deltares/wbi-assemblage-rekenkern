#region Copyright (C) Rijkswaterstaat 2022. All rights reserved

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

#endregion

using System.Collections.Generic;
using System.Linq;
using Assembly.Kernel.Exceptions;
using Assembly.Kernel.Model.Categories;
using Assembly.Kernel.Model.FailureMechanismSections;
using NUnit.Framework;

// ReSharper disable ObjectCreationAsStatement

namespace Assembly.Kernel.Tests.Model.FailureMechanismSections
{
    [TestFixture]
    public class FailureMechanismSectionListTests
    {
        [Test]
        public void ResultListNullInputTest()
        {
            try
            {
                new FailureMechanismSectionList(null);
            }
            catch (AssemblyException e)
            {
                CheckException(e, EAssemblyErrors.ValueMayNotBeNull);
            }

            Assert.Fail("Expected exception was not thrown");
        }

        [Test]
        public void EmptyListInputTest()
        {
            try
            {
                new FailureMechanismSectionList(new List<FailureMechanismSection>());
            }
            catch (AssemblyException e)
            {
                CheckException(e, EAssemblyErrors.CommonFailureMechanismSectionsInvalid);
            }

            Assert.Fail("Expected exception was not thrown");
        }

        [Test]
        public void DifferentSectionTypesInputTest()
        {
            try
            {
                new FailureMechanismSectionList(
                    new List<FailureMechanismSection>
                    {
                        new FailureMechanismSection(1, 5),
                        new FailureMechanismSectionWithCategory(5, 15, EInterpretationCategory.I)
                    });
            }
            catch (AssemblyException e)
            {
                CheckException(e, EAssemblyErrors.InputNotTheSameType);
            }

            Assert.Fail("Expected exception was not thrown");
        }

        [Test]
        public void FirstSectionStartNotZeroInputTest()
        {
            try
            {
                new FailureMechanismSectionList(
                    new List<FailureMechanismSection>
                    {
                        new FailureMechanismSectionWithCategory(1, 5, EInterpretationCategory.I),
                        new FailureMechanismSectionWithCategory(10, 15, EInterpretationCategory.I)
                    });
            }
            catch (AssemblyException e)
            {
                CheckException(e, EAssemblyErrors.CommonFailureMechanismSectionsInvalid);
            }

            Assert.Fail("Expected exception was not thrown");
        }

        [Test]
        public void GapBetweenSectionsInputTest()
        {
            try
            {
                new FailureMechanismSectionList(
                    new List<FailureMechanismSection>
                    {
                        new FailureMechanismSectionWithCategory(0, 5, EInterpretationCategory.I),
                        new FailureMechanismSectionWithCategory(10, 15, EInterpretationCategory.I)
                    });
            }
            catch (AssemblyException e)
            {
                CheckException(e, EAssemblyErrors.CommonFailureMechanismSectionsNotConsecutive);
            }

            Assert.Fail("Expected exception was not thrown");
        }

        [Test]
        public void GetCategoryOfSectionOutsideOfRange()
        {
            try
            {
                var failureMechanismSectionList = new FailureMechanismSectionList(
                    new List<FailureMechanismSection>
                    {
                        new FailureMechanismSectionWithCategory(0, 10, EInterpretationCategory.I),
                        new FailureMechanismSectionWithCategory(10, 20, EInterpretationCategory.I)
                    });
                failureMechanismSectionList.GetSectionAtPoint(25.0);
            }
            catch (AssemblyException e)
            {
                CheckException(e, EAssemblyErrors.RequestedPointOutOfRange);
            }

            Assert.Fail("Expected exception was not thrown");
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
            try
            {
                new FailureMechanismSectionList(
                    new List<FailureMechanismSection>
                    {
                        new FailureMechanismSectionWithCategory(0, 10, EInterpretationCategory.I),
                        new FailureMechanismSectionWithCategory(5, 15, EInterpretationCategory.I)
                    });
            }
            catch (AssemblyException e)
            {
                CheckException(e, EAssemblyErrors.CommonFailureMechanismSectionsNotConsecutive);
            }

            Assert.Fail("Expected exception was not thrown");
        }

        [Test]
        public void ZeroLengthSectionInputTest()
        {
            try
            {
                new FailureMechanismSectionList(
                    new List<FailureMechanismSection>
                    {
                        new FailureMechanismSectionWithCategory(0, 10, EInterpretationCategory.I),
                        new FailureMechanismSectionWithCategory(10, 10, EInterpretationCategory.I)
                    });
            }
            catch (AssemblyException e)
            {
                CheckException(e, EAssemblyErrors.FailureMechanismSectionSectionStartEndInvalid);
            }

            Assert.Fail("Expected exception was not thrown");
        }

        private static void CheckException(AssemblyException e, EAssemblyErrors expectedError)
        {
            Assert.NotNull(e.Errors);
            var message = e.Errors.FirstOrDefault();
            Assert.NotNull(message);
            Assert.AreEqual(expectedError, message.ErrorCode);
            Assert.Pass();
        }
    }
}