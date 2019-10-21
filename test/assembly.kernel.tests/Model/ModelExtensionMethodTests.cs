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

using Assembly.Kernel.Model;
using Assembly.Kernel.Model.FmSectionTypes;
using NUnit.Framework;

namespace Assembly.Kernel.Tests
{
    [TestFixture]
    public class ModelExtensionMethodTests
    {
        [Test]
        public void ToAssessmentGradeTests()
        {
            Assert.AreEqual(EAssessmentGrade.APlus, EFailureMechanismCategory.It.ToAssessmentGrade());
            Assert.AreEqual(EAssessmentGrade.A, EFailureMechanismCategory.IIt.ToAssessmentGrade());
            Assert.AreEqual(EAssessmentGrade.B, EFailureMechanismCategory.IIIt.ToAssessmentGrade());
            Assert.AreEqual(EAssessmentGrade.C, EFailureMechanismCategory.IVt.ToAssessmentGrade());
            Assert.AreEqual(EAssessmentGrade.C, EFailureMechanismCategory.Vt.ToAssessmentGrade());
            Assert.AreEqual(EAssessmentGrade.D, EFailureMechanismCategory.VIt.ToAssessmentGrade());
            Assert.AreEqual(EAssessmentGrade.Ngo, EFailureMechanismCategory.VIIt.ToAssessmentGrade());
            Assert.AreEqual(EAssessmentGrade.Nvt, EFailureMechanismCategory.Nvt.ToAssessmentGrade());
            Assert.AreEqual(EAssessmentGrade.Gr, EFailureMechanismCategory.Gr.ToAssessmentGrade());
        }

        [Test]
        public void ToFailureMechanismCategoryTests()
        {
            Assert.AreEqual(EFailureMechanismCategory.It, EFmSectionCategory.Iv.ToAssessmentGrade());
            Assert.AreEqual(EFailureMechanismCategory.IIt, EFmSectionCategory.IIv.ToAssessmentGrade());
            Assert.AreEqual(EFailureMechanismCategory.IIIt, EFmSectionCategory.IIIv.ToAssessmentGrade());
            Assert.AreEqual(EFailureMechanismCategory.IVt, EFmSectionCategory.IVv.ToAssessmentGrade());
            Assert.AreEqual(EFailureMechanismCategory.Vt, EFmSectionCategory.Vv.ToAssessmentGrade());
            Assert.AreEqual(EFailureMechanismCategory.VIt, EFmSectionCategory.VIv.ToAssessmentGrade());
            Assert.AreEqual(EFailureMechanismCategory.VIIt, EFmSectionCategory.VIIv.ToAssessmentGrade());
            Assert.AreEqual(EFailureMechanismCategory.Gr, EFmSectionCategory.Gr.ToAssessmentGrade());
            Assert.AreEqual(EFailureMechanismCategory.Nvt, EFmSectionCategory.NotApplicable.ToAssessmentGrade());
        }
    }
}