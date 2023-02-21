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

using System;
using System.Collections.Generic;
using System.Linq;
using Assembly.Kernel.Exceptions;
using Assembly.Kernel.Implementations;
using Assembly.Kernel.Interfaces;
using Assembly.Kernel.Model.Categories;
using Assembly.Kernel.Model.FailureMechanismSections;
using NUnit.Framework;

namespace Assembly.Kernel.Test.Implementations
{
    [TestFixture]
    public class CommonFailureMechanismSectionAssemblerTest
    {
        [Test]
        public void Constructor_ExpectedValues()
        {
            // Call
            var assembler = new CommonFailureMechanismSectionAssembler();

            // Assert
            Assert.IsInstanceOf<ICommonFailureMechanismSectionAssembler>(assembler);
        }

        [Test]
        [TestCase(double.NaN)]
        [TestCase(0.0)]
        [TestCase(-1.0)]
        public void FindGreatestCommonDenominatorSectionsBoi3A1_InvalidAssessmentSectionLength_ThrowsAssemblyException(
            double assessmentSectionLength)
        {
            // Setup
            var assembler = new CommonFailureMechanismSectionAssembler();

            // Call
            void Call() => assembler.FindGreatestCommonDenominatorSectionsBoi3A1(new[]
            {
                new FailureMechanismSectionList(new[]
                {
                    new FailureMechanismSection(0.0, 10.0)
                })
            }, assessmentSectionLength);

            // Assert
            TestHelper.AssertThrowsAssemblyExceptionWithAssemblyErrorMessages(Call, new[]
            {
                new AssemblyErrorMessage("assessmentSectionLength", EAssemblyErrors.SectionLengthOutOfRange)
            });
        }

        [Test]
        public void FindGreatestCommonDenominatorSectionsBoi3A1_FailureMechanismSectionsNull_ThrowsArgumentNullException()
        {
            // Setup
            var assembler = new CommonFailureMechanismSectionAssembler();

            // Call
            void Call() => assembler.FindGreatestCommonDenominatorSectionsBoi3A1(null, 1.0);

            // Assert
            var exception = Assert.Throws<ArgumentNullException>(Call);
            Assert.AreEqual("failureMechanismSectionLists", exception.ParamName);
        }

        [Test]
        public void FindGreatestCommonDenominatorSectionsBoi3A1_FailureMechanismSectionsEmpty_ThrowsAssemblyException()
        {
            // Setup
            var assembler = new CommonFailureMechanismSectionAssembler();

            // Call
            void Call() => assembler.FindGreatestCommonDenominatorSectionsBoi3A1(Enumerable.Empty<FailureMechanismSectionList>(), 1.0);

            // Assert
            TestHelper.AssertThrowsAssemblyExceptionWithAssemblyErrorMessages(Call, new[]
            {
                new AssemblyErrorMessage("failureMechanismSectionLists", EAssemblyErrors.EmptyResultsList)
            });
        }

        [Test]
        public void FindGreatestCommonDenominatorSectionsBoi3A1_FailureMechanismSectionsLengthNotEqualToAssessmentSectionLength_ThrowsAssemblyException()
        {
            // Setup
            var assembler = new CommonFailureMechanismSectionAssembler();

            // Call
            void Call() => assembler.FindGreatestCommonDenominatorSectionsBoi3A1(new[]
            {
                new FailureMechanismSectionList(new[]
                {
                    new FailureMechanismSection(0.0, 10.011)
                })
            }, 10.0);

            // Assert
            TestHelper.AssertThrowsAssemblyExceptionWithAssemblyErrorMessages(Call, new[]
            {
                new AssemblyErrorMessage("failureMechanismSectionList", EAssemblyErrors.FailureMechanismSectionLengthInvalid)
            });
        }

        [Test]
        public void FindGreatestCommonDenominatorSectionsBoi3A1_WithData_ReturnsExpectedSections()
        {
            // Setup
            const double assessmentSectionLength = 30.0;
            var list1 = new FailureMechanismSectionList(new[]
            {
                new FailureMechanismSection(0.0, 10.0),
                new FailureMechanismSection(10.0, 20.001),
                new FailureMechanismSection(20.001, assessmentSectionLength)
            });
            var list2 = new FailureMechanismSectionList(new[]
            {
                new FailureMechanismSection(0.0, 5.0),
                new FailureMechanismSection(5.0, 20.0),
                new FailureMechanismSection(20.0, 25.0),
                new FailureMechanismSection(25.0, assessmentSectionLength + 0.0001)
            });
            var list3 = new FailureMechanismSectionList(new[]
            {
                new FailureMechanismSection(0.0, 15.0),
                new FailureMechanismSection(15.0, 25.0001),
                new FailureMechanismSection(25.0001, 28.0),
                new FailureMechanismSection(28.0, assessmentSectionLength)
            });

            var assembler = new CommonFailureMechanismSectionAssembler();

            // Call
            FailureMechanismSectionList commonSections = assembler.FindGreatestCommonDenominatorSectionsBoi3A1(
                new[]
                {
                    list1,
                    list2,
                    list3
                }, assessmentSectionLength);

            // Assert
            AssertFailureMechanismSections(new[]
            {
                new FailureMechanismSection(0.0, 5.0),
                new FailureMechanismSection(5.0, 10.0),
                new FailureMechanismSection(10.0, 15.0),
                new FailureMechanismSection(15.0, 20.0),
                new FailureMechanismSection(20.0, 20.001),
                new FailureMechanismSection(20.001, 25.0),
                new FailureMechanismSection(25.0, 28.0),
                new FailureMechanismSection(28.0, assessmentSectionLength)
            }, commonSections.Sections);
        }

        [Test]
        public void TranslateFailureMechanismResultsToCommonSectionsBoi3B1_FailureMechanismSectionsNull_ThrowsArgumentNullException()
        {
            // Setup
            var assembler = new CommonFailureMechanismSectionAssembler();

            // Call
            void Call() => assembler.TranslateFailureMechanismResultsToCommonSectionsBoi3B1(
                null, new FailureMechanismSectionList(new[]
                {
                    new FailureMechanismSection(0.0, 10.0)
                }));

            // Assert
            var exception = Assert.Throws<ArgumentNullException>(Call);
            Assert.AreEqual("failureMechanismSections", exception.ParamName);
        }

        [Test]
        public void TranslateFailureMechanismResultsToCommonSectionsBoi3B1_CommonSectionsNull_ThrowsArgumentNullException()
        {
            // Setup
            var assembler = new CommonFailureMechanismSectionAssembler();

            // Call
            void Call() => assembler.TranslateFailureMechanismResultsToCommonSectionsBoi3B1(
                new FailureMechanismSectionList(new[]
                {
                    new FailureMechanismSectionWithCategory(0.0, 10.0, EInterpretationCategory.Zero)
                }), null);

            // Assert
            var exception = Assert.Throws<ArgumentNullException>(Call);
            Assert.AreEqual("commonSections", exception.ParamName);
        }

        [Test]
        [TestCase(20.0, 20.0 + 1e-8)]
        [TestCase(20.0 + 1e-8, 20.0)]
        public void TranslateFailureMechanismResultsToCommonSectionsBoi3B1_CommonSectionsLengthNotEqualToFailureMechanismSectionsLength_ThrowsAssemblyException(
            double failureMechanismSectionsLength, double commonSectionsLength)
        {
            // Setup
            var assembler = new CommonFailureMechanismSectionAssembler();

            // Call
            void Call() => assembler.TranslateFailureMechanismResultsToCommonSectionsBoi3B1(
                new FailureMechanismSectionList(new[]
                {
                    new FailureMechanismSectionWithCategory(0.0, 10.0, EInterpretationCategory.Zero),
                    new FailureMechanismSectionWithCategory(10.0, failureMechanismSectionsLength, EInterpretationCategory.Zero)
                }),
                new FailureMechanismSectionList(new[]
                {
                    new FailureMechanismSection(0.0, 10.0),
                    new FailureMechanismSection(10.0, commonSectionsLength)
                }));

            // Assert
            TestHelper.AssertThrowsAssemblyExceptionWithAssemblyErrorMessages(Call, new[]
            {
                new AssemblyErrorMessage("commonSections", EAssemblyErrors.CommonFailureMechanismSectionsInvalid)
            });
        }

        [Test]
        public void TranslateFailureMechanismResultsToCommonSectionsBoi3B1_FailureMechanismSectionsWithoutCategory_ThrowsAssemblyException()
        {
            // Setup
            var assembler = new CommonFailureMechanismSectionAssembler();

            // Call
            void Call() => assembler.TranslateFailureMechanismResultsToCommonSectionsBoi3B1(
                new FailureMechanismSectionList(new[]
                {
                    new FailureMechanismSection(0.0, 10.0),
                    new FailureMechanismSection(10.0, 20.0)
                }),
                new FailureMechanismSectionList(new[]
                {
                    new FailureMechanismSection(0.0, 10.0),
                    new FailureMechanismSection(10.0, 20.0)
                }));

            // Assert
            TestHelper.AssertThrowsAssemblyExceptionWithAssemblyErrorMessages(Call, new[]
            {
                new AssemblyErrorMessage("failureMechanismSections", EAssemblyErrors.SectionsWithoutCategory)
            });
        }

        [Test]
        public void TranslateFailureMechanismResultsToCommonSectionsBoi3B1_WithData_ReturnsExpectedSectionsWithResults()
        {
            // Setup
            var failureMechanismSections = new FailureMechanismSectionList(new[]
            {
                new FailureMechanismSectionWithCategory(0.0, 5.0, EInterpretationCategory.III),
                new FailureMechanismSectionWithCategory(5.0, 10.0, EInterpretationCategory.I)
            });
            var commonSections = new FailureMechanismSectionList(new[]
            {
                new FailureMechanismSection(0.0, 2.5),
                new FailureMechanismSection(2.5, 5.0),
                new FailureMechanismSection(5.0, 7.5),
                new FailureMechanismSection(7.5, 10.0)
            });

            var assembler = new CommonFailureMechanismSectionAssembler();

            // Call
            FailureMechanismSectionList commonSectionsWithResults = assembler.TranslateFailureMechanismResultsToCommonSectionsBoi3B1(
                failureMechanismSections, commonSections);

            // Assert
            AssertFailureMechanismSections(new[]
            {
                new FailureMechanismSectionWithCategory(0.0, 2.5, EInterpretationCategory.III),
                new FailureMechanismSectionWithCategory(2.5, 5.0, EInterpretationCategory.III),
                new FailureMechanismSectionWithCategory(5.0, 7.5, EInterpretationCategory.I),
                new FailureMechanismSectionWithCategory(7.5, 10.0, EInterpretationCategory.I)
            }, commonSectionsWithResults.Sections.Cast<FailureMechanismSectionWithCategory>());
        }

        [Test]
        public void DetermineCombinedResultPerCommonSectionBoi3C1_FailureMechanismResultsForCommonSectionsNull_ThrowsArgumentNullException()
        {
            // Setup
            var assembler = new CommonFailureMechanismSectionAssembler();

            // Call
            void Call() => assembler.DetermineCombinedResultPerCommonSectionBoi3C1(null, false);

            // Assert
            var exception = Assert.Throws<ArgumentNullException>(Call);
            Assert.AreEqual("failureMechanismResultsForCommonSections", exception.ParamName);
        }

        [Test]
        public void DetermineCombinedResultPerCommonSectionBoi3C1_FailureMechanismResultsForCommonSectionsEmpty_ThrowsAssemblyException()
        {
            // Setup
            var assembler = new CommonFailureMechanismSectionAssembler();

            // Call
            void Call() => assembler.DetermineCombinedResultPerCommonSectionBoi3C1(Enumerable.Empty<FailureMechanismSectionList>(), false);

            // Assert
            TestHelper.AssertThrowsAssemblyExceptionWithAssemblyErrorMessages(Call, new[]
            {
                new AssemblyErrorMessage("failureMechanismResultsForCommonSections", EAssemblyErrors.CommonSectionsWithoutCategoryValues)
            });
        }

        [Test]
        public void DetermineCombinedResultPerCommonSectionBoi3C1_FailureMechanismResultsForCommonSectionsWithoutCategory_ThrowsAssemblyException()
        {
            // Setup
            var assembler = new CommonFailureMechanismSectionAssembler();

            // Call
            void Call() => assembler.DetermineCombinedResultPerCommonSectionBoi3C1(new[]
            {
                new FailureMechanismSectionList(new[]
                {
                    new FailureMechanismSectionWithCategory(0.0, 10.0, EInterpretationCategory.Zero),
                    new FailureMechanismSectionWithCategory(10.0, 20.0, EInterpretationCategory.Zero)
                }),
                new FailureMechanismSectionList(new[]
                {
                    new FailureMechanismSection(0.0, 10.0),
                    new FailureMechanismSection(10.0, 20.0)
                })
            }, false);

            // Assert
            TestHelper.AssertThrowsAssemblyExceptionWithAssemblyErrorMessages(Call, new[]
            {
                new AssemblyErrorMessage("failureMechanismResultsForCommonSections", EAssemblyErrors.CommonSectionsWithoutCategoryValues)
            });
        }

        [Test]
        public void DetermineCombinedResultPerCommonSectionBoi3C1_FailureMechanismResultsForCommonSectionsNotEqualLength_ThrowsAssemblyException()
        {
            // Setup
            var assembler = new CommonFailureMechanismSectionAssembler();

            // Call
            void Call() => assembler.DetermineCombinedResultPerCommonSectionBoi3C1(new[]
            {
                new FailureMechanismSectionList(new[]
                {
                    new FailureMechanismSectionWithCategory(0.0, 10.0, EInterpretationCategory.Zero),
                    new FailureMechanismSectionWithCategory(10.0, 20.0, EInterpretationCategory.Zero)
                }),
                new FailureMechanismSectionList(new[]
                {
                    new FailureMechanismSectionWithCategory(0.0, 5.0, EInterpretationCategory.Zero),
                    new FailureMechanismSectionWithCategory(5.0, 10.0, EInterpretationCategory.Zero),
                    new FailureMechanismSectionWithCategory(10.0, 20.0, EInterpretationCategory.Zero)
                })
            }, false);

            // Assert
            TestHelper.AssertThrowsAssemblyExceptionWithAssemblyErrorMessages(Call, new[]
            {
                new AssemblyErrorMessage("failureMechanismResultsForCommonSections", EAssemblyErrors.UnequalCommonFailureMechanismSectionLists)
            });
        }

        [Test]
        public void DetermineCombinedResultPerCommonSectionBoi3C1_FailureMechanismResultsForCommonSectionsNotEqual_ThrowsAssemblyException()
        {
            // Setup
            var assembler = new CommonFailureMechanismSectionAssembler();

            // Call
            void Call() => assembler.DetermineCombinedResultPerCommonSectionBoi3C1(new[]
            {
                new FailureMechanismSectionList(new[]
                {
                    new FailureMechanismSectionWithCategory(0.0, 10.0, EInterpretationCategory.Zero),
                    new FailureMechanismSectionWithCategory(10.0, 20.0, EInterpretationCategory.Zero)
                }),
                new FailureMechanismSectionList(new[]
                {
                    new FailureMechanismSectionWithCategory(0.0, 5.0, EInterpretationCategory.Zero),
                    new FailureMechanismSectionWithCategory(5.0, 20.0, EInterpretationCategory.Zero)
                })
            }, false);

            // Assert
            TestHelper.AssertThrowsAssemblyExceptionWithAssemblyErrorMessages(Call, new[]
            {
                new AssemblyErrorMessage("failureMechanismResultsForCommonSections", EAssemblyErrors.CommonFailureMechanismSectionsDoNotHaveEqualSections)
            });
        }

        [Test]
        public void DetermineCombinedResultPerCommonSectionBoi3C1_WithValidDataAndPartialAssemblyFalse_ReturnsExpectedResult()
        {
            // Setup
            var sectionsList1 = new FailureMechanismSectionList(new[]
            {
                new FailureMechanismSectionWithCategory(0.0, 0.5, EInterpretationCategory.NotRelevant),
                new FailureMechanismSectionWithCategory(0.5, 1.0, EInterpretationCategory.III),
                new FailureMechanismSectionWithCategory(1.0, 1.5, EInterpretationCategory.Zero),
                new FailureMechanismSectionWithCategory(1.5, 2.0, EInterpretationCategory.NoResult),
                new FailureMechanismSectionWithCategory(2.0, 2.5, EInterpretationCategory.NotDominant),
                new FailureMechanismSectionWithCategory(2.5, 3.0, EInterpretationCategory.III)
            });

            var sectionsList2 = new FailureMechanismSectionList(new[]
            {
                new FailureMechanismSectionWithCategory(0.0, 0.5, EInterpretationCategory.NotRelevant),
                new FailureMechanismSectionWithCategory(0.5, 1.0, EInterpretationCategory.Zero),
                new FailureMechanismSectionWithCategory(1.0, 1.5, EInterpretationCategory.IMin),
                new FailureMechanismSectionWithCategory(1.5, 2.0, EInterpretationCategory.IIIMin),
                new FailureMechanismSectionWithCategory(2.0, 2.5, EInterpretationCategory.NotRelevant),
                new FailureMechanismSectionWithCategory(2.5, 3.0, EInterpretationCategory.II)
            });

            var sectionsList3 = new FailureMechanismSectionList(new[]
            {
                new FailureMechanismSectionWithCategory(0.0, 0.5, EInterpretationCategory.NotRelevant),
                new FailureMechanismSectionWithCategory(0.5, 1.0, EInterpretationCategory.IIIMin),
                new FailureMechanismSectionWithCategory(1.0, 1.5, EInterpretationCategory.Zero),
                new FailureMechanismSectionWithCategory(1.5, 2.0, EInterpretationCategory.IIIMin),
                new FailureMechanismSectionWithCategory(2.0, 2.5, EInterpretationCategory.NotDominant),
                new FailureMechanismSectionWithCategory(2.5, 3.0, EInterpretationCategory.I)
            });

            var assembler = new CommonFailureMechanismSectionAssembler();

            // Call
            IEnumerable<FailureMechanismSectionWithCategory> commonSectionsWithResults = assembler.DetermineCombinedResultPerCommonSectionBoi3C1(
                new[]
                {
                    sectionsList1,
                    sectionsList2,
                    sectionsList3
                }, false);

            AssertFailureMechanismSections(new[]
            {
                new FailureMechanismSectionWithCategory(0.0, 0.5, EInterpretationCategory.NotRelevant),
                new FailureMechanismSectionWithCategory(0.5, 1.0, EInterpretationCategory.IIIMin),
                new FailureMechanismSectionWithCategory(1.0, 1.5, EInterpretationCategory.IMin),
                new FailureMechanismSectionWithCategory(1.5, 2.0, EInterpretationCategory.NoResult),
                new FailureMechanismSectionWithCategory(2.0, 2.5, EInterpretationCategory.NotDominant),
                new FailureMechanismSectionWithCategory(2.5, 3.0, EInterpretationCategory.I)
            }, commonSectionsWithResults);
        }

        [Test]
        public void DetermineCombinedResultPerCommonSectionBoi3C1_WithValidDataAndPartialAssemblyTrue_ReturnsExpectedResult()
        {
            // Setup
            var sectionsList1 = new FailureMechanismSectionList(new[]
            {
                new FailureMechanismSectionWithCategory(0.0, 0.5, EInterpretationCategory.NotRelevant),
                new FailureMechanismSectionWithCategory(0.5, 1.0, EInterpretationCategory.III),
                new FailureMechanismSectionWithCategory(1.0, 1.5, EInterpretationCategory.Zero),
                new FailureMechanismSectionWithCategory(1.5, 2.0, EInterpretationCategory.NoResult),
                new FailureMechanismSectionWithCategory(2.0, 2.5, EInterpretationCategory.NotDominant),
                new FailureMechanismSectionWithCategory(2.5, 3.0, EInterpretationCategory.III)
            });

            var sectionsList2 = new FailureMechanismSectionList(new[]
            {
                new FailureMechanismSectionWithCategory(0.0, 0.5, EInterpretationCategory.NotRelevant),
                new FailureMechanismSectionWithCategory(0.5, 1.0, EInterpretationCategory.Zero),
                new FailureMechanismSectionWithCategory(1.0, 1.5, EInterpretationCategory.IMin),
                new FailureMechanismSectionWithCategory(1.5, 2.0, EInterpretationCategory.IIIMin),
                new FailureMechanismSectionWithCategory(2.0, 2.5, EInterpretationCategory.NotRelevant),
                new FailureMechanismSectionWithCategory(2.5, 3.0, EInterpretationCategory.II)
            });

            var sectionsList3 = new FailureMechanismSectionList(new[]
            {
                new FailureMechanismSectionWithCategory(0.0, 0.5, EInterpretationCategory.NotRelevant),
                new FailureMechanismSectionWithCategory(0.5, 1.0, EInterpretationCategory.IIIMin),
                new FailureMechanismSectionWithCategory(1.0, 1.5, EInterpretationCategory.Zero),
                new FailureMechanismSectionWithCategory(1.5, 2.0, EInterpretationCategory.IIIMin),
                new FailureMechanismSectionWithCategory(2.0, 2.5, EInterpretationCategory.NotDominant),
                new FailureMechanismSectionWithCategory(2.5, 3.0, EInterpretationCategory.I)
            });

            var assembler = new CommonFailureMechanismSectionAssembler();

            // Call
            IEnumerable<FailureMechanismSectionWithCategory> commonSectionsWithResults = assembler.DetermineCombinedResultPerCommonSectionBoi3C1(
                new[]
                {
                    sectionsList1,
                    sectionsList2,
                    sectionsList3
                }, true);

            AssertFailureMechanismSections(new[]
            {
                new FailureMechanismSectionWithCategory(0.0, 0.5, EInterpretationCategory.NotRelevant),
                new FailureMechanismSectionWithCategory(0.5, 1.0, EInterpretationCategory.IIIMin),
                new FailureMechanismSectionWithCategory(1.0, 1.5, EInterpretationCategory.IMin),
                new FailureMechanismSectionWithCategory(1.5, 2.0, EInterpretationCategory.IIIMin),
                new FailureMechanismSectionWithCategory(2.0, 2.5, EInterpretationCategory.NotDominant),
                new FailureMechanismSectionWithCategory(2.5, 3.0, EInterpretationCategory.I)
            }, commonSectionsWithResults);
        }

        private static void AssertFailureMechanismSections(
            IEnumerable<FailureMechanismSection> expectedSections,
            IEnumerable<FailureMechanismSection> actualSections)
        {
            Assert.AreEqual(expectedSections.Count(), actualSections.Count());

            for (var i = 0; i < expectedSections.Count(); i++)
            {
                FailureMechanismSection expectedSection = expectedSections.ElementAt(i);
                FailureMechanismSection actualSection = actualSections.ElementAt(i);

                Assert.AreEqual(expectedSection.Start, actualSection.Start);
                Assert.AreEqual(expectedSection.End, actualSection.End);
            }
        }

        private static void AssertFailureMechanismSections(
            IEnumerable<FailureMechanismSectionWithCategory> expectedSections,
            IEnumerable<FailureMechanismSectionWithCategory> actualSections)
        {
            Assert.AreEqual(expectedSections.Count(), actualSections.Count());

            for (var i = 0; i < expectedSections.Count(); i++)
            {
                FailureMechanismSectionWithCategory expectedSection = expectedSections.ElementAt(i);
                FailureMechanismSectionWithCategory actualSection = actualSections.ElementAt(i);

                Assert.AreEqual(expectedSection.Start, actualSection.Start);
                Assert.AreEqual(expectedSection.End, actualSection.End);
                Assert.AreEqual(expectedSection.Category, actualSection.Category);
            }
        }
    }
}