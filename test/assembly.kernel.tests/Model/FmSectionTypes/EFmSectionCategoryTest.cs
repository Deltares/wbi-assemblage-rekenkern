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
using Assembly.Kernel.Model.FmSectionTypes;
using NUnit.Framework;

namespace Assembly.Kernel.Tests.Model.FmSectionTypes
{
    [TestFixture]
    public class EFmSectionCategoryTest
    {
        [Test]
        public void TestEnumContract()
        {
            Assert.AreEqual(9, Enum.GetValues(typeof(EFmSectionCategory)).Length);
            Assert.AreEqual(-1, (int) EFmSectionCategory.NotApplicable);
            Assert.AreEqual(1, (int) EFmSectionCategory.Iv);
            Assert.AreEqual(2, (int) EFmSectionCategory.IIv);
            Assert.AreEqual(3, (int) EFmSectionCategory.IIIv);
            Assert.AreEqual(4, (int) EFmSectionCategory.IVv);
            Assert.AreEqual(5, (int) EFmSectionCategory.Vv);
            Assert.AreEqual(6, (int) EFmSectionCategory.VIv);
            Assert.AreEqual(7, (int) EFmSectionCategory.VIIv);
            Assert.AreEqual(8, (int) EFmSectionCategory.Gr);
            Assert.Greater(EFmSectionCategory.IIv, EFmSectionCategory.Iv);
        }
    }
}