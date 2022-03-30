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

using Assembly.Kernel.Implementations;
using Assembly.Kernel.Interfaces;
using Assembly.Kernel.Model;
using Assembly.Kernel.Model.AssessmentSection;
using Assembly.Kernel.Model.Categories;
using NUnit.Framework;

namespace Assembly.Kernel.Tests.Implementations
{
    [TestFixture]
    public class CategoryLimitsCalculatorTests
    {
        private ICategoryLimitsCalculator categoryLimitsCalculator;

        [SetUp]
        public void Init()
        {
            categoryLimitsCalculator = new CategoryLimitsCalculator();
        }

        #region BOI-0-1
        [Test]
        [TestCase(0.0003,0.034)]
        [TestCase(0.00003, 0.0003)]
        public void CalculateBoi01FunctionalTest(double signalingLimit, double lowerLimit)
        {
            var section = new AssessmentSection((Probability) signalingLimit, (Probability) lowerLimit);

            CategoriesList<InterpretationCategory> results =
                categoryLimitsCalculator.CalculateInterpretationCategoryLimitsBoi01(section);

            Assert.AreEqual(7, results.Categories.Length);

            foreach (var limitResults in results.Categories)
            {
                switch (limitResults.Category)
                {
                    case EInterpretationCategory.III:
                        Assert.AreEqual(0.0, limitResults.LowerLimit);
                        Assert.AreEqual(signalingLimit / 1000.0, limitResults.UpperLimit, 1e-6);
                        break;
                    case EInterpretationCategory.II:
                        Assert.AreEqual(signalingLimit / 1000.0, limitResults.LowerLimit, 1e-6);
                        Assert.AreEqual(signalingLimit / 100.0, limitResults.UpperLimit, 1e-6);
                        break;
                    case EInterpretationCategory.I:
                        Assert.AreEqual(signalingLimit / 100.0, limitResults.LowerLimit, 1e-6);
                        Assert.AreEqual(signalingLimit / 10.0, limitResults.UpperLimit, 1e-6);
                        break;
                    case EInterpretationCategory.Zero:
                        Assert.AreEqual(signalingLimit / 10.0, limitResults.LowerLimit, 1e-6);
                        Assert.AreEqual(signalingLimit, limitResults.UpperLimit, 1e-6);
                        break;
                    case EInterpretationCategory.IMin:
                        Assert.AreEqual(signalingLimit, limitResults.LowerLimit, 1e-6);
                        Assert.AreEqual(lowerLimit, limitResults.UpperLimit, 1e-6);
                        break;
                    case EInterpretationCategory.IIMin:
                        Assert.AreEqual(lowerLimit, limitResults.LowerLimit, 1e-6);
                        Assert.AreEqual(lowerLimit*10.0, limitResults.UpperLimit, 1e-6);
                        break;
                    case EInterpretationCategory.IIIMin:
                        Assert.AreEqual(lowerLimit*10.0, limitResults.LowerLimit, 1e-6);
                        Assert.AreEqual(1.0, limitResults.UpperLimit, 1e-6);
                        break;
                    default:
                        Assert.Fail("Unexpected category received");
                        break;
                }
            }
        }

        [Test]
        public void CalculateBoi01CapToOneTest()
        {
            var signalingLimit = 0.001;
            var lowerLimit = 0.5;
            var section = new AssessmentSection((Probability)signalingLimit, (Probability)lowerLimit);

            CategoriesList<InterpretationCategory> results =
                categoryLimitsCalculator.CalculateInterpretationCategoryLimitsBoi01(section);

            Assert.AreEqual(7, results.Categories.Length);

            foreach (var limitResults in results.Categories)
            {
                switch (limitResults.Category)
                {
                    case EInterpretationCategory.III:
                        Assert.AreEqual(0.0, limitResults.LowerLimit);
                        Assert.AreEqual(signalingLimit / 1000.0, limitResults.UpperLimit, 1e-6);
                        break;
                    case EInterpretationCategory.II:
                        Assert.AreEqual(signalingLimit / 1000.0, limitResults.LowerLimit, 1e-6);
                        Assert.AreEqual(signalingLimit / 100.0, limitResults.UpperLimit, 1e-6);
                        break;
                    case EInterpretationCategory.I:
                        Assert.AreEqual(signalingLimit / 100.0, limitResults.LowerLimit, 1e-6);
                        Assert.AreEqual(signalingLimit / 10.0, limitResults.UpperLimit, 1e-6);
                        break;
                    case EInterpretationCategory.Zero:
                        Assert.AreEqual(signalingLimit / 10.0, limitResults.LowerLimit, 1e-6);
                        Assert.AreEqual(signalingLimit, limitResults.UpperLimit, 1e-6);
                        break;
                    case EInterpretationCategory.IMin:
                        Assert.AreEqual(signalingLimit, limitResults.LowerLimit, 1e-6);
                        Assert.AreEqual(lowerLimit, limitResults.UpperLimit, 1e-6);
                        break;
                    case EInterpretationCategory.IIMin:
                        Assert.AreEqual(lowerLimit, limitResults.LowerLimit, 1e-6);
                        Assert.AreEqual(1.0, limitResults.UpperLimit, 1e-6);
                        break;
                    case EInterpretationCategory.IIIMin:
                        Assert.AreEqual(1.0, limitResults.LowerLimit, 1e-6);
                        Assert.AreEqual(1.0, limitResults.UpperLimit, 1e-6);
                        break;
                    default:
                        Assert.Fail("Unexpected category received");
                        break;
                }
            }
        }

        #endregion

        #region BOI-2-1

        [Test]
        public void CalculateBoi21FunctionalTest()
        {
            var signalingLimit = new Probability(0.003);
            var lowerLimit = new Probability(0.03);

            var section = new AssessmentSection(signalingLimit, lowerLimit);

            CategoriesList<AssessmentSectionCategory> results =
                categoryLimitsCalculator.CalculateAssessmentSectionCategoryLimitsBoi21(section);

            Assert.AreEqual(5, results.Categories.Length);

            foreach (var limitResults in results.Categories)
            {
                switch (limitResults.Category)
                {
                    case EAssessmentGrade.APlus:
                        Assert.AreEqual(0.0, limitResults.LowerLimit);
                        Assert.AreEqual(0.0001, limitResults.UpperLimit, 1e-4);
                        break;
                    case EAssessmentGrade.A:
                        Assert.AreEqual(0.0001, limitResults.LowerLimit, 1e-4);
                        Assert.AreEqual(0.003, limitResults.UpperLimit, 1e-3);
                        break;
                    case EAssessmentGrade.B:
                        Assert.AreEqual(0.003, limitResults.LowerLimit, 1e-3);
                        Assert.AreEqual(0.03, limitResults.UpperLimit, 1e-3);
                        break;
                    case EAssessmentGrade.C:
                        Assert.AreEqual(0.03, limitResults.LowerLimit, 1e-2);
                        Assert.AreEqual(0.9, limitResults.UpperLimit, 1e-3);
                        break;
                    case EAssessmentGrade.D:
                        Assert.AreEqual(0.9, limitResults.LowerLimit, 1e-1);
                        Assert.AreEqual(1.0, limitResults.UpperLimit);
                        break;
                    default:
                        Assert.Fail("Unexpected category received");
                        break;
                }
            }
        }

        [Test]
        public void CalculateBoi21CapToOneTest()
        {
            var signalingLimit = new Probability(0.003);
            var lowerLimit = new Probability(0.034);

            var section = new AssessmentSection(signalingLimit, lowerLimit);

            CategoriesList<AssessmentSectionCategory> results =
                categoryLimitsCalculator.CalculateAssessmentSectionCategoryLimitsBoi21(section);

            Assert.AreEqual(5, results.Categories.Length);

            foreach (var limitResults in results.Categories)
            {
                switch (limitResults.Category)
                {
                    case EAssessmentGrade.APlus:
                        Assert.AreEqual(0.0, limitResults.LowerLimit);
                        Assert.AreEqual(0.0001, limitResults.UpperLimit, 1e-4);
                        break;
                    case EAssessmentGrade.A:
                        Assert.AreEqual(0.0001, limitResults.LowerLimit, 1e-4);
                        Assert.AreEqual(0.003, limitResults.UpperLimit, 1e-3);
                        break;
                    case EAssessmentGrade.B:
                        Assert.AreEqual(0.003, limitResults.LowerLimit, 1e-3);
                        Assert.AreEqual(0.034, limitResults.UpperLimit, 1e-4);
                        break;
                    case EAssessmentGrade.C:
                        Assert.AreEqual(0.034, limitResults.LowerLimit, 1e-4);
                        Assert.AreEqual(1.0, limitResults.UpperLimit, 1e-1);
                        break;
                    case EAssessmentGrade.D:
                        Assert.AreEqual(1.0, limitResults.LowerLimit, 1e-1);
                        Assert.AreEqual(1.0, limitResults.UpperLimit, 1e-1);
                        break;
                    default:
                        Assert.Fail("Unexpected category received");
                        break;
                }
            }
        }

        #endregion
    }
}