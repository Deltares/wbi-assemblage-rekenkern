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

using System;
using System.Collections.Generic;
using Assembly.Kernel.Exceptions;
using Assembly.Kernel.Implementations;
using Assembly.Kernel.Interfaces;
using Assembly.Kernel.Model;
using Assembly.Kernel.Model.AssessmentSection;
using Assembly.Kernel.Model.Categories;
using NUnit.Framework;

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

        [Test]
        [TestCase(0.0,0.1,0.1)]
        [TestCase(0.0005, 0.00005, 0.000549975)]
        public void Boi2A1FailureProbabilityTests(double prob1, double prob2, double expectedProb)
        {
            var failureMechanismProbabilities = new[]
            {
                (Probability) prob1,
                (Probability) prob2,
            };
            var result = assembler.CalculateAssessmentSectionFailureProbabilityBoi2A1(failureMechanismProbabilities, false);

            Assert.IsTrue(result.IsDefined);
            Assert.IsTrue(result.IsNegligibleDifference((Probability)expectedProb));
        }

        [Test]
        [TestCase(0.1, EAssessmentGrade.C)]
        [TestCase(0.000549975, EAssessmentGrade.A)]
        public void Boi2B1AssessmentGradeTests(double failureProbability, EAssessmentGrade expectedGrade)
        {
            var categories = categoriesCalculator.CalculateAssessmentSectionCategoryLimitsBoi21(assessmentSection);
            var result = assembler.DetermineAssessmentGradeBoi2B1((Probability)failureProbability, categories);

            Assert.NotNull(result);
            Assert.AreEqual(expectedGrade, result);
        }

        [Test]
        public void Boi2A1PartialAssembly()
        {
            var sectionFailureProbability = 0.00003;
            var result = assembler.CalculateAssessmentSectionFailureProbabilityBoi2A1(
                new[]
                {
                    Probability.Undefined,
                    new Probability(sectionFailureProbability),
                    new Probability(sectionFailureProbability)
                },
                true);

            var expectedProbability = 1 - Math.Pow(1 - sectionFailureProbability, 2);
            Assert.AreEqual(expectedProbability, result);
        }

        [Test]
        public void Boi2A1ProbabilitiesNullTest()
        {
            TestHelper.AssertExpectedErrorMessage(
                () => assembler.CalculateAssessmentSectionFailureProbabilityBoi2A1(null, false),
                EAssemblyErrors.ValueMayNotBeNull
            );
        }

        [Test]
        public void Boi2A1EmptyProbabilitiesList()
        {
            TestHelper.AssertExpectedErrorMessage(
                () => assembler.CalculateAssessmentSectionFailureProbabilityBoi2A1(new List<Probability>(), false),
                EAssemblyErrors.EmptyResultsList
            );
        }

        [Test]
        public void Boi2A1PartialAssemblyNoResults()
        {
            TestHelper.AssertExpectedErrorMessage(
                () =>
                {
                    var result = assembler.CalculateAssessmentSectionFailureProbabilityBoi2A1(
                        new[]
                        {
                            Probability.Undefined,
                            Probability.Undefined,
                            Probability.Undefined
                        },
                        true);
                }, EAssemblyErrors.EmptyResultsList
            );
        }


        [Test]
        public void Boi2A1NoResultSomeFailureMechanisms()
        {
            TestHelper.AssertExpectedErrorMessage(() =>
            {
                var result = assembler.CalculateAssessmentSectionFailureProbabilityBoi2A1(
                    new[]
                    {
                        Probability.Undefined,
                        Probability.Undefined,
                        new Probability(0.00003),
                        new Probability(0.00003)
                    },
                    false);
            }, EAssemblyErrors.UndefinedProbability);
        }

        [Test]
        public void Boi2A1NoResultAtAll()
        {
            TestHelper.AssertExpectedErrorMessage(() =>
                {
                    var result = assembler.CalculateAssessmentSectionFailureProbabilityBoi2A1(
                        new[]
                        {
                            Probability.Undefined,
                            Probability.Undefined
                        }, false);
                }, EAssemblyErrors.UndefinedProbability
            );
        }

        [Test]
        public void Boi2B1CategoriesNullTest()
        {
            TestHelper.AssertExpectedErrorMessage(
                () => assembler.DetermineAssessmentGradeBoi2B1(new Probability(0.003), null),
                EAssemblyErrors.ValueMayNotBeNull
            );
        }

        [Test]
        public void Boi2B1MultipleInputErrorsList()
        {
            TestHelper.AssertExpectedErrorMessage(
                () => assembler.DetermineAssessmentGradeBoi2B1(Probability.Undefined, null),
                EAssemblyErrors.UndefinedProbability, EAssemblyErrors.ValueMayNotBeNull
            );
        }
    }
}