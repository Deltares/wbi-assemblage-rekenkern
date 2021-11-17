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

using System.Collections;
using Assembly.Kernel.Implementations;
using Assembly.Kernel.Interfaces;
using Assembly.Kernel.Model;
using Assembly.Kernel.Model.AssessmentResultTypes;
using Assembly.Kernel.Model.FmSectionTypes;
using NUnit.Framework;

namespace Assembly.Kernel.Tests.Implementations
{
    [TestFixture]
    public class AssessmentResultsTranslatorAssessmentResultTests
    {
        private IAssessmentResultsTranslator translator;

        [SetUp]
        public void Init()
        {
            translator = new AssessmentResultsTranslator();
        }

        [Test, TestCaseSource(
             typeof(AssessmentResultTestCases),
             nameof(AssessmentResultTestCases.Wbi0T1))]
        public EFmSectionCategory Wbi0T1Test(EAssessmentResultTypeT1 assessmentResult)
        {
            FmSectionAssemblyDirectResult result = translator.TranslateAssessmentResultWbi0T1(assessmentResult);
            Assert.NotNull(result);
            Assert.IsAssignableFrom<FmSectionAssemblyDirectResult>(result);

            return result.Result;
        }

        [Test, TestCaseSource(
             typeof(AssessmentResultTestCases),
             nameof(AssessmentResultTestCases.Wbi0T2))]
        public EIndirectAssessmentResult Wbi0T2Test(EAssessmentResultTypeT2 assessmentResult)
        {
            var result = translator.TranslateAssessmentResultWbi0T2(assessmentResult);
            Assert.NotNull(result);
            Assert.IsAssignableFrom<FmSectionAssemblyIndirectResult>(result);

            return result.Result;
        }

        private class AssessmentResultTestCases
        {
            public static IEnumerable Wbi0T1
            {
                get
                {
                    yield return new TestCaseData(EAssessmentResultTypeT1.V).Returns(EFmSectionCategory.IIv);
                    yield return new TestCaseData(EAssessmentResultTypeT1.Vn).Returns(EFmSectionCategory.Vv);
                    yield return new TestCaseData(EAssessmentResultTypeT1.Ngo).Returns(EFmSectionCategory.VIIv);
                    yield return new TestCaseData(EAssessmentResultTypeT1.Fv).Returns(EFmSectionCategory.Iv);
                    yield return new TestCaseData(EAssessmentResultTypeT1.Gr).Returns(EFmSectionCategory.Gr);
                }
            }

            public static IEnumerable Wbi0T2
            {
                get
                {
                    yield return new TestCaseData(EAssessmentResultTypeT2.V).Returns(EIndirectAssessmentResult.FvTom);
                    yield return new TestCaseData(EAssessmentResultTypeT2.Vn).Returns(EIndirectAssessmentResult.Ngo);
                    yield return new TestCaseData(EAssessmentResultTypeT2.Ngo).Returns(EIndirectAssessmentResult.Ngo);
                    yield return new TestCaseData(EAssessmentResultTypeT2.Fv).Returns(EIndirectAssessmentResult.FvTom);
                    yield return new TestCaseData(EAssessmentResultTypeT2.Verd).Returns(EIndirectAssessmentResult.FactoredInOtherFailureMechanism);
                }
            }
        }
    }
}