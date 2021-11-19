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
using System.Collections;
using System.Linq;
using Assembly.Kernel.Implementations;
using Assembly.Kernel.Interfaces;
using Assembly.Kernel.Model;
using Assembly.Kernel.Model.FmSectionTypes;
using NUnit.Framework;

namespace Assembly.Kernel.Tests.Implementations
{
    [TestFixture]
    public class FailureMechanismResultAssemblerTests
    {
        private readonly FailurePath testFailurePath1 = new FailurePath(14.4);
        private readonly FailurePath testFailureMechanism2 = new FailurePath(10);
        private IFailurePathResultAssembler assembler;

        [SetUp]
        public void Init()
        {
            assembler = new FailurePathResultAssembler();
        }

        [Test, TestCaseSource(typeof(AssembleFailurePathTestData), nameof(AssembleFailurePathTestData.Wbi1B1))]
        public void Wbi1B1FailureProbabilityTests(Tuple<double,double>[] failureProbabilities,
                                                  EAssemblyType assemblyType, double expectedResult)
        {
            // Use correct probabilities
            var result = assembler.AssembleFailurePathWbi1B1(testFailurePath1,
                                                                  failureProbabilities.Select(failureProbability =>
                                                                                                  new
                                                                                                      FpSectionAssemblyResult(
                                                                                                          failureProbability.Item1, failureProbability.Item2, EInterpretationCategory.III)),
                                                                  assemblyType == EAssemblyType.Partial);

            Assert.NotNull(result.FailureProbability);
            Assert.AreEqual(expectedResult, result.FailureProbability, 1e-10);
        }

        [Test]
        public void Wbi1B1LengthEffectFactor()
        {
            var result = assembler.AssembleFailurePathWbi1B1(new FailurePath(5),
                                                                  new[]
                                                                  {
                                                                      new FpSectionAssemblyResult(0.001, 0.001, EInterpretationCategory.II),
                                                                      new FpSectionAssemblyResult(0.001, 0.001, EInterpretationCategory.II),
                                                                      new FpSectionAssemblyResult(0.001, 0.001, EInterpretationCategory.IIIMin),
                                                                      new FpSectionAssemblyResult(0.001, 0.001, EInterpretationCategory.IMin),
                                                                      new FpSectionAssemblyResult(0.001, 0.001, EInterpretationCategory.IIMin),
                                                                      new FpSectionAssemblyResult(0.001, 0.001, EInterpretationCategory.IIIMin)
                                                                  },
                                                                  false);

            Assert.NotNull(result.FailureProbability);
            Assert.AreEqual(0.005, result.FailureProbability, 1e-4);
        }

        [Test]
        public void Wbi1B1NoResultPartly()
        {
            var result = assembler.AssembleFailurePathWbi1B1(testFailureMechanism2,
                                                                  new[]
                                                                  {
                                                                      new FpSectionAssemblyResult(double.NaN, double.NaN, EInterpretationCategory.IMin),
                                                                      new FpSectionAssemblyResult(0.00026, 0.00026, EInterpretationCategory.D),
                                                                      new FpSectionAssemblyResult(0.00026, 0.00026, EInterpretationCategory.D)
                                                                  },
                                                                  false);

            Assert.IsNaN(result.FailureProbability);
        }

        [Test]
        public void Wbi1B1NoResult()
        {
            var result = assembler.AssembleFailurePathWbi1B1(testFailureMechanism2,
                                                                  new[]
                                                                  {
                                                                      new FpSectionAssemblyResult(double.NaN, double.NaN, EInterpretationCategory.D),
                                                                      new FpSectionAssemblyResult(double.NaN, double.NaN, EInterpretationCategory.D),
                                                                      new FpSectionAssemblyResult(double.NaN, double.NaN, EInterpretationCategory.D)
                                                                  },
                                                                  false);

            Assert.IsNaN(result.FailureProbability);
        }

        [Test]
        public void Wbi1B1NotApplicable()
        {
            var result = assembler.AssembleFailurePathWbi1B1(testFailureMechanism2,
                                                                  new[]
                                                                  {
                                                                      new FpSectionAssemblyResult(0.0, 0.0, EInterpretationCategory.D),
                                                                      new FpSectionAssemblyResult(0.0, 0.0, EInterpretationCategory.D),
                                                                      new FpSectionAssemblyResult(0.0, 0.0, EInterpretationCategory.D)
                                                                  },
                                                                  false);

            Assert.AreEqual(0.0, result.FailureProbability);
        }

        [Test]
        public void Wbi1B1Partial()
        {
            var result = assembler.AssembleFailurePathWbi1B1(testFailureMechanism2,
                                                                  new[]
                                                                  {
                                                                      new FpSectionAssemblyResult(0.9, 0.9, EInterpretationCategory.D),
                                                                      new FpSectionAssemblyResult(0.000026, 0.000026, EInterpretationCategory.D),
                                                                      new FpSectionAssemblyResult(0.000010, 0.000010, EInterpretationCategory.II),
                                                                      new FpSectionAssemblyResult(0.0000011, 0.0000011, EInterpretationCategory.II),
                                                                      new FpSectionAssemblyResult(0.000015, 0.000015, EInterpretationCategory.II),
                                                                      new FpSectionAssemblyResult(0.000009, 0.000009, EInterpretationCategory.III),
                                                                      new FpSectionAssemblyResult(double.NaN, double.NaN, EInterpretationCategory.III),
                                                                      new FpSectionAssemblyResult(0.0, 0.0, EInterpretationCategory.IIIMin)
                                                                  },
                                                                  true);

            Assert.NotNull(result.FailureProbability);
            Assert.AreEqual(0.9, result.FailureProbability, 1e-4);
        }

        public enum EAssemblyType
        {
            Full,
            Partial
        }

        public class AssembleFailurePathTestData
        {
            public static IEnumerable Wbi1B1
            {
                get
                {
                    yield return new TestCaseData(
                        new[]
                        {
                            new Tuple<double, double>(0.0,0.0), 
                            new Tuple<double, double>(0.1, 0.1)
                        },
                        EAssemblyType.Full,
                        0.1);

                    yield return new TestCaseData(
                        new[]
                        {
                            new Tuple<double, double>(0.0,0.0),
                            new Tuple<double, double>(0.01, 0.0001),
                            new Tuple<double, double>(0.01,0.0005),
                            new Tuple<double, double>(0.01, 0.0007)
                        },
                        EAssemblyType.Full,
                        0.0012995300);

                    yield return new TestCaseData(
                        new[]
                        {
                            new Tuple<double, double>(0.0005,0.0005),
                            new Tuple<double, double>(0.00005,0.00005)
                        },
                        EAssemblyType.Full,
                        0.000549975);
                }
            }
        }
    }
}