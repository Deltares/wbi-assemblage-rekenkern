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

using System;
using Assembly.Kernel.Model.Categories;
using NUnit.Framework;

namespace Assembly.Kernel.Test.Model.Categories
{
    [TestFixture]
    public class EAssessmentGradeTest
    {
        [Test]
        public void EAssessmentGrade_Always_ExpectedValues()
        {
            Assert.AreEqual(5, Enum.GetValues(typeof(EAssessmentGrade)).Length);
            Assert.AreEqual(0, (int) EAssessmentGrade.APlus);
            Assert.AreEqual(1, (int) EAssessmentGrade.A);
            Assert.AreEqual(2, (int) EAssessmentGrade.B);
            Assert.AreEqual(3, (int) EAssessmentGrade.C);
            Assert.AreEqual(4, (int) EAssessmentGrade.D);
        }
    }
}