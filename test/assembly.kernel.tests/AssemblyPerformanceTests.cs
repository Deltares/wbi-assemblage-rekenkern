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
        const double SectionLength = 3750.0;

        private readonly IFailureMechanismResultAssembler failureMechanismResultAssembler = new FailureMechanismResultAssembler();
        private readonly IAssessmentGradeAssembler assessmentSectionAssembler = new AssessmentGradeAssembler();
        private readonly ICommonFailureMechanismSectionAssembler combinedSectionAssembler =
            new CommonFailureMechanismSectionAssembler();
        private Dictionary<double, List<FailureMechanismSection>> failureMechanismSectionResultsDictionary;

        [SetUp]
        public void Setup()
        {
            CreateTestInput();
        }

        [Test]
        [Timeout(1000)]
        public void FullAssembly()
        {
            var watch = Stopwatch.StartNew();

            var section = new AssessmentSection((Probability) 1.0E-3, (Probability) (1.0 / 300.0));
            var failureMechanismSectionLists = new List<FailureMechanismSectionList>();

            var failureMechanismResultsWithFailureProb = new List<FailureMechanismAssemblyResult>();

            AssembleFailureProbabilitiesPerFailureMechanism(failureMechanismResultsWithFailureProb, failureMechanismSectionLists);

            CalculateAssessmentGrade(section, failureMechanismResultsWithFailureProb);

            combinedSectionAssembler.AssembleCommonFailureMechanismSections(failureMechanismSectionLists, SectionLength,false);

            watch.Stop();

            Console.Out.WriteLine($"Elapsed time since start of assembly: {watch.Elapsed.TotalMilliseconds} ms (max: 1000 ms)");
        }

        private void CalculateAssessmentGrade(AssessmentSection section, List<FailureMechanismAssemblyResult> failureMechanismResultsWithFailureProb)
        {
            var categoriesCalculator = new CategoryLimitsCalculator();
            var categories = categoriesCalculator.CalculateAssessmentSectionCategoryLimitsBoi21(section);
            var failureProb =
                assessmentSectionAssembler.CalculateAssessmentSectionFailureProbabilityBoi2A1(
                    failureMechanismResultsWithFailureProb.Select(r => r.Probability).ToArray(), false);
            var assessmentGrade = assessmentSectionAssembler.DetermineAssessmentGradeBoi2B1(failureProb, categories);
        }

        private void AssembleFailureProbabilitiesPerFailureMechanism(
            List<FailureMechanismAssemblyResult> failureMechanismResultsWithFailureProb, List<FailureMechanismSectionList> failureMechanismSectionLists)
        {
            foreach (var failureMechanismSectionResults in failureMechanismSectionResultsDictionary)
            {
                var result = failureMechanismResultAssembler.CalculateFailureMechanismFailureProbabilityWithLengthEffectBoi1A2(
                    failureMechanismSectionResults.Key,
                    failureMechanismSectionResults.Value.Select(failureMechanismSection => failureMechanismSection.Result),
                    false);
                failureMechanismResultsWithFailureProb.Add(result);

                failureMechanismSectionLists.Add(
                    CreateFailureMechanismSectionListForStep3(failureMechanismSectionResults.Value));
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

        private void CreateTestInput()
        {
            failureMechanismSectionResultsDictionary = new Dictionary<double, List<FailureMechanismSection>>();

            for (var i = 1; i <= 15; i++)
            {
                var failureMechanismSections = new List<FailureMechanismSection>();

                var sectionLengthRemaining = SectionLength;
                for (var k = 0; k < 250; k++)
                {
                    var sectionStart = sectionLengthRemaining / (250 - k) * k;
                    var sectionEnd = sectionLengthRemaining / (250 - k) * (k + 1);
                    failureMechanismSections.Add(
                        new FailureMechanismSection(
                            new FailureMechanismSectionAssemblyResultWithLengthEffect((Probability)5.0E-5, (Probability)1.0E-4, EInterpretationCategory.I),
                            sectionStart,
                            sectionEnd));

                    sectionLengthRemaining -= sectionEnd - sectionStart;
                }

                failureMechanismSectionResultsDictionary.Add(i, failureMechanismSections);
            }
        }

        private sealed class FailureMechanismSection
        {
            public FailureMechanismSection(FailureMechanismSectionAssemblyResultWithLengthEffect result, double sectionStart,
                             double sectionEnd)
            {
                Result = result;
                SectionStart = sectionStart;
                SectionEnd = sectionEnd;
            }

            public FailureMechanismSectionAssemblyResultWithLengthEffect Result { get; }

            public double SectionStart { get; }

            public double SectionEnd { get; }
        }
    }
}