#region Copyright (C) Rijkswaterstaat 2022. All rights reserved

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

#endregion

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Assembly.Kernel.Implementations;
using Assembly.Kernel.Interfaces;
using Assembly.Kernel.Model;
using Assembly.Kernel.Model.AssessmentSection;
using Assembly.Kernel.Model.Categories;
using Assembly.Kernel.Model.FailureMechanismSections;
using NUnit.Framework;

namespace Assembly.Kernel.Tests
{
    [TestFixture]
    public class AssemblyPerformanceTests
    {
        private readonly IFailureMechanismResultAssembler failureMechanismResultAssembler = new FailureMechanismResultAssembler();
        private readonly IAssessmentGradeAssembler assessmentSectionAssembler = new AssessmentGradeAssembler();

        private readonly ICommonFailureMechanismSectionAssembler combinedSectionAssembler =
            new CommonFailureMechanismSectionAssembler();

        [Test]
        public void FullAssembly()
        {
            const int sectionLength = 3750;
            var section = new AssessmentSection((Probability) 1.0E-3, (Probability) (1.0 / 300.0));
            var failureMechanismSectionResultsDictionary = new Dictionary<double, List<FailureMechanismSection>>();
            var failureMechanismSectionLists = new List<FailureMechanismSectionList>();

            // create section results for 15 failure mechanisms with 250 sections each
            CreateTestInput(sectionLength, failureMechanismSectionResultsDictionary);

            // start timer
            var watch = Stopwatch.StartNew();

            var failureMechanismResultsWithFailureProb = new List<FailureMechanismAssemblyResult>();

            // assembly step 1
            var categoriesCalculator = new CategoryLimitsCalculator();
            foreach (var failureMechanismSectionResults in failureMechanismSectionResultsDictionary)
            {
                var result = failureMechanismResultAssembler.AssembleFailureMechanismWbi1B1(
                    failureMechanismSectionResults.Key,
                    failureMechanismSectionResults.Value.Select(failureMechanismSection => failureMechanismSection.Result),
                    false);
                failureMechanismResultsWithFailureProb.Add(result);

                failureMechanismSectionLists.Add(CreateFailureMechanismSectionListForStep3(failureMechanismSectionResults.Value));
            }

            // assembly step 2
            var categories = categoriesCalculator.CalculateAssessmentSectionCategoryLimitsBoi21(section);
            var assessmentGradeWithFailureProb =
                assessmentSectionAssembler.CalculateAssessmentSectionFailureProbabilityBoi2B1(failureMechanismResultsWithFailureProb.Select(r => r.Probability).ToArray(), categories, false);

            // assembly step 3
            combinedSectionAssembler.AssembleCommonFailureMechanismSections(failureMechanismSectionLists, sectionLength,false);
            watch.Stop();

            var elapsedMs = watch.Elapsed.TotalMilliseconds;
            Console.Out.WriteLine($"Elapsed time since start of assembly: {elapsedMs} ms (max: 1000 ms)");
        }

        private static void CreateTestInput(int sectionLength, IDictionary<double, List<FailureMechanismSection>> withFailureProbabilities)
        {
            for (var i = 1; i <= 15; i++)
            {
                var failureMechanismSections = new List<FailureMechanismSection>();

                var sectionLengthRemaining = sectionLength;
                for (var k = 0; k < 250; k++)
                {
                    var sectionStart = sectionLengthRemaining / (250 - k) * k;
                    var sectionEnd = sectionLengthRemaining / (250 - k) * (k + 1);
                    failureMechanismSections.Add(
                        new FailureMechanismSection(
                            new FailureMechanismSectionAssemblyResult((Probability)5.0E-5, (Probability)1.0E-4, EInterpretationCategory.I),
                            $"TEST{i}F",
                            sectionStart,
                            sectionEnd));

                    sectionLengthRemaining -= sectionEnd - sectionStart;
                }

                withFailureProbabilities.Add(i, failureMechanismSections);
            }
        }

        private static FailureMechanismSectionList CreateFailureMechanismSectionListForStep3(
            List<FailureMechanismSection> failureMechanismSectionResults)
        {
            var failureMechanismSectionList = new FailureMechanismSectionList(failureMechanismSectionResults.Select(failureMechanismSection =>
                                                                                            new FailureMechanismSectionWithCategory(
                                                                                                failureMechanismSection.SectionStart,
                                                                                                failureMechanismSection.SectionEnd,
                                                                                                failureMechanismSection.Result.InterpretationCategory)));
            return failureMechanismSectionList;
        }

        private sealed class FailureMechanismSection
        {
            public FailureMechanismSection(FailureMechanismSectionAssemblyResult result, string failureMechanismType, double sectionStart,
                             double sectionEnd)
            {
                Result = result;
                FailureMechanismType = failureMechanismType;
                SectionStart = sectionStart;
                SectionEnd = sectionEnd;
            }

            public FailureMechanismSectionAssemblyResult Result { get; }

            public string FailureMechanismType { get; }

            public double SectionStart { get; }

            public double SectionEnd { get; }
        }
    }
}