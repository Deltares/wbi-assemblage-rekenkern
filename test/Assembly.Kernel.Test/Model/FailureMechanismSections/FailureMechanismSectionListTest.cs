// Copyright (C) Stichting Deltares and State of the Netherlands 2023. All rights reserved.
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
// All names, logos, and references to "Deltares" are registered trademarks of
// Stichting Deltares and remain full property of Stichting Deltares at all times.
// All rights reserved.

using System;
using System.Linq;
using Assembly.Kernel.Exceptions;
using Assembly.Kernel.Model.Categories;
using Assembly.Kernel.Model.FailureMechanismSections;
using NUnit.Framework;

namespace Assembly.Kernel.Test.Model.FailureMechanismSections
{
    [TestFixture]
    public class FailureMechanismSectionListTest
    {
        [Test]
        public void Constructor_SectionsNull_ThrowsArgumentNullException()
        {
            // Call
            void Call() => new FailureMechanismSectionList(null);

            // Assert
            var exception = Assert.Throws<ArgumentNullException>(Call);
            Assert.AreEqual("sections", exception.ParamName);
        }

        [Test]
        public void Constructor_SectionsEmpty_ThrowsAssemblyException()
        {
            // Call
            void Call() => new FailureMechanismSectionList(Enumerable.Empty<FailureMechanismSection>());

            // Assert
            TestHelper.AssertThrowsAssemblyExceptionWithAssemblyErrorMessages(Call, new[]
            {
                new AssemblyErrorMessage("sections", EAssemblyErrors.CommonFailureMechanismSectionsInvalid)
            });
        }

        [Test]
        public void Constructor_SectionsNotSameType_ThrowsAssemblyException()
        {
            // Call
            void Call() => new FailureMechanismSectionList(new[]
            {
                new FailureMechanismSection(0.0, 1.0),
                new FailureMechanismSectionWithCategory(1.0, 2.0, EInterpretationCategory.Dominant)
            });

            // Assert
            TestHelper.AssertThrowsAssemblyExceptionWithAssemblyErrorMessages(Call, new[]
            {
                new AssemblyErrorMessage("sections", EAssemblyErrors.InputNotTheSameType)
            });
        }

        [Test]
        public void Constructor_SectionsNotStartingAtZero_ThrowsAssemblyException()
        {
            // Call
            void Call() => new FailureMechanismSectionList(new[]
            {
                new FailureMechanismSection(0.011, 1.0)
            });

            // Assert
            TestHelper.AssertThrowsAssemblyExceptionWithAssemblyErrorMessages(Call, new[]
            {
                new AssemblyErrorMessage("sections", EAssemblyErrors.CommonFailureMechanismSectionsInvalid)
            });
        }

        [Test]
        public void Constructor_SectionsNotConsecutive_ThrowsAssemblyException()
        {
            // Call
            void Call() => new FailureMechanismSectionList(new[]
            {
                new FailureMechanismSection(0.0, 1.0),
                new FailureMechanismSection(1.011, 2.0)
            });

            // Assert
            TestHelper.AssertThrowsAssemblyExceptionWithAssemblyErrorMessages(Call, new[]
            {
                new AssemblyErrorMessage("sections", EAssemblyErrors.CommonFailureMechanismSectionsNotConsecutive)
            });
        }

        [Test]
        public void Constructor_ExpectedValues()
        {
            // Setup
            var sections = new[]
            {
                new FailureMechanismSection(0.0, 132.0),
                new FailureMechanismSection(132.0, 823.01),
                new FailureMechanismSection(823.0, 1846.0),
                new FailureMechanismSection(1846.01, 3201.8)
            };

            // Call
            var sectionList = new FailureMechanismSectionList(sections);

            // Assert
            Assert.AreSame(sections, sectionList.Sections);
        }

        [Test]
        public void GetSectionAtPoint_PointOutOfRange_ThrowsAssemblyException()
        {
            // Setup
            var sections = new[]
            {
                new FailureMechanismSection(0.0, 132.0),
                new FailureMechanismSection(132.0, 823.01),
                new FailureMechanismSection(823.0, 1846.0),
                new FailureMechanismSection(1846.01, 3201.8)
            };

            var sectionList = new FailureMechanismSectionList(sections);

            // Call
            void Call() => sectionList.GetSectionAtPoint(3201.81);

            // Assert
            TestHelper.AssertThrowsAssemblyExceptionWithAssemblyErrorMessages(Call, new[]
            {
                new AssemblyErrorMessage("pointInAssessmentSection", EAssemblyErrors.RequestedPointOutOfRange)
            });
        }

        [Test]
        public void GetSectionAtPoint_PointInRange_ReturnsExpectedSection()
        {
            // Setup
            var expectedSection = new FailureMechanismSection(132.0, 823.01);
            FailureMechanismSection[] sections =
            {
                new FailureMechanismSection(0.0, 132.0),
                expectedSection,
                new FailureMechanismSection(823.0, 1846.0),
                new FailureMechanismSection(1846.01, 3201.8)
            };

            var sectionList = new FailureMechanismSectionList(sections);

            // Call
            FailureMechanismSection section = sectionList.GetSectionAtPoint(574);

            // Assert
            Assert.AreSame(expectedSection, section);
        }
    }
}