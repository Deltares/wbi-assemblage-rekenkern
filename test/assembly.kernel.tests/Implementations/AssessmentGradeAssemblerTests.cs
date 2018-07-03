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

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assembly.Kernel.Exceptions;
using Assembly.Kernel.Implementations;
using Assembly.Kernel.Interfaces;
using Assembly.Kernel.Model;
using Assembly.Kernel.Model.CategoryLimits;
using Assembly.Kernel.Tests.Model.CategoryLimits;
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
                        .Returns(EAssessmentGrade.Ngo);

                    yield return new TestCaseData(new[]
                        {
                            EFailureMechanismCategory.It,
                            EFailureMechanismCategory.IIt,
                            EFailureMechanismCategory.IIIt,
                            EFailureMechanismCategory.IVt,
                            EFailureMechanismCategory.Vt,
                            EFailureMechanismCategory.VIt
                        }, EAssemblyType.Full)
                        .Returns(EAssessmentGrade.D);

                    yield return new TestCaseData(new[]
                        {
                            EFailureMechanismCategory.It,
                            EFailureMechanismCategory.IIt,
                            EFailureMechanismCategory.IIIt,
                            EFailureMechanismCategory.IVt,
                            EFailureMechanismCategory.Vt
                        }, EAssemblyType.Full)
                        .Returns(EAssessmentGrade.C);

                    yield return new TestCaseData(new[]
                        {
                            EFailureMechanismCategory.It,
                            EFailureMechanismCategory.IIt,
                            EFailureMechanismCategory.IIIt,
                            EFailureMechanismCategory.IVt
                        }, EAssemblyType.Full)
                        .Returns(EAssessmentGrade.C);

                    yield return new TestCaseData(new[]
                        {
                            EFailureMechanismCategory.It,
                            EFailureMechanismCategory.IIt,
                            EFailureMechanismCategory.IIIt
                        }, EAssemblyType.Full)
                        .Returns(EAssessmentGrade.B);

                    yield return new TestCaseData(new[]
                        {
                            EFailureMechanismCategory.It,
                            EFailureMechanismCategory.IIt
                        }, EAssemblyType.Full)
                        .Returns(EAssessmentGrade.A);

                    yield return new TestCaseData(new[]
                        {
                            EFailureMechanismCategory.It
                        }, EAssemblyType.Full)
                        .Returns(EAssessmentGrade.APlus);

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
                        .Returns(EAssessmentGrade.Gr);

                    yield return new TestCaseData(new[]
                        {
                            EFailureMechanismCategory.VIIt,
                            EFailureMechanismCategory.It,
                            EFailureMechanismCategory.IIt,
                            EFailureMechanismCategory.IIIt,
                            EFailureMechanismCategory.IVt,
                            EFailureMechanismCategory.Vt
                        }, EAssemblyType.Full)
                        .Returns(EAssessmentGrade.Ngo);

                    yield return new TestCaseData(new[]
                        {
                            EFailureMechanismCategory.Nvt,
                            EFailureMechanismCategory.Nvt,
                            EFailureMechanismCategory.Nvt,
                            EFailureMechanismCategory.VIIt
                        }, EAssemblyType.Partial)
                        .Returns(EAssessmentGrade.Nvt);
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
        public EAssessmentGrade Wbi2A1(IEnumerable<EFailureMechanismCategory> failureMechanismCategories,
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

        [Test, TestCaseSource(
             typeof(AssessMentGradeAssemblerTestData),
             nameof(AssessMentGradeAssemblerTestData.Wbi2B1))]
        public void Wbi2B1FailureProbabilityTests(IEnumerable<double> failureProbabilities,
            EAssemblyType assemblyType, double expectedResult)
        {
            var result = assembler.AssembleAssessmentSectionWbi2B1(failureProbabilities.Select(failureProbability =>
                    new FailureMechanismAssemblyResult(EFailureMechanismCategory.It, failureProbability)),
                categoriesCalculator.CalculateAssessmentSectionCategoryLimitsWbi21(assessmentSection),
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
                    categoriesCalculator.CalculateAssessmentSectionCategoryLimitsWbi21(assessmentSection),
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
                    new CategoriesList<AssessmentSectionCategory>(new[]
                        {new AssessmentSectionCategory(EAssessmentGrade.A, 0.0, 1.0)}), false);
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
        public void Wbi2B1NullTest()
        {
            try
            {
                assembler.AssembleAssessmentSectionWbi2B1(null,null, false);
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
        public void Wbi2B1NoResult()
        {
            var result = assembler.AssembleAssessmentSectionWbi2B1(
                new[]
                {
                    new FailureMechanismAssemblyResult(EFailureMechanismCategory.VIIt, double.NaN),
                    new FailureMechanismAssemblyResult(EFailureMechanismCategory.Gr, double.NaN),
                    new FailureMechanismAssemblyResult(EFailureMechanismCategory.It, 0.00003),
                    new FailureMechanismAssemblyResult(EFailureMechanismCategory.It, 0.00003)
                },
                categoriesCalculator.CalculateAssessmentSectionCategoryLimitsWbi21(assessmentSection),
                false);

            Assert.IsNaN(result.FailureProbability);
            Assert.AreEqual(EAssessmentGrade.Gr, result.Category);
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
                categoriesCalculator.CalculateAssessmentSectionCategoryLimitsWbi21(assessmentSection),
                false);

            Assert.IsNaN(result.FailureProbability);
            Assert.AreEqual(EAssessmentGrade.Ngo, result.Category);
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
                categoriesCalculator.CalculateAssessmentSectionCategoryLimitsWbi21(assessmentSection),
                false);

            Assert.IsNaN(result.FailureProbability);
            Assert.AreEqual(EAssessmentGrade.Nvt, result.Category);
        }

        [Test]
        public void Wbi2C1NullException()
        {
            var withFailureProb = new AssessmentSectionAssemblyResult(EAssessmentGrade.C, 0.000056);
            try
            {
                assembler.AssembleAssessmentSectionWbi2C1(null, withFailureProb);
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
        public void Wbi2C1NullException2()
        {
            var noFailureProb = new AssessmentSectionAssemblyResult(EAssessmentGrade.A);

            try
            {
                assembler.AssembleAssessmentSectionWbi2C1(noFailureProb, null);
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
        public void Wbi2C1NullException3()
        {
            try
            {
                assembler.AssembleAssessmentSectionWbi2C1(null, null);
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
        public void Wbi2C1ResultNvt()
        {
            var noFailureProb = new AssessmentSectionAssemblyResult(EAssessmentGrade.Nvt);
            var withFailureProb = new AssessmentSectionAssemblyResult(EAssessmentGrade.A, 0.00002);

            var result = assembler.AssembleAssessmentSectionWbi2C1(noFailureProb, withFailureProb);

            Assert.NotNull(result);
            Assert.AreEqual(EAssessmentGrade.A, result.Category);
            Assert.IsNotNull(result.FailureProbability);
        }

        [Test]
        public void Wbi2C1ResultWithFailureProb()
        {
            const double FailureProb = 0.00002;
            var noFailureProb = new AssessmentSectionAssemblyResult(EAssessmentGrade.A);
            var withFailureProb = new AssessmentSectionAssemblyResult(EAssessmentGrade.C, FailureProb);

            var result = assembler.AssembleAssessmentSectionWbi2C1(noFailureProb, withFailureProb);

            Assert.NotNull(result);
            Assert.AreEqual(EAssessmentGrade.C, result.Category);
            Assert.AreEqual(FailureProb, result.FailureProbability);
        }

        [Test]
        public void Wbi2C1ResultWithoutFailureProb()
        {
            var noFailureProb = new AssessmentSectionAssemblyResult(EAssessmentGrade.D);
            var withFailureProb = new AssessmentSectionAssemblyResult(EAssessmentGrade.A, 0.00002);

            var result = assembler.AssembleAssessmentSectionWbi2C1(noFailureProb, withFailureProb);

            Assert.NotNull(result);
            Assert.AreEqual(EAssessmentGrade.D, result.Category);
            Assert.IsNaN(result.FailureProbability);
        }
    }
}