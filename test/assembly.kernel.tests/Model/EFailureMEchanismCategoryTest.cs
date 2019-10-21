#region Copyright (C) Rijkswaterstaat 2019. All rights reserved
// Copyright (C) Rijkswaterstaat 2019. All rights reserved.
//
// This file is part of the Assembly kernel.
//
// Assembly kernel is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program. If not, see <http://www.gnu.org/licenses/>.
//
// All names, logos, and references to "Rijkswaterstaat" are registered trademarks of
// Rijkswaterstaat and remain full property of Rijkswaterstaat at all times.
// All rights reserved.
#endregion

using System;
using Assembly.Kernel.Model;
using NUnit.Framework;

namespace Assembly.Kernel.Tests.Model
{
    [TestFixture]
    public class EFailureMEchanismCategoryTest
    {
        [Test]
        public void TestEnumContract()
        {
            Assert.AreEqual(9, Enum.GetValues(typeof(EFailureMechanismCategory)).Length);
            Assert.AreEqual(-1, (int) EFailureMechanismCategory.Nvt);
            Assert.AreEqual(1, (int) EFailureMechanismCategory.It);
            Assert.AreEqual(2, (int) EFailureMechanismCategory.IIt);
            Assert.AreEqual(3, (int) EFailureMechanismCategory.IIIt);
            Assert.AreEqual(4, (int) EFailureMechanismCategory.IVt);
            Assert.AreEqual(5, (int) EFailureMechanismCategory.Vt);
            Assert.AreEqual(6, (int) EFailureMechanismCategory.VIt);
            Assert.AreEqual(7, (int) EFailureMechanismCategory.VIIt);
            Assert.AreEqual(8, (int) EFailureMechanismCategory.Gr);
            Assert.Greater(EFailureMechanismCategory.IIt, EFailureMechanismCategory.It);
        }
    }
}