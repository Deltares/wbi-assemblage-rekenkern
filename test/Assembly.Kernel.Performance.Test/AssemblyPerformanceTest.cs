﻿// Copyright (C) Stichting Deltares and State of the Netherlands 2023. All rights reserved.
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
// All names, logos, and references to "Deltares" are registered trademarks of
// Stichting Deltares and remain full property of Stichting Deltares at all times.
// All rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Assembly.Kernel.Implementations;
using Assembly.Kernel.Model;
using Assembly.Kernel.Model.Categories;
using Assembly.Kernel.Model.FailureMechanismSections;
using NUnit.Framework;

namespace Assembly.Kernel.Test
{
    [TestFixture]
    public class AssemblyPerformanceTest
    {
        const double SectionLength = 3750.0;
        private IDictionary<double, List<Tuple<FailureMechanismSection, ResultWithProfileAndSectionProbabilities>>> failureMechanismSectionResultsDictionary;

        [SetUp]
        public void Setup()
        {
            CreateTestInput();
        }

        [Test]
        public void GivenAssemblyKernel_WhenCategoriesAndFailureMechanismSectionsAssemblyPerformed_ThenElapsedTimeLessThan200()
        {
            // Given
            var section = new AssessmentSection((Probability) 1.0e-3, (Probability) (1.0 / 300.0));
            var failureMechanismSectionLists = new List<FailureMechanismSectionList>();

            var failureMechanismResultsWithFailureProb = new List<Probability>();

            // When
            var watch = Stopwatch.StartNew();

            var categoriesCalculator = new CategoryLimitsCalculator();
            CategoriesList<InterpretationCategory> c = categoriesCalculator.CalculateInterpretationCategoryLimitsBoi01(section);
            AssembleFailureProbabilitiesPerFailureMechanism(failureMechanismResultsWithFailureProb, failureMechanismSectionLists, c);

            watch.Stop();

            // Then
            Console.WriteLine($"Elapsed time since start of assembly: {watch.Elapsed.TotalMilliseconds} ms");
            Assert.Less(watch.Elapsed.TotalMilliseconds, 200);
        }

        [Test]
        public void GivenAssemblyKernel_WhenFullAssemblyPerformed_ThenElapsedTimeLessThan1000()
        {
            // Given
            var section = new AssessmentSection((Probability) 1.0e-3, (Probability) (1.0 / 300.0));
            var failureMechanismSectionLists = new List<FailureMechanismSectionList>();

            var failureMechanismResultsWithFailureProb = new List<Probability>();

            // When
            var watch = Stopwatch.StartNew();

            var categoriesCalculator = new CategoryLimitsCalculator();
            CategoriesList<InterpretationCategory> c = categoriesCalculator.CalculateInterpretationCategoryLimitsBoi01(section);
            AssembleFailureProbabilitiesPerFailureMechanism(failureMechanismResultsWithFailureProb, failureMechanismSectionLists, c);

            CategoriesList<AssessmentSectionCategory> assessmentGradeCategories = categoriesCalculator.CalculateAssessmentSectionCategoryLimitsBoi21(
                section);
            CalculateAssessmentGrade(failureMechanismResultsWithFailureProb, assessmentGradeCategories);

            AssembleCommonFailureMechanismSections(failureMechanismSectionLists);

            watch.Stop();

            // Then
            Console.WriteLine($"Elapsed time since start of assembly: {watch.Elapsed.TotalMilliseconds} ms");
            Assert.Less(watch.Elapsed.TotalMilliseconds, 1000);
        }

        private static void AssembleCommonFailureMechanismSections(IEnumerable<FailureMechanismSectionList> failureMechanismSectionLists)
        {
            var combinedSectionAssembler = new CommonFailureMechanismSectionAssembler();
            FailureMechanismSectionList commonSections = combinedSectionAssembler.FindGreatestCommonDenominatorSectionsBoi3A1(
                failureMechanismSectionLists, SectionLength);

            IEnumerable<FailureMechanismSectionList> failureMechanismResults = failureMechanismSectionLists.Select(
                fms => combinedSectionAssembler.TranslateFailureMechanismResultsToCommonSectionsBoi3B1(fms, commonSections));

            combinedSectionAssembler.DetermineCombinedResultPerCommonSectionBoi3C1(failureMechanismResults, false);
        }

        private static void CalculateAssessmentGrade(IEnumerable<Probability> failureMechanismResultsWithFailureProb,
                                                     CategoriesList<AssessmentSectionCategory> categories)
        {
            var assessmentSectionAssembler = new AssessmentGradeAssembler();
            Probability failureProb = assessmentSectionAssembler.CalculateAssessmentSectionFailureProbabilityBoi2A1(
                failureMechanismResultsWithFailureProb, false);

            assessmentSectionAssembler.DetermineAssessmentGradeBoi2B1(failureProb, categories);
        }

        private void AssembleFailureProbabilitiesPerFailureMechanism(List<Probability> failureMechanismResultsWithFailureProb,
                                                                     List<FailureMechanismSectionList> failureMechanismSectionLists,
                                                                     CategoriesList<InterpretationCategory> categoriesList)
        {
            var failureMechanismResultAssembler = new FailureMechanismResultAssembler();

            foreach (KeyValuePair<double, List<Tuple<FailureMechanismSection, ResultWithProfileAndSectionProbabilities>>> failureMechanismSectionResults in failureMechanismSectionResultsDictionary)
            {
                Probability result = failureMechanismResultAssembler.CalculateFailureMechanismFailureProbabilityBoi1A2(
                    failureMechanismSectionResults.Value
                                                  .Select(failureMechanismSection => failureMechanismSection.Item2)
                                                  .Select(fmsar => fmsar.ProbabilitySection),
                    failureMechanismSectionResults.Key,
                    false);
                failureMechanismResultsWithFailureProb.Add(result);

                failureMechanismSectionLists.Add(CreateFailureMechanismSectionListForStep3(failureMechanismSectionResults.Value, categoriesList));
            }
        }

        private static FailureMechanismSectionList CreateFailureMechanismSectionListForStep3(
            IEnumerable<Tuple<FailureMechanismSection, ResultWithProfileAndSectionProbabilities>> failureMechanismSectionResults,
            CategoriesList<InterpretationCategory> categoriesList)
        {
            return new FailureMechanismSectionList(failureMechanismSectionResults.Select(
                                                       failureMechanismSection =>
                                                           new FailureMechanismSectionWithCategory(
                                                               failureMechanismSection.Item1.Start,
                                                               failureMechanismSection.Item1.End,
                                                               categoriesList.GetCategoryForFailureProbability(
                                                                   failureMechanismSection.Item2.ProbabilitySection).Category)));
        }

        private void CreateTestInput()
        {
            failureMechanismSectionResultsDictionary = new Dictionary<double, List<Tuple<FailureMechanismSection, ResultWithProfileAndSectionProbabilities>>>();

            for (var i = 1; i <= 15; i++)
            {
                var failureMechanismSections = new List<Tuple<FailureMechanismSection, ResultWithProfileAndSectionProbabilities>>();

                double sectionLengthRemaining = SectionLength;
                for (var k = 0; k < 250; k++)
                {
                    double sectionStart = sectionLengthRemaining / (250 - k) * k;
                    double sectionEnd = sectionLengthRemaining / (250 - k) * (k + 1);
                    failureMechanismSections.Add(
                        new Tuple<FailureMechanismSection, ResultWithProfileAndSectionProbabilities>(
                            new FailureMechanismSection(sectionStart, sectionEnd),
                            new ResultWithProfileAndSectionProbabilities(
                                new Probability(5.0e-5), new Probability(1.0e-4))));

                    sectionLengthRemaining -= sectionEnd - sectionStart;
                }

                failureMechanismSectionResultsDictionary.Add(i, failureMechanismSections);
            }
        }
    }
}