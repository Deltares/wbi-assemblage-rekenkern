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
        private readonly ICommonFailureMechanismSectionAssembler assembler =
            new CommonFailureMechanismSectionAssembler();

        private void TestCombinedFailureMechanismSectionAssembler(
            IEnumerable<FailureMechanismSectionList> failureMechanismSections,
            IList<FailureMechanismSectionList> expectedFailureMechanismResults,
            IList<FmSectionWithDirectCategory> expectedCombinedResult,
            bool partial)
        {
            var result = assembler.AssembleCommonFailureMechanismSections(failureMechanismSections, 50.0, partial);
            Assert.NotNull(result);

            List<FailureMechanismSectionList> resultPerFailureMechanism = result.ResultPerFailureMechanism.ToList();
            Assert.AreEqual(expectedFailureMechanismResults.Count, resultPerFailureMechanism.Count);

            for (var i = 0; i < expectedFailureMechanismResults.Count; i++)
            {
                var fmResult = resultPerFailureMechanism[i];

                Assert.AreEqual(expectedFailureMechanismResults[i].FailureMechanismId, fmResult.FailureMechanismId);

                IEnumerable<FailureMechanismSection> sectionResults = fmResult.Results;
                IEnumerable<FailureMechanismSection> expectedSectionResults = expectedFailureMechanismResults[i].Results;

                Assert.AreEqual(expectedSectionResults.Count(), sectionResults.Count());
                for (var k = 0; k < expectedSectionResults.Count(); k++)
                {
                    var expectedResult = expectedSectionResults.ElementAt(k);
                    var sectionResult = sectionResults.ElementAt(k);
                    Assert.NotNull(expectedResult);
                    Assert.NotNull(sectionResult);

                    Assert.AreEqual(expectedResult.SectionStart, sectionResult.SectionStart);
                    Assert.AreEqual(expectedResult.SectionEnd, sectionResult.SectionEnd);

                    if (sectionResult is FmSectionWithIndirectCategory)
                    {
                        Assert.AreEqual(
                            ((FmSectionWithIndirectCategory) expectedResult).Category,
                            ((FmSectionWithIndirectCategory) sectionResult).Category);
                    }
                    else
                    {
                        Assert.AreEqual(
                            ((FmSectionWithDirectCategory) expectedResult).Category,
                            ((FmSectionWithDirectCategory) sectionResult).Category);
                    }
                }
            }

            List<FmSectionWithDirectCategory> combinedResult = result.CombinedSectionResult.ToList();
            Assert.AreEqual(expectedCombinedResult.Count, combinedResult.Count);
            for (var i = 0; i < expectedCombinedResult.Count; i++)
            {
                Assert.AreEqual(expectedCombinedResult[i].SectionStart, combinedResult[i].SectionStart);
                Assert.AreEqual(expectedCombinedResult[i].SectionEnd, combinedResult[i].SectionEnd);
                Assert.AreEqual(expectedCombinedResult[i].Category, combinedResult[i].Category);
            }
        }

        [Test]
        public void AssembleCommonFailureMechanismSectionsTest()
        {
            var failureMechanismSections = new List<FailureMechanismSectionList>
            {
                new FailureMechanismSectionList("TEST", new[]
                {
                    new FmSectionWithDirectCategory(0, 10, EFmSectionCategory.Iv),
                    new FmSectionWithDirectCategory(10, 20, EFmSectionCategory.Iv),
                    new FmSectionWithDirectCategory(20, 30, EFmSectionCategory.Vv),
                    new FmSectionWithDirectCategory(30, 40, EFmSectionCategory.Iv),
                    new FmSectionWithDirectCategory(40, 50, EFmSectionCategory.Iv)
                }),
                new FailureMechanismSectionList("TEST1", new[]
                {
                    new FmSectionWithDirectCategory(0, 5, EFmSectionCategory.IIv),
                    new FmSectionWithDirectCategory(5, 10, EFmSectionCategory.IIIv),
                    new FmSectionWithDirectCategory(10, 40, EFmSectionCategory.IVv),
                    new FmSectionWithDirectCategory(40, 45, EFmSectionCategory.Vv),
                    new FmSectionWithDirectCategory(45, 50, EFmSectionCategory.Iv)
                })
            };

            var expectedFailureMechanismResults = new List<FailureMechanismSectionList>
            {
                new FailureMechanismSectionList("TEST", new[]
                {
                    new FmSectionWithDirectCategory(0.0, 5.0, EFmSectionCategory.Iv),
                    new FmSectionWithDirectCategory(5.0, 10.0, EFmSectionCategory.Iv),
                    new FmSectionWithDirectCategory(10.0, 20.0, EFmSectionCategory.Iv),
                    new FmSectionWithDirectCategory(20.0, 30.0, EFmSectionCategory.Vv),
                    new FmSectionWithDirectCategory(30.0, 40.0, EFmSectionCategory.Iv),
                    new FmSectionWithDirectCategory(40.0, 45.0, EFmSectionCategory.Iv),
                    new FmSectionWithDirectCategory(45.0, 50.0, EFmSectionCategory.Iv)
                }),
                new FailureMechanismSectionList("TEST1", new[]
                {
                    new FmSectionWithDirectCategory(0.0, 5.0, EFmSectionCategory.IIv),
                    new FmSectionWithDirectCategory(5.0, 10.0, EFmSectionCategory.IIIv),
                    new FmSectionWithDirectCategory(10.0, 20.0, EFmSectionCategory.IVv),
                    new FmSectionWithDirectCategory(20.0, 30.0, EFmSectionCategory.IVv),
                    new FmSectionWithDirectCategory(30.0, 40.0, EFmSectionCategory.IVv),
                    new FmSectionWithDirectCategory(40.0, 45.0, EFmSectionCategory.Vv),
                    new FmSectionWithDirectCategory(45.0, 50.0, EFmSectionCategory.Iv)
                })
            };

            var expectedCombinedResult = new List<FmSectionWithDirectCategory>
            {
                new FmSectionWithDirectCategory(0.0, 5.0, EFmSectionCategory.IIv),
                new FmSectionWithDirectCategory(5.0, 10.0, EFmSectionCategory.IIIv),
                new FmSectionWithDirectCategory(10.0, 20.0, EFmSectionCategory.IVv),
                new FmSectionWithDirectCategory(20.0, 30.0, EFmSectionCategory.Vv),
                new FmSectionWithDirectCategory(30.0, 40.0, EFmSectionCategory.IVv),
                new FmSectionWithDirectCategory(40.0, 45.0, EFmSectionCategory.Vv),
                new FmSectionWithDirectCategory(45.0, 50.0, EFmSectionCategory.Iv)
            };

            TestCombinedFailureMechanismSectionAssembler(
                failureMechanismSections,
                expectedFailureMechanismResults,
                expectedCombinedResult,
                false);
        }

        [Test]
        public void IndirectFailureMechanismTest()
        {
            var failureMechanismSections = new List<FailureMechanismSectionList>
            {
                new FailureMechanismSectionList("TEST", new[]
                {
                    new FmSectionWithDirectCategory(0, 10, EFmSectionCategory.Iv),
                    new FmSectionWithDirectCategory(10, 20, EFmSectionCategory.VIIv),
                    new FmSectionWithDirectCategory(20, 30, EFmSectionCategory.Vv),
                    new FmSectionWithDirectCategory(30, 40, EFmSectionCategory.Iv),
                    new FmSectionWithDirectCategory(40, 50, EFmSectionCategory.Iv)
                }),
                new FailureMechanismSectionList("TEST1", new[]
                {
                    new FmSectionWithDirectCategory(0, 5, EFmSectionCategory.IIv),
                    new FmSectionWithDirectCategory(5, 10, EFmSectionCategory.IIIv),
                    new FmSectionWithDirectCategory(10, 40, EFmSectionCategory.IVv),
                    new FmSectionWithDirectCategory(40, 45, EFmSectionCategory.Vv),
                    new FmSectionWithDirectCategory(45, 50, EFmSectionCategory.Iv)
                }),

                new FailureMechanismSectionList("TEST2", new[]
                {
                    new FmSectionWithIndirectCategory(0, 7, EIndirectAssessmentResult.FvGt),
                    new FmSectionWithIndirectCategory(7, 8, EIndirectAssessmentResult.Ngo),
                    new FmSectionWithIndirectCategory(8, 10, EIndirectAssessmentResult.FvGt),
                    new FmSectionWithIndirectCategory(10, 50, EIndirectAssessmentResult.FvTom)
                })
            };

            var expectedFailureMechanismResults = new List<FailureMechanismSectionList>
            {
                new FailureMechanismSectionList("TEST", new[]
                {
                    new FmSectionWithDirectCategory(0.0, 5.0, EFmSectionCategory.Iv),
                    new FmSectionWithDirectCategory(5.0, 7.0, EFmSectionCategory.Iv),
                    new FmSectionWithDirectCategory(7.0, 8.0, EFmSectionCategory.Iv),
                    new FmSectionWithDirectCategory(8.0, 10.0, EFmSectionCategory.Iv),
                    new FmSectionWithDirectCategory(10.0, 20.0, EFmSectionCategory.VIIv),
                    new FmSectionWithDirectCategory(20.0, 30.0, EFmSectionCategory.Vv),
                    new FmSectionWithDirectCategory(30.0, 40.0, EFmSectionCategory.Iv),
                    new FmSectionWithDirectCategory(40.0, 45.0, EFmSectionCategory.Iv),
                    new FmSectionWithDirectCategory(45.0, 50.0, EFmSectionCategory.Iv)
                }),
                new FailureMechanismSectionList("TEST1", new[]
                {
                    new FmSectionWithDirectCategory(0.0, 5.0, EFmSectionCategory.IIv),
                    new FmSectionWithDirectCategory(5.0, 7.0, EFmSectionCategory.IIIv),
                    new FmSectionWithDirectCategory(7.0, 8.0, EFmSectionCategory.IIIv),
                    new FmSectionWithDirectCategory(8.0, 10.0, EFmSectionCategory.IIIv),
                    new FmSectionWithDirectCategory(10.0, 20.0, EFmSectionCategory.IVv),
                    new FmSectionWithDirectCategory(20.0, 30.0, EFmSectionCategory.IVv),
                    new FmSectionWithDirectCategory(30.0, 40.0, EFmSectionCategory.IVv),
                    new FmSectionWithDirectCategory(40.0, 45.0, EFmSectionCategory.Vv),
                    new FmSectionWithDirectCategory(45.0, 50.0, EFmSectionCategory.Iv)
                }),
                new FailureMechanismSectionList("TEST2", new[]
                {
                    new FmSectionWithIndirectCategory(0.0, 5.0, EIndirectAssessmentResult.FvGt),
                    new FmSectionWithIndirectCategory(5.0, 7.0, EIndirectAssessmentResult.FvGt),
                    new FmSectionWithIndirectCategory(7.0, 8.0, EIndirectAssessmentResult.Ngo),
                    new FmSectionWithIndirectCategory(8.0, 10.0, EIndirectAssessmentResult.FvGt),
                    new FmSectionWithIndirectCategory(10.0, 20.0, EIndirectAssessmentResult.FvTom),
                    new FmSectionWithIndirectCategory(20.0, 30.0, EIndirectAssessmentResult.FvTom),
                    new FmSectionWithIndirectCategory(30.0, 40.0, EIndirectAssessmentResult.FvTom),
                    new FmSectionWithIndirectCategory(40.0, 45.0, EIndirectAssessmentResult.FvTom),
                    new FmSectionWithIndirectCategory(45.0, 50.0, EIndirectAssessmentResult.FvTom)
                })
            };

            var expectedCombinedResult = new List<FmSectionWithDirectCategory>
            {
                new FmSectionWithDirectCategory(0.0, 5.0, EFmSectionCategory.IIv),
                new FmSectionWithDirectCategory(5.0, 7.0, EFmSectionCategory.IIIv),
                new FmSectionWithDirectCategory(7.0, 8.0, EFmSectionCategory.IIIv),
                new FmSectionWithDirectCategory(8.0, 10.0, EFmSectionCategory.IIIv),
                new FmSectionWithDirectCategory(10.0, 20.0, EFmSectionCategory.VIIv),
                new FmSectionWithDirectCategory(20.0, 30.0, EFmSectionCategory.Vv),
                new FmSectionWithDirectCategory(30.0, 40.0, EFmSectionCategory.IVv),
                new FmSectionWithDirectCategory(40.0, 45.0, EFmSectionCategory.Vv),
                new FmSectionWithDirectCategory(45.0, 50.0, EFmSectionCategory.Iv)
            };

            TestCombinedFailureMechanismSectionAssembler(
                failureMechanismSections,
                expectedFailureMechanismResults,
                expectedCombinedResult,
                false);
        }

        [Test]
        public void NoResultFailureMechanismTest()
        {
            var failureMechanismSections = new List<FailureMechanismSectionList>
            {
                new FailureMechanismSectionList("TEST", new[]
                {
                    new FmSectionWithDirectCategory(0, 10, EFmSectionCategory.Iv),
                    new FmSectionWithDirectCategory(10, 20, EFmSectionCategory.Iv),
                    new FmSectionWithDirectCategory(20, 30, EFmSectionCategory.VIIv),
                    new FmSectionWithDirectCategory(30, 40, EFmSectionCategory.Iv),
                    new FmSectionWithDirectCategory(40, 50, EFmSectionCategory.Gr)
                }),
                new FailureMechanismSectionList("TEST1", new[]
                {
                    new FmSectionWithDirectCategory(0, 5, EFmSectionCategory.IIv),
                    new FmSectionWithDirectCategory(5, 10, EFmSectionCategory.IIIv),
                    new FmSectionWithDirectCategory(10, 40, EFmSectionCategory.IVv),
                    new FmSectionWithDirectCategory(40, 45, EFmSectionCategory.Vv),
                    new FmSectionWithDirectCategory(45, 50, EFmSectionCategory.Iv)
                }),
                new FailureMechanismSectionList("TEST2", new[]
                {
                    new FmSectionWithIndirectCategory(0, 20, EIndirectAssessmentResult.Gr),
                    new FmSectionWithIndirectCategory(20, 50, EIndirectAssessmentResult.FvGt)
                })
            };

            var expectedFailureMechanismResults = new List<FailureMechanismSectionList>
            {
                new FailureMechanismSectionList("TEST", new[]
                {
                    new FmSectionWithDirectCategory(0.0, 5.0, EFmSectionCategory.Iv),
                    new FmSectionWithDirectCategory(5.0, 10.0, EFmSectionCategory.Iv),
                    new FmSectionWithDirectCategory(10.0, 20.0, EFmSectionCategory.Iv),
                    new FmSectionWithDirectCategory(20.0, 30.0, EFmSectionCategory.VIIv),
                    new FmSectionWithDirectCategory(30.0, 40.0, EFmSectionCategory.Iv),
                    new FmSectionWithDirectCategory(40.0, 45.0, EFmSectionCategory.Gr),
                    new FmSectionWithDirectCategory(45.0, 50.0, EFmSectionCategory.Gr)
                }),
                new FailureMechanismSectionList("TEST1", new[]
                {
                    new FmSectionWithDirectCategory(0.0, 5.0, EFmSectionCategory.IIv),
                    new FmSectionWithDirectCategory(5.0, 10.0, EFmSectionCategory.IIIv),
                    new FmSectionWithDirectCategory(10.0, 20.0, EFmSectionCategory.IVv),
                    new FmSectionWithDirectCategory(20.0, 30.0, EFmSectionCategory.IVv),
                    new FmSectionWithDirectCategory(30.0, 40.0, EFmSectionCategory.IVv),
                    new FmSectionWithDirectCategory(40.0, 45.0, EFmSectionCategory.Vv),
                    new FmSectionWithDirectCategory(45.0, 50.0, EFmSectionCategory.Iv)
                }),

                new FailureMechanismSectionList("TEST2", new[]
                {
                    new FmSectionWithIndirectCategory(0.0, 5.0, EIndirectAssessmentResult.Gr),
                    new FmSectionWithIndirectCategory(5.0, 10.0, EIndirectAssessmentResult.Gr),
                    new FmSectionWithIndirectCategory(10.0, 20.0, EIndirectAssessmentResult.Gr),
                    new FmSectionWithIndirectCategory(20.0, 30.0, EIndirectAssessmentResult.FvGt),
                    new FmSectionWithIndirectCategory(30.0, 40.0, EIndirectAssessmentResult.FvGt),
                    new FmSectionWithIndirectCategory(40.0, 45.0, EIndirectAssessmentResult.FvGt),
                    new FmSectionWithIndirectCategory(45.0, 50.0, EIndirectAssessmentResult.FvGt)
                })
            };

            var expectedCombinedResult = new List<FmSectionWithDirectCategory>
            {
                new FmSectionWithDirectCategory(0.0, 5.0, EFmSectionCategory.IIv),
                new FmSectionWithDirectCategory(5.0, 10.0, EFmSectionCategory.IIIv),
                new FmSectionWithDirectCategory(10.0, 20.0, EFmSectionCategory.IVv),
                new FmSectionWithDirectCategory(20.0, 30.0, EFmSectionCategory.VIIv),
                new FmSectionWithDirectCategory(30.0, 40.0, EFmSectionCategory.IVv),
                new FmSectionWithDirectCategory(40.0, 45.0, EFmSectionCategory.Gr),
                new FmSectionWithDirectCategory(45.0, 50.0, EFmSectionCategory.Gr)
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
            var failureMechanismSections = new List<FailureMechanismSectionList>
            {
                new FailureMechanismSectionList("TEST", new[]
                {
                    new FmSectionWithDirectCategory(0, 10, EFmSectionCategory.Iv),
                    new FmSectionWithDirectCategory(10, 20, EFmSectionCategory.Iv),
                    new FmSectionWithDirectCategory(30, 40, EFmSectionCategory.Iv),
                    new FmSectionWithDirectCategory(20, 30, EFmSectionCategory.IIv),
                    new FmSectionWithDirectCategory(40, 45, EFmSectionCategory.IIv)
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
                Assert.AreEqual(EAssemblyErrors.FmSectionLengthInvalid, message.ErrorCode);
                Assert.Pass();
            }

            Assert.Fail("Expected exception was not thrown");
        }

        [Test]
        public void NotApplicableFailureMechanismTest()
        {
            var failureMechanismSections = new List<FailureMechanismSectionList>
            {
                new FailureMechanismSectionList("TEST", new[]
                {
                    new FmSectionWithDirectCategory(0, 10, EFmSectionCategory.Iv),
                    new FmSectionWithDirectCategory(10, 20, EFmSectionCategory.Iv),
                    new FmSectionWithDirectCategory(20, 30, EFmSectionCategory.NotApplicable),
                    new FmSectionWithDirectCategory(30, 40, EFmSectionCategory.Iv),
                    new FmSectionWithDirectCategory(40, 50, EFmSectionCategory.IIv)
                }),
                new FailureMechanismSectionList("TEST1", new[]
                {
                    new FmSectionWithDirectCategory(0, 5, EFmSectionCategory.IIv),
                    new FmSectionWithDirectCategory(5, 10, EFmSectionCategory.NotApplicable),
                    new FmSectionWithDirectCategory(10, 40, EFmSectionCategory.NotApplicable),
                    new FmSectionWithDirectCategory(40, 45, EFmSectionCategory.Vv),
                    new FmSectionWithDirectCategory(45, 50, EFmSectionCategory.Iv)
                })
            };

            var expectedFailureMechanismResults = new List<FailureMechanismSectionList>
            {
                new FailureMechanismSectionList("TEST", new[]
                {
                    new FmSectionWithDirectCategory(0.0, 5.0, EFmSectionCategory.Iv),
                    new FmSectionWithDirectCategory(5.0, 10.0, EFmSectionCategory.Iv),
                    new FmSectionWithDirectCategory(10.0, 20.0, EFmSectionCategory.Iv),
                    new FmSectionWithDirectCategory(20.0, 30.0, EFmSectionCategory.NotApplicable),
                    new FmSectionWithDirectCategory(30.0, 40.0, EFmSectionCategory.Iv),
                    new FmSectionWithDirectCategory(40.0, 45.0, EFmSectionCategory.IIv),
                    new FmSectionWithDirectCategory(45.0, 50.0, EFmSectionCategory.IIv)
                }),
                new FailureMechanismSectionList("TEST1", new[]
                {
                    new FmSectionWithDirectCategory(0.0, 5.0, EFmSectionCategory.IIv),
                    new FmSectionWithDirectCategory(5.0, 10.0, EFmSectionCategory.NotApplicable),
                    new FmSectionWithDirectCategory(10.0, 20.0, EFmSectionCategory.NotApplicable),
                    new FmSectionWithDirectCategory(20.0, 30.0, EFmSectionCategory.NotApplicable),
                    new FmSectionWithDirectCategory(30.0, 40.0, EFmSectionCategory.NotApplicable),
                    new FmSectionWithDirectCategory(40.0, 45.0, EFmSectionCategory.Vv),
                    new FmSectionWithDirectCategory(45.0, 50.0, EFmSectionCategory.Iv)
                })
            };

            var expectedCombinedResult = new List<FmSectionWithDirectCategory>
            {
                new FmSectionWithDirectCategory(0.0, 5.0, EFmSectionCategory.IIv),
                new FmSectionWithDirectCategory(5.0, 10.0, EFmSectionCategory.Iv),
                new FmSectionWithDirectCategory(10.0, 20.0, EFmSectionCategory.Iv),
                new FmSectionWithDirectCategory(20.0, 30.0, EFmSectionCategory.NotApplicable),
                new FmSectionWithDirectCategory(30.0, 40.0, EFmSectionCategory.Iv),
                new FmSectionWithDirectCategory(40.0, 45.0, EFmSectionCategory.Vv),
                new FmSectionWithDirectCategory(45.0, 50.0, EFmSectionCategory.IIv)
            };

            TestCombinedFailureMechanismSectionAssembler(
                failureMechanismSections,
                expectedFailureMechanismResults,
                expectedCombinedResult,
                true);
        }

        [Test]
        public void NotApplicablePartialFailureMechanismTest()
        {
            var failureMechanismSections = new List<FailureMechanismSectionList>
            {
                new FailureMechanismSectionList("TEST", new[]
                {
                    new FmSectionWithDirectCategory(0, 10, EFmSectionCategory.Iv),
                    new FmSectionWithDirectCategory(10, 20, EFmSectionCategory.Iv),
                    new FmSectionWithDirectCategory(20, 30, EFmSectionCategory.NotApplicable),
                    new FmSectionWithDirectCategory(30, 40, EFmSectionCategory.Iv),
                    new FmSectionWithDirectCategory(40, 50, EFmSectionCategory.IIv)
                }),
                new FailureMechanismSectionList("TEST1", new[]
                {
                    new FmSectionWithDirectCategory(0, 5, EFmSectionCategory.IIv),
                    new FmSectionWithDirectCategory(5, 10, EFmSectionCategory.IIIv),
                    new FmSectionWithDirectCategory(10, 40, EFmSectionCategory.NotApplicable),
                    new FmSectionWithDirectCategory(40, 45, EFmSectionCategory.Vv),
                    new FmSectionWithDirectCategory(45, 50, EFmSectionCategory.Iv)
                })
            };

            var expectedFailureMechanismResults = new List<FailureMechanismSectionList>
            {
                new FailureMechanismSectionList("TEST", new[]
                {
                    new FmSectionWithDirectCategory(0.0, 5.0, EFmSectionCategory.Iv),
                    new FmSectionWithDirectCategory(5.0, 10.0, EFmSectionCategory.Iv),
                    new FmSectionWithDirectCategory(10.0, 20.0, EFmSectionCategory.Iv),
                    new FmSectionWithDirectCategory(20.0, 30.0, EFmSectionCategory.NotApplicable),
                    new FmSectionWithDirectCategory(30.0, 40.0, EFmSectionCategory.Iv),
                    new FmSectionWithDirectCategory(40.0, 45.0, EFmSectionCategory.IIv),
                    new FmSectionWithDirectCategory(45.0, 50.0, EFmSectionCategory.IIv)
                }),
                new FailureMechanismSectionList("TEST1", new[]
                {
                    new FmSectionWithDirectCategory(0.0, 5.0, EFmSectionCategory.IIv),
                    new FmSectionWithDirectCategory(5.0, 10.0, EFmSectionCategory.IIIv),
                    new FmSectionWithDirectCategory(10.0, 20.0, EFmSectionCategory.NotApplicable),
                    new FmSectionWithDirectCategory(20.0, 30.0, EFmSectionCategory.NotApplicable),
                    new FmSectionWithDirectCategory(30.0, 40.0, EFmSectionCategory.NotApplicable),
                    new FmSectionWithDirectCategory(40.0, 45.0, EFmSectionCategory.Vv),
                    new FmSectionWithDirectCategory(45.0, 50.0, EFmSectionCategory.Iv)
                })
            };

            var expectedCombinedResult = new List<FmSectionWithDirectCategory>
            {
                new FmSectionWithDirectCategory(0.0, 5.0, EFmSectionCategory.IIv),
                new FmSectionWithDirectCategory(5.0, 10.0, EFmSectionCategory.IIIv),
                new FmSectionWithDirectCategory(10.0, 20.0, EFmSectionCategory.Iv),
                new FmSectionWithDirectCategory(20.0, 30.0, EFmSectionCategory.NotApplicable),
                new FmSectionWithDirectCategory(30.0, 40.0, EFmSectionCategory.Iv),
                new FmSectionWithDirectCategory(40.0, 45.0, EFmSectionCategory.Vv),
                new FmSectionWithDirectCategory(45.0, 50.0, EFmSectionCategory.IIv)
            };

            TestCombinedFailureMechanismSectionAssembler(
                failureMechanismSections,
                expectedFailureMechanismResults,
                expectedCombinedResult,
                true);
        }

        [Test]
        public void PartialFailureMechanismTest()
        {
            var failureMechanismSections = new List<FailureMechanismSectionList>
            {
                new FailureMechanismSectionList("TEST", new[]
                {
                    new FmSectionWithDirectCategory(0, 10, EFmSectionCategory.Iv),
                    new FmSectionWithDirectCategory(10, 20, EFmSectionCategory.Iv),
                    new FmSectionWithDirectCategory(20, 30, EFmSectionCategory.VIIv),
                    new FmSectionWithDirectCategory(30, 40, EFmSectionCategory.Iv),
                    new FmSectionWithDirectCategory(40, 50, EFmSectionCategory.IIv)
                }),
                new FailureMechanismSectionList("TEST1", new[]
                {
                    new FmSectionWithDirectCategory(0, 5, EFmSectionCategory.IIv),
                    new FmSectionWithDirectCategory(5, 10, EFmSectionCategory.IIIv),
                    new FmSectionWithDirectCategory(10, 40, EFmSectionCategory.IVv),
                    new FmSectionWithDirectCategory(40, 45, EFmSectionCategory.Vv),
                    new FmSectionWithDirectCategory(45, 50, EFmSectionCategory.Iv)
                })
            };

            var expectedFailureMechanismResults = new List<FailureMechanismSectionList>
            {
                new FailureMechanismSectionList("TEST", new[]
                {
                    new FmSectionWithDirectCategory(0.0, 5.0, EFmSectionCategory.Iv),
                    new FmSectionWithDirectCategory(5.0, 10.0, EFmSectionCategory.Iv),
                    new FmSectionWithDirectCategory(10.0, 20.0, EFmSectionCategory.Iv),
                    new FmSectionWithDirectCategory(20.0, 30.0, EFmSectionCategory.VIIv),
                    new FmSectionWithDirectCategory(30.0, 40.0, EFmSectionCategory.Iv),
                    new FmSectionWithDirectCategory(40.0, 45.0, EFmSectionCategory.IIv),
                    new FmSectionWithDirectCategory(45.0, 50.0, EFmSectionCategory.IIv)
                }),
                new FailureMechanismSectionList("TEST1", new[]
                {
                    new FmSectionWithDirectCategory(0.0, 5.0, EFmSectionCategory.IIv),
                    new FmSectionWithDirectCategory(5.0, 10.0, EFmSectionCategory.IIIv),
                    new FmSectionWithDirectCategory(10.0, 20.0, EFmSectionCategory.IVv),
                    new FmSectionWithDirectCategory(20.0, 30.0, EFmSectionCategory.IVv),
                    new FmSectionWithDirectCategory(30.0, 40.0, EFmSectionCategory.IVv),
                    new FmSectionWithDirectCategory(40.0, 45.0, EFmSectionCategory.Vv),
                    new FmSectionWithDirectCategory(45.0, 50.0, EFmSectionCategory.Iv)
                })
            };

            var expectedCombinedResult = new List<FmSectionWithDirectCategory>
            {
                new FmSectionWithDirectCategory(0.0, 5.0, EFmSectionCategory.IIv),
                new FmSectionWithDirectCategory(5.0, 10.0, EFmSectionCategory.IIIv),
                new FmSectionWithDirectCategory(10.0, 20.0, EFmSectionCategory.IVv),
                new FmSectionWithDirectCategory(20.0, 30.0, EFmSectionCategory.IVv),
                new FmSectionWithDirectCategory(30.0, 40.0, EFmSectionCategory.IVv),
                new FmSectionWithDirectCategory(40.0, 45.0, EFmSectionCategory.Vv),
                new FmSectionWithDirectCategory(45.0, 50.0, EFmSectionCategory.IIv)
            };

            TestCombinedFailureMechanismSectionAssembler(
                failureMechanismSections,
                expectedFailureMechanismResults,
                expectedCombinedResult,
                true);
        }

        [Test]
        public void FindGreatestCommonDenominatorSectionsWbi1C1ReturnsCorrectSections()
        {
            var assessmentSectionLength = 30.0;
            var list1 = new FailureMechanismSectionList("FM1",new []
            {
                new FailureMechanismSection(0.0,10.0),
                new FailureMechanismSection(10.0,20.0),
                new FailureMechanismSection(20.0,assessmentSectionLength)
            });
            var list2 = new FailureMechanismSectionList("FM2", new[]
            {
                new FailureMechanismSection(0.0,5.0),
                new FailureMechanismSection(5.0,25.0),
                new FailureMechanismSection(25.0,assessmentSectionLength)
            });
            var list3 = new FailureMechanismSectionList("FM3", new[]
            {
                new FailureMechanismSection(0.0,15.0),
                new FailureMechanismSection(15.0,28.0),
                new FailureMechanismSection(28.0,assessmentSectionLength)
            });

            var commonSections =
                assembler.FindGreatestCommonDenominatorSectionsWbi1C1(new[] {list1, list2, list3},
                    assessmentSectionLength);

            var expectedSectionLimits = new[] {0.0}
                .Concat(list1.Results.Select(r => r.SectionEnd).ToArray())
                .Concat(list2.Results.Select(r => r.SectionEnd).ToArray())
                .Concat(list3.Results.Select(r => r.SectionEnd).ToArray())
                .Distinct().OrderBy(v => v).ToArray();

            var calculatedCommonSecions = commonSections.Results.ToArray();
            Assert.AreEqual(expectedSectionLimits.Length - 1, calculatedCommonSecions.Length);
            for (int i = 0; i < calculatedCommonSecions.Length; i++)
            {
                Assert.AreEqual(expectedSectionLimits[i],calculatedCommonSecions[i].SectionStart);
                Assert.AreEqual(expectedSectionLimits[i+1], calculatedCommonSecions[i].SectionEnd);
            }
        }

        [Test]
        public void FindGreatestCommonDenominatorSectionsWbi1C1IgnoresSmallScetionBoundaryDifferences()
        {
            var assessmentSectionLength = 30.0;
            var list1 = new FailureMechanismSectionList("FM1", new[]
            {
                new FailureMechanismSection(0.0,10.0),
                new FailureMechanismSection(10.001,20.0),
                new FailureMechanismSection(20.0,assessmentSectionLength)
            });
            var list2 = new FailureMechanismSectionList("FM2", new[]
            {
                new FailureMechanismSection(0.0,5.0),
                new FailureMechanismSection(5.0,25.0),
                new FailureMechanismSection(25.0,assessmentSectionLength)
            });
            var list3 = new FailureMechanismSectionList("FM3", new[]
            {
                new FailureMechanismSection(0.0,15.0),
                new FailureMechanismSection(15.0,28.0),
                new FailureMechanismSection(28.0,assessmentSectionLength)
            });

            var commonSections =
                assembler.FindGreatestCommonDenominatorSectionsWbi1C1(new[] { list1, list2, list3 },
                    assessmentSectionLength);

            var expectedSectionLimits = new[] { 0.0 }
                .Concat(list1.Results.Select(r => r.SectionEnd).ToArray())
                .Concat(list2.Results.Select(r => r.SectionEnd).ToArray())
                .Concat(list3.Results.Select(r => r.SectionEnd).ToArray())
                .Distinct().OrderBy(v => v).ToArray();

            var calculatedCommonSecions = commonSections.Results.ToArray();
            Assert.AreEqual(expectedSectionLimits.Length - 1, calculatedCommonSecions.Length);
            for (int i = 0; i < calculatedCommonSecions.Length; i++)
            {
                Assert.AreEqual(expectedSectionLimits[i], calculatedCommonSecions[i].SectionStart);
                Assert.AreEqual(expectedSectionLimits[i + 1], calculatedCommonSecions[i].SectionEnd);
            }
        }

        [Test]
        [TestCase(-2.3, EAssemblyErrors.SectionLengthOutOfRange)]
        [TestCase(double.NaN, EAssemblyErrors.ValueMayNotBeNull)]
        public void FindGreatestCommonDenominatorSectionsWbi1C1ThrowsOnInvalidAssessmentLength(double assessmentLength, EAssemblyErrors expectedError)
        {
            var list1 = new FailureMechanismSectionList("FM1", new[]
            {
                new FailureMechanismSection(0.0,10.0),
                new FailureMechanismSection(10.0,20.0),
                new FailureMechanismSection(20.0,30.0)
            });
            try
            {
                var commonSections =
                    assembler.FindGreatestCommonDenominatorSectionsWbi1C1(new[] { list1, },
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
        public void FindGreatestCommonDenominatorSectionsWbi1C1ThrowsOnInvalidSectionLists()
        {
            try
            {
                var commonSections = assembler.FindGreatestCommonDenominatorSectionsWbi1C1(null,10.0);
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
        public void FindGreatestCommonDenominatorSectionsWbi1C1ThrowsOnEmptySectionLists()
        {
            try
            {
                var commonSections = assembler.FindGreatestCommonDenominatorSectionsWbi1C1(new FailureMechanismSectionList[]{}, 10.0);
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
        public void FindGreatestCommonDenominatorSectionsWbi1C1ThrowsOnInvalidSectionList()
        {
            var assessmentSectionLength = 30.0;
            var list1 = new FailureMechanismSectionList("FM1", new[]
            {
                new FailureMechanismSection(0.0,10.0),
                new FailureMechanismSection(10.001,20.0),
                new FailureMechanismSection(20.0,assessmentSectionLength)
            });
            var list2 = new FailureMechanismSectionList("FM2", new[]
            {
                new FailureMechanismSection(0.0,5.0),
                new FailureMechanismSection(5.0,25.0),
                new FailureMechanismSection(25.0,assessmentSectionLength - 1.0)
            });

            try
            {
                var commonSections =
                    assembler.FindGreatestCommonDenominatorSectionsWbi1C1(new[] { list1, list2, },
                        assessmentSectionLength);
            }
            catch (AssemblyException exception)
            {
                Assert.IsNotNull(exception.Errors);
                Assert.AreEqual(1, exception.Errors.Count());
                Assert.AreEqual(EAssemblyErrors.FmSectionLengthInvalid, exception.Errors.First().ErrorCode);
                Assert.Pass();
            }

            Assert.Fail("Expected exception did not occur");
        }
    }
}