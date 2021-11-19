#region Copyright (C) Rijkswaterstaat 2019. All rights reserved
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
using System.Linq;
using Assembly.Kernel.Exceptions;
using Assembly.Kernel.Model;
using Assembly.Kernel.Model.FailurePathSectionResults;
using NUnit.Framework;

// ReSharper disable ObjectCreationAsStatement

namespace Assembly.Kernel.Tests.Model
{
    [TestFixture]
    public class FailurePathSectionListTests
    {
        [Test]
        public void EmptyListInputTest()
        {
            try
            {
                new FailurePathSectionList(
                    "TEST",
                    new List<FailurePathSection>());
            }
            catch (AssemblyException e)
            {
                CheckException(e, EAssemblyErrors.CommonFailurePathSectionsInvalid);
            }

            Assert.Fail("Expected exception was not thrown");
        }

        [Test]
        public void FailurePathNullInputTest()
        {
            var list = new FailurePathSectionList(
                null,
                new[]
                {
                    new FailurePathSectionWithCategory(0, 1, EInterpretationCategory.Gr)
                }
            );

            Assert.AreEqual("", list.FailurePathId);
        }

        [Test]
        public void FirstSectionStartNotZeroInputTest()
        {
            try
            {
                new FailurePathSectionList(
                    "TEST",
                    new List<FailurePathSection>
                    {
                        new FailurePathSectionWithCategory(1, 5, EInterpretationCategory.I),
                        new FailurePathSectionWithCategory(10, 15, EInterpretationCategory.I)
                    });
            }
            catch (AssemblyException e)
            {
                CheckException(e, EAssemblyErrors.CommonFailurePathSectionsInvalid);
            }

            Assert.Fail("Expected exception was not thrown");
        }

        [Test]
        public void GapBetweenSectionsInputTest()
        {
            try
            {
                new FailurePathSectionList(
                    "TEST",
                    new List<FailurePathSection>
                    {
                        new FailurePathSectionWithCategory(0, 5, EInterpretationCategory.I),
                        new FailurePathSectionWithCategory(10, 15, EInterpretationCategory.I)
                    });
            }
            catch (AssemblyException e)
            {
                CheckException(e, EAssemblyErrors.CommonFailurePathSectionsNotConsecutive);
            }

            Assert.Fail("Expected exception was not thrown");
        }

        [Test]
        public void GetCategoryOfSectionOutsideOfRange()
        {
            try
            {
                var fmSectionList = new FailurePathSectionList(
                    "TEST",
                    new List<FailurePathSection>
                    {
                        new FailurePathSectionWithCategory(0, 10, EInterpretationCategory.I),
                        new FailurePathSectionWithCategory(10, 20, EInterpretationCategory.I)
                    });
                fmSectionList.GetSectionResultForPoint(25.0);
            }
            catch (AssemblyException e)
            {
                CheckException(e, EAssemblyErrors.RequestedPointOutOfRange);
            }

            Assert.Fail("Expected exception was not thrown");
        }

        [Test]
        public void OverlappingSectionsInputTest()
        {
            try
            {
                new FailurePathSectionList(
                    "TEST",
                    new List<FailurePathSection>
                    {
                        new FailurePathSectionWithCategory(0, 10, EInterpretationCategory.I),
                        new FailurePathSectionWithCategory(5, 15, EInterpretationCategory.I)
                    });
            }
            catch (AssemblyException e)
            {
                CheckException(e, EAssemblyErrors.CommonFailurePathSectionsNotConsecutive);
            }

            Assert.Fail("Expected exception was not thrown");
        }

        [Test]
        public void ResultListNullInputTest()
        {
            try
            {
                new FailurePathSectionList("TEST", null);
            }
            catch (AssemblyException e)
            {
                CheckException(e, EAssemblyErrors.ValueMayNotBeNull);
            }

            Assert.Fail("Expected exception was not thrown");
        }

        [Test]
        public void TwoNullInputTest()
        {
            try
            {
                new FailurePathSectionList(null, null);
            }
            catch (AssemblyException e)
            {
                CheckException(e, EAssemblyErrors.ValueMayNotBeNull);
            }

            Assert.Fail("Expected exception was not thrown");
        }

        [Test]
        public void ZeroLengthSectionInputTest()
        {
            try
            {
                new FailurePathSectionList(
                    "TEST",
                    new List<FailurePathSection>
                    {
                        new FailurePathSectionWithCategory(0, 10, EInterpretationCategory.I),
                        new FailurePathSectionWithCategory(10, 10, EInterpretationCategory.I)
                    });
            }
            catch (AssemblyException e)
            {
                CheckException(e, EAssemblyErrors.FpSectionSectionStartEndInvalid);
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