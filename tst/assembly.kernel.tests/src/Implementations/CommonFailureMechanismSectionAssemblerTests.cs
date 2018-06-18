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

namespace Assembly.Kernel.Tests.Implementations {
    [TestFixture]
    public class CommonFailureMechanismSectionAssemblerTests {
        private readonly ICommonFailureMechanismSectionAssembler assembler = new CommonFailureMechanismSectionAssembler();

        private static FmSectionWithDirectCategory Direct(double sectionStart, double sectionEnd, string category) {
            Enum.TryParse(category, out EFmSectionCategory parsedCategory);
            return new FmSectionWithDirectCategory(sectionStart, sectionEnd, parsedCategory);
        }

        private static FmSectionWithIndirectCategory Indirect(double sectionStart, double sectionEnd, string category) {
            Enum.TryParse(category, out EIndirectAssessmentResult parsedCategory);
            return new FmSectionWithIndirectCategory(sectionStart, sectionEnd, parsedCategory);
        }

        [Test]
        public void AssembleCommonFailureMechanismSectionsTest() {
            var failureMechanismSections = new List<FailureMechanismSectionList> {
                new FailureMechanismSectionList("TEST", new [] {
                    Direct(0,10,"Iv"),
                    Direct(10,20,"Iv"),
                    Direct(20,30,"Vv"),
                    Direct(30,40,"Iv"),
                    Direct(40,50,"Iv"),
                }),
                new FailureMechanismSectionList("TEST1", new [] {
                    Direct(0,5,"IIv"),
                    Direct(5,10,"IIIv"),
                    Direct(10,40,"IVv"),
                    Direct(40,45,"Vv"),
                    Direct(45,50,"Iv"),
                })
            };

            var expectedFailureMechanismResults = new List<FailureMechanismSectionList> {
                new FailureMechanismSectionList ("TEST", new[] {
                    Direct( 0.0,  5.0, "Iv"),
                    Direct( 5.0, 10.0, "Iv"),
                    Direct(10.0, 20.0, "Iv"),
                    Direct(20.0, 30.0, "Vv"),
                    Direct(30.0, 40.0, "Iv"),
                    Direct(40.0, 45.0, "Iv"),
                    Direct(45.0, 50.0, "Iv"),
                }),
                new FailureMechanismSectionList ("TEST1", new[] {
                    Direct( 0.0,  5.0, "IIv"),
                    Direct( 5.0, 10.0, "IIIv"),
                    Direct(10.0, 20.0, "IVv"),
                    Direct(20.0, 30.0, "IVv"),
                    Direct(30.0, 40.0, "IVv"),
                    Direct(40.0, 45.0, "Vv"),
                    Direct(45.0, 50.0, "Iv"),
                }),
            };

            var expectedCombinedResult = new List<FmSectionWithDirectCategory> {
                Direct( 0.0,  5.0, "IIv"),
                Direct( 5.0, 10.0, "IIIv"),
                Direct(10.0, 20.0, "IVv"),
                Direct(20.0, 30.0, "Vv"),
                Direct(30.0, 40.0, "IVv"),
                Direct(40.0, 45.0, "Vv"),
                Direct(45.0, 50.0, "Iv"),
            };

            TestCombinedFailureMechanismSectionAssembler(
                failureMechanismSections, 
                expectedFailureMechanismResults, 
                expectedCombinedResult,
                false);
        }

        [Test]
        public void IndirectFailureMechanismTest() {
            var failureMechanismSections = new List<FailureMechanismSectionList> {
                new FailureMechanismSectionList("TEST", new [] {
                    Direct(0,10,"Iv"),
                    Direct(10,20,"VIIv"),
                    Direct(20,30,"Vv"),
                    Direct(30,40,"Iv"),
                    Direct(40,50,"Iv"),
                }),
                new FailureMechanismSectionList("TEST1", new [] {
                    Direct(0,5,"IIv"),
                    Direct(5,10,"IIIv"),
                    Direct(10,40,"IVv"),
                    Direct(40,45,"Vv"),
                    Direct(45,50,"Iv"),
                }),

                new FailureMechanismSectionList("TEST2", new [] {
                    Indirect(0,7,"FvGt"),
                    Indirect(7,8,"Ngo"),
                    Indirect(8,10,"FvGt"),
                    Indirect(10,50,"FvTom"),
                })
            };

            var expectedFailureMechanismResults = new List<FailureMechanismSectionList> {
                new FailureMechanismSectionList ("TEST", new[] {
                    Direct( 0.0,  5.0, "Iv"),
                    Direct( 5.0, 7.0, "Iv"),
                    Direct( 7.0, 8.0, "Iv"),
                    Direct( 8.0, 10.0, "Iv"),
                    Direct(10.0, 20.0, "VIIv"),
                    Direct(20.0, 30.0, "Vv"),
                    Direct(30.0, 40.0, "Iv"),
                    Direct(40.0, 45.0, "Iv"),
                    Direct(45.0, 50.0, "Iv"),
                }),
                new FailureMechanismSectionList ("TEST1", new[] {
                    Direct( 0.0,  5.0, "IIv"),
                    Direct( 5.0, 7.0, "IIIv"),
                    Direct( 7.0, 8.0, "IIIv"),
                    Direct( 8.0, 10.0, "IIIv"),
                    Direct(10.0, 20.0, "IVv"),
                    Direct(20.0, 30.0, "IVv"),
                    Direct(30.0, 40.0, "IVv"),
                    Direct(40.0, 45.0, "Vv"),
                    Direct(45.0, 50.0, "Iv"),
                }),
                new FailureMechanismSectionList ("TEST2", new[] {
                    Indirect( 0.0,  5.0, "FvGt"),
                    Indirect( 5.0, 7.0, "FvGt"),
                    Indirect( 7.0, 8.0, "Ngo"),
                    Indirect( 8.0, 10.0, "FvGt"),
                    Indirect(10.0, 20.0, "FvTom"),
                    Indirect(20.0, 30.0, "FvTom"),
                    Indirect(30.0, 40.0, "FvTom"),
                    Indirect(40.0, 45.0, "FvTom"),
                    Indirect(45.0, 50.0, "FvTom"),
                }),
            };

            var expectedCombinedResult = new List<FmSectionWithDirectCategory> {
                Direct( 0.0,  5.0, "IIv"),
                Direct( 5.0, 7.0, "IIIv"),
                Direct( 7.0, 8.0, "IIIv"),
                Direct( 8.0, 10.0, "IIIv"),
                Direct(10.0, 20.0, "VIIv"),
                Direct(20.0, 30.0, "Vv"),
                Direct(30.0, 40.0, "IVv"),
                Direct(40.0, 45.0, "Vv"),
                Direct(45.0, 50.0, "Iv"),
            };

            TestCombinedFailureMechanismSectionAssembler(
                failureMechanismSections,
                expectedFailureMechanismResults,
                expectedCombinedResult,
                false);
        }

        [Test]
        public void NoResultFailureMechanismTest() {
            var failureMechanismSections = new List<FailureMechanismSectionList> {
                new FailureMechanismSectionList("TEST", new [] {
                    Direct(0,10,"Iv"),
                    Direct(10,20,"Iv"),
                    Direct(20,30,"VIIv"),
                    Direct(30,40,"Iv"),
                    Direct(40,50,"Gr"),
                }),
                new FailureMechanismSectionList("TEST1", new [] {
                    Direct(0,5,"IIv"),
                    Direct(5,10,"IIIv"),
                    Direct(10,40,"IVv"),
                    Direct(40,45,"Vv"),
                    Direct(45,50,"Iv"),
                }),
                new FailureMechanismSectionList("TEST2", new [] {
                    Indirect(0,20,"Gr"),
                    Indirect(20,50,"FvGt"),
                })
            };

            var expectedFailureMechanismResults = new List<FailureMechanismSectionList> {
                new FailureMechanismSectionList ("TEST", new[] {
                    Direct( 0.0,  5.0, "Iv"),
                    Direct( 5.0, 10.0, "Iv"),
                    Direct(10.0, 20.0, "Iv"),
                    Direct(20.0, 30.0, "VIIv"),
                    Direct(30.0, 40.0, "Iv"),
                    Direct(40.0, 45.0, "Gr"),
                    Direct(45.0, 50.0, "Gr"),
                }),
                new FailureMechanismSectionList ("TEST1", new[] {
                    Direct( 0.0,  5.0, "IIv"),
                    Direct( 5.0, 10.0, "IIIv"),
                    Direct(10.0, 20.0, "IVv"),
                    Direct(20.0, 30.0, "IVv"),
                    Direct(30.0, 40.0, "IVv"),
                    Direct(40.0, 45.0, "Vv"),
                    Direct(45.0, 50.0, "Iv"),
                }),

                new FailureMechanismSectionList ("TEST2", new[] {
                    Indirect( 0.0,  5.0, "Gr"),
                    Indirect( 5.0, 10.0, "Gr"),
                    Indirect(10.0, 20.0, "Gr"),
                    Indirect(20.0, 30.0, "FvGt"),
                    Indirect(30.0, 40.0, "FvGt"),
                    Indirect(40.0, 45.0, "FvGt"),
                    Indirect(45.0, 50.0, "FvGt"),
                }),
            };

            var expectedCombinedResult = new List<FmSectionWithDirectCategory> {
                Direct( 0.0,  5.0, "IIv"),
                Direct( 5.0, 10.0, "IIIv"),
                Direct(10.0, 20.0, "IVv"),
                Direct(20.0, 30.0, "VIIv"),
                Direct(30.0, 40.0, "IVv"),
                Direct(40.0, 45.0, "Gr"),
                Direct(45.0, 50.0, "Gr"),
            };

            TestCombinedFailureMechanismSectionAssembler(
                failureMechanismSections,
                expectedFailureMechanismResults,
                expectedCombinedResult,
                false);
        }

        [Test]
        public void PartialFailureMechanismTest() {
            var failureMechanismSections = new List<FailureMechanismSectionList> {
                new FailureMechanismSectionList("TEST", new [] {
                    Direct(0,10,"Iv"),
                    Direct(10,20,"Iv"),
                    Direct(20,30,"VIIv"),
                    Direct(30,40,"Iv"),
                    Direct(40,50,"IIv"),
                }),
                new FailureMechanismSectionList("TEST1", new [] {
                    Direct(0,5,"IIv"),
                    Direct(5,10,"IIIv"),
                    Direct(10,40,"IVv"),
                    Direct(40,45,"Vv"),
                    Direct(45,50,"Iv"),
                })
            };

            var expectedFailureMechanismResults = new List<FailureMechanismSectionList> {
                new FailureMechanismSectionList ("TEST", new[] {
                    Direct( 0.0,  5.0, "Iv"),
                    Direct( 5.0, 10.0, "Iv"),
                    Direct(10.0, 20.0, "Iv"),
                    Direct(20.0, 30.0, "VIIv"),
                    Direct(30.0, 40.0, "Iv"),
                    Direct(40.0, 45.0, "IIv"),
                    Direct(45.0, 50.0, "IIv"),
                }),
                new FailureMechanismSectionList ("TEST1", new[] {
                    Direct( 0.0,  5.0, "IIv"),
                    Direct( 5.0, 10.0, "IIIv"),
                    Direct(10.0, 20.0, "IVv"),
                    Direct(20.0, 30.0, "IVv"),
                    Direct(30.0, 40.0, "IVv"),
                    Direct(40.0, 45.0, "Vv"),
                    Direct(45.0, 50.0, "Iv"),
                }),
            };

            var expectedCombinedResult = new List<FmSectionWithDirectCategory> {
                Direct( 0.0,  5.0, "IIv"),
                Direct( 5.0, 10.0, "IIIv"),
                Direct(10.0, 20.0, "IVv"),
                Direct(20.0, 30.0, "IVv"),
                Direct(30.0, 40.0, "IVv"),
                Direct(40.0, 45.0, "Vv"),
                Direct(45.0, 50.0, "IIv"),
            };

            TestCombinedFailureMechanismSectionAssembler(
                failureMechanismSections,
                expectedFailureMechanismResults,
                expectedCombinedResult,
                true);
        }

        [Test]
        public void NotApplicablePartialFailureMechanismTest() {
            var failureMechanismSections = new List<FailureMechanismSectionList> {
                new FailureMechanismSectionList("TEST", new [] {
                    Direct(0,10,"Iv"),
                    Direct(10,20,"Iv"),
                    Direct(20,30,"NotApplicable"),
                    Direct(30,40,"Iv"),
                    Direct(40,50,"IIv"),
                }),
                new FailureMechanismSectionList("TEST1", new [] {
                    Direct(0,5,"IIv"),
                    Direct(5,10,"IIIv"),
                    Direct(10,40,"NotApplicable"),
                    Direct(40,45,"Vv"),
                    Direct(45,50,"Iv"),
                })
            };

            var expectedFailureMechanismResults = new List<FailureMechanismSectionList> {
                new FailureMechanismSectionList ("TEST", new[] {
                    Direct( 0.0,  5.0, "Iv"),
                    Direct( 5.0, 10.0, "Iv"),
                    Direct(10.0, 20.0, "Iv"),
                    Direct(20.0, 30.0, "NotApplicable"),
                    Direct(30.0, 40.0, "Iv"),
                    Direct(40.0, 45.0, "IIv"),
                    Direct(45.0, 50.0, "IIv"),
                }),
                new FailureMechanismSectionList ("TEST1", new[] {
                    Direct( 0.0,  5.0, "IIv"),
                    Direct( 5.0, 10.0, "IIIv"),
                    Direct(10.0, 20.0, "NotApplicable"),
                    Direct(20.0, 30.0, "NotApplicable"),
                    Direct(30.0, 40.0, "NotApplicable"),
                    Direct(40.0, 45.0, "Vv"),
                    Direct(45.0, 50.0, "Iv"),
                }),
            };

            var expectedCombinedResult = new List<FmSectionWithDirectCategory> {
                Direct( 0.0,  5.0, "IIv"),
                Direct( 5.0, 10.0, "IIIv"),
                Direct(10.0, 20.0, "Iv"),
                Direct(20.0, 30.0, "NotApplicable"),
                Direct(30.0, 40.0, "Iv"),
                Direct(40.0, 45.0, "Vv"),
                Direct(45.0, 50.0, "IIv"),
            };

            TestCombinedFailureMechanismSectionAssembler(
                failureMechanismSections,
                expectedFailureMechanismResults,
                expectedCombinedResult,
                true);
        }

        [Test]
        public void NotApplicableFailureMechanismTest() {
            var failureMechanismSections = new List<FailureMechanismSectionList> {
                new FailureMechanismSectionList("TEST", new [] {
                    Direct(0,10,"Iv"),
                    Direct(10,20,"Iv"),
                    Direct(20,30,"NotApplicable"),
                    Direct(30,40,"Iv"),
                    Direct(40,50,"IIv"),
                }),
                new FailureMechanismSectionList("TEST1", new [] {
                    Direct(0,5,"IIv"),
                    Direct(5,10,"NotApplicable"),
                    Direct(10,40,"NotApplicable"),
                    Direct(40,45,"Vv"),
                    Direct(45,50,"Iv"),
                })
            };

            var expectedFailureMechanismResults = new List<FailureMechanismSectionList> {
                new FailureMechanismSectionList ("TEST", new[] {
                    Direct( 0.0,  5.0, "Iv"),
                    Direct( 5.0, 10.0, "Iv"),
                    Direct(10.0, 20.0, "Iv"),
                    Direct(20.0, 30.0, "NotApplicable"),
                    Direct(30.0, 40.0, "Iv"),
                    Direct(40.0, 45.0, "IIv"),
                    Direct(45.0, 50.0, "IIv"),
                }),
                new FailureMechanismSectionList ("TEST1", new[] {
                    Direct( 0.0,  5.0, "IIv"),
                    Direct( 5.0, 10.0, "NotApplicable"),
                    Direct(10.0, 20.0, "NotApplicable"),
                    Direct(20.0, 30.0, "NotApplicable"),
                    Direct(30.0, 40.0, "NotApplicable"),
                    Direct(40.0, 45.0, "Vv"),
                    Direct(45.0, 50.0, "Iv"),
                }),
            };

            var expectedCombinedResult = new List<FmSectionWithDirectCategory> {
                Direct( 0.0,  5.0, "IIv"),
                Direct( 5.0, 10.0, "Iv"),
                Direct(10.0, 20.0, "Iv"),
                Direct(20.0, 30.0, "NotApplicable"),
                Direct(30.0, 40.0, "Iv"),
                Direct(40.0, 45.0, "Vv"),
                Direct(45.0, 50.0, "IIv"),
            };

            TestCombinedFailureMechanismSectionAssembler(
                failureMechanismSections,
                expectedFailureMechanismResults,
                expectedCombinedResult,
                true);
        }

        [Test]
        public void NotAllSectionsProvidedExceptionTest() {
            var failureMechanismSections = new List<FailureMechanismSectionList> {
                new FailureMechanismSectionList("TEST", new [] {
                    Direct(0,10,"Iv"),
                    Direct(10,20,"Iv"),
                    Direct(30,40,"Iv"),
                    Direct(20,30,"IIv"),
                    Direct(40,45,"IIv"),
                    // section starting at 45 to 50 is missing.
                })
            };

            try {
                assembler.AssembleCommonFailureMechanismSections(failureMechanismSections, 50.0, false);
            } catch (AssemblyException e) {
                Assert.NotNull(e.Errors);
                var message = e.Errors.FirstOrDefault();
                Assert.NotNull(message);
                Assert.AreEqual(EAssemblyErrors.FmSectionLengthInvalid, message.ErrorCode);
                Assert.Pass();
            }
            Assert.Fail("Expected exception was not thrown");
        }

        private void TestCombinedFailureMechanismSectionAssembler(
            IEnumerable<FailureMechanismSectionList> failureMechanismSections,
            IList<FailureMechanismSectionList> expectedFailureMechanismResults, 
            IList<FmSectionWithDirectCategory> expectedCombinedResult,
            bool partial) {
            var result = assembler.AssembleCommonFailureMechanismSections(failureMechanismSections, 50.0, partial);
            Assert.NotNull(result);

            List<FailureMechanismSectionList> resultPerFailureMechanism = result.ResultPerFailureMechanism.ToList();
            Assert.AreEqual(expectedFailureMechanismResults.Count, resultPerFailureMechanism.Count);

            for (var i = 0; i < expectedFailureMechanismResults.Count; i++) {
                var fmResult = resultPerFailureMechanism[i];

                Assert.AreEqual(expectedFailureMechanismResults[i].FailureMechanismId, fmResult.FailureMechanismId);

                List<FmSectionWithCategory> sectionResults = fmResult.Results;
                List<FmSectionWithCategory> expectedSectionResults = expectedFailureMechanismResults[i].Results;

                Assert.AreEqual(expectedSectionResults.Count, sectionResults.Count);
                for (var k = 0; k < expectedSectionResults.Count; k++) {
                    var expectedResult = expectedSectionResults[k];
                    var sectionResult = sectionResults[k];
                    Assert.NotNull(expectedResult);
                    Assert.NotNull(sectionResult);

                    Assert.AreEqual(expectedResult.SectionStart, sectionResult.SectionStart);
                    Assert.AreEqual(expectedResult.SectionEnd, sectionResult.SectionEnd);

                    if (sectionResult.Type == EAssembledAssessmentResultType.IndirectAssessment) {
                        
                        Assert.AreEqual(
                            ((FmSectionWithIndirectCategory)expectedResult).Category, 
                            ((FmSectionWithIndirectCategory)sectionResult).Category);
                    } else {
                        Assert.AreEqual(
                            ((FmSectionWithDirectCategory)expectedResult).Category,
                            ((FmSectionWithDirectCategory)sectionResult).Category);
                    }
                }
            }

            List<FmSectionWithDirectCategory> combinedResult = result.CombinedSectionResult.ToList();
            Assert.AreEqual(expectedCombinedResult.Count, combinedResult.Count);
            for (var i = 0; i < expectedCombinedResult.Count; i++) {
                Assert.AreEqual(expectedCombinedResult[i].SectionStart, combinedResult[i].SectionStart);
                Assert.AreEqual(expectedCombinedResult[i].SectionEnd, combinedResult[i].SectionEnd);
                Assert.AreEqual(expectedCombinedResult[i].Category, combinedResult[i].Category);
            }
        }
    }
}