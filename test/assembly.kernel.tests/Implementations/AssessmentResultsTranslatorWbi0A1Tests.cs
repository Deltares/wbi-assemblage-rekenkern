#region Copyright (c) 2018 Technolution BV. All Rights Reserved. 

// // Copyright (C) Technolution BV. 2018. All rights reserved.
// //
// // This file is part of the Assembly kernel.
// //
// // Assembly kernel is free software: you can redistribute it and/or modify
// // it under the terms of the GNU Lesser General Public License as published by
// // the Free Software Foundation, either version 3 of the License, or
// // (at your option) any later version.
// // 
// // This program is distributed in the hope that it will be useful,
// // but WITHOUT ANY WARRANTY; without even the implied warranty of
// // MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// // GNU Lesser General Public License for more details.
// //
// // You should have received a copy of the GNU Lesser General Public License
// // along with this program. If not, see <http://www.gnu.org/licenses/>.
// //
// // All names, logos, and references to "Technolution BV" are registered trademarks of
// // Technolution BV and remain full property of Technolution BV at all times.
// // All rights reserved.

#endregion

using System.Collections;
using System.Linq;
using Assembly.Kernel.Exceptions;
using Assembly.Kernel.Implementations;
using Assembly.Kernel.Interfaces;
using Assembly.Kernel.Model;
using Assembly.Kernel.Model.FmSectionTypes;
using NUnit.Framework;

// ReSharper disable UnusedMember.Local

namespace Assembly.Kernel.Tests.Implementations
{
    public class AssessmentResultsTranslatorWbi0A1Tests
    {
        private IAssessmentResultsTranslator translator;

        [SetUp]
        public void Init()
        {
            translator = new AssessmentResultsTranslator();
        }

        [Test, TestCaseSource(
             typeof(Wbi0A1TestCases),
             nameof(Wbi0A1TestCases.Wbi0A1Direct))]
        public EFmSectionCategory Wbi0A1DirectTest(
            EFmSectionCategory? simpleAssessmentResult,
            EFmSectionCategory? detailedAssessmentResult,
            EFmSectionCategory? customAssessmentResult)
        {
            var result = (FmSectionAssemblyDirectResult) translator.TranslateAssessmentResultWbi0A1(
                simpleAssessmentResult == null ? null : new FmSectionAssemblyDirectResult(simpleAssessmentResult.Value),
                detailedAssessmentResult == null
                    ? null
                    : new FmSectionAssemblyDirectResult(detailedAssessmentResult.Value),
                customAssessmentResult == null
                    ? null
                    : new FmSectionAssemblyDirectResult(customAssessmentResult.Value));

            Assert.IsNotNull(result);
            return result.Result;
        }

        [Test]
        public void Wbi0A1DirectNullTest()
        {
            try
            {
                translator.TranslateAssessmentResultWbi0A1(
                    (FmSectionAssemblyDirectResult) null,
                    null,
                    null);
            }
            catch (AssemblyException e)
            {
                Assert.NotNull(e.Errors);
                var message = e.Errors.FirstOrDefault();
                Assert.NotNull(message);
                Assert.AreEqual(EAssemblyErrors.ValueMayNotBeNull, message.ErrorCode);
                Assert.Pass();
            }

            Assert.Fail("No expected exception not thrown");
        }

        [Test, TestCaseSource(
             typeof(Wbi0A1TestCases),
             nameof(Wbi0A1TestCases.Wbi0A1Indirect))]
        public EIndirectAssessmentResult Wbi0A1IndirectTest(
            EIndirectAssessmentResult? simpleAssessmentResult,
            EIndirectAssessmentResult? detailedAssessmentResult,
            EIndirectAssessmentResult? customAssessmentResult)
        {
            var result = (FmSectionAssemblyIndirectResult) translator.TranslateAssessmentResultWbi0A1(
                simpleAssessmentResult == null
                    ? null
                    : new FmSectionAssemblyIndirectResult(simpleAssessmentResult.Value),
                detailedAssessmentResult == null
                    ? null
                    : new FmSectionAssemblyIndirectResult(detailedAssessmentResult.Value),
                customAssessmentResult == null
                    ? null
                    : new FmSectionAssemblyIndirectResult(customAssessmentResult.Value));

            Assert.IsNotNull(result);
            return result.Result;
        }

        [Test]
        public void Wbi0A1IndirectNullTest()
        {
            try
            {
                translator.TranslateAssessmentResultWbi0A1(
                    (FmSectionAssemblyIndirectResult) null,
                    null,
                    null);
            }
            catch (AssemblyException e)
            {
                Assert.NotNull(e.Errors);
                var message = e.Errors.FirstOrDefault();
                Assert.NotNull(message);
                Assert.AreEqual(EAssemblyErrors.ValueMayNotBeNull, message.ErrorCode);
                Assert.Pass();
            }

            Assert.Fail("No expected exception not thrown");
        }

        private sealed class Wbi0A1TestCases
        {
            public static IEnumerable Wbi0A1Direct
            {
                get
                {
                    yield return new TestCaseData(
                            EFmSectionCategory.IIIv,
                            EFmSectionCategory.IIv,
                            EFmSectionCategory.Iv)
                        .Returns(EFmSectionCategory.Iv);

                    yield return new TestCaseData(
                            EFmSectionCategory.IIIv,
                            EFmSectionCategory.IIv,
                            null)
                        .Returns(EFmSectionCategory.IIv);

                    yield return new TestCaseData(
                            EFmSectionCategory.IIIv,
                            null,
                            EFmSectionCategory.Iv)
                        .Returns(EFmSectionCategory.Iv);

                    yield return new TestCaseData(
                            EFmSectionCategory.IIIv,
                            null,
                            null)
                        .Returns(EFmSectionCategory.IIIv);

                    yield return new TestCaseData(
                            EFmSectionCategory.Gr,
                            null,
                            null)
                        .Returns(EFmSectionCategory.Gr);

                    yield return new TestCaseData(
                            EFmSectionCategory.Gr,
                            EFmSectionCategory.Gr,
                            EFmSectionCategory.Gr)
                        .Returns(EFmSectionCategory.Gr);
                }
            }

            public static IEnumerable Wbi0A1Indirect
            {
                get
                {
                    yield return new TestCaseData(
                            EIndirectAssessmentResult.FvEt,
                            EIndirectAssessmentResult.FactoredInOtherFailureMechanism,
                            EIndirectAssessmentResult.FvGt)
                        .Returns(EIndirectAssessmentResult.FvGt);

                    yield return new TestCaseData(
                            EIndirectAssessmentResult.FvEt,
                            EIndirectAssessmentResult.FactoredInOtherFailureMechanism,
                            null)
                        .Returns(EIndirectAssessmentResult.FactoredInOtherFailureMechanism);

                    yield return new TestCaseData(
                            EIndirectAssessmentResult.FvEt,
                            null,
                            EIndirectAssessmentResult.FvGt)
                        .Returns(EIndirectAssessmentResult.FvGt);

                    yield return new TestCaseData(
                            EIndirectAssessmentResult.FvEt,
                            null,
                            null)
                        .Returns(EIndirectAssessmentResult.FvEt);

                    yield return new TestCaseData(
                            EIndirectAssessmentResult.Gr,
                            null,
                            null)
                        .Returns(EIndirectAssessmentResult.Gr);

                    yield return new TestCaseData(
                            EIndirectAssessmentResult.Gr,
                            EIndirectAssessmentResult.Gr,
                            EIndirectAssessmentResult.Gr)
                        .Returns(EIndirectAssessmentResult.Gr);
                }
            }
        }
    }
}