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
        private readonly IFailureMechanismResultAssembler fmAssembler = new FailureMechanismResultAssembler();
        private readonly IAssessmentGradeAssembler assessmentSectionAssembler = new AssessmentGradeAssembler();

        private readonly ICommonFailureMechanismSectionAssembler combinedSectionAssembler =
            new CommonFailureMechanismSectionAssembler();

        private static void CreateTestInput(int sectionLength,
            IDictionary<FailureMechanism, List<FmSection>> withoutFailureProb,
            IDictionary<FailureMechanism, List<FmSection>> withFailureProb)
        {
            for (var i = 1; i <= 15; i++)
            {
                var failureMechanism = new FailureMechanism(i, 0.01 + i / 100.0);
                var failureMechanismSections = new List<FmSection>();

                var sectionLengthRemaining = sectionLength;
                for (var k = 0; k < 250; k++)
                {
                    var sectionStart = sectionLengthRemaining / (250 - k) * k;
                    var sectionEnd = sectionLengthRemaining / (250 - k) * (k + 1);
                    if (i < 3)
                    {
                        failureMechanismSections.Add(
                            new FmSection(
                                new FmSectionAssemblyDirectResult(EFmSectionCategory.IIIv),
                                $"TEST{i}",
                                sectionStart,
                                sectionEnd));
                    }
                    else
                    {
                        failureMechanismSections.Add(
                            new FmSection(
                                new FmSectionAssemblyDirectResultWithProbability(EFmSectionCategory.IIIv, 0.002),
                                $"TEST{i}F",
                                sectionStart,
                                sectionEnd));
                    }

                    sectionLengthRemaining -= sectionEnd - sectionStart;
                }

                if (i < 3)
                {
                    withoutFailureProb.Add(failureMechanism, failureMechanismSections);
                }
                else
                {
                    withFailureProb.Add(failureMechanism, failureMechanismSections);
                }
            }
        }

        private static FailureMechanismSectionList CreateFailureMechanismSectionListForStep3(
            List<FmSection> fmSectionResults)
        {
            var fmsectionList = new FailureMechanismSectionList(fmSectionResults.FirstOrDefault()?.FmType,
                fmSectionResults.Select(fmsection =>
                    new FmSectionWithDirectCategory(fmsection.SectionStart, fmsection.SectionEnd,
                        fmsection.Result.Result)));
            return fmsectionList;
        }

        private sealed class FmSection
        {
            public FmSection(FmSectionAssemblyDirectResult result, string fmType, double sectionStart,
                double sectionEnd)
            {
                Result = result;
                FmType = fmType;
                SectionStart = sectionStart;
                SectionEnd = sectionEnd;
            }

            public FmSectionAssemblyDirectResult Result { get; }

            public string FmType { get; }

            public double SectionStart { get; }

            public double SectionEnd { get; }
        }

        [Test]
        public void FullAssembly()
        {
            const int sectionLength = 3750;
            var section = new AssessmentSection(sectionLength, 1.0E-3, 1.0 / 300.0);
            var withoutFailureProb = new Dictionary<FailureMechanism, List<FmSection>>();
            var withFailureProb = new Dictionary<FailureMechanism, List<FmSection>>();
            var failureMechanismSectionLists = new List<FailureMechanismSectionList>();

            // create section results for 15 failure mechanisms with 250 sections each
            CreateTestInput(sectionLength, withoutFailureProb, withFailureProb);

            // start timer
            var watch = Stopwatch.StartNew();

            var failureMechanismResultsWithFailureProb = new List<FailureMechanismAssemblyResult>();
            var failureMechanismResultsWithoutFailureProb = new List<FailureMechanismAssemblyResult>();

            // assembly step 1
            foreach (KeyValuePair<FailureMechanism, List<FmSection>> fmSectionResults in withoutFailureProb)
            {
                var result = fmAssembler.AssembleFailureMechanismWbi1A1(
                    fmSectionResults.Value.Select(fmSection => fmSection.Result),
                    false);
                failureMechanismResultsWithoutFailureProb.Add(new FailureMechanismAssemblyResult(result, double.NaN));

                failureMechanismSectionLists.Add(CreateFailureMechanismSectionListForStep3(fmSectionResults.Value));
            }

            var categoriesCalculator = new CategoryLimitsCalculator();
            foreach (KeyValuePair<FailureMechanism, List<FmSection>> fmSectionResults in withFailureProb)
            {
                var categoriesList =
                    categoriesCalculator.CalculateFailureMechanismCategoryLimitsWbi11(section, fmSectionResults.Key);

                var result = fmAssembler.AssembleFailureMechanismWbi1B1(
                    fmSectionResults.Key,
                    fmSectionResults.Value.Select(fmSection =>
                        (FmSectionAssemblyDirectResultWithProbability) fmSection.Result),
                    categoriesList,
                    false);
                failureMechanismResultsWithFailureProb.Add(result);

                failureMechanismSectionLists.Add(CreateFailureMechanismSectionListForStep3(fmSectionResults.Value));
            }

            // assembly step 2

            var assessmentGradeWithoutFailureProb =
                assessmentSectionAssembler.AssembleAssessmentSectionWbi2A1(failureMechanismResultsWithoutFailureProb,
                    false);
            var assessmentGradeWithFailureProb =
                assessmentSectionAssembler.AssembleAssessmentSectionWbi2B1(failureMechanismResultsWithFailureProb,
                    categoriesCalculator.CalculateFailureMechanismCategoryLimitsWbi11(section,
                        new FailureMechanism(1.0, 0.7)), false);

            assessmentSectionAssembler.AssembleAssessmentSectionWbi2C1(assessmentGradeWithoutFailureProb,
                assessmentGradeWithFailureProb);

            // assembly step 3
            combinedSectionAssembler.AssembleCommonFailureMechanismSections(failureMechanismSectionLists, sectionLength,
                false);
            watch.Stop();

            var elapsedMs = watch.Elapsed.TotalMilliseconds;
            Console.Out.WriteLine($"Elapsed time since start of assembly: {elapsedMs} ms (max: 1000 ms)");
        }
    }
}