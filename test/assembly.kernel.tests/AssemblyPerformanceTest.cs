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
using Assembly.Kernel.Model;
using Assembly.Kernel.Model.AssessmentSection;
using Assembly.Kernel.Model.Categories;
using Assembly.Kernel.Model.FailureMechanismSections;
using NUnit.Framework;

namespace Assembly.Kernel.Tests
{
    [TestFixture]
    public class AssemblyPerformanceTest
    {
        const double SectionLength = 3750.0;
        private IDictionary<double, List<Tuple<FailureMechanismSection, FailureMechanismSectionAssemblyResultWithLengthEffect>>> failureMechanismSectionResultsDictionary;

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

            AssembleCommonFailureMechanismSections(failureMechanismSectionLists);

            watch.Stop();

            Console.Out.WriteLine($"Elapsed time since start of assembly: {watch.Elapsed.TotalMilliseconds} ms (max: 1000 ms)");
        }

        private static void AssembleCommonFailureMechanismSections(IEnumerable<FailureMechanismSectionList> failureMechanismSectionLists)
        {
            var combinedSectionAssembler = new CommonFailureMechanismSectionAssembler();
            FailureMechanismSectionList commonSections = combinedSectionAssembler.FindGreatestCommonDenominatorSectionsBoi3A1(
                failureMechanismSectionLists, SectionLength);

            IEnumerable<FailureMechanismSectionList> failureMechanismResults = failureMechanismSectionLists.Select(
                fms => combinedSectionAssembler.TranslateFailureMechanismResultsToCommonSectionsBoi3B1(fms, commonSections));

            IEnumerable<FailureMechanismSectionWithCategory> combinedSectionResults = combinedSectionAssembler.DetermineCombinedResultPerCommonSectionBoi3C1(
                failureMechanismResults, false);
        }

        private static void CalculateAssessmentGrade(AssessmentSection section, IEnumerable<FailureMechanismAssemblyResult> failureMechanismResultsWithFailureProb)
        {
            var categoriesCalculator = new CategoryLimitsCalculator();
            CategoriesList<AssessmentSectionCategory> categories = categoriesCalculator.CalculateAssessmentSectionCategoryLimitsBoi21(section);

            var assessmentSectionAssembler = new AssessmentGradeAssembler();
            Probability failureProb = assessmentSectionAssembler.CalculateAssessmentSectionFailureProbabilityBoi2A1(
                failureMechanismResultsWithFailureProb.Select(r => r.Probability).ToArray(), false);

            EAssessmentGrade assessmentGrade = assessmentSectionAssembler.DetermineAssessmentGradeBoi2B1(failureProb, categories);
        }

        private void AssembleFailureProbabilitiesPerFailureMechanism(
            List<FailureMechanismAssemblyResult> failureMechanismResultsWithFailureProb,
            List<FailureMechanismSectionList> failureMechanismSectionLists)
        {
            var failureMechanismResultAssembler = new FailureMechanismResultAssembler();

            foreach (KeyValuePair<double, List<Tuple<FailureMechanismSection, FailureMechanismSectionAssemblyResultWithLengthEffect>>> failureMechanismSectionResults in failureMechanismSectionResultsDictionary)
            {
                FailureMechanismAssemblyResult result = failureMechanismResultAssembler.CalculateFailureMechanismFailureProbabilityWithLengthEffectBoi1A2(
                    failureMechanismSectionResults.Key,
                    failureMechanismSectionResults.Value.Select(failureMechanismSection => failureMechanismSection.Item2),
                    false);
                failureMechanismResultsWithFailureProb.Add(result);

                failureMechanismSectionLists.Add(CreateFailureMechanismSectionListForStep3(failureMechanismSectionResults.Value));
            }
        }

        private static FailureMechanismSectionList CreateFailureMechanismSectionListForStep3(
            IEnumerable<Tuple<FailureMechanismSection, FailureMechanismSectionAssemblyResultWithLengthEffect>> failureMechanismSectionResults)
        {
            return new FailureMechanismSectionList(failureMechanismSectionResults.Select(
                                                       failureMechanismSection =>
                                                           new FailureMechanismSectionWithCategory(
                                                               failureMechanismSection.Item1.Start,
                                                               failureMechanismSection.Item1.End,
                                                               failureMechanismSection.Item2.InterpretationCategory)));
        }

        private void CreateTestInput()
        {
            failureMechanismSectionResultsDictionary = new Dictionary<double, List<Tuple<FailureMechanismSection, FailureMechanismSectionAssemblyResultWithLengthEffect>>>();

            for (var i = 1; i <= 15; i++)
            {
                var failureMechanismSections = new List<Tuple<FailureMechanismSection, FailureMechanismSectionAssemblyResultWithLengthEffect>>();

                double sectionLengthRemaining = SectionLength;
                for (var k = 0; k < 250; k++)
                {
                    double sectionStart = sectionLengthRemaining / (250 - k) * k;
                    double sectionEnd = sectionLengthRemaining / (250 - k) * (k + 1);
                    failureMechanismSections.Add(
                        new Tuple<FailureMechanismSection, FailureMechanismSectionAssemblyResultWithLengthEffect>(
                            new FailureMechanismSection(sectionStart, sectionEnd),
                            new FailureMechanismSectionAssemblyResultWithLengthEffect(
                                new Probability(5.0E-5), new Probability(1.0E-4), EInterpretationCategory.I)));

                    sectionLengthRemaining -= sectionEnd - sectionStart;
                }

                failureMechanismSectionResultsDictionary.Add(i, failureMechanismSections);
            }
        }
    }
}