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

using Assembly.Kernel.Implementations;
using Assembly.Kernel.Interfaces;
using Assembly.Kernel.Model;
using Assembly.Kernel.Model.AssessmentSection;
using Assembly.Kernel.Model.Categories;
using NUnit.Framework;

namespace Assembly.Kernel.Tests.Implementations
{
    [TestFixture]
    public class CategoryLimitsCalculatorTest
    {
        private ICategoryLimitsCalculator categoryLimitsCalculator;

        [SetUp]
        public void Init()
        {
            categoryLimitsCalculator = new CategoryLimitsCalculator();
        }

        [Test]
        [TestCase(0.0003,0.034)]
        [TestCase(0.00003, 0.0003)]
        public void CalculateBoi01FunctionalTest(double signalFloodingProbability, double maximumAllowableFloodingProbability)
        {
            var section = new AssessmentSection((Probability) signalFloodingProbability, (Probability) maximumAllowableFloodingProbability);
            var results = categoryLimitsCalculator.CalculateInterpretationCategoryLimitsBoi01(section);

            Assert.AreEqual(7, results.Categories.Length);
            InterpretationCategory[] expectedCategories = 
            {
                new InterpretationCategory(EInterpretationCategory.III, (Probability) 0.0, (Probability) (signalFloodingProbability / 1000.0)),
                new InterpretationCategory(EInterpretationCategory.II, (Probability) (signalFloodingProbability / 1000.0), (Probability) (signalFloodingProbability / 100.0)),
                new InterpretationCategory(EInterpretationCategory.I, (Probability) (signalFloodingProbability / 100.0), (Probability) (signalFloodingProbability / 10.0)),
                new InterpretationCategory(EInterpretationCategory.Zero, (Probability) (signalFloodingProbability / 10.0), (Probability) signalFloodingProbability),
                new InterpretationCategory(EInterpretationCategory.IMin, (Probability) signalFloodingProbability, (Probability) maximumAllowableFloodingProbability),
                new InterpretationCategory(EInterpretationCategory.IIMin, (Probability) maximumAllowableFloodingProbability, (Probability) maximumAllowableFloodingProbability*10.0),
                new InterpretationCategory(EInterpretationCategory.IIIMin, (Probability) maximumAllowableFloodingProbability*10.0, (Probability)1.0)
            };
            CollectionAssert.AreEqual(results.Categories, expectedCategories,new CategoryLimitsEqualityComparer());
        }

        [Test]
        public void CalculateBoi01CapToOneTest()
        {
            var signalFloodingProbability = 0.001;
            var maximumAllowableFloodingProbability = 0.5;
            var section = new AssessmentSection((Probability)signalFloodingProbability, (Probability)maximumAllowableFloodingProbability);
            var results = categoryLimitsCalculator.CalculateInterpretationCategoryLimitsBoi01(section);

            Assert.AreEqual(7, results.Categories.Length);

            InterpretationCategory[] expectedCategories =
            {
                new InterpretationCategory(EInterpretationCategory.III, (Probability) 0.0, (Probability) (signalFloodingProbability / 1000.0)),
                new InterpretationCategory(EInterpretationCategory.II, (Probability) (signalFloodingProbability / 1000.0), (Probability) (signalFloodingProbability / 100.0)),
                new InterpretationCategory(EInterpretationCategory.I, (Probability) (signalFloodingProbability / 100.0), (Probability) (signalFloodingProbability / 10.0)),
                new InterpretationCategory(EInterpretationCategory.Zero, (Probability) (signalFloodingProbability / 10.0), (Probability) signalFloodingProbability),
                new InterpretationCategory(EInterpretationCategory.IMin, (Probability) signalFloodingProbability, (Probability) maximumAllowableFloodingProbability),
                new InterpretationCategory(EInterpretationCategory.IIMin, (Probability) maximumAllowableFloodingProbability, (Probability)1.0),
                new InterpretationCategory(EInterpretationCategory.IIIMin, (Probability)1.0, (Probability)1.0)
            };
            CollectionAssert.AreEqual(results.Categories, expectedCategories, new CategoryLimitsEqualityComparer());
        }

        [Test]
        public void CalculateBoi21FunctionalTest()
        {
            var section = new AssessmentSection(new Probability(0.003), new Probability(0.03));
            var results = categoryLimitsCalculator.CalculateAssessmentSectionCategoryLimitsBoi21(section);

            Assert.AreEqual(5, results.Categories.Length);

            AssessmentSectionCategory[] expectedCategories =
            {
                new AssessmentSectionCategory(EAssessmentGrade.APlus, (Probability) 0.0, (Probability) 0.0001 ),
                new AssessmentSectionCategory(EAssessmentGrade.A, (Probability) 0.0001 , (Probability) 0.003 ),
                new AssessmentSectionCategory(EAssessmentGrade.B, (Probability) 0.003, (Probability) 0.03),
                new AssessmentSectionCategory(EAssessmentGrade.C, (Probability) 0.03, (Probability) 0.9),
                new AssessmentSectionCategory(EAssessmentGrade.D, (Probability) 0.9, (Probability) 1.0)
            };

            CollectionAssert.AreEqual(results.Categories, expectedCategories, new CategoryLimitsEqualityComparer());
        }

        [Test]
        public void CalculateBoi21CapToOneTest()
        {
            var signalFloodingProbability = new Probability(0.003);
            var maximumAllowableFloodingProbability = new Probability(0.034);

            var section = new AssessmentSection(signalFloodingProbability, maximumAllowableFloodingProbability);

            CategoriesList<AssessmentSectionCategory> results =
                categoryLimitsCalculator.CalculateAssessmentSectionCategoryLimitsBoi21(section);

            Assert.AreEqual(5, results.Categories.Length);

            AssessmentSectionCategory[] expectedCategories =
            {
                new AssessmentSectionCategory(EAssessmentGrade.APlus, (Probability) 0.0, (Probability) 0.0001 ),
                new AssessmentSectionCategory(EAssessmentGrade.A, (Probability) 0.0001 , (Probability) 0.003 ),
                new AssessmentSectionCategory(EAssessmentGrade.B, (Probability) 0.003, (Probability) 0.034),
                new AssessmentSectionCategory(EAssessmentGrade.C, (Probability) 0.034, (Probability) 1.0),
                new AssessmentSectionCategory(EAssessmentGrade.D, (Probability) 1.0, (Probability) 1.0)
            };

            CollectionAssert.AreEqual(results.Categories, expectedCategories, new CategoryLimitsEqualityComparer());
        }
    }
}