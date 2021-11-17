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

using Assembly.Kernel.Implementations;
using Assembly.Kernel.Interfaces;
using Assembly.Kernel.Model;
using Assembly.Kernel.Model.CategoryLimits;
using Assembly.Kernel.Model.FmSectionTypes;
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

        [Test]
        [TestCase(0.0003,0.034)]
        [TestCase(0.00003, 0.0003)]
        public void CalculateWbi03Test(double signallingLimit, double lowerLimit)
        {
            var section = new AssessmentSection(10000, signallingLimit, lowerLimit);

            CategoriesList<InterpretationCategory> results =
                categoryLimitsCalculator.CalculateInterpretationCategoryLimitsWbi03(section);

            Assert.AreEqual(8, results.Categories.Length);

            foreach (var limitResults in results.Categories)
            {
                switch (limitResults.Category)
                {
                    case EInterpretationCategory.III:
                        Assert.AreEqual(0, limitResults.LowerLimit);
                        Assert.AreEqual(signallingLimit / 30.0, limitResults.UpperLimit, 1e-6);
                        break;
                    case EInterpretationCategory.II:
                        Assert.AreEqual(signallingLimit / 30.0, limitResults.LowerLimit, 1e-6);
                        Assert.AreEqual(signallingLimit / 10.0, limitResults.UpperLimit, 1e-6);
                        break;
                    case EInterpretationCategory.I:
                        Assert.AreEqual(signallingLimit / 10.0, limitResults.LowerLimit, 1e-6);
                        Assert.AreEqual(signallingLimit / 3.0, limitResults.UpperLimit, 1e-6);
                        break;
                    case EInterpretationCategory.ZeroPlus:
                        Assert.AreEqual(signallingLimit / 3.0, limitResults.LowerLimit, 1e-6);
                        Assert.AreEqual(signallingLimit, limitResults.UpperLimit, 1e-6);
                        break;
                    case EInterpretationCategory.Zero:
                        Assert.AreEqual(signallingLimit, limitResults.LowerLimit, 1e-6);
                        Assert.AreEqual(lowerLimit, limitResults.UpperLimit, 1e-6);
                        break;
                    case EInterpretationCategory.IMin:
                        Assert.AreEqual(lowerLimit, limitResults.LowerLimit, 1e-6);
                        Assert.AreEqual(lowerLimit*3.0, limitResults.UpperLimit, 1e-6);
                        break;
                    case EInterpretationCategory.IIMin:
                        Assert.AreEqual(lowerLimit*3.0, limitResults.LowerLimit, 1e-6);
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
        public void CalculateWbi21MaximizeTest()
        {
            const double SignallingLimit = 0.003;
            const double LowerLimit = 0.034;

            var section = new AssessmentSection(10000, SignallingLimit, LowerLimit);

            CategoriesList<AssessmentSectionCategory> results =
                categoryLimitsCalculator.CalculateAssessmentSectionCategoryLimitsWbi21(section);

            Assert.AreEqual(5, results.Categories.Length);

            foreach (var limitResults in results.Categories)
            {
                switch (limitResults.Category)
                {
                    case EAssessmentGrade.APlus:
                        Assert.AreEqual(0, limitResults.LowerLimit);
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

        [Test]
        public void CalculateWbi21Test()
        {
            const double SignallingLimit = 0.003;
            const double LowerLimit = 0.03;

            var section = new AssessmentSection(10000, SignallingLimit, LowerLimit);

            CategoriesList<AssessmentSectionCategory> results =
                categoryLimitsCalculator.CalculateAssessmentSectionCategoryLimitsWbi21(section);

            Assert.AreEqual(5, results.Categories.Length);

            foreach (var limitResults in results.Categories)
            {
                switch (limitResults.Category)
                {
                    case EAssessmentGrade.APlus:
                        Assert.AreEqual(0, limitResults.LowerLimit);
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
                        Assert.AreEqual(1, limitResults.UpperLimit);
                        break;
                    default:
                        Assert.Fail("Unexpected category received");
                        break;
                }
            }
        }
    }
}