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

                List<FmSectionWithCategory> sectionResults = fmResult.Results;
                List<FmSectionWithCategory> expectedSectionResults = expectedFailureMechanismResults[i].Results;

                Assert.AreEqual(expectedSectionResults.Count, sectionResults.Count);
                for (var k = 0; k < expectedSectionResults.Count; k++)
                {
                    var expectedResult = expectedSectionResults[k];
                    var sectionResult = sectionResults[k];
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
    }
}