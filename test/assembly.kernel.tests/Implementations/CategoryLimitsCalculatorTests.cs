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

using System.Collections.Generic;
using System.Linq;
using Assembly.Kernel.Exceptions;
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
        [SetUp]
        public void Init()
        {
            categoryLimitsCalculator = new CategoryLimitsCalculator();
        }

        private ICategoryLimitsCalculator categoryLimitsCalculator;

        [Test]
        public void CalculateWbi01Exceptions()
        {
            const double SignallingLimit = 1.0 / 1000.0;
            const double LowerLimit = 1.0 / 300.0;
            const double FailurePobabilityMarginFactor = 1;
            const double LengthEffectFactor = 1;

            var section = new AssessmentSection(20306, SignallingLimit, LowerLimit);
            var failureMechanism = new FailureMechanism(LengthEffectFactor,
                FailurePobabilityMarginFactor);

            try
            {
                categoryLimitsCalculator.CalculateFmSectionCategoryLimitsWbi01(section, failureMechanism);
            }
            catch (AssemblyException e)
            {
                Assert.NotNull(e.Errors);

                Assert.AreEqual(2, e.Errors.Count());
                List<AssemblyErrorMessage> messages = e.Errors.ToList();
                Assert.NotNull(messages[0]);
                Assert.NotNull(messages[1]);

                Assert.AreEqual(EAssemblyErrors.PsigDsnAbovePsig, messages[0].ErrorCode);
                Assert.AreEqual(EAssemblyErrors.PlowDsnAbovePlow, messages[1].ErrorCode);
                Assert.Pass();
            }

            Assert.Fail("Expected Exception not thrown");
        }

        [Test]
        public void CalculateWbi01MaximizeTest()
        {
            const double SignallingLimit = 1.0 / 1000.0;
            const double LowerLimit = 1.0 / 30.0;
            const double FailurePobabilityMarginFactor = 0.04;
            const double LengthEffectFactor = 14.4;

            var section = new AssessmentSection(20306, SignallingLimit, LowerLimit);
            var failureMechanism = new FailureMechanism(LengthEffectFactor,
                FailurePobabilityMarginFactor);

            CategoriesList<FmSectionCategory> results =
                categoryLimitsCalculator.CalculateFmSectionCategoryLimitsWbi01(section, failureMechanism);

            Assert.AreEqual(6, results.Categories.Length);

            foreach (var limitResults in results.Categories)
            {
                switch (limitResults.Category)
                {
                    case EFmSectionCategory.Iv:
                        Assert.AreEqual(0, limitResults.LowerLimit);
                        Assert.AreEqual(9.26E-8, limitResults.UpperLimit, 1e-10);
                        break;
                    case EFmSectionCategory.IIv:
                        Assert.AreEqual(9.26E-8, limitResults.LowerLimit, 1e-10);
                        Assert.AreEqual(2.78E-6, limitResults.UpperLimit, 1e-8);
                        break;
                    case EFmSectionCategory.IIIv:
                        Assert.AreEqual(2.78E-6, limitResults.LowerLimit, 1e-8);
                        Assert.AreEqual(9.26E-5, limitResults.UpperLimit, 1e-7);
                        break;
                    case EFmSectionCategory.IVv:
                        Assert.AreEqual(9.26E-5, limitResults.LowerLimit, 1e-7);
                        Assert.AreEqual(3.33E-2, limitResults.UpperLimit, 1e-4);
                        break;
                    case EFmSectionCategory.Vv:
                        Assert.AreEqual(3.33E-2, limitResults.LowerLimit, 1e-4);
                        Assert.AreEqual(1.0, limitResults.UpperLimit, 1e-1);
                        break;
                    case EFmSectionCategory.VIv:
                        Assert.AreEqual(1.0, limitResults.LowerLimit, 1e-1);
                        Assert.AreEqual(1.0, limitResults.UpperLimit, 1e-1);
                        break;
                    default:
                        Assert.Fail("Unexpected category received");
                        break;
                }
            }
        }

        /// <summary>
        /// Test data is from section 2-1 (Ameland (2))
        /// </summary>
        [Test]
        public void CalculateWbi01Test()
        {
            const double SignallingLimit = 1.0 / 1000.0;
            const double LowerLimit = 1.0 / 300.0;
            const double FailurePobabilityMarginFactor = 0.04;
            const double LengthEffectFactor = 14.4;

            var section = new AssessmentSection(20306, SignallingLimit, LowerLimit);
            var failureMechanism = new FailureMechanism(LengthEffectFactor,
                FailurePobabilityMarginFactor);

            CategoriesList<FmSectionCategory> results =
                categoryLimitsCalculator.CalculateFmSectionCategoryLimitsWbi01(section, failureMechanism);

            Assert.AreEqual(6, results.Categories.Length);

            foreach (var limitResults in results.Categories)
            {
                switch (limitResults.Category)
                {
                    case EFmSectionCategory.Iv:
                        Assert.AreEqual(0, limitResults.LowerLimit);
                        Assert.AreEqual(9.26E-8, limitResults.UpperLimit, 1e-10);
                        break;
                    case EFmSectionCategory.IIv:
                        Assert.AreEqual(9.26E-8, limitResults.LowerLimit, 1e-10);
                        Assert.AreEqual(2.78E-6, limitResults.UpperLimit, 1e-8);
                        break;
                    case EFmSectionCategory.IIIv:
                        Assert.AreEqual(2.78E-6, limitResults.LowerLimit, 1e-8);
                        Assert.AreEqual(9.26E-6, limitResults.UpperLimit, 1e-8);
                        break;
                    case EFmSectionCategory.IVv:
                        Assert.AreEqual(9.26E-6, limitResults.LowerLimit, 1e-8);
                        Assert.AreEqual(3.33E-3, limitResults.UpperLimit, 1e-5);
                        break;
                    case EFmSectionCategory.Vv:
                        Assert.AreEqual(3.33E-3, limitResults.LowerLimit, 1e-5);
                        Assert.AreEqual(0.1, limitResults.UpperLimit, 1e-1);
                        break;
                    case EFmSectionCategory.VIv:
                        Assert.AreEqual(0.1, limitResults.LowerLimit, 1e-1);
                        Assert.AreEqual(1, limitResults.UpperLimit);
                        break;
                    default:
                        Assert.Fail("Unexpected category received");
                        break;
                }
            }
        }

        [Test]
        public void CalculateWbi02MaximizeTest()
        {
            const double norm = 0.00003;
            const double failurePobabilityMarginFactor = 0.1;
            const double lengthEffectFactor = 2;

            var failureMechanism = new FailureMechanism(lengthEffectFactor,
                failurePobabilityMarginFactor);

            CategoriesList<FmSectionCategory> results =
                categoryLimitsCalculator.CalculateFmSectionCategoryLimitsWbi02(norm, failureMechanism);

            Assert.AreEqual(2, results.Categories.Length);

            var expectedCategoryBoundary = failurePobabilityMarginFactor * norm * 10 / lengthEffectFactor;

            foreach (var limitResults in results.Categories)
            {
                switch (limitResults.Category)
                {
                    case EFmSectionCategory.IIv:
                        Assert.AreEqual(0.0, limitResults.LowerLimit, 1e-7);
                        Assert.AreEqual(expectedCategoryBoundary, limitResults.UpperLimit, 1e-6);
                        break;
                    case EFmSectionCategory.Vv:
                        Assert.AreEqual(expectedCategoryBoundary, limitResults.LowerLimit, 1e-6);
                        Assert.AreEqual(1.0, limitResults.UpperLimit, 1e-1);
                        break;
                    default:
                        Assert.Fail("Unexpected category received");
                        break;
                }
            }
        }

        [Test]
        public void CalculateWbi02Test()
        {
            const double norm = 0.00003;
            const double failurePobabilityMarginFactor = 0.1;
            const double lengthEffectFactor = 2;

            var failureMechanism = new FailureMechanism(lengthEffectFactor, failurePobabilityMarginFactor);

            CategoriesList<FmSectionCategory> results =
                categoryLimitsCalculator.CalculateFmSectionCategoryLimitsWbi02(norm, failureMechanism);

            Assert.AreEqual(2, results.Categories.Length);

            var expectedCategoryBoundary = failurePobabilityMarginFactor * norm * 10 / lengthEffectFactor;

            foreach (var limitResults in results.Categories)
            {
                switch (limitResults.Category)
                {
                    case EFmSectionCategory.IIv:
                        Assert.AreEqual(0.0, limitResults.LowerLimit, 1e-7);
                        Assert.AreEqual(expectedCategoryBoundary, limitResults.UpperLimit, 1e-8);
                        break;
                    case EFmSectionCategory.Vv:
                        Assert.AreEqual(expectedCategoryBoundary, limitResults.LowerLimit, 1e-8);
                        Assert.AreEqual(1.0, limitResults.UpperLimit, 1e-3);
                        break;
                    default:
                        Assert.Fail("Unexpected category received");
                        break;
                }
            }
        }

        [Test]
        public void CalculateWbi11MaximizeTest()
        {
            const double SignallingLimit = 0.0003;
            const double LowerLimit = 0.034;
            const double FailurePobabilityMarginFactor = 0.1;
            const double LengthEffectFactor = 1;

            var section = new AssessmentSection(10000, SignallingLimit, LowerLimit);
            var failureMechanism = new FailureMechanism(LengthEffectFactor,
                FailurePobabilityMarginFactor);

            CategoriesList<FailureMechanismCategory> results =
                categoryLimitsCalculator.CalculateFailureMechanismCategoryLimitsWbi11(section, failureMechanism);

            Assert.AreEqual(6, results.Categories.Length);

            foreach (var limitResults in results.Categories)
            {
                switch (limitResults.Category)
                {
                    case EFailureMechanismCategory.It:
                        Assert.AreEqual(0, limitResults.LowerLimit);
                        Assert.AreEqual(0.000001, limitResults.UpperLimit, 1e-6);
                        break;
                    case EFailureMechanismCategory.IIt:
                        Assert.AreEqual(0.000001, limitResults.LowerLimit, 1e-6);
                        Assert.AreEqual(0.00003, limitResults.UpperLimit, 1e-5);
                        break;
                    case EFailureMechanismCategory.IIIt:
                        Assert.AreEqual(0.00003, limitResults.LowerLimit, 1e-5);
                        Assert.AreEqual(0.0034, limitResults.UpperLimit, 1e-4);
                        break;
                    case EFailureMechanismCategory.IVt:
                        Assert.AreEqual(0.0034, limitResults.LowerLimit, 1e-4);
                        Assert.AreEqual(0.034, limitResults.UpperLimit, 1e-3);
                        break;
                    case EFailureMechanismCategory.Vt:
                        Assert.AreEqual(0.034, limitResults.LowerLimit, 1e-3);
                        Assert.AreEqual(1.0, limitResults.UpperLimit, 1e-1);
                        break;
                    case EFailureMechanismCategory.VIt:
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
        public void CalculateWbi11Test()
        {
            const double SignallingLimit = 0.0003;
            const double LowerLimit = 0.003;
            const double FailurePobabilityMarginFactor = 0.1;
            const double LengthEffectFactor = 1;

            var section = new AssessmentSection(10000, SignallingLimit, LowerLimit);
            var failureMechanism = new FailureMechanism(LengthEffectFactor,
                FailurePobabilityMarginFactor);

            CategoriesList<FailureMechanismCategory> results =
                categoryLimitsCalculator.CalculateFailureMechanismCategoryLimitsWbi11(section, failureMechanism);

            Assert.AreEqual(6, results.Categories.Length);

            foreach (var limitResults in results.Categories)
            {
                switch (limitResults.Category)
                {
                    case EFailureMechanismCategory.It:
                        Assert.AreEqual(0, limitResults.LowerLimit);
                        Assert.AreEqual(0.000001, limitResults.UpperLimit, 1e-6);
                        break;
                    case EFailureMechanismCategory.IIt:
                        Assert.AreEqual(0.000001, limitResults.LowerLimit, 1e-6);
                        Assert.AreEqual(0.00003, limitResults.UpperLimit, 1e-5);
                        break;
                    case EFailureMechanismCategory.IIIt:
                        Assert.AreEqual(0.00003, limitResults.LowerLimit, 1e-5);
                        Assert.AreEqual(0.0003, limitResults.UpperLimit, 1e-4);
                        break;
                    case EFailureMechanismCategory.IVt:
                        Assert.AreEqual(0.0003, limitResults.LowerLimit, 1e-4);
                        Assert.AreEqual(0.003, limitResults.UpperLimit, 1e-3);
                        break;
                    case EFailureMechanismCategory.Vt:
                        Assert.AreEqual(0.003, limitResults.LowerLimit, 1e-3);
                        Assert.AreEqual(0.09, limitResults.UpperLimit, 1e-2);
                        break;
                    case EFailureMechanismCategory.VIt:
                        Assert.AreEqual(0.09, limitResults.LowerLimit, 1e-2);
                        Assert.AreEqual(1, limitResults.UpperLimit);
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