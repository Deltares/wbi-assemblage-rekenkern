#region Copyright (C) Rijkswaterstaat 2022. All rights reserved.

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
using Assembly.Kernel.Implementations;
using Assembly.Kernel.Interfaces;
using Assembly.Kernel.Model.Categories;
using Assembly.Kernel.Model.FailureMechanismSections;
using NUnit.Framework;

namespace Assembly.Kernel.Tests.Implementations
{
    [TestFixture]
    public class CommonFailureMechanismSectionAssemblerTests
    {
        private readonly ICommonFailureMechanismSectionAssembler assembler =
            new CommonFailureMechanismSectionAssembler();

        [Test]
        public void FindGreatestCommonDenominatorSectionsBoi3A1ReturnsCorrectSections()
        {
            const double assessmentSectionLength = 30.0;
            var list1 = new FailureMechanismSectionList(new[]
            {
                new FailureMechanismSection(0.0, 10.0),
                new FailureMechanismSection(10.0, 20.0),
                new FailureMechanismSection(20.0, assessmentSectionLength)
            });
            var list2 = new FailureMechanismSectionList(new[]
            {
                new FailureMechanismSection(0.0, 5.0),
                new FailureMechanismSection(5.0, 25.0),
                new FailureMechanismSection(25.0, assessmentSectionLength)
            });
            var list3 = new FailureMechanismSectionList(new[]
            {
                new FailureMechanismSection(0.0, 15.0),
                new FailureMechanismSection(15.0, 28.0),
                new FailureMechanismSection(28.0, assessmentSectionLength)
            });

            var commonSections =
                assembler.FindGreatestCommonDenominatorSectionsBoi3A1(new[]
                    {
                        list1,
                        list2,
                        list3
                    },
                    assessmentSectionLength);

            var expectedSectionLimits = new[]
                {
                    0.0
                }
                .Concat(list1.Sections.Select(r => r.End).ToArray())
                .Concat(list2.Sections.Select(r => r.End).ToArray())
                .Concat(list3.Sections.Select(r => r.End).ToArray())
                .Distinct().OrderBy(v => v).ToArray();

            var calculatedCommonSections = commonSections.Sections.ToArray();
            Assert.AreEqual(expectedSectionLimits.Length - 1, calculatedCommonSections.Length);
            for (int i = 0; i < calculatedCommonSections.Length; i++)
            {
                AssertAreEqualSectionLimits(calculatedCommonSections[i], expectedSectionLimits[i], expectedSectionLimits[i + 1]);
            }
        }

        [Test]
        public void FindGreatestCommonDenominatorSectionsBoi3A1IgnoresSmallCombinedSections()
        {
            var assessmentSectionLength = 30.0;
            var list1 = new FailureMechanismSectionList(new[]
            {
                new FailureMechanismSection(0.0, 10.0),
                new FailureMechanismSection(10.0, 20.0),
                new FailureMechanismSection(20.0, assessmentSectionLength)
            });
            var list2 = new FailureMechanismSectionList(new[]
            {
                new FailureMechanismSection(0.0, 10.001),
                new FailureMechanismSection(10.001, 20.0),
                new FailureMechanismSection(20.0, assessmentSectionLength)
            });
            var list3 = new FailureMechanismSectionList(new[]
            {
                new FailureMechanismSection(0.0, 10.0),
                new FailureMechanismSection(10.0, 20.0+1E-6),
                new FailureMechanismSection(20.0+1E-6, assessmentSectionLength)
            });

            var commonSections =
                assembler.FindGreatestCommonDenominatorSectionsBoi3A1(new[]
                                                                      {
                                                                          list1,
                                                                          list2,
                                                                          list3
                                                                      },
                                                                      assessmentSectionLength);

            var expectedSectionLimits = new[]
            {
                0.0,
                10.0,
                10.001,
                20.0,
                assessmentSectionLength
            };

            var calculatedCommonSections = commonSections.Sections.ToArray();
            Assert.AreEqual(expectedSectionLimits.Length - 1, calculatedCommonSections.Length);
            for (int i = 0; i < calculatedCommonSections.Length; i++)
            {
                AssertAreEqualSectionLimits(calculatedCommonSections[i], expectedSectionLimits[i], expectedSectionLimits[i + 1]);
            }
        }

        [Test]
        public void FindGreatestCommonDenominatorSectionsBoi3A1IgnoresSmallSectionBoundaryDifferences()
        {
            var delta = 1e-6;
            var assessmentSectionLength = 30.0;
            var list1 = new FailureMechanismSectionList(new[]
            {
                new FailureMechanismSection(0.0, 10.0),
                new FailureMechanismSection(10.001, 20.0),
                new FailureMechanismSection(20.0, assessmentSectionLength + delta)
            });
            var list2 = new FailureMechanismSectionList(new[]
            {
                new FailureMechanismSection(0.0, 5.0),
                new FailureMechanismSection(5.0, 25.0),
                new FailureMechanismSection(25.0, assessmentSectionLength)
            });
            var list3 = new FailureMechanismSectionList(new[]
            {
                new FailureMechanismSection(0.0, 5.001),
                new FailureMechanismSection(5.001, 15.0),
                new FailureMechanismSection(15.0, 28.0),
                new FailureMechanismSection(28.0, assessmentSectionLength)
            });

            var commonSections =
                assembler.FindGreatestCommonDenominatorSectionsBoi3A1(new[]
                                                                      {
                                                                          list1,
                                                                          list2,
                                                                          list3
                                                                      },
                                                                      assessmentSectionLength);

            var expectedSectionLimits = new[]
            {
                0.0,
                5.0,
                5.001,
                10.0,
                15.0,
                20.0,
                25.0,
                28.0,
                assessmentSectionLength
            };

            var calculatedCommonSections = commonSections.Sections.ToArray();
            Assert.AreEqual(expectedSectionLimits.Length - 1, calculatedCommonSections.Length);
            for (int i = 0; i < calculatedCommonSections.Length; i++)
            {
                AssertAreEqualSectionLimits(calculatedCommonSections[i], expectedSectionLimits[i], expectedSectionLimits[i + 1]);
            }
        }

        [Test]
        public void FindGreatestCommonDenominatorSectionsBoi3A1ThrowsOnEmptySectionLists()
        {
            TestHelper.AssertExpectedErrorMessage(() =>
            {
                var commonSections =
                    assembler.FindGreatestCommonDenominatorSectionsBoi3A1(new FailureMechanismSectionList[]
                        { }, 10.0);
            }, EAssemblyErrors.EmptyResultsList);
        }

        [Test]
        public void FindGreatestCommonDenominatorSectionsBoi3A1ThrowsOnNullSectionLists()
        {
            TestHelper.AssertExpectedErrorMessage(() =>
            {
                var commonSections =
                    assembler.FindGreatestCommonDenominatorSectionsBoi3A1(null, 10.0);
            }, EAssemblyErrors.ValueMayNotBeNull);
        }

        [Test]
        public void FindGreatestCommonDenominatorSectionsBoi3A1ThrowsMultipleErrors()
        {
            TestHelper.AssertExpectedErrorMessage(() =>
            {
                var commonSections =
                    assembler.FindGreatestCommonDenominatorSectionsBoi3A1(null, -10.0);
            }, new[] {EAssemblyErrors.SectionLengthOutOfRange, EAssemblyErrors.ValueMayNotBeNull});
        }

        [Test]
        [TestCase(-2.3, EAssemblyErrors.SectionLengthOutOfRange)]
        [TestCase(0.0, EAssemblyErrors.SectionLengthOutOfRange)]
        [TestCase(double.NaN, EAssemblyErrors.ValueMayNotBeNull)]
        public void FindGreatestCommonDenominatorSectionsBoi3A1ThrowsOnInvalidAssessmentLength(double assessmentLength,
                                                                                               EAssemblyErrors expectedError)
        {
            var list1 = new FailureMechanismSectionList(new[]
            {
                new FailureMechanismSection(0.0, 10.0),
                new FailureMechanismSection(10.0, 20.0),
                new FailureMechanismSection(20.0, 30.0)
            });
            TestHelper.AssertExpectedErrorMessage(() =>
            {
                var commonSections =
                    assembler.FindGreatestCommonDenominatorSectionsBoi3A1(new[]
                        {
                            list1
                        },
                        assessmentLength);
            }, expectedError);
        }

        [Test]
        public void FindGreatestCommonDenominatorSectionsBoi3A1ThrowsOnInvalidSectionList()
        {
            var assessmentSectionLength = 30.0;
            var list1 = new FailureMechanismSectionList(new[]
            {
                new FailureMechanismSection(0.0, 10.0),
                new FailureMechanismSection(10.001, 20.0),
                new FailureMechanismSection(20.0, assessmentSectionLength)
            });
            var list2 = new FailureMechanismSectionList(new[]
            {
                new FailureMechanismSection(0.0, 5.0),
                new FailureMechanismSection(5.0, 25.0),
                new FailureMechanismSection(25.0, assessmentSectionLength - 1.0)
            });

            TestHelper.AssertExpectedErrorMessage(() =>
            {
                var commonSections =
                    assembler.FindGreatestCommonDenominatorSectionsBoi3A1(new[]
                        {
                            list1,
                            list2
                        },
                        assessmentSectionLength);
            }, EAssemblyErrors.FailureMechanismSectionLengthInvalid);
        }

        [Test]
        public void TranslateFailureMechanismResultsToCommonSectionsBoi3B1TranslatesCorrectly()
        {
            var resultSectionsList = new FailureMechanismSectionList(new[]
            {
                new FailureMechanismSectionWithCategory(0.0, 5.0, EInterpretationCategory.III),
                new FailureMechanismSectionWithCategory(5.0, 10.0, EInterpretationCategory.I)
            });
            var commonSectionsList = new FailureMechanismSectionList(new[]
            {
                new FailureMechanismSection(0.0, 2.5),
                new FailureMechanismSection(2.5, 5.0),
                new FailureMechanismSection(5.0, 7.5),
                new FailureMechanismSection(7.5, 10.0)
            });
            var commonSectionsWithResults =
                assembler.TranslateFailureMechanismResultsToCommonSectionsBoi3B1(resultSectionsList,
                    commonSectionsList);

            Assert.IsNotNull(commonSectionsWithResults.Sections);
            Assert.AreEqual(4, commonSectionsWithResults.Sections.Count());
            Assert.AreEqual(EInterpretationCategory.III,
                ((FailureMechanismSectionWithCategory)commonSectionsWithResults.Sections.ElementAt(0)).Category);
            Assert.AreEqual(EInterpretationCategory.III,
                ((FailureMechanismSectionWithCategory)commonSectionsWithResults.Sections.ElementAt(1)).Category);
            Assert.AreEqual(EInterpretationCategory.I,
                ((FailureMechanismSectionWithCategory)commonSectionsWithResults.Sections.ElementAt(2)).Category);
            Assert.AreEqual(EInterpretationCategory.I,
                ((FailureMechanismSectionWithCategory)commonSectionsWithResults.Sections.ElementAt(3)).Category);
        }

        [Test]
        public void TranslateFailureMechanismResultsToCommonSectionsBoi3B1ThrowsOnIncorrectListType()
        {
            var resultSectionsList = new FailureMechanismSectionList(new[]
            {
                new FailureMechanismSection(0.0, 5.0),
                new FailureMechanismSection(5.0, 10.0)
            });

            var commonSectionsList = new FailureMechanismSectionList(new[]
            {
                new FailureMechanismSection(0.0, 2.5),
                new FailureMechanismSection(2.5, 5.0),
                new FailureMechanismSection(5.0, 7.5),
                new FailureMechanismSection(7.5, 10.0)
            });

            TestHelper.AssertExpectedErrorMessage(() =>
            {
                var commonSectionsWithResults =
                    assembler.TranslateFailureMechanismResultsToCommonSectionsBoi3B1(resultSectionsList,
                        commonSectionsList);
            }, EAssemblyErrors.SectionsWithoutCategory);
        }

        [Test]
        public void TranslateFailureMechanismResultsToCommonSectionsBoi3B1ThrowsOnInvalidSectionLengthsEmpty()
        {
            var list = new FailureMechanismSectionList(new[]
            {
                new FailureMechanismSectionWithCategory(0.0, 2.85, EInterpretationCategory.III)
            });

            var longList = new FailureMechanismSectionList(new FailureMechanismSection[]
            {
                new FailureMechanismSectionWithCategory(0.0, 10.0, EInterpretationCategory.III)
            });

            TestHelper.AssertExpectedErrorMessage(() =>
            {
                var commonSectionsWithResults =
                    assembler.TranslateFailureMechanismResultsToCommonSectionsBoi3B1(longList, list);
            }, EAssemblyErrors.CommonFailureMechanismSectionsInvalid);
        }

        [Test]
        public void TranslateFailureMechanismResultsToCommonSectionsBoi3B1ThrowsOnNullLists()
        {
            var list = new FailureMechanismSectionList(new[]
            {
                new FailureMechanismSection(0.0, 2.5)
            });

            TestHelper.AssertExpectedErrorMessage(() =>
            {
                var commonSectionsWithResults =
                    assembler.TranslateFailureMechanismResultsToCommonSectionsBoi3B1(null, list);
            },EAssemblyErrors.ValueMayNotBeNull);
        }

        [Test]
        public void TranslateFailureMechanismResultsToCommonSectionsBoi3B1ThrowsOnNullLists2()
        {
            var list = new FailureMechanismSectionList(new[]
            {
                new FailureMechanismSection(0.0, 2.5)
            });

            TestHelper.AssertExpectedErrorMessage(() => 
            {
                var commonSectionsWithResults =
                    assembler.TranslateFailureMechanismResultsToCommonSectionsBoi3B1(list, null);
            }, EAssemblyErrors.ValueMayNotBeNull);
        }

        [Test]
        public void TranslateFailureMechanismResultsToCommonSectionsBoi3B1WithRoundingTranslatesCorrectly()
        {
            var resultSectionsList = new FailureMechanismSectionList(new[]
            {
                new FailureMechanismSectionWithCategory(0.0, 5.0, EInterpretationCategory.III),
                new FailureMechanismSectionWithCategory(5.0, 10.0, EInterpretationCategory.I)
            });
            var commonSectionsList = new FailureMechanismSectionList(new[]
            {
                new FailureMechanismSection(0.0, 2.5),
                new FailureMechanismSection(2.5, 5.0),
                new FailureMechanismSection(5.0, 7.5),
                new FailureMechanismSection(7.5, 10.000000001)
            });
            var commonSectionsWithResults =
                assembler.TranslateFailureMechanismResultsToCommonSectionsBoi3B1(resultSectionsList,
                                                                                 commonSectionsList);

            Assert.IsNotNull(commonSectionsWithResults.Sections);
            Assert.AreEqual(4, commonSectionsWithResults.Sections.Count());
            Assert.AreEqual(EInterpretationCategory.III,
                            ((FailureMechanismSectionWithCategory)commonSectionsWithResults.Sections.ElementAt(0)).Category);
            Assert.AreEqual(EInterpretationCategory.III,
                            ((FailureMechanismSectionWithCategory)commonSectionsWithResults.Sections.ElementAt(1)).Category);
            Assert.AreEqual(EInterpretationCategory.I,
                            ((FailureMechanismSectionWithCategory)commonSectionsWithResults.Sections.ElementAt(2)).Category);
            Assert.AreEqual(EInterpretationCategory.I,
                            ((FailureMechanismSectionWithCategory)commonSectionsWithResults.Sections.ElementAt(3)).Category);
        }

        [Test]
        public void DetermineCombinedResultPerCommonSectionReturnsCorrectResults()
        {
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

            var commonSectionsWithResults =
                assembler.DetermineCombinedResultPerCommonSectionBoi3C1(new[]
                {
                    sectionsList1,
                    sectionsList2,
                    sectionsList3
                }, false);

            var expectedResults = new List<FailureMechanismSectionWithCategory>
            {
                new FailureMechanismSectionWithCategory(0.0, 0.5, EInterpretationCategory.NotRelevant),
                new FailureMechanismSectionWithCategory(0.5, 1.0, EInterpretationCategory.IIIMin),
                new FailureMechanismSectionWithCategory(1.0, 1.5, EInterpretationCategory.IMin),
                new FailureMechanismSectionWithCategory(1.5, 2.0, EInterpretationCategory.NoResult),
                new FailureMechanismSectionWithCategory(2.0, 2.5, EInterpretationCategory.NotDominant),
                new FailureMechanismSectionWithCategory(2.5, 3.0, EInterpretationCategory.I)
            };

            AssertCombinedResultsList(expectedResults, commonSectionsWithResults);
        }

        [Test]
        public void DetermineCombinedResultPerCommonSectionPartialReturnsCorrectResults()
        {
            var sectionsList1 = new FailureMechanismSectionList(new[]
            {
                new FailureMechanismSectionWithCategory(0.0, 1.0, EInterpretationCategory.III),
                new FailureMechanismSectionWithCategory(1.0, 1.5, EInterpretationCategory.Zero),
                new FailureMechanismSectionWithCategory(1.5, 2.0, EInterpretationCategory.NoResult),
                new FailureMechanismSectionWithCategory(2.0, 3.0, EInterpretationCategory.III)
            });

            var sectionsList2 = new FailureMechanismSectionList(new[]
            {
                new FailureMechanismSectionWithCategory(0.0, 1.0, EInterpretationCategory.Zero),
                new FailureMechanismSectionWithCategory(1.0, 1.5, EInterpretationCategory.IMin),
                new FailureMechanismSectionWithCategory(1.5, 2.0, EInterpretationCategory.IIIMin),
                new FailureMechanismSectionWithCategory(2.0, 3.0, EInterpretationCategory.II)
            });

            var sectionsList3 = new FailureMechanismSectionList(new[]
            {
                new FailureMechanismSectionWithCategory(0.0, 1.0, EInterpretationCategory.IIIMin),
                new FailureMechanismSectionWithCategory(1.0, 1.5, EInterpretationCategory.Zero),
                new FailureMechanismSectionWithCategory(1.5, 2.0, EInterpretationCategory.IIIMin),
                new FailureMechanismSectionWithCategory(2.0, 3.0, EInterpretationCategory.I)
            });

            var commonSectionsWithResults =
                assembler.DetermineCombinedResultPerCommonSectionBoi3C1(new[]
                {
                    sectionsList1,
                    sectionsList2,
                    sectionsList3
                }, true);

            var expectedResults = new List<FailureMechanismSectionWithCategory>
            {
                new FailureMechanismSectionWithCategory(0.0, 1.0, EInterpretationCategory.IIIMin),
                new FailureMechanismSectionWithCategory(1.0, 1.5, EInterpretationCategory.IMin),
                new FailureMechanismSectionWithCategory(1.5, 2.0, EInterpretationCategory.IIIMin),
                new FailureMechanismSectionWithCategory(2.0, 3.0, EInterpretationCategory.I)
            };

            AssertCombinedResultsList(expectedResults, commonSectionsWithResults);
        }

        [Test]
        public void DetermineCombinedResultPerCommonSectionThrowsOnEmptyList()
        {
            TestHelper.AssertExpectedErrorMessage(() =>
            {
                var commonSectionsWithResults =
                    assembler.DetermineCombinedResultPerCommonSectionBoi3C1(new FailureMechanismSectionList[] {},
                                                                            false);
            }, EAssemblyErrors.CommonSectionsWithoutCategoryValues);
        }

        [Test]
        public void DetermineCombinedResultPerCommonSectionThrowsOnInvalidSections1()
        {
            var sectionsList1 = new FailureMechanismSectionList(new[]
            {
                new FailureMechanismSectionWithCategory(0.0, 1.0, EInterpretationCategory.III),
                new FailureMechanismSectionWithCategory(1.0, 2.0, EInterpretationCategory.III)
            });

            var sectionsList2 = new FailureMechanismSectionList(new[]
            {
                new FailureMechanismSectionWithCategory(0.0, 1.0, EInterpretationCategory.III)
            });

            TestHelper.AssertExpectedErrorMessage(() =>
            {
                var commonSectionsWithResults =
                    assembler.DetermineCombinedResultPerCommonSectionBoi3C1(new[]
                    {
                        sectionsList1,
                        sectionsList2
                    }, false);
            },EAssemblyErrors.UnequalCommonFailureMechanismSectionLists);
        }

        [Test]
        public void DetermineCombinedResultPerCommonSectionThrowsOnInvalidSections2()
        {
            var sectionsList1 = new FailureMechanismSectionList(new[]
            {
                new FailureMechanismSectionWithCategory(0.0, 1.0, EInterpretationCategory.III),
                new FailureMechanismSectionWithCategory(1.0, 2.0, EInterpretationCategory.III),
                new FailureMechanismSectionWithCategory(2.0, 3.0, EInterpretationCategory.III)
            });

            var sectionsList2 = new FailureMechanismSectionList(new[]
            {
                new FailureMechanismSectionWithCategory(0.0, 1.0, EInterpretationCategory.III),
                new FailureMechanismSectionWithCategory(1.0, 1.5, EInterpretationCategory.III),
                new FailureMechanismSectionWithCategory(1.5, 2.5, EInterpretationCategory.III),
                new FailureMechanismSectionWithCategory(2.5, 3.0, EInterpretationCategory.III)
            });

            TestHelper.AssertExpectedErrorMessage(() =>
            {
                var commonSectionsWithResults =
                    assembler.DetermineCombinedResultPerCommonSectionBoi3C1(new[]
                    {
                        sectionsList1,
                        sectionsList2
                    }, false);
            }, EAssemblyErrors.UnequalCommonFailureMechanismSectionLists);
        }

        [Test]
        public void DetermineCombinedResultPerCommonSectionThrowsOnInvalidSections3()
        {
            var sectionsList1 = new FailureMechanismSectionList(new[]
            {
                new FailureMechanismSectionWithCategory(0.0, 1.0, EInterpretationCategory.III),
                new FailureMechanismSectionWithCategory(1.0, 2.0, EInterpretationCategory.III),
                new FailureMechanismSectionWithCategory(2.0, 3.0, EInterpretationCategory.III)
            });

            var sectionsList2 = new FailureMechanismSectionList(new[]
            {
                new FailureMechanismSectionWithCategory(0.0, 1.0, EInterpretationCategory.III),
                new FailureMechanismSectionWithCategory(1.0, 2.5, EInterpretationCategory.III),
                new FailureMechanismSectionWithCategory(2.5, 3.0, EInterpretationCategory.III)
            });

            TestHelper.AssertExpectedErrorMessage(() =>
            {
                var commonSectionsWithResults =
                    assembler.DetermineCombinedResultPerCommonSectionBoi3C1(new[]
                    {
                        sectionsList1,
                        sectionsList2
                    }, false);
            }, EAssemblyErrors.CommonFailureMechanismSectionsDoNotHaveEqualSections);
        }

        [Test]
        public void DetermineCombinedResultPerCommonSectionThrowsOnNullValue()
        {
            TestHelper.AssertExpectedErrorMessage(() =>
            {
                var commonSectionsWithResults = assembler.DetermineCombinedResultPerCommonSectionBoi3C1(null, false);
            }, EAssemblyErrors.ValueMayNotBeNull);
        }

        [Test]
        public void NoResultFailureMechanismTest()
        {
            var failureMechanismSectionLists = new List<FailureMechanismSectionList>
            {
                new FailureMechanismSectionList(new[]
                {
                    new FailureMechanismSectionWithCategory(0, 10, EInterpretationCategory.I),
                    new FailureMechanismSectionWithCategory(10, 20, EInterpretationCategory.I),
                    new FailureMechanismSectionWithCategory(20, 30, EInterpretationCategory.IIMin),
                    new FailureMechanismSectionWithCategory(30, 40, EInterpretationCategory.I),
                    new FailureMechanismSectionWithCategory(40, 50, EInterpretationCategory.NoResult)
                }),
                new FailureMechanismSectionList(new[]
                {
                    new FailureMechanismSectionWithCategory(0, 5, EInterpretationCategory.II),
                    new FailureMechanismSectionWithCategory(5, 10, EInterpretationCategory.III),
                    new FailureMechanismSectionWithCategory(10, 40, EInterpretationCategory.I),
                    new FailureMechanismSectionWithCategory(40, 45, EInterpretationCategory.Zero),
                    new FailureMechanismSectionWithCategory(45, 50, EInterpretationCategory.I)
                })
            };

            var expectedFailureMechanismResults = new List<FailureMechanismSectionList>
            {
                new FailureMechanismSectionList(new[]
                {
                    new FailureMechanismSectionWithCategory(0.0, 5.0, EInterpretationCategory.I),
                    new FailureMechanismSectionWithCategory(5.0, 10.0, EInterpretationCategory.I),
                    new FailureMechanismSectionWithCategory(10.0, 20.0, EInterpretationCategory.I),
                    new FailureMechanismSectionWithCategory(20.0, 30.0, EInterpretationCategory.IIMin),
                    new FailureMechanismSectionWithCategory(30.0, 40.0, EInterpretationCategory.I),
                    new FailureMechanismSectionWithCategory(40.0, 45.0, EInterpretationCategory.NoResult),
                    new FailureMechanismSectionWithCategory(45.0, 50.0, EInterpretationCategory.NoResult)
                }),
                new FailureMechanismSectionList(new[]
                {
                    new FailureMechanismSectionWithCategory(0.0, 5.0, EInterpretationCategory.II),
                    new FailureMechanismSectionWithCategory(5.0, 10.0, EInterpretationCategory.III),
                    new FailureMechanismSectionWithCategory(10.0, 20.0, EInterpretationCategory.I),
                    new FailureMechanismSectionWithCategory(20.0, 30.0, EInterpretationCategory.I),
                    new FailureMechanismSectionWithCategory(30.0, 40.0, EInterpretationCategory.I),
                    new FailureMechanismSectionWithCategory(40.0, 45.0, EInterpretationCategory.Zero),
                    new FailureMechanismSectionWithCategory(45.0, 50.0, EInterpretationCategory.I)
                })
            };

            var expectedCombinedResult = new List<FailureMechanismSectionWithCategory>
            {
                new FailureMechanismSectionWithCategory(0.0, 5.0, EInterpretationCategory.I),
                new FailureMechanismSectionWithCategory(5.0, 10.0, EInterpretationCategory.I),
                new FailureMechanismSectionWithCategory(10.0, 20.0, EInterpretationCategory.I),
                new FailureMechanismSectionWithCategory(20.0, 30.0, EInterpretationCategory.IIMin),
                new FailureMechanismSectionWithCategory(30.0, 40.0, EInterpretationCategory.I),
                new FailureMechanismSectionWithCategory(40.0, 45.0, EInterpretationCategory.NoResult),
                new FailureMechanismSectionWithCategory(45.0, 50.0, EInterpretationCategory.NoResult)
            };

            TestCombinedFailureMechanismSectionAssembler(
                failureMechanismSectionLists,
                expectedFailureMechanismResults,
                expectedCombinedResult,
                false);
        }

        [Test]
        public void NotAllSectionsProvidedExceptionTest()
        {
            var failureMechanismSectionLists = new List<FailureMechanismSectionList>
            {
                new FailureMechanismSectionList(new[]
                {
                    new FailureMechanismSectionWithCategory(0, 10, EInterpretationCategory.I),
                    new FailureMechanismSectionWithCategory(10, 20, EInterpretationCategory.I),
                    new FailureMechanismSectionWithCategory(30, 40, EInterpretationCategory.I),
                    new FailureMechanismSectionWithCategory(20, 30, EInterpretationCategory.II),
                    new FailureMechanismSectionWithCategory(40, 45, EInterpretationCategory.II)
                    // section starting at 45 to 50 is missing.
                })
            };

            TestHelper.AssertExpectedErrorMessage(() =>
            {
                assembler.AssembleCommonFailureMechanismSections(failureMechanismSectionLists, 50.0, false);
            }, EAssemblyErrors.FailureMechanismSectionLengthInvalid);
        }

        [Test]
        public void PartialFailureMechanismTest()
        {
            var failureMechanismSectionLists = new List<FailureMechanismSectionList>
            {
                new FailureMechanismSectionList(new[]
                {
                    new FailureMechanismSectionWithCategory(0, 10, EInterpretationCategory.I),
                    new FailureMechanismSectionWithCategory(10, 20, EInterpretationCategory.I),
                    new FailureMechanismSectionWithCategory(20, 30, EInterpretationCategory.IIIMin),
                    new FailureMechanismSectionWithCategory(30, 40, EInterpretationCategory.I),
                    new FailureMechanismSectionWithCategory(40, 50, EInterpretationCategory.II)
                }),
                new FailureMechanismSectionList(new[]
                {
                    new FailureMechanismSectionWithCategory(0, 5, EInterpretationCategory.II),
                    new FailureMechanismSectionWithCategory(5, 10, EInterpretationCategory.III),
                    new FailureMechanismSectionWithCategory(10, 40, EInterpretationCategory.I),
                    new FailureMechanismSectionWithCategory(40, 45, EInterpretationCategory.Zero),
                    new FailureMechanismSectionWithCategory(45, 50, EInterpretationCategory.I)
                })
            };

            var expectedFailureMechanismResults = new List<FailureMechanismSectionList>
            {
                new FailureMechanismSectionList(new[]
                {
                    new FailureMechanismSectionWithCategory(0.0, 5.0, EInterpretationCategory.I),
                    new FailureMechanismSectionWithCategory(5.0, 10.0, EInterpretationCategory.I),
                    new FailureMechanismSectionWithCategory(10.0, 20.0, EInterpretationCategory.I),
                    new FailureMechanismSectionWithCategory(20.0, 30.0, EInterpretationCategory.IIIMin),
                    new FailureMechanismSectionWithCategory(30.0, 40.0, EInterpretationCategory.I),
                    new FailureMechanismSectionWithCategory(40.0, 45.0, EInterpretationCategory.II),
                    new FailureMechanismSectionWithCategory(45.0, 50.0, EInterpretationCategory.II)
                }),
                new FailureMechanismSectionList(new[]
                {
                    new FailureMechanismSectionWithCategory(0.0, 5.0, EInterpretationCategory.II),
                    new FailureMechanismSectionWithCategory(5.0, 10.0, EInterpretationCategory.III),
                    new FailureMechanismSectionWithCategory(10.0, 20.0, EInterpretationCategory.I),
                    new FailureMechanismSectionWithCategory(20.0, 30.0, EInterpretationCategory.I),
                    new FailureMechanismSectionWithCategory(30.0, 40.0, EInterpretationCategory.I),
                    new FailureMechanismSectionWithCategory(40.0, 45.0, EInterpretationCategory.Zero),
                    new FailureMechanismSectionWithCategory(45.0, 50.0, EInterpretationCategory.I)
                })
            };

            var expectedCombinedResult = new List<FailureMechanismSectionWithCategory>
            {
                new FailureMechanismSectionWithCategory(0.0, 5.0, EInterpretationCategory.I),
                new FailureMechanismSectionWithCategory(5.0, 10.0, EInterpretationCategory.I),
                new FailureMechanismSectionWithCategory(10.0, 20.0, EInterpretationCategory.I),
                new FailureMechanismSectionWithCategory(20.0, 30.0, EInterpretationCategory.IIIMin),
                new FailureMechanismSectionWithCategory(30.0, 40.0, EInterpretationCategory.I),
                new FailureMechanismSectionWithCategory(40.0, 45.0, EInterpretationCategory.Zero),
                new FailureMechanismSectionWithCategory(45.0, 50.0, EInterpretationCategory.I)
            };

            TestCombinedFailureMechanismSectionAssembler(
                failureMechanismSectionLists,
                expectedFailureMechanismResults,
                expectedCombinedResult,
                true);
        }

        [Test]
        public void AssembleCommonFailureMechanismSectionsTest()
        {
            var failureMechanismSectionLists = new List<FailureMechanismSectionList>
            {
                new FailureMechanismSectionList(new[]
                {
                    new FailureMechanismSectionWithCategory(0, 10, EInterpretationCategory.I),
                    new FailureMechanismSectionWithCategory(10, 20, EInterpretationCategory.I),
                    new FailureMechanismSectionWithCategory(20, 30, EInterpretationCategory.Zero),
                    new FailureMechanismSectionWithCategory(30, 40, EInterpretationCategory.I),
                    new FailureMechanismSectionWithCategory(40, 50, EInterpretationCategory.II)
                }),
                new FailureMechanismSectionList(new[]
                {
                    new FailureMechanismSectionWithCategory(0, 5, EInterpretationCategory.II),
                    new FailureMechanismSectionWithCategory(5, 10, EInterpretationCategory.III),
                    new FailureMechanismSectionWithCategory(10, 40, EInterpretationCategory.I),
                    new FailureMechanismSectionWithCategory(40, 45, EInterpretationCategory.Zero),
                    new FailureMechanismSectionWithCategory(45, 50, EInterpretationCategory.II)
                })
            };

            var expectedFailureMechanismResults = new List<FailureMechanismSectionList>
            {
                new FailureMechanismSectionList(new[]
                {
                    new FailureMechanismSectionWithCategory(0.0, 5.0, EInterpretationCategory.I),
                    new FailureMechanismSectionWithCategory(5.0, 10.0, EInterpretationCategory.I),
                    new FailureMechanismSectionWithCategory(10.0, 20.0, EInterpretationCategory.I),
                    new FailureMechanismSectionWithCategory(20.0, 30.0, EInterpretationCategory.Zero),
                    new FailureMechanismSectionWithCategory(30.0, 40.0, EInterpretationCategory.I),
                    new FailureMechanismSectionWithCategory(40.0, 45.0, EInterpretationCategory.II),
                    new FailureMechanismSectionWithCategory(45.0, 50.0, EInterpretationCategory.II)
                }),
                new FailureMechanismSectionList(new[]
                {
                    new FailureMechanismSectionWithCategory(0.0, 5.0, EInterpretationCategory.II),
                    new FailureMechanismSectionWithCategory(5.0, 10.0, EInterpretationCategory.III),
                    new FailureMechanismSectionWithCategory(10.0, 20.0, EInterpretationCategory.I),
                    new FailureMechanismSectionWithCategory(20.0, 30.0, EInterpretationCategory.I),
                    new FailureMechanismSectionWithCategory(30.0, 40.0, EInterpretationCategory.I),
                    new FailureMechanismSectionWithCategory(40.0, 45.0, EInterpretationCategory.Zero),
                    new FailureMechanismSectionWithCategory(45.0, 50.0, EInterpretationCategory.II)
                })
            };

            var expectedCombinedResult = new List<FailureMechanismSectionWithCategory>
            {
                new FailureMechanismSectionWithCategory(0.0, 5.0, EInterpretationCategory.I),
                new FailureMechanismSectionWithCategory(5.0, 10.0, EInterpretationCategory.I),
                new FailureMechanismSectionWithCategory(10.0, 20.0, EInterpretationCategory.I),
                new FailureMechanismSectionWithCategory(20.0, 30.0, EInterpretationCategory.Zero),
                new FailureMechanismSectionWithCategory(30.0, 40.0, EInterpretationCategory.I),
                new FailureMechanismSectionWithCategory(40.0, 45.0, EInterpretationCategory.Zero),
                new FailureMechanismSectionWithCategory(45.0, 50.0, EInterpretationCategory.II)
            };

            TestCombinedFailureMechanismSectionAssembler(
                failureMechanismSectionLists,
                expectedFailureMechanismResults,
                expectedCombinedResult,
                false);
        }

        [Test]
        public void FailureMechanismSectionsListsShouldNotBeNull()
        {
            TestHelper.AssertExpectedErrorMessage(() =>
            {
                var result = assembler.AssembleCommonFailureMechanismSections(null, 50.0, false);
            }, EAssemblyErrors.ValueMayNotBeNull);
        }

        private void TestCombinedFailureMechanismSectionAssembler(
            IEnumerable<FailureMechanismSectionList> failureMechanismSections,
            IList<FailureMechanismSectionList> expectedFailureMechanismResults,
            IList<FailureMechanismSectionWithCategory> expectedCombinedResult,
            bool partial)
        {
            var result = assembler.AssembleCommonFailureMechanismSections(failureMechanismSections, 50.0, partial);
            Assert.NotNull(result);

            AssertFailureMechanismSectionLists(expectedFailureMechanismResults, result.ResultPerFailureMechanism);

            AssertCombinedResultsList(expectedCombinedResult, result.CombinedSectionResult);
        }

        private static void AssertAreEqualSectionLimits(FailureMechanismSection calculatedCommonSection, double expectedSectionStart, double expectedSectionEnd)
        {
            Assert.AreEqual(expectedSectionStart, calculatedCommonSection.Start);
            Assert.AreEqual(expectedSectionEnd, calculatedCommonSection.End);
        }

        private static void AssertCombinedResultsList(IList<FailureMechanismSectionWithCategory> expectedCombinedResult,
                                                      IEnumerable<FailureMechanismSectionWithCategory> result)
        {
            List<FailureMechanismSectionWithCategory> combinedResult = result.ToList();
            Assert.AreEqual(expectedCombinedResult.Count, combinedResult.Count);
            for (var i = 0; i < expectedCombinedResult.Count; i++)
            {
                Assert.AreEqual(expectedCombinedResult[i].Start, combinedResult[i].Start);
                Assert.AreEqual(expectedCombinedResult[i].End, combinedResult[i].End);
                Assert.AreEqual(expectedCombinedResult[i].Category, combinedResult[i].Category);
            }
        }

        private static void AssertFailureMechanismSectionLists(
            IList<FailureMechanismSectionList> expectedFailureMechanismResults,
            IEnumerable<FailureMechanismSectionList> result)
        {
            List<FailureMechanismSectionList> resultPerFailureMechanism = result.ToList();
            Assert.AreEqual(expectedFailureMechanismResults.Count, resultPerFailureMechanism.Count);

            for (var i = 0; i < expectedFailureMechanismResults.Count; i++)
            {
                var failureMechanismResult = resultPerFailureMechanism[i];

                var sectionResults = failureMechanismResult.Sections.ToArray();
                var expectedSectionResults = expectedFailureMechanismResults[i].Sections.ToArray();

                Assert.AreEqual(expectedSectionResults.Length, sectionResults.Length);
                for (var k = 0; k < expectedSectionResults.Length; k++)
                {
                    var expectedResult = expectedSectionResults[k];
                    var sectionResult = sectionResults[k];
                    Assert.NotNull(expectedResult);
                    Assert.NotNull(sectionResult);

                    Assert.AreEqual(expectedResult.Start, sectionResult.Start);
                    Assert.AreEqual(expectedResult.End, sectionResult.End);

                    Assert.AreEqual(
                            ((FailureMechanismSectionWithCategory) expectedResult).Category,
                            ((FailureMechanismSectionWithCategory) sectionResult).Category);
                }
            }
        }
    }
}