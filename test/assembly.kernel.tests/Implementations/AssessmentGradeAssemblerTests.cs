#region Copyright (C) Rijkswaterstaat 2022. All rights reserved

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

using System;
using System.Collections.Generic;
using System.Linq;
using Assembly.Kernel.Exceptions;
using Assembly.Kernel.Implementations;
using Assembly.Kernel.Interfaces;
using Assembly.Kernel.Model;
using Assembly.Kernel.Model.AssessmentSection;
using Assembly.Kernel.Model.Categories;
using NUnit.Framework;

// ReSharper disable UnusedMember.Local

namespace Assembly.Kernel.Tests.Implementations
{
    [TestFixture]
    public class AssessmentGradeAssemblerTests
    {
        private IAssessmentGradeAssembler assembler;

        private readonly AssessmentSection assessmentSection = new AssessmentSection((Probability) (1.0 / 1000.0), (Probability) (1.0 / 300.0));
        private readonly CategoryLimitsCalculator categoriesCalculator = new CategoryLimitsCalculator();

        [SetUp]
        public void Init()
        {
            assembler = new AssessmentGradeAssembler();
        }

        #region functional tests
        [Test]
        [TestCase(0.0,0.1,0.1,EAssessmentGrade.C)]
        [TestCase(0.0005, 0.00005, 0.000549975, EAssessmentGrade.A)]
        public void Boi2B1FailureProbabilityTests(double prob1, double prob2, double expectedProb, EAssessmentGrade expectedGrade)
        {
            var categories = categoriesCalculator.CalculateAssessmentSectionCategoryLimitsWbi21(assessmentSection);
            var failureMechanismProbabilities = new[]
            {
                (Probability) prob1,
                (Probability) prob2,
            };
            var result = assembler.CalculateAssessmentSectionFailureProbabilityBoi2B1(failureMechanismProbabilities, categories, false);

            Assert.NotNull(result.FailureProbability);
            Assert.AreEqual(expectedProb, result.FailureProbability, 10);
            Assert.AreEqual(expectedGrade, result.Category);
        }
        #endregion

        #region functional tests partial assembly

        [Test]
        public void Boi2B1PartialAssembly()
        {
            var sectionFailureProbability = 0.00003;
            var categories = categoriesCalculator.CalculateAssessmentSectionCategoryLimitsWbi21(assessmentSection);
            var result = assembler.CalculateAssessmentSectionFailureProbabilityBoi2B1(
                new[]
                {
                    new Probability(double.NaN),
                    new Probability(sectionFailureProbability),
                    new Probability(sectionFailureProbability)
                }, categories,
                true);

            var expectedProbability = 1 - Math.Pow(1 - sectionFailureProbability, 2);
            Assert.AreEqual(expectedProbability, result.FailureProbability);
            Assert.AreEqual(EAssessmentGrade.A, result.Category);
        }

        #endregion

        #region Input handling

        [Test]
        public void Boi2B1ProbabilitiesNullTest()
        {
            var categories = categoriesCalculator.CalculateAssessmentSectionCategoryLimitsWbi21(assessmentSection);
            try
            {
                assembler.CalculateAssessmentSectionFailureProbabilityBoi2B1(null, categories, false);
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
        public void Boi2B1EmptyProbabilitiesList()
        {
            var categories = categoriesCalculator.CalculateAssessmentSectionCategoryLimitsWbi21(assessmentSection);
            try
            {
                assembler.CalculateAssessmentSectionFailureProbabilityBoi2B1(new List<Probability>(), categories, false);
            }
            catch (AssemblyException e)
            {
                Assert.NotNull(e.Errors);
                var message = e.Errors.FirstOrDefault();
                Assert.NotNull(message);
                Assert.AreEqual(EAssemblyErrors.EmptyResultsList, message.ErrorCode);
            }
        }

        [Test]
        public void Boi2B1CategoriesNullTest()
        {
            try
            {
                List<Probability> results = new List<Probability>
                {
                    new Probability(0.003),
                    new Probability(0.003),
                };
                assembler.CalculateAssessmentSectionFailureProbabilityBoi2B1(results, null, false);
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
        public void Boi2B1MultipleInputErrorsList()
        {
            try
            {
                assembler.CalculateAssessmentSectionFailureProbabilityBoi2B1(new List<Probability>(), null, false);
            }
            catch (AssemblyException e)
            {
                Assert.NotNull(e.Errors);
                Assert.AreEqual(2, e.Errors.Count());

                var message = e.Errors.FirstOrDefault();
                Assert.NotNull(message);
                Assert.AreEqual(EAssemblyErrors.EmptyResultsList, message.ErrorCode);
                var message2 = e.Errors.ElementAt(1);
                Assert.NotNull(message2);
                Assert.AreEqual(EAssemblyErrors.ValueMayNotBeNull, message2.ErrorCode);
            }
        }

        [Test]
        public void Boi2B1PartialAssemblyNoResults()
        {
            try
            {
                var categories = categoriesCalculator.CalculateAssessmentSectionCategoryLimitsWbi21(assessmentSection);
                var result = assembler.CalculateAssessmentSectionFailureProbabilityBoi2B1(
                    new[]
                    {
                        new Probability(double.NaN),
                        new Probability(double.NaN),
                        new Probability(double.NaN)
                    }, categories,
                    true);
            }
            catch (AssemblyException e)
            {
                Assert.NotNull(e.Errors);
                Assert.AreEqual(1, e.Errors.Count());

                var message = e.Errors.FirstOrDefault();
                Assert.NotNull(message);
                Assert.AreEqual(EAssemblyErrors.EmptyResultsList, message.ErrorCode);
            }
        }


        [Test]
        public void Boi2B1NoResultSomeFailureMechanisms()
        {
            try
            {
                var categories = categoriesCalculator.CalculateAssessmentSectionCategoryLimitsWbi21(assessmentSection);
                var result = assembler.CalculateAssessmentSectionFailureProbabilityBoi2B1(
                    new[]
                    {
                        new Probability(double.NaN),
                        new Probability(double.NaN),
                        new Probability(0.00003),
                        new Probability(0.00003)
                    }, categories,
                    false);
            }
            catch (AssemblyException e)
            {
                Assert.NotNull(e.Errors);
                Assert.AreEqual(1, e.Errors.Count());

                var message = e.Errors.FirstOrDefault();
                Assert.NotNull(message);
                Assert.AreEqual(EAssemblyErrors.ProbabilityMayNotBeUndefined, message.ErrorCode);
            }
        }

        [Test]
        public void Boi2B1NoResultAtAll()
        {
            try
            {
                var categories = categoriesCalculator.CalculateAssessmentSectionCategoryLimitsWbi21(assessmentSection);
                var result = assembler.CalculateAssessmentSectionFailureProbabilityBoi2B1(
                    new[]
                    {
                        new Probability(double.NaN),
                        new Probability(double.NaN)
                    }, categories, false);

            }
            catch (AssemblyException e)
            {
                Assert.NotNull(e.Errors);
                Assert.AreEqual(1, e.Errors.Count());

                var message = e.Errors.FirstOrDefault();
                Assert.NotNull(message);
                Assert.AreEqual(EAssemblyErrors.ProbabilityMayNotBeUndefined, message.ErrorCode);
            }
        }

        #endregion
    }
}