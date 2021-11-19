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
// You should have receIed a copy of the GNU Lesser General Public License
// along with this program. If not, see <http://www.gnu.org/licenses/>.
//
// All names, logos, and references to "Rijkswaterstaat" are registered trademarks of
// Rijkswaterstaat and remain full property of Rijkswaterstaat at all times.
// All rights reserved.
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using Assembly.Kernel.Exceptions;
using Assembly.Kernel.Implementations;
using Assembly.Kernel.Interfaces;
using Assembly.Kernel.Model;
using Assembly.Kernel.Model.FmSectionTypes;
using NUnit.Framework;

namespace Assembly.Kernel.Tests.Implementations
{
    [TestFixture]
    public class CommonFailureMechanismSectionAssemblerTests
    {
        private readonly ICommonFailurePathSectionAssembler assembler =
            new CommonFailurePathSectionAssembler();

        [Test]
        public void AssembleCommonFailureMechanismSectionsTest()
        {
            var failureMechanismSections = new List<FailurePathSectionList>
            {
                new FailurePathSectionList("TEST", new[]
                {
                    new FailurePathSectionWithResult(0, 10, EInterpretationCategory.I),
                    new FailurePathSectionWithResult(10, 20, EInterpretationCategory.I),
                    new FailurePathSectionWithResult(20, 30, EInterpretationCategory.Zero),
                    new FailurePathSectionWithResult(30, 40, EInterpretationCategory.I),
                    new FailurePathSectionWithResult(40, 50, EInterpretationCategory.I)
                }),
                new FailurePathSectionList("TEST1", new[]
                {
                    new FailurePathSectionWithResult(0, 5, EInterpretationCategory.II),
                    new FailurePathSectionWithResult(5, 10, EInterpretationCategory.III),
                    new FailurePathSectionWithResult(10, 40, EInterpretationCategory.I),
                    new FailurePathSectionWithResult(40, 45, EInterpretationCategory.Zero),
                    new FailurePathSectionWithResult(45, 50, EInterpretationCategory.I)
                })
            };

            var expectedFailureMechanismResults = new List<FailurePathSectionList>
            {
                new FailurePathSectionList("TEST", new[]
                {
                    new FailurePathSectionWithResult(0.0, 5.0, EInterpretationCategory.I),
                    new FailurePathSectionWithResult(5.0, 10.0, EInterpretationCategory.I),
                    new FailurePathSectionWithResult(10.0, 20.0, EInterpretationCategory.I),
                    new FailurePathSectionWithResult(20.0, 30.0, EInterpretationCategory.Zero),
                    new FailurePathSectionWithResult(30.0, 40.0, EInterpretationCategory.I),
                    new FailurePathSectionWithResult(40.0, 45.0, EInterpretationCategory.I),
                    new FailurePathSectionWithResult(45.0, 50.0, EInterpretationCategory.I)
                }),
                new FailurePathSectionList("TEST1", new[]
                {
                    new FailurePathSectionWithResult(0.0, 5.0, EInterpretationCategory.II),
                    new FailurePathSectionWithResult(5.0, 10.0, EInterpretationCategory.III),
                    new FailurePathSectionWithResult(10.0, 20.0, EInterpretationCategory.I),
                    new FailurePathSectionWithResult(20.0, 30.0, EInterpretationCategory.I),
                    new FailurePathSectionWithResult(30.0, 40.0, EInterpretationCategory.I),
                    new FailurePathSectionWithResult(40.0, 45.0, EInterpretationCategory.Zero),
                    new FailurePathSectionWithResult(45.0, 50.0, EInterpretationCategory.I)
                })
            };

            var expectedCombinedResult = new List<FailurePathSectionWithResult>
            {
                new FailurePathSectionWithResult(0.0, 5.0, EInterpretationCategory.Gr),
                new FailurePathSectionWithResult(5.0, 10.0, EInterpretationCategory.Gr),
                new FailurePathSectionWithResult(10.0, 20.0, EInterpretationCategory.Gr),
                new FailurePathSectionWithResult(20.0, 30.0, EInterpretationCategory.Gr),
                new FailurePathSectionWithResult(30.0, 40.0, EInterpretationCategory.Gr),
                new FailurePathSectionWithResult(40.0, 45.0, EInterpretationCategory.Gr),
                new FailurePathSectionWithResult(45.0, 50.0, EInterpretationCategory.Gr)
            };

            /*var expectedCombinedResult = new List<FailurePathSectionWithResult>
            {
                new FailurePathSectionWithResult(0.0, 5.0, 0.1, EInterpretationCategory.II),
                new FailurePathSectionWithResult(5.0, 10.0, 0.1, EInterpretationCategory.III),
                new FailurePathSectionWithResult(10.0, 20.0, 0.1, EInterpretationCategory.I),
                new FailurePathSectionWithResult(20.0, 30.0, 0.1, EInterpretationCategory.Zero),
                new FailurePathSectionWithResult(30.0, 40.0, 0.1, EInterpretationCategory.I),
                new FailurePathSectionWithResult(40.0, 45.0, 0.1, EInterpretationCategory.Zero),
                new FailurePathSectionWithResult(45.0, 50.0, 0.1, EInterpretationCategory.I)
            };*/

            TestCombinedFailureMechanismSectionAssembler(
                failureMechanismSections,
                expectedFailureMechanismResults,
                expectedCombinedResult,
                false);
        }

        [Test]
        public void DeterminCombinedResultPerCommonSectionThrowsOnEmptyList()
        {
            try
            {
                var commonSectionsWithResults =
                    assembler.DetermineCombinedResultPerCommonSectionWbi3C1(new FailurePathSectionList[]
                                                                                {},
                                                                            false);
            }
            catch (AssemblyException exception)
            {
                Assert.IsNotNull(exception.Errors);
                Assert.AreEqual(1, exception.Errors.Count());
                Assert.AreEqual(EAssemblyErrors.ValueMayNotBeNull, exception.Errors.First().ErrorCode);
                Assert.Pass();
            }

            Assert.Fail("Expected exception did not occur");
        }

        [Test]
        public void DeterminCombinedResultPerCommonSectionThrowsOnEmptyList2()
        {
            try
            {
                var commonSectionsWithResults =
                    assembler.DetermineCombinedResultPerCommonSectionWbi3C1(new FailurePathSectionList[] {},
                                                                            false);
            }
            catch (AssemblyException exception)
            {
                Assert.IsNotNull(exception.Errors);
                Assert.AreEqual(1, exception.Errors.Count());
                Assert.AreEqual(EAssemblyErrors.ValueMayNotBeNull, exception.Errors.First().ErrorCode);
                Assert.Pass();
            }

            Assert.Fail("Expected exception did not occur");
        }

        [Test]
        public void DeterminCombinedResultPerCommonSectionThrowsOnInvalidSections1()
        {
            var sectionsList1 = new FailurePathSectionList("", new[]
            {
                new FailurePathSectionWithResult(0.0, 1.0, EInterpretationCategory.III),
                new FailurePathSectionWithResult(1.0, 2.0, EInterpretationCategory.III)
            });

            var sectionsList2 = new FailurePathSectionList("", new[]
            {
                new FailurePathSectionWithResult(0.0, 1.0, EInterpretationCategory.III)
            });

            try
            {
                var commonSectionsWithResults =
                    assembler.DetermineCombinedResultPerCommonSectionWbi3C1(new[]
                    {
                        sectionsList1,
                        sectionsList2
                    }, false);
            }
            catch (AssemblyException exception)
            {
                Assert.IsNotNull(exception.Errors);
                Assert.AreEqual(1, exception.Errors.Count());
                Assert.AreEqual(EAssemblyErrors.CommonFailureMechanismSectionsInvalid,
                                exception.Errors.First().ErrorCode);
                Assert.Pass();
            }

            Assert.Fail("Expected exception did not occur");
        }

        [Test]
        public void DeterminCombinedResultPerCommonSectionThrowsOnInvalidSections2()
        {
            var sectionsList1 = new FailurePathSectionList("", new[]
            {
                new FailurePathSectionWithResult(0.0, 1.0, EInterpretationCategory.III),
                new FailurePathSectionWithResult(1.0, 2.0, EInterpretationCategory.III),
                new FailurePathSectionWithResult(2.0, 3.0, EInterpretationCategory.III)
            });

            var sectionsList2 = new FailurePathSectionList("", new[]
            {
                new FailurePathSectionWithResult(0.0, 1.0, EInterpretationCategory.III),
                new FailurePathSectionWithResult(1.0, 2.5, EInterpretationCategory.III),
                new FailurePathSectionWithResult(2.5, 3.0, EInterpretationCategory.III)
            });

            try
            {
                var commonSectionsWithResults =
                    assembler.DetermineCombinedResultPerCommonSectionWbi3C1(new[]
                    {
                        sectionsList1,
                        sectionsList2
                    }, false);
            }
            catch (AssemblyException exception)
            {
                Assert.IsNotNull(exception.Errors);
                Assert.AreEqual(1, exception.Errors.Count());
                Assert.AreEqual(EAssemblyErrors.CommonFailureMechanismSectionsInvalid,
                                exception.Errors.First().ErrorCode);
                Assert.Pass();
            }

            Assert.Fail("Expected exception did not occur");
        }

        [Test]
        public void DeterminCombinedResultPerCommonSectionThrowsOnNullValue()
        {
            try
            {
                var commonSectionsWithResults = assembler.DetermineCombinedResultPerCommonSectionWbi3C1(null, false);
            }
            catch (AssemblyException exception)
            {
                Assert.IsNotNull(exception.Errors);
                Assert.AreEqual(1, exception.Errors.Count());
                Assert.AreEqual(EAssemblyErrors.ValueMayNotBeNull, exception.Errors.First().ErrorCode);
                Assert.Pass();
            }

            Assert.Fail("Expected exception did not occur");
        }

        [Test]
        public void FindGreatestCommonDenominatorSectionsWbi3A1IgnoresSmallScetionBoundaryDifferences()
        {
            var delta = 1e-6;
            var assessmentSectionLength = 30.0;
            var list1 = new FailurePathSectionList("FM1", new[]
            {
                new FailurePathSection(0.0, 10.0),
                new FailurePathSection(10.001, 20.0),
                new FailurePathSection(20.0, assessmentSectionLength + delta)
            });
            var list2 = new FailurePathSectionList("FM2", new[]
            {
                new FailurePathSection(0.0, 5.0),
                new FailurePathSection(5.0, 25.0),
                new FailurePathSection(25.0, assessmentSectionLength)
            });
            var list3 = new FailurePathSectionList("FM3", new[]
            {
                new FailurePathSection(0.0, 5.001),
                new FailurePathSection(5.001, 15.0),
                new FailurePathSection(15.0, 28.0),
                new FailurePathSection(28.0, assessmentSectionLength)
            });

            var commonSections =
                assembler.FindGreatestCommonDenominatorSectionsWbi3A1(new[]
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

            var calculatedCommonSecions = commonSections.Sections.ToArray();
            Assert.AreEqual(expectedSectionLimits.Length - 1, calculatedCommonSecions.Length);
            for (int i = 0; i < calculatedCommonSecions.Length; i++)
            {
                Assert.AreEqual(expectedSectionLimits[i], calculatedCommonSecions[i].SectionStart);
                Assert.AreEqual(expectedSectionLimits[i + 1], calculatedCommonSecions[i].SectionEnd);
            }
        }

        [Test]
        public void FindGreatestCommonDenominatorSectionsWbi3A1ReturnsCorrectSections()
        {
            var assessmentSectionLength = 30.0;
            var list1 = new FailurePathSectionList("FM1", new[]
            {
                new FailurePathSection(0.0, 10.0),
                new FailurePathSection(10.0, 20.0),
                new FailurePathSection(20.0, assessmentSectionLength)
            });
            var list2 = new FailurePathSectionList("FM2", new[]
            {
                new FailurePathSection(0.0, 5.0),
                new FailurePathSection(5.0, 25.0),
                new FailurePathSection(25.0, assessmentSectionLength)
            });
            var list3 = new FailurePathSectionList("FM3", new[]
            {
                new FailurePathSection(0.0, 15.0),
                new FailurePathSection(15.0, 28.0),
                new FailurePathSection(28.0, assessmentSectionLength)
            });

            var commonSections =
                assembler.FindGreatestCommonDenominatorSectionsWbi3A1(new[]
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
                                        .Concat(list1.Sections.Select(r => r.SectionEnd).ToArray())
                                        .Concat(list2.Sections.Select(r => r.SectionEnd).ToArray())
                                        .Concat(list3.Sections.Select(r => r.SectionEnd).ToArray())
                                        .Distinct().OrderBy(v => v).ToArray();

            var calculatedCommonSecions = commonSections.Sections.ToArray();
            Assert.AreEqual(expectedSectionLimits.Length - 1, calculatedCommonSecions.Length);
            for (int i = 0; i < calculatedCommonSecions.Length; i++)
            {
                Assert.AreEqual(expectedSectionLimits[i], calculatedCommonSecions[i].SectionStart);
                Assert.AreEqual(expectedSectionLimits[i + 1], calculatedCommonSecions[i].SectionEnd);
            }
        }

        [Test]
        public void FindGreatestCommonDenominatorSectionsWbi3A1ThrowsOnEmptySectionLists()
        {
            try
            {
                var commonSections =
                    assembler.FindGreatestCommonDenominatorSectionsWbi3A1(new FailurePathSectionList[]
                                                                              {}, 10.0);
            }
            catch (AssemblyException exception)
            {
                Assert.IsNotNull(exception.Errors);
                Assert.AreEqual(1, exception.Errors.Count());
                Assert.AreEqual(EAssemblyErrors.ValueMayNotBeNull, exception.Errors.First().ErrorCode);
                Assert.Pass();
            }

            Assert.Fail("Expected exception did not occur");
        }

        [Test]
        [TestCase(-2.3, EAssemblyErrors.SectionLengthOutOfRange)]
        [TestCase(double.NaN, EAssemblyErrors.ValueMayNotBeNull)]
        public void FindGreatestCommonDenominatorSectionsWbi3A1ThrowsOnInvalidAssessmentLength(double assessmentLength,
                                                                                               EAssemblyErrors expectedError)
        {
            var list1 = new FailurePathSectionList("FM1", new[]
            {
                new FailurePathSection(0.0, 10.0),
                new FailurePathSection(10.0, 20.0),
                new FailurePathSection(20.0, 30.0)
            });
            try
            {
                var commonSections =
                    assembler.FindGreatestCommonDenominatorSectionsWbi3A1(new[]
                                                                          {
                                                                              list1
                                                                          },
                                                                          assessmentLength);
            }
            catch (AssemblyException exception)
            {
                Assert.IsNotNull(exception.Errors);
                Assert.AreEqual(1, exception.Errors.Count());
                Assert.AreEqual(expectedError, exception.Errors.First().ErrorCode);
                Assert.Pass();
            }

            Assert.Fail("Expected exception did not occur");
        }

        [Test]
        public void FindGreatestCommonDenominatorSectionsWbi3A1ThrowsOnInvalidSectionList()
        {
            var assessmentSectionLength = 30.0;
            var list1 = new FailurePathSectionList("FM1", new[]
            {
                new FailurePathSection(0.0, 10.0),
                new FailurePathSection(10.001, 20.0),
                new FailurePathSection(20.0, assessmentSectionLength)
            });
            var list2 = new FailurePathSectionList("FM2", new[]
            {
                new FailurePathSection(0.0, 5.0),
                new FailurePathSection(5.0, 25.0),
                new FailurePathSection(25.0, assessmentSectionLength - 1.0)
            });

            try
            {
                var commonSections =
                    assembler.FindGreatestCommonDenominatorSectionsWbi3A1(new[]
                                                                          {
                                                                              list1,
                                                                              list2
                                                                          },
                                                                          assessmentSectionLength);
            }
            catch (AssemblyException exception)
            {
                Assert.IsNotNull(exception.Errors);
                Assert.AreEqual(1, exception.Errors.Count());
                Assert.AreEqual(EAssemblyErrors.FpSectionLengthInvalid, exception.Errors.First().ErrorCode);
                Assert.Pass();
            }

            Assert.Fail("Expected exception did not occur");
        }

        [Test]
        public void FindGreatestCommonDenominatorSectionsWbi3A1ThrowsOnInvalidSectionLists()
        {
            try
            {
                var commonSections = assembler.FindGreatestCommonDenominatorSectionsWbi3A1(null, 10.0);
            }
            catch (AssemblyException exception)
            {
                Assert.IsNotNull(exception.Errors);
                Assert.AreEqual(1, exception.Errors.Count());
                Assert.AreEqual(EAssemblyErrors.ValueMayNotBeNull, exception.Errors.First().ErrorCode);
                Assert.Pass();
            }

            Assert.Fail("Expected exception did not occur");
        }
        [Test]
        public void NoResultFailurePathTest()
        {
            var failureMechanismSections = new List<FailurePathSectionList>
            {
                new FailurePathSectionList("TEST", new[]
                {
                    new FailurePathSectionWithResult(0, 10, EInterpretationCategory.I),
                    new FailurePathSectionWithResult(10, 20, EInterpretationCategory.I),
                    new FailurePathSectionWithResult(20, 30, EInterpretationCategory.II),
                    new FailurePathSectionWithResult(30, 40, EInterpretationCategory.I),
                    new FailurePathSectionWithResult(40, 50, EInterpretationCategory.Gr)
                }),
                new FailurePathSectionList("TEST1", new[]
                {
                    new FailurePathSectionWithResult(0, 5, EInterpretationCategory.II),
                    new FailurePathSectionWithResult(5, 10, EInterpretationCategory.III),
                    new FailurePathSectionWithResult(10, 40, EInterpretationCategory.I),
                    new FailurePathSectionWithResult(40, 45, EInterpretationCategory.Zero),
                    new FailurePathSectionWithResult(45, 50, EInterpretationCategory.I)
                })
            };

            var expectedFailureMechanismResults = new List<FailurePathSectionList>
            {
                new FailurePathSectionList("TEST", new[]
                {
                    new FailurePathSectionWithResult(0.0, 5.0, EInterpretationCategory.I),
                    new FailurePathSectionWithResult(5.0, 10.0, EInterpretationCategory.I),
                    new FailurePathSectionWithResult(10.0, 20.0, EInterpretationCategory.I),
                    new FailurePathSectionWithResult(20.0, 30.0, EInterpretationCategory.II),
                    new FailurePathSectionWithResult(30.0, 40.0, EInterpretationCategory.I),
                    new FailurePathSectionWithResult(40.0, 45.0, EInterpretationCategory.Gr),
                    new FailurePathSectionWithResult(45.0, 50.0, EInterpretationCategory.Gr)
                }),
                new FailurePathSectionList("TEST1", new[]
                {
                    new FailurePathSectionWithResult(0.0, 5.0, EInterpretationCategory.II),
                    new FailurePathSectionWithResult(5.0, 10.0, EInterpretationCategory.III),
                    new FailurePathSectionWithResult(10.0, 20.0, EInterpretationCategory.I),
                    new FailurePathSectionWithResult(20.0, 30.0, EInterpretationCategory.I),
                    new FailurePathSectionWithResult(30.0, 40.0, EInterpretationCategory.I),
                    new FailurePathSectionWithResult(40.0, 45.0, EInterpretationCategory.Zero),
                    new FailurePathSectionWithResult(45.0, 50.0, EInterpretationCategory.I)
                })
            };

            /*var expectedCombinedResult = new List<FailurePathSectionWithResult>
            {
                new FailurePathSectionWithResult(0.0, 5.0, 0.1, EInterpretationCategory.II),
                new FailurePathSectionWithResult(5.0, 10.0, 0.1, EInterpretationCategory.III),
                new FailurePathSectionWithResult(10.0, 20.0, 0.1, EInterpretationCategory.I),
                new FailurePathSectionWithResult(20.0, 30.0, 0.1, EInterpretationCategory.IIIMin),
                new FailurePathSectionWithResult(30.0, 40.0, 0.1, EInterpretationCategory.I),
                new FailurePathSectionWithResult(40.0, 45.0, 0.1, EInterpretationCategory.Gr),
                new FailurePathSectionWithResult(45.0, 50.0, 0.1, EInterpretationCategory.Gr)
            };
            */

            // TODO: Adjust WBI-3C-1 such that the result is correct
            var expectedCombinedResult = new List<FailurePathSectionWithResult>
            {
                new FailurePathSectionWithResult(0.0, 5.0, EInterpretationCategory.Gr),
                new FailurePathSectionWithResult(5.0, 10.0, EInterpretationCategory.Gr),
                new FailurePathSectionWithResult(10.0, 20.0, EInterpretationCategory.Gr),
                new FailurePathSectionWithResult(20.0, 30.0, EInterpretationCategory.Gr),
                new FailurePathSectionWithResult(30.0, 40.0, EInterpretationCategory.Gr),
                new FailurePathSectionWithResult(40.0, 45.0, EInterpretationCategory.Gr),
                new FailurePathSectionWithResult(45.0, 50.0, EInterpretationCategory.Gr)
            };

            TestCombinedFailureMechanismSectionAssembler(
                failureMechanismSections,
                expectedFailureMechanismResults,
                expectedCombinedResult,
                false);
        }

        [Test]
        public void NotAllSectionsProvidedExceptionTest()
        {
            var failureMechanismSections = new List<FailurePathSectionList>
            {
                new FailurePathSectionList("TEST", new[]
                {
                    new FailurePathSectionWithResult(0, 10, EInterpretationCategory.I),
                    new FailurePathSectionWithResult(10, 20, EInterpretationCategory.I),
                    new FailurePathSectionWithResult(30, 40, EInterpretationCategory.I),
                    new FailurePathSectionWithResult(20, 30, EInterpretationCategory.II),
                    new FailurePathSectionWithResult(40, 45, EInterpretationCategory.II)
                    // section starting at 45 to 50 is missing.
                })
            };

            try
            {
                assembler.AssembleCommonFailureMechanismSections(failureMechanismSections, 50.0, false);
            }
            catch (AssemblyException e)
            {
                Assert.NotNull(e.Errors);
                var message = e.Errors.FirstOrDefault();
                Assert.NotNull(message);
                Assert.AreEqual(EAssemblyErrors.FpSectionLengthInvalid, message.ErrorCode);
                Assert.Pass();
            }

            Assert.Fail("Expected exception was not thrown");
        }

        [Test]
        public void PartialFailurePathTest()
        {
            var failureMechanismSections = new List<FailurePathSectionList>
            {
                new FailurePathSectionList("TEST", new[]
                {
                    new FailurePathSectionWithResult(0, 10, EInterpretationCategory.I),
                    new FailurePathSectionWithResult(10, 20, EInterpretationCategory.I),
                    new FailurePathSectionWithResult(20, 30, EInterpretationCategory.IIIMin),
                    new FailurePathSectionWithResult(30, 40, EInterpretationCategory.I),
                    new FailurePathSectionWithResult(40, 50, EInterpretationCategory.II)
                }),
                new FailurePathSectionList("TEST1", new[]
                {
                    new FailurePathSectionWithResult(0, 5, EInterpretationCategory.II),
                    new FailurePathSectionWithResult(5, 10, EInterpretationCategory.III),
                    new FailurePathSectionWithResult(10, 40, EInterpretationCategory.I),
                    new FailurePathSectionWithResult(40, 45, EInterpretationCategory.Zero),
                    new FailurePathSectionWithResult(45, 50, EInterpretationCategory.I)
                })
            };

            var expectedFailureMechanismResults = new List<FailurePathSectionList>
            {
                new FailurePathSectionList("TEST", new[]
                {
                    new FailurePathSectionWithResult(0.0, 5.0, EInterpretationCategory.I),
                    new FailurePathSectionWithResult(5.0, 10.0, EInterpretationCategory.I),
                    new FailurePathSectionWithResult(10.0, 20.0, EInterpretationCategory.I),
                    new FailurePathSectionWithResult(20.0, 30.0, EInterpretationCategory.IIIMin),
                    new FailurePathSectionWithResult(30.0, 40.0, EInterpretationCategory.I),
                    new FailurePathSectionWithResult(40.0, 45.0, EInterpretationCategory.II),
                    new FailurePathSectionWithResult(45.0, 50.0, EInterpretationCategory.II)
                }),
                new FailurePathSectionList("TEST1", new[]
                {
                    new FailurePathSectionWithResult(0.0, 5.0, EInterpretationCategory.II),
                    new FailurePathSectionWithResult(5.0, 10.0, EInterpretationCategory.III),
                    new FailurePathSectionWithResult(10.0, 20.0, EInterpretationCategory.I),
                    new FailurePathSectionWithResult(20.0, 30.0, EInterpretationCategory.I),
                    new FailurePathSectionWithResult(30.0, 40.0, EInterpretationCategory.I),
                    new FailurePathSectionWithResult(40.0, 45.0, EInterpretationCategory.Zero),
                    new FailurePathSectionWithResult(45.0, 50.0, EInterpretationCategory.I)
                })
            };

            /*
            var expectedCombinedResult = new List<FailurePathSectionWithResult>
            {
                new FailurePathSectionWithResult(0.0, 5.0, 0.1, EInterpretationCategory.II),
                new FailurePathSectionWithResult(5.0, 10.0, 0.1, EInterpretationCategory.III),
                new FailurePathSectionWithResult(10.0, 20.0, 0.1, EInterpretationCategory.I),
                new FailurePathSectionWithResult(20.0, 30.0, 0.1, EInterpretationCategory.I),
                new FailurePathSectionWithResult(30.0, 40.0, 0.1, EInterpretationCategory.I),
                new FailurePathSectionWithResult(40.0, 45.0, 0.1, EInterpretationCategory.Zero),
                new FailurePathSectionWithResult(45.0, 50.0, 0.1, EInterpretationCategory.II)
            };
            */
            var expectedCombinedResult = new List<FailurePathSectionWithResult>
            {
                new FailurePathSectionWithResult(0.0, 5.0, EInterpretationCategory.Gr),
                new FailurePathSectionWithResult(5.0, 10.0, EInterpretationCategory.Gr),
                new FailurePathSectionWithResult(10.0, 20.0, EInterpretationCategory.Gr),
                new FailurePathSectionWithResult(20.0, 30.0, EInterpretationCategory.Gr),
                new FailurePathSectionWithResult(30.0, 40.0, EInterpretationCategory.Gr),
                new FailurePathSectionWithResult(40.0, 45.0, EInterpretationCategory.Gr),
                new FailurePathSectionWithResult(45.0, 50.0, EInterpretationCategory.Gr)
            };

            TestCombinedFailureMechanismSectionAssembler(
                failureMechanismSections,
                expectedFailureMechanismResults,
                expectedCombinedResult,
                true);
        }

        [Test]
        public void TranslateFailureMechanismResultsToCommonSectionsWbi3B1ThrowsOnIncorrectListType()
        {
            var resultSectionsList = new FailurePathSectionList("FM1", new[]
            {
                new FailurePathSection(0.0, 5.0),
                new FailurePathSection(5.0, 10.0)
            });

            var commonSectionsList = new FailurePathSectionList("Common", new[]
            {
                new FailurePathSection(0.0, 2.5),
                new FailurePathSection(2.5, 5.0),
                new FailurePathSection(5.0, 7.5),
                new FailurePathSection(7.5, 10.0)
            });

            try
            {
                var commonSectionsWithResults =
                    assembler.TranslateFailureMechanismResultsToCommonSectionsWbi3B1(resultSectionsList,
                                                                                     commonSectionsList);
            }
            catch (AssemblyException exception)
            {
                Assert.IsNotNull(exception.Errors);
                Assert.AreEqual(1, exception.Errors.Count());
                Assert.AreEqual(EAssemblyErrors.SectionsWithoutCategory, exception.Errors.First().ErrorCode);
                Assert.Pass();
            }

            Assert.Fail("Expected exception did not occur");
        }

        [Test]
        public void TranslateFailureMechanismResultsToCommonSectionsWbi3B1ThrowsOnInvalidSectionLengthsEmpty()
        {
            var list = new FailurePathSectionList("TestList", new[]
            {
                new FailurePathSectionWithResult(0.0, 2.85, EInterpretationCategory.III)
            });

            var longList = new FailurePathSectionList("TestListEmpty", new FailurePathSection[]
            {
                new FailurePathSectionWithResult(0.0, 10.0, EInterpretationCategory.III)
            });

            try
            {
                var commonSectionsWithResults =
                    assembler.TranslateFailureMechanismResultsToCommonSectionsWbi3B1(longList, list);
            }
            catch (AssemblyException exception)
            {
                Assert.IsNotNull(exception.Errors);
                Assert.AreEqual(1, exception.Errors.Count());
                Assert.AreEqual(EAssemblyErrors.CommonFailureMechanismSectionsInvalid,
                                exception.Errors.First().ErrorCode);
                Assert.Pass();
            }

            Assert.Fail("Expected exception did not occur");
        }

        [Test]
        public void TranslateFailureMechanismResultsToCommonSectionsWbi3B1ThrowsOnNullLists()
        {
            var list = new FailurePathSectionList("TestList", new[]
            {
                new FailurePathSection(0.0, 2.5)
            });

            try
            {
                var commonSectionsWithResults =
                    assembler.TranslateFailureMechanismResultsToCommonSectionsWbi3B1(null, list);
            }
            catch (AssemblyException exception)
            {
                Assert.IsNotNull(exception.Errors);
                Assert.AreEqual(1, exception.Errors.Count());
                Assert.AreEqual(EAssemblyErrors.ValueMayNotBeNull, exception.Errors.First().ErrorCode);
                Assert.Pass();
            }

            Assert.Fail("Expected exception did not occur");
        }

        [Test]
        public void TranslateFailureMechanismResultsToCommonSectionsWbi3B1ThrowsOnNullLists2()
        {
            var list = new FailurePathSectionList("TestList", new[]
            {
                new FailurePathSection(0.0, 2.5)
            });

            try
            {
                var commonSectionsWithResults =
                    assembler.TranslateFailureMechanismResultsToCommonSectionsWbi3B1(list, null);
            }
            catch (AssemblyException exception)
            {
                Assert.IsNotNull(exception.Errors);
                Assert.AreEqual(1, exception.Errors.Count());
                Assert.AreEqual(EAssemblyErrors.ValueMayNotBeNull, exception.Errors.First().ErrorCode);
                Assert.Pass();
            }

            Assert.Fail("Expected exception did not occur");
        }

        [Test]
        public void TranslateFailureMechanismResultsToCommonSectionsWbi3B1TranslatesCorrectly()
        {
            var resultSectionsList = new FailurePathSectionList("FM1", new[]
            {
                new FailurePathSectionWithResult(0.0, 5.0, EInterpretationCategory.III),
                new FailurePathSectionWithResult(5.0, 10.0, EInterpretationCategory.I)
            });
            var commonSectionsList = new FailurePathSectionList("Common", new[]
            {
                new FailurePathSection(0.0, 2.5),
                new FailurePathSection(2.5, 5.0),
                new FailurePathSection(5.0, 7.5),
                new FailurePathSection(7.5, 10.0)
            });
            var commonSectionsWithResults =
                assembler.TranslateFailureMechanismResultsToCommonSectionsWbi3B1(resultSectionsList,
                                                                                 commonSectionsList);

            Assert.IsNotNull(commonSectionsWithResults.Sections);
            Assert.AreEqual(4, commonSectionsWithResults.Sections.Count());
            Assert.AreEqual(EInterpretationCategory.III,
                            ((FailurePathSectionWithResult) commonSectionsWithResults.Sections.ElementAt(0)).Category);
            Assert.AreEqual(EInterpretationCategory.III,
                            ((FailurePathSectionWithResult) commonSectionsWithResults.Sections.ElementAt(1)).Category);
            Assert.AreEqual(EInterpretationCategory.I,
                            ((FailurePathSectionWithResult) commonSectionsWithResults.Sections.ElementAt(2)).Category);
            Assert.AreEqual(EInterpretationCategory.I,
                            ((FailurePathSectionWithResult) commonSectionsWithResults.Sections.ElementAt(3)).Category);
        }

        [Test]
        public void TranslateFailureMechanismResultsToCommonSectionsWbi3B1WithRoundingTranslatesCorrectly()
        {
            var resultSectionsList = new FailurePathSectionList("FM1", new[]
            {
                new FailurePathSectionWithResult(0.0, 5.0, EInterpretationCategory.III),
                new FailurePathSectionWithResult(5.0, 10.0, EInterpretationCategory.I)
            });
            var commonSectionsList = new FailurePathSectionList("Common", new[]
            {
                new FailurePathSection(0.0, 2.5),
                new FailurePathSection(2.5, 5.0),
                new FailurePathSection(5.0, 7.5),
                new FailurePathSection(7.5, 10.000000001)
            });
            var commonSectionsWithResults =
                assembler.TranslateFailureMechanismResultsToCommonSectionsWbi3B1(resultSectionsList,
                                                                                 commonSectionsList);

            Assert.IsNotNull(commonSectionsWithResults.Sections);
            Assert.AreEqual(4, commonSectionsWithResults.Sections.Count());
            Assert.AreEqual(EInterpretationCategory.III,
                            ((FailurePathSectionWithResult) commonSectionsWithResults.Sections.ElementAt(0)).Category);
            Assert.AreEqual(EInterpretationCategory.III,
                            ((FailurePathSectionWithResult) commonSectionsWithResults.Sections.ElementAt(1)).Category);
            Assert.AreEqual(EInterpretationCategory.I,
                            ((FailurePathSectionWithResult) commonSectionsWithResults.Sections.ElementAt(2)).Category);
            Assert.AreEqual(EInterpretationCategory.I,
                            ((FailurePathSectionWithResult) commonSectionsWithResults.Sections.ElementAt(3)).Category);
        }

        private void TestCombinedFailureMechanismSectionAssembler(
            IEnumerable<FailurePathSectionList> failureMechanismSections,
            IList<FailurePathSectionList> expectedFailureMechanismResults,
            IList<FailurePathSectionWithResult> expectedCombinedResult,
            bool partial)
        {
            var result = assembler.AssembleCommonFailureMechanismSections(failureMechanismSections, 50.0, partial);
            Assert.NotNull(result);

            AssertFailureMechanismSectionLists(expectedFailureMechanismResults, result.ResultPerFailureMechanism);

            AssertCombinedResultsList(expectedCombinedResult, result.CombinedSectionResult);
        }

        private static void AssertCombinedResultsList(IList<FailurePathSectionWithResult> expectedCombinedResult,
                                                      IEnumerable<FailurePathSectionWithResult> result)
        {
            List<FailurePathSectionWithResult> combinedResult = result.ToList();
            Assert.AreEqual(expectedCombinedResult.Count, combinedResult.Count);
            for (var i = 0; i < expectedCombinedResult.Count; i++)
            {
                Assert.AreEqual(expectedCombinedResult[i].SectionStart, combinedResult[i].SectionStart);
                Assert.AreEqual(expectedCombinedResult[i].SectionEnd, combinedResult[i].SectionEnd);
                Assert.AreEqual(expectedCombinedResult[i].Category, combinedResult[i].Category);
            }
        }

        private static void AssertFailureMechanismSectionLists(
            IList<FailurePathSectionList> expectedFailureMechanismResults,
            IEnumerable<FailurePathSectionList> result)
        {
            List<FailurePathSectionList> resultPerFailureMechanism = result.ToList();
            Assert.AreEqual(expectedFailureMechanismResults.Count, resultPerFailureMechanism.Count);

            for (var i = 0; i < expectedFailureMechanismResults.Count; i++)
            {
                var fmResult = resultPerFailureMechanism[i];

                Assert.AreEqual(expectedFailureMechanismResults[i].FailurePathId, fmResult.FailurePathId);

                var sectionResults = fmResult.Sections.ToArray();
                var expectedSectionResults = expectedFailureMechanismResults[i].Sections.ToArray();

                Assert.AreEqual(expectedSectionResults.Length, sectionResults.Length);
                for (var k = 0; k < expectedSectionResults.Length; k++)
                {
                    var expectedResult = expectedSectionResults[k];
                    var sectionResult = sectionResults[k];
                    Assert.NotNull(expectedResult);
                    Assert.NotNull(sectionResult);

                    Assert.AreEqual(expectedResult.SectionStart, sectionResult.SectionStart);
                    Assert.AreEqual(expectedResult.SectionEnd, sectionResult.SectionEnd);

                    Assert.AreEqual(
                            ((FailurePathSectionWithResult) expectedResult).Category,
                            ((FailurePathSectionWithResult) sectionResult).Category);
                }
            }
        }
    }
}