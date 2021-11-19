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
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Assembly.Kernel.Implementations;
using Assembly.Kernel.Interfaces;
using Assembly.Kernel.Model;
using Assembly.Kernel.Model.FmSectionTypes;
using NUnit.Framework;

namespace Assembly.Kernel.Tests
{
    [TestFixture]
    public class AssemblyPerformanceTests
    {
        private readonly IFailurePathResultAssembler fmAssembler = new FailurePathResultAssembler();
        private readonly IAssessmentGradeAssembler assessmentSectionAssembler = new AssessmentGradeAssembler();

        private readonly ICommonFailureMechanismSectionAssembler combinedSectionAssembler =
            new CommonFailureMechanismSectionAssembler();

        [Test]
        public void FullAssembly()
        {
            const int sectionLength = 3750;
            var section = new AssessmentSection(sectionLength, 1.0E-3, 1.0 / 300.0);
            var fpSectionResultsDictionary = new Dictionary<FailurePath, List<FpSection>>();
            // TODO: Temp disable and wait for implementation of ASK-36 and ASK37
            //var failureMechanismSectionLists = new List<FailurePathSectionList>();

            // create section results for 15 failure mechanisms with 250 sections each
            CreateTestInput(sectionLength, fpSectionResultsDictionary);

            // start timer
            var watch = Stopwatch.StartNew();

            var failureMechanismResultsWithFailureProb = new List<FailurePathAssemblyResult>();

            // assembly step 1
            var categoriesCalculator = new CategoryLimitsCalculator();
            foreach (KeyValuePair<FailurePath, List<FpSection>> fpSectionResults in fpSectionResultsDictionary)
            {
                var result = fmAssembler.AssembleFailurePathWbi1B1(
                    fpSectionResults.Key,
                    fpSectionResults.Value.Select(fpSection => fpSection.Result),
                    false);
                failureMechanismResultsWithFailureProb.Add(result);

                // TODO: Temp disable and wait for implementation of ASK-36 and ASK37
                //failureMechanismSectionLists.Add(CreateFailureMechanismSectionListForStep3(fpSectionResults.Value));
            }

            // assembly step 2
            var categories = categoriesCalculator.CalculateAssessmentSectionCategoryLimitsWbi21(section);
            var assessmentGradeWithFailureProb =
                assessmentSectionAssembler.AssembleAssessmentSectionWbi2B1(failureMechanismResultsWithFailureProb, categories, false);

            // assembly step 3
            // TODO: Temp disable and wait for implementation of ASK-36 and ASK37
            //combinedSectionAssembler.AssembleCommonFailureMechanismSections(failureMechanismSectionLists, sectionLength,false);
            watch.Stop();

            var elapsedMs = watch.Elapsed.TotalMilliseconds;
            Console.Out.WriteLine($"Elapsed time since start of assembly: {elapsedMs} ms (max: 1000 ms)");
        }

        private static void CreateTestInput(int sectionLength,
            IDictionary<FailurePath, List<FpSection>> WithFailureProbabilities)
        {
            for (var i = 1; i <= 15; i++)
            {
                var failureMechanism = new FailurePath(i, 0.01 + i / 100.0);
                var failureMechanismSections = new List<FpSection>();

                var sectionLengthRemaining = sectionLength;
                for (var k = 0; k < 250; k++)
                {
                    var sectionStart = sectionLengthRemaining / (250 - k) * k;
                    var sectionEnd = sectionLengthRemaining / (250 - k) * (k + 1);
                    failureMechanismSections.Add(
                        new FpSection(
                            new FpSectionAssemblyResult(0.002, 1.0E-4, EInterpretationCategory.I),
                            $"TEST{i}F",
                            sectionStart,
                            sectionEnd));

                    sectionLengthRemaining -= sectionEnd - sectionStart;
                }

                WithFailureProbabilities.Add(failureMechanism, failureMechanismSections);
            }
        }

        // TODO: Temp disable and wait for implementation of ASK-36 and ASK37
        /*private static FailurePathSectionList CreateFailureMechanismSectionListForStep3(
            List<FpSection> fmSectionResults)
        {
            var fmsectionList = new FailurePathSectionList(fmSectionResults.FirstOrDefault()?.FmType,
                                                                fmSectionResults.Select(fmsection =>
                                                                                            new FailurePathSectionWithResult(
                                                                                                fmsection.SectionStart,
                                                                                                fmsection.SectionEnd,
                                                                                                fmsection.Result.)));
            return fmsectionList;
        }*/

        private sealed class FpSection
        {
            public FpSection(FpSectionAssemblyResult result, string fmType, double sectionStart,
                             double sectionEnd)
            {
                Result = result;
                FmType = fmType;
                SectionStart = sectionStart;
                SectionEnd = sectionEnd;
            }

            public FpSectionAssemblyResult Result { get; }

            public string FmType { get; }

            public double SectionStart { get; }

            public double SectionEnd { get; }
        }
    }
}