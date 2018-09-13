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
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assembly.Kernel.Exceptions;
using Assembly.Kernel.Implementations;
using Assembly.Kernel.Interfaces;
using Assembly.Kernel.Model;
using Assembly.Kernel.Model.CategoryLimits;
using NUnit.Framework;

// ReSharper disable UnusedMember.Local

namespace Assembly.Kernel.Tests.Implementations
{
    [TestFixture]
    public class AssessmentGradeAssemblerTests
    {
        [SetUp]
        public void Init()
        {
            assembler = new AssessmentGradeAssembler();
        }

        private IAssessmentGradeAssembler assembler;
        private readonly AssessmentSection assessmentSection = new AssessmentSection(10000, 1.0 / 1000.0, 1.0 / 300.0);
        private readonly CategoryLimitsCalculator categoriesCalculator = new CategoryLimitsCalculator();

        public enum EAssemblyType
        {
            Full,
            Partial
        }

        private sealed class AssessMentGradeAssemblerTestData
        {
            public static IEnumerable Wbi2C1
            {
                get
                {
                    yield return new TestCaseData(EFailureMechanismCategory.Nvt, EFailureMechanismCategory.Nvt).Returns(EAssessmentGrade.Nvt);
                    yield return new TestCaseData(EFailureMechanismCategory.It, EFailureMechanismCategory.It).Returns(EAssessmentGrade.APlus);
                    yield return new TestCaseData(EFailureMechanismCategory.IIt, EFailureMechanismCategory.IIt).Returns(EAssessmentGrade.A);
                    yield return new TestCaseData(EFailureMechanismCategory.IIIt, EFailureMechanismCategory.IIIt).Returns(EAssessmentGrade.B);
                    yield return new TestCaseData(EFailureMechanismCategory.IVt, EFailureMechanismCategory.IVt).Returns(EAssessmentGrade.C);
                    yield return new TestCaseData(EFailureMechanismCategory.Vt, EFailureMechanismCategory.Vt).Returns(EAssessmentGrade.C);
                    yield return new TestCaseData(EFailureMechanismCategory.VIt, EFailureMechanismCategory.VIt).Returns(EAssessmentGrade.D);
                    yield return new TestCaseData(EFailureMechanismCategory.VIIt, EFailureMechanismCategory.VIIt).Returns(EAssessmentGrade.Ngo);
                    yield return new TestCaseData(EFailureMechanismCategory.Gr, EFailureMechanismCategory.Gr).Returns(EAssessmentGrade.Gr);
                    yield return new TestCaseData(EFailureMechanismCategory.It, EFailureMechanismCategory.IIt).Returns(EAssessmentGrade.A);
                    yield return new TestCaseData(EFailureMechanismCategory.IIt, EFailureMechanismCategory.It).Returns(EAssessmentGrade.A);
                    yield return new TestCaseData(EFailureMechanismCategory.IIt, EFailureMechanismCategory.Gr).Returns(EAssessmentGrade.Ngo);
                    yield return new TestCaseData(EFailureMechanismCategory.Gr, EFailureMechanismCategory.IIt).Returns(EAssessmentGrade.Ngo);
                    yield return new TestCaseData(EFailureMechanismCategory.Nvt, EFailureMechanismCategory.IIt).Returns(EAssessmentGrade.A);
                    yield return new TestCaseData(EFailureMechanismCategory.IIIt, EFailureMechanismCategory.IIt).Returns(EAssessmentGrade.B);
                    yield return new TestCaseData(EFailureMechanismCategory.IVt, EFailureMechanismCategory.IIt).Returns(EAssessmentGrade.C);
                    yield return new TestCaseData(EFailureMechanismCategory.IIt, EFailureMechanismCategory.IVt).Returns(EAssessmentGrade.C);
                    yield return new TestCaseData(EFailureMechanismCategory.Vt, EFailureMechanismCategory.IIt).Returns(EAssessmentGrade.C);
                    yield return new TestCaseData(EFailureMechanismCategory.VIt, EFailureMechanismCategory.IIt).Returns(EAssessmentGrade.D);
                    yield return new TestCaseData(EFailureMechanismCategory.VIIt, EFailureMechanismCategory.Gr).Returns(EAssessmentGrade.Ngo);
                }
            }

            public static IEnumerable Wbi2A1Categories
            {
                get
                {
                    yield return new TestCaseData(new[]
                        {
                            EFailureMechanismCategory.It,
                            EFailureMechanismCategory.IIt,
                            EFailureMechanismCategory.IIIt,
                            EFailureMechanismCategory.IVt,
                            EFailureMechanismCategory.Vt,
                            EFailureMechanismCategory.VIt,
                            EFailureMechanismCategory.VIIt
                        }, EAssemblyType.Full)
                        .Returns(EFailureMechanismCategory.VIIt);

                    yield return new TestCaseData(new[]
                        {
                            EFailureMechanismCategory.It,
                            EFailureMechanismCategory.IIt,
                            EFailureMechanismCategory.IIIt,
                            EFailureMechanismCategory.IVt,
                            EFailureMechanismCategory.Vt,
                            EFailureMechanismCategory.VIt
                        }, EAssemblyType.Full)
                        .Returns(EFailureMechanismCategory.VIt);

                    yield return new TestCaseData(new[]
                        {
                            EFailureMechanismCategory.It,
                            EFailureMechanismCategory.IIt,
                            EFailureMechanismCategory.IIIt,
                            EFailureMechanismCategory.IVt,
                            EFailureMechanismCategory.Vt
                        }, EAssemblyType.Full)
                        .Returns(EFailureMechanismCategory.Vt);

                    yield return new TestCaseData(new[]
                        {
                            EFailureMechanismCategory.It,
                            EFailureMechanismCategory.IIt,
                            EFailureMechanismCategory.IIIt,
                            EFailureMechanismCategory.IVt
                        }, EAssemblyType.Full)
                        .Returns(EFailureMechanismCategory.IVt);

                    yield return new TestCaseData(new[]
                        {
                            EFailureMechanismCategory.It,
                            EFailureMechanismCategory.IIt,
                            EFailureMechanismCategory.IIIt
                        }, EAssemblyType.Full)
                        .Returns(EFailureMechanismCategory.IIIt);

                    yield return new TestCaseData(new[]
                        {
                            EFailureMechanismCategory.It,
                            EFailureMechanismCategory.IIt
                        }, EAssemblyType.Full)
                        .Returns(EFailureMechanismCategory.IIt);

                    yield return new TestCaseData(new[]
                        {
                            EFailureMechanismCategory.It
                        }, EAssemblyType.Full)
                        .Returns(EFailureMechanismCategory.It);

                    yield return new TestCaseData(new[]
                        {
                            EFailureMechanismCategory.VIIt,
                            EFailureMechanismCategory.Gr,
                            EFailureMechanismCategory.It,
                            EFailureMechanismCategory.IIt,
                            EFailureMechanismCategory.IIIt,
                            EFailureMechanismCategory.IVt,
                            EFailureMechanismCategory.Vt
                        }, EAssemblyType.Full)
                        .Returns(EFailureMechanismCategory.VIIt);

                    yield return new TestCaseData(new[]
                        {
                            EFailureMechanismCategory.VIIt,
                            EFailureMechanismCategory.It,
                            EFailureMechanismCategory.IIt,
                            EFailureMechanismCategory.IIIt,
                            EFailureMechanismCategory.IVt,
                            EFailureMechanismCategory.Vt
                        }, EAssemblyType.Full)
                        .Returns(EFailureMechanismCategory.VIIt);

                    yield return new TestCaseData(new[]
                        {
                            EFailureMechanismCategory.Gr,
                            EFailureMechanismCategory.Gr
                        }, EAssemblyType.Full)
                        .Returns(EFailureMechanismCategory.Gr);

                    yield return new TestCaseData(new[]
                        {
                            EFailureMechanismCategory.Gr,
                            EFailureMechanismCategory.VIIt
                        }, EAssemblyType.Full)
                        .Returns(EFailureMechanismCategory.VIIt);

                    yield return new TestCaseData(new[]
                        {
                            EFailureMechanismCategory.Nvt,
                            EFailureMechanismCategory.Nvt,
                            EFailureMechanismCategory.Nvt,
                            EFailureMechanismCategory.VIIt
                        }, EAssemblyType.Partial)
                        .Returns(EFailureMechanismCategory.Nvt);

                    yield return new TestCaseData(new[]
                        {
                            EFailureMechanismCategory.Nvt,
                            EFailureMechanismCategory.Nvt,
                            EFailureMechanismCategory.Nvt,
                            EFailureMechanismCategory.VIIt
                        }, EAssemblyType.Full)
                        .Returns(EFailureMechanismCategory.VIIt);

                    yield return new TestCaseData(new[]
                        {
                            EFailureMechanismCategory.Nvt,
                            EFailureMechanismCategory.Nvt,
                            EFailureMechanismCategory.Gr,
                            EFailureMechanismCategory.VIIt
                        }, EAssemblyType.Partial)
                        .Returns(EFailureMechanismCategory.Nvt);
                }
            }

            public static IEnumerable Wbi2B1
            {
                get
                {
                    yield return new TestCaseData(new[]
                        {
                            0.0,
                            0.1
                        },
                        EAssemblyType.Full,
                        0.1);

                    yield return new TestCaseData(new[]
                        {
                            0.0005,
                            0.00005
                        },
                        EAssemblyType.Full,
                        0.000549975);
                }
            }
        }

        [Test, TestCaseSource(
             typeof(AssessMentGradeAssemblerTestData),
             nameof(AssessMentGradeAssemblerTestData.Wbi2A1Categories))]
        public EFailureMechanismCategory Wbi2A1(IEnumerable<EFailureMechanismCategory> failureMechanismCategories,
            EAssemblyType assemblyType)
        {
            return assembler.AssembleAssessmentSectionWbi2A1(
                failureMechanismCategories.Select(category => new FailureMechanismAssemblyResult(category, double.NaN)),
                assemblyType == EAssemblyType.Partial);
        }

        [Test]
        public void Wbi2A1EmptyList()
        {
            try
            {
                assembler.AssembleAssessmentSectionWbi2A1(new List<FailureMechanismAssemblyResult>(), false);
            }
            catch (AssemblyException e)
            {
                Assert.NotNull(e.Errors);
                var message = e.Errors.FirstOrDefault();
                Assert.NotNull(message);
                Assert.AreEqual(EAssemblyErrors.FailureMechanismAssemblerInputInvalid, message.ErrorCode);
            }
        }

        [Test]
        public void Wbi2A1NullTest()
        {
            try
            {
                assembler.AssembleAssessmentSectionWbi2A1(null, false);
            }
            catch (AssemblyException e)
            {
                Assert.NotNull(e.Errors);
                var message = e.Errors.FirstOrDefault();
                Assert.NotNull(message);
                Assert.AreEqual(EAssemblyErrors.ValueMayNotBeNull, message.ErrorCode);
            }
        }

        [Test]
        public void Wbi2B1CategoriesNull()
        {
            try
            {
                var result = assembler.AssembleAssessmentSectionWbi2B1(
                    new[]
                    {
                        new FailureMechanismAssemblyResult(EFailureMechanismCategory.It, double.NaN),
                        new FailureMechanismAssemblyResult(EFailureMechanismCategory.It, 0.00003)
                    },
                    null,
                    false);
            }
            catch (AssemblyException e)
            {
                Assert.NotNull(e.Errors);
                var message = e.Errors.FirstOrDefault();
                Assert.NotNull(message);
                Assert.AreEqual(EAssemblyErrors.ValueMayNotBeNull, message.ErrorCode);
                Assert.Pass();
            }

            Assert.Fail("Expected exception was not thrown");
        }

        [Test]
        public void Wbi2B1EmptyList()
        {
            try
            {
                assembler.AssembleAssessmentSectionWbi2B1(new List<FailureMechanismAssemblyResult>(),
                    new CategoriesList<FailureMechanismCategory>(new[]
                        {new FailureMechanismCategory(EFailureMechanismCategory.It, 0.0, 1.0)}), false);
            }
            catch (AssemblyException e)
            {
                Assert.NotNull(e.Errors);
                var message = e.Errors.FirstOrDefault();
                Assert.NotNull(message);
                Assert.AreEqual(EAssemblyErrors.FailureMechanismAssemblerInputInvalid, message.ErrorCode);
            }
        }

        [Test, TestCaseSource(
             typeof(AssessMentGradeAssemblerTestData),
             nameof(AssessMentGradeAssemblerTestData.Wbi2B1))]
        public void Wbi2B1FailureProbabilityTests(IEnumerable<double> failureProbabilities,
            EAssemblyType assemblyType, double expectedResult)
        {
            var result = assembler.AssembleAssessmentSectionWbi2B1(failureProbabilities.Select(failureProbability =>
                    new FailureMechanismAssemblyResult(EFailureMechanismCategory.It, failureProbability)),
                categoriesCalculator.CalculateFailureMechanismCategoryLimitsWbi11(assessmentSection,
                    new FailureMechanism(1.0, 0.7)),
                assemblyType == EAssemblyType.Partial);

            Assert.NotNull(result.FailureProbability);
            Assert.AreEqual(result.FailureProbability, expectedResult, 10);
        }

        [Test]
        public void Wbi2B1FailureProbNull()
        {
            try
            {
                var result = assembler.AssembleAssessmentSectionWbi2B1(
                    new[]
                    {
                        new FailureMechanismAssemblyResult(EFailureMechanismCategory.It, double.NaN),
                        new FailureMechanismAssemblyResult(EFailureMechanismCategory.It, 0.00003)
                    },
                    categoriesCalculator.CalculateFailureMechanismCategoryLimitsWbi11(assessmentSection,
                        new FailureMechanism(1.0, 0.7)),
                    false);
            }
            catch (AssemblyException e)
            {
                Assert.NotNull(e.Errors);
                var message = e.Errors.FirstOrDefault();
                Assert.NotNull(message);
                Assert.AreEqual(EAssemblyErrors.ValueMayNotBeNull, message.ErrorCode);
                Assert.Pass();
            }

            Assert.Fail("Expected exception was not thrown");
        }

        [Test]
        public void Wbi2B1NoResultOneFailureMechanism()
        {
            var result = assembler.AssembleAssessmentSectionWbi2B1(
                new[]
                {
                    new FailureMechanismAssemblyResult(EFailureMechanismCategory.VIIt, double.NaN),
                    new FailureMechanismAssemblyResult(EFailureMechanismCategory.Gr, double.NaN),
                    new FailureMechanismAssemblyResult(EFailureMechanismCategory.It, 0.00003),
                    new FailureMechanismAssemblyResult(EFailureMechanismCategory.It, 0.00003)
                },
                categoriesCalculator.CalculateFailureMechanismCategoryLimitsWbi11(assessmentSection,
                    new FailureMechanism(1.0, 0.7)),
                false);

            Assert.IsNaN(result.FailureProbability);
            Assert.AreEqual(EFailureMechanismCategory.VIIt, result.Category);
        }

        [Test]
        public void Wbi2B1NoResultAtAll()
        {
            var result = assembler.AssembleAssessmentSectionWbi2B1(
                new[]
                {
                    new FailureMechanismAssemblyResult(EFailureMechanismCategory.Gr, double.NaN),
                    new FailureMechanismAssemblyResult(EFailureMechanismCategory.Gr, double.NaN)
                },
                categoriesCalculator.CalculateFailureMechanismCategoryLimitsWbi11(assessmentSection,
                    new FailureMechanism(1.0, 0.7)),
                false);

            Assert.IsNaN(result.FailureProbability);
            Assert.AreEqual(EFailureMechanismCategory.Gr, result.Category);
        }

        [Test]
        public void Wbi2B1NoResultYet()
        {
            var result = assembler.AssembleAssessmentSectionWbi2B1(
                new[]
                {
                    new FailureMechanismAssemblyResult(EFailureMechanismCategory.VIIt, double.NaN),
                    new FailureMechanismAssemblyResult(EFailureMechanismCategory.It, 0.00003),
                    new FailureMechanismAssemblyResult(EFailureMechanismCategory.It, 0.00003)
                },
                categoriesCalculator.CalculateFailureMechanismCategoryLimitsWbi11(assessmentSection,
                    new FailureMechanism(1.0, 0.7)),
                false);

            Assert.IsNaN(result.FailureProbability);
            Assert.AreEqual(EFailureMechanismCategory.VIIt, result.Category);
        }

        [Test]
        public void Wbi2B1NullTest()
        {
            try
            {
                assembler.AssembleAssessmentSectionWbi2B1(null, null, false);
            }
            catch (AssemblyException e)
            {
                Assert.NotNull(e.Errors);
                var message = e.Errors.FirstOrDefault();
                Assert.NotNull(message);
                Assert.AreEqual(EAssemblyErrors.ValueMayNotBeNull, message.ErrorCode);
            }
        }

        [Test]
        public void Wbi2B1Nvt()
        {
            var result = assembler.AssembleAssessmentSectionWbi2B1(
                new[]
                {
                    new FailureMechanismAssemblyResult(EFailureMechanismCategory.Nvt, double.NaN),
                    new FailureMechanismAssemblyResult(EFailureMechanismCategory.Nvt, double.NaN)
                },
                categoriesCalculator.CalculateFailureMechanismCategoryLimitsWbi11(assessmentSection,
                    new FailureMechanism(1.0, 0.7)),
                false);

            Assert.AreEqual(0.0, result.FailureProbability);
            Assert.AreEqual(EFailureMechanismCategory.Nvt, result.Category);
        }

        [Test]
        public void Wbi2B1PartialAssembly()
        {
            var sectionFailureProbability = 0.00003;
            var categoryLimits = categoriesCalculator.CalculateFailureMechanismCategoryLimitsWbi11(assessmentSection,
                new FailureMechanism(1.0, 0.7));
            var result = assembler.AssembleAssessmentSectionWbi2B1(
                new[]
                {
                    new FailureMechanismAssemblyResult(EFailureMechanismCategory.VIIt, double.NaN),
                    new FailureMechanismAssemblyResult(EFailureMechanismCategory.It, sectionFailureProbability),
                    new FailureMechanismAssemblyResult(EFailureMechanismCategory.It, sectionFailureProbability)
                },
                categoryLimits,
                true);

            var expectedProbability = 1-Math.Pow(1-sectionFailureProbability,2);
            Assert.AreEqual(expectedProbability, result.FailureProbability);
            Assert.AreEqual(categoryLimits.Categories.First(c => expectedProbability <= c.UpperLimit).Category, result.Category);
        }

        [Test]
        public void Wbi2C1NullException()
        {
            try
            {
                assembler.AssembleAssessmentSectionWbi2C1(EFailureMechanismCategory.It, null);
            }
            catch (AssemblyException e)
            {
                Assert.NotNull(e.Errors);
                var message = e.Errors.FirstOrDefault();
                Assert.NotNull(message);
                Assert.AreEqual(EAssemblyErrors.ValueMayNotBeNull, message.ErrorCode);
            }
        }

        [Test, TestCaseSource(
             typeof(AssessMentGradeAssemblerTestData),
             nameof(AssessMentGradeAssemblerTestData.Wbi2C1))]
        public EAssessmentGrade Wbi2C1Tests(EFailureMechanismCategory noFailureProb, EFailureMechanismCategory withFailureProbabilityCategory)
        {
            var withFailureProb = new FailureMechanismAssemblyResult(withFailureProbabilityCategory,double.NaN);

            var result = assembler.AssembleAssessmentSectionWbi2C1(noFailureProb, withFailureProb);

            Assert.NotNull(result);

            return result;
        }
    }
}