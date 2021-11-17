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
// You should have received a copy of the GNU Lesser General Public License
// along with this program. If not, see <http://www.gnu.org/licenses/>.
//
// All names, logos, and references to "Rijkswaterstaat" are registered trademarks of
// Rijkswaterstaat and remain full property of Rijkswaterstaat at all times.
// All rights reserved.
#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assembly.Kernel.Exceptions;
using Assembly.Kernel.Implementations;
using Assembly.Kernel.Interfaces;
using Assembly.Kernel.Model;
using NUnit.Framework;

// ReSharper disable UnusedMember.Local

namespace Assembly.Kernel.Tests.Implementations
{
    [TestFixture]
    public class AssessmentGradeAssemblerTests
    {
        private IAssessmentGradeAssembler assembler;
        private readonly AssessmentSection assessmentSection = new AssessmentSection(10000, 1.0 / 1000.0, 1.0 / 300.0);
        private readonly CategoryLimitsCalculator categoriesCalculator = new CategoryLimitsCalculator();

        [SetUp]
        public void Init()
        {
            assembler = new AssessmentGradeAssembler();
        }

        [Test]
        public void Wbi2B1EmptyList()
        {
            var categories = categoriesCalculator.CalculateAssessmentSectionCategoryLimitsWbi21(assessmentSection);
            try
            {
                assembler.AssembleAssessmentSectionWbi2B1(new List<FailureMechanismAssemblyResult>(), categories,
                                                          false);
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
                                                  EAssemblyType assemblyType, double expectedResult, EAssessmentGrade expectedGrade)
        {
            var categories = categoriesCalculator.CalculateAssessmentSectionCategoryLimitsWbi21(assessmentSection);

            var result = assembler.AssembleAssessmentSectionWbi2B1(failureProbabilities.Select(failureProbability =>
                    new
                        FailureMechanismAssemblyResult(failureProbability)), categories,
                                                                   assemblyType == EAssemblyType.Partial);

            Assert.NotNull(result.FailureProbability);
            Assert.AreEqual(result.FailureProbability, expectedResult, 10);
            Assert.AreEqual(expectedGrade, result.Category);
        }

         [Test]
        public void Wbi2B1NoResultSomeFailurePaths()
        {
            var categories = categoriesCalculator.CalculateAssessmentSectionCategoryLimitsWbi21(assessmentSection);
            var result = assembler.AssembleAssessmentSectionWbi2B1(
                new[]
                {
                    new FailureMechanismAssemblyResult(double.NaN),
                    new FailureMechanismAssemblyResult(double.NaN),
                    new FailureMechanismAssemblyResult(0.00003),
                    new FailureMechanismAssemblyResult(0.00003)
                }, categories,
                false);

            Assert.IsNaN(result.FailureProbability);
            Assert.AreEqual(EAssessmentGrade.Gr, result.Category);
        }

        [Test]
        public void Wbi2B1NoResultAtAll()
        {
            var categories = categoriesCalculator.CalculateAssessmentSectionCategoryLimitsWbi21(assessmentSection);
            var result = assembler.AssembleAssessmentSectionWbi2B1(
                new[]
                {
                    new FailureMechanismAssemblyResult(double.NaN),
                    new FailureMechanismAssemblyResult(double.NaN)
                }, categories, false);

            Assert.IsNaN(result.FailureProbability);
            Assert.AreEqual(EAssessmentGrade.Gr, result.Category);
        }

        [Test]
        public void Wbi2B1NullTest()
        {
            var categories = categoriesCalculator.CalculateAssessmentSectionCategoryLimitsWbi21(assessmentSection);
            try
            {
                assembler.AssembleAssessmentSectionWbi2B1(null, categories, false);
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
        public void Wbi2B1CategoriesNullTest()
        {
            try
            {
                List<FailureMechanismAssemblyResult> results = new List<FailureMechanismAssemblyResult>
                {
                    new FailureMechanismAssemblyResult(0.003),
                    new FailureMechanismAssemblyResult(0.003),
                };
                assembler.AssembleAssessmentSectionWbi2B1(results, null, false);
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
        public void Wbi2B1PartialAssembly()
        {
            var sectionFailureProbability = 0.00003;
            var categories = categoriesCalculator.CalculateAssessmentSectionCategoryLimitsWbi21(assessmentSection);
            var result = assembler.AssembleAssessmentSectionWbi2B1(
                new[]
                {
                    new FailureMechanismAssemblyResult(double.NaN),
                    new FailureMechanismAssemblyResult(sectionFailureProbability),
                    new FailureMechanismAssemblyResult(sectionFailureProbability)
                }, categories,
                true);

            var expectedProbability = 1 - Math.Pow(1 - sectionFailureProbability, 2);
            Assert.AreEqual(expectedProbability, result.FailureProbability);
        }

        public enum EAssemblyType
        {
            Full,
            Partial
        }

        private sealed class AssessMentGradeAssemblerTestData
        {
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
                                                  0.1,
                                                  EAssessmentGrade.C);

                    yield return new TestCaseData(new[]
                                                  {
                                                      0.0005,
                                                      0.00005
                                                  },
                                                  EAssemblyType.Full,
                                                  0.000549975,
                                                  EAssessmentGrade.A);
                }
            }
        }
    }
}