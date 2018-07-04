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
using System.Linq;
using Assembly.Kernel.Exceptions;
using Assembly.Kernel.Model;
using Assembly.Kernel.Model.FmSectionTypes;
using NUnit.Framework;

// ReSharper disable ObjectCreationAsStatement

namespace Assembly.Kernel.Tests.Model
{
    [TestFixture]
    public class FailureMechanismSectionListTests
    {
        private static void CheckException(AssemblyException e, EAssemblyErrors expectedError)
        {
            Assert.NotNull(e.Errors);
            var message = e.Errors.FirstOrDefault();
            Assert.NotNull(message);
            Assert.AreEqual(expectedError, message.ErrorCode);
            Assert.Pass();
        }

        [Test]
        public void EmptyListInputTest()
        {
            try
            {
                new FailureMechanismSectionList(
                    "TEST",
                    new List<FailureMechanismSection>());
            }
            catch (AssemblyException e)
            {
                CheckException(e, EAssemblyErrors.CommonFailureMechanismSectionsInvalid);
            }

            Assert.Fail("Expected exception was not thrown");
        }

        [Test]
        public void FailureMechanismNullInputTest()
        {
            var list = new FailureMechanismSectionList(
                null,
                new[] {new FmSectionWithDirectCategory(0, 1, EFmSectionCategory.Gr)}
            );

            Assert.AreEqual("", list.FailureMechanismId);
        }

        [Test]
        public void FirstSectionStartNotZeroInputTest()
        {
            try
            {
                new FailureMechanismSectionList(
                    "TEST",
                    new List<FailureMechanismSection>
                    {
                        new FmSectionWithDirectCategory(1, 5, EFmSectionCategory.Iv),
                        new FmSectionWithDirectCategory(10, 15, EFmSectionCategory.Iv)
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
                    "TEST",
                    new List<FailureMechanismSection>
                    {
                        new FmSectionWithDirectCategory(0, 5, EFmSectionCategory.Iv),
                        new FmSectionWithDirectCategory(10, 15, EFmSectionCategory.Iv)
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
                var fmSectionList = new FailureMechanismSectionList(
                    "TEST",
                    new List<FailureMechanismSection>
                    {
                        new FmSectionWithDirectCategory(0, 10, EFmSectionCategory.Iv),
                        new FmSectionWithDirectCategory(10, 20, EFmSectionCategory.Iv)
                    });
                fmSectionList.GetSectionCategoryForPoint(25.0);
            }
            catch (AssemblyException e)
            {
                CheckException(e, EAssemblyErrors.RequestedPointOutOfRange);
            }

            Assert.Fail("Expected exception was not thrown");
        }

        [Test]
        public void NotTheSameTypeInputTest()
        {
            try
            {
                new FailureMechanismSectionList(
                    "TEST",
                    new List<FailureMechanismSection>
                    {
                        new FmSectionWithDirectCategory(0, 10, EFmSectionCategory.Iv),
                        new FmSectionWithIndirectCategory(10, 20, EIndirectAssessmentResult.Gr)
                    });
            }
            catch (AssemblyException e)
            {
                CheckException(e, EAssemblyErrors.InputNotTheSameType);
            }

            Assert.Fail("Expected exception was not thrown");
        }

        [Test]
        public void OverlappingSectionsInputTest()
        {
            try
            {
                new FailureMechanismSectionList(
                    "TEST",
                    new List<FailureMechanismSection>
                    {
                        new FmSectionWithDirectCategory(0, 10, EFmSectionCategory.Iv),
                        new FmSectionWithDirectCategory(5, 15, EFmSectionCategory.Iv)
                    });
            }
            catch (AssemblyException e)
            {
                CheckException(e, EAssemblyErrors.CommonFailureMechanismSectionsNotConsecutive);
            }

            Assert.Fail("Expected exception was not thrown");
        }

        [Test]
        public void ResultListNullInputTest()
        {
            try
            {
                new FailureMechanismSectionList("TEST", null);
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
                new FailureMechanismSectionList(null, null);
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
                new FailureMechanismSectionList(
                    "TEST",
                    new List<FailureMechanismSection>
                    {
                        new FmSectionWithDirectCategory(0, 10, EFmSectionCategory.Iv),
                        new FmSectionWithDirectCategory(10, 10, EFmSectionCategory.Iv)
                    });
            }
            catch (AssemblyException e)
            {
                CheckException(e, EAssemblyErrors.FmSectionSectionStartEndInvalid);
            }

            Assert.Fail("Expected exception was not thrown");
        }
    }
}