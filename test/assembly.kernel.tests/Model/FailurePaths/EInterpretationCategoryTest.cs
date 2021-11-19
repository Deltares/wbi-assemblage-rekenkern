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
using Assembly.Kernel.Model.Categories;
using NUnit.Framework;

namespace Assembly.Kernel.Tests.Model.FailurePaths
{
    [TestFixture]
    public class EInterpretationCategoryTest
    {
        [Test]
        public void TestEnumContract()
        {
            Assert.AreEqual(11, Enum.GetValues(typeof(EInterpretationCategory)).Length);
            Assert.AreEqual(1, (int)EInterpretationCategory.ND);
            Assert.AreEqual(2, (int)EInterpretationCategory.III);
            Assert.AreEqual(3, (int)EInterpretationCategory.II);
            Assert.AreEqual(4, (int)EInterpretationCategory.I);
            Assert.AreEqual(5, (int)EInterpretationCategory.ZeroPlus);
            Assert.AreEqual(6, (int)EInterpretationCategory.Zero);
            Assert.AreEqual(7, (int)EInterpretationCategory.IMin);
            Assert.AreEqual(8, (int)EInterpretationCategory.IIMin);
            Assert.AreEqual(9, (int)EInterpretationCategory.IIIMin);
            Assert.AreEqual(10, (int)EInterpretationCategory.D);
            Assert.AreEqual(11, (int)EInterpretationCategory.Gr);
            Assert.Greater(EInterpretationCategory.IIMin, EInterpretationCategory.II);
        }
    }
}