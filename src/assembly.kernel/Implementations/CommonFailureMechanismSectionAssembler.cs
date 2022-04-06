#region Copyright (C) Rijkswaterstaat 2022. All rights reserved.

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
using System.Linq;
using Assembly.Kernel.Exceptions;
using Assembly.Kernel.Interfaces;
using Assembly.Kernel.Model.Categories;
using Assembly.Kernel.Model.FailureMechanismSections;

namespace Assembly.Kernel.Implementations
{
    /// <inheritdoc />
    public class CommonFailureMechanismSectionAssembler : ICommonFailureMechanismSectionAssembler
    {
        private const double TooSmallSectionLength = 1e-4;
        private const double MaximumAllowedAssessmentSectionLengthDifference = 0.01;
        private const double VerySmallLengthDifference = 1e-8;

        /// <inheritdoc />
        public GreatestCommonDenominatorAssemblyResult AssembleCommonFailureMechanismSections(
            IEnumerable<FailureMechanismSectionList> failureMechanismSectionLists, double assessmentSectionLength,
            bool partialAssembly)
        {
            if (failureMechanismSectionLists == null)
            {
                throw new AssemblyException(nameof(failureMechanismSectionLists), EAssemblyErrors.ValueMayNotBeNull);
            }

            FailureMechanismSectionList[] failureMechanismSections =
                failureMechanismSectionLists as FailureMechanismSectionList[] ?? failureMechanismSectionLists.ToArray();

            FailureMechanismSectionList commonSections = FindGreatestCommonDenominatorSectionsBoi3A1(failureMechanismSections, assessmentSectionLength);

            var failureMechanismResults = new List<FailureMechanismSectionList>();
            foreach (FailureMechanismSectionList failureMechanismSectionList in failureMechanismSections)
            {
                failureMechanismResults.Add(
                    TranslateFailureMechanismResultsToCommonSectionsBoi3B1(failureMechanismSectionList,
                        commonSections));
            }

            var combinedSectionResult = DetermineCombinedResultPerCommonSectionBoi3C1(failureMechanismResults, partialAssembly);

            return new GreatestCommonDenominatorAssemblyResult(failureMechanismResults, combinedSectionResult);
        }

        /// <inheritdoc />
        public FailureMechanismSectionList FindGreatestCommonDenominatorSectionsBoi3A1(
            IEnumerable<FailureMechanismSectionList> failureMechanismSectionLists,
            double assessmentSectionLength)
        {
            var mechanismSectionLists = CheckGreatestCommonDenominatorInput(failureMechanismSectionLists, assessmentSectionLength);

            var sectionLimits = new List<double>();

            var minimumAssessmentSectionLength = double.PositiveInfinity;
            minimumAssessmentSectionLength = GatherAllSectionLimits(assessmentSectionLength, mechanismSectionLists, sectionLimits, minimumAssessmentSectionLength);

            var previousSectionEnd = 0.0;
            var commonSections = GetCommonSectionLimitsIgnoringSmallDifferences(sectionLimits, minimumAssessmentSectionLength, previousSectionEnd);

            return new FailureMechanismSectionList(commonSections);
        }

        /// <inheritdoc />
        public FailureMechanismSectionList TranslateFailureMechanismResultsToCommonSectionsBoi3B1(
            FailureMechanismSectionList failureMechanismSectionList,
            FailureMechanismSectionList commonSections)
        {
            CheckResultsToCommonSectionsInput(commonSections, failureMechanismSectionList);

            var commonSectionsArray = commonSections.Sections as FailureMechanismSection[] ??
                                      commonSections.Sections.ToArray();

            var resultsToCommonSections = new List<FailureMechanismSection>();
            foreach (var commonSection in commonSectionsArray)
            {
                var section = failureMechanismSectionList.GetSectionAtPoint(commonSection.Center);

                if (section is FailureMechanismSectionWithCategory sectionWithCategory)
                {
                    resultsToCommonSections.Add(new FailureMechanismSectionWithCategory(
                        commonSection.Start,
                        commonSection.End,
                        sectionWithCategory.Category));
                }
            }

            return new FailureMechanismSectionList(resultsToCommonSections);
        }

        /// <inheritdoc />
        public IEnumerable<FailureMechanismSectionWithCategory> DetermineCombinedResultPerCommonSectionBoi3C1(
            IEnumerable<FailureMechanismSectionList> failureMechanismResults, bool partialAssembly)
        {
            FailureMechanismSectionWithCategory[][] failureMechanismSectionLists = CheckInputBoi3C1(failureMechanismResults);

            FailureMechanismSectionWithCategory[] firstSectionsList = failureMechanismSectionLists.First();
            var combinedSectionResults = new List<FailureMechanismSectionWithCategory>();

            for (var iSection = 0; iSection < firstSectionsList.Length; iSection++)
            {
                var newCombinedSection = new FailureMechanismSectionWithCategory(firstSectionsList[iSection].Start,
                    firstSectionsList[iSection].End,
                    EInterpretationCategory.NotRelevant);

                foreach (var failureMechanismSectionList in failureMechanismSectionLists)
                {
                    var section = failureMechanismSectionList[iSection];
                    if (!AreEqualSections(section, newCombinedSection))
                    {
                        throw new AssemblyException(nameof(failureMechanismResults),
                            EAssemblyErrors.CommonFailureMechanismSectionsDoNotHaveEqualSections);
                    }

                    newCombinedSection.Category = DetermineCombinedCategory(newCombinedSection.Category,
                        section.Category, partialAssembly);
                    if (newCombinedSection.Category == EInterpretationCategory.NoResult)
                    {
                        break;
                    }
                }

                combinedSectionResults.Add(newCombinedSection);
            }

            return combinedSectionResults;
        }

        private static double GatherAllSectionLimits(double assessmentSectionLength,
            FailureMechanismSectionList[] mechanismSectionLists, List<double> sectionLimits, double minimumAssessmentSectionLength)
        {
            foreach (var failureMechanismSectionList in mechanismSectionLists)
            {
                foreach (var failureMechanismSection in failureMechanismSectionList.Sections)
                {
                    var sectionEnd = failureMechanismSection.End;
                    if (!sectionLimits.Contains(sectionEnd))
                    {
                        sectionLimits.Add(sectionEnd);
                    }
                }

                var lastSection = failureMechanismSectionList.Sections.Last();
                if (lastSection.End < minimumAssessmentSectionLength)
                {
                    minimumAssessmentSectionLength = lastSection.End;
                }

                if (Math.Abs(minimumAssessmentSectionLength - assessmentSectionLength) > MaximumAllowedAssessmentSectionLengthDifference)
                {
                    throw new AssemblyException(nameof(CommonFailureMechanismSectionAssembler),
                        EAssemblyErrors.FailureMechanismSectionLengthInvalid);
                }
            }

            sectionLimits.Sort();
            return minimumAssessmentSectionLength;
        }

        private static IEnumerable<FailureMechanismSection> GetCommonSectionLimitsIgnoringSmallDifferences(IEnumerable<double> sectionLimits, double minimumAssessmentSectionLength,
            double previousSectionEnd)
        {
            var commonSections = new List<FailureMechanismSection>();
            foreach (var sectionLimit in sectionLimits)
            {
                if (sectionLimit > minimumAssessmentSectionLength)
                {
                    break;
                }

                if (Math.Abs(sectionLimit - previousSectionEnd) < TooSmallSectionLength)
                {
                    continue;
                }

                commonSections.Add(new FailureMechanismSection(previousSectionEnd, sectionLimit));
                previousSectionEnd = sectionLimit;
            }

            return commonSections;
        }

        private static FailureMechanismSectionWithCategory[][] CheckInputBoi3C1(
            IEnumerable<FailureMechanismSectionList> failureMechanismResults)
        {
            if (failureMechanismResults == null)
            {
                throw new AssemblyException(nameof(failureMechanismResults),
                    EAssemblyErrors.ValueMayNotBeNull);
            }

            var failureMechanismSectionLists = failureMechanismResults
                .Select(resultsList => resultsList.Sections.OfType<FailureMechanismSectionWithCategory>().ToArray())
                .Where(l => l.Any())
                .ToArray();

            if (!failureMechanismSectionLists.Any())
            {
                throw new AssemblyException(nameof(failureMechanismResults), EAssemblyErrors.CommonSectionsDidNotHaveCategoryValues);
            }

            if (failureMechanismSectionLists.Select(l => l.Length).Distinct().Count() > 1)
            {
                throw new AssemblyException(nameof(failureMechanismResults), EAssemblyErrors.UnequalCommonFailureMechanismSectionLists);
            }

            return failureMechanismSectionLists;
        }

        private static bool AreEqualSections(FailureMechanismSection section1,
            FailureMechanismSection section2)
        {
            return Math.Abs(section1.Start - section2.Start) < VerySmallLengthDifference &&
                   Math.Abs(section1.End - section2.End) < VerySmallLengthDifference;
        }

        private static void CheckResultsToCommonSectionsInput(FailureMechanismSectionList commonSections,
            FailureMechanismSectionList failureMechanismSectionList)
        {
            if (commonSections == null || failureMechanismSectionList == null)
            {
                throw new AssemblyException(nameof(FailureMechanismSectionList),
                    EAssemblyErrors.ValueMayNotBeNull);
            }

            if (Math.Abs(commonSections.Sections.Last().End -
                         failureMechanismSectionList.Sections.Last().End) > VerySmallLengthDifference)
            {
                throw new AssemblyException(nameof(FailureMechanismSectionList),
                    EAssemblyErrors.CommonFailureMechanismSectionsInvalid);
            }

            var firstResult = failureMechanismSectionList.Sections.First();
            if (!(firstResult is FailureMechanismSectionWithCategory))
            {
                throw new AssemblyException(nameof(failureMechanismSectionList),
                    EAssemblyErrors.SectionsWithoutCategory);
            }
        }

        private static FailureMechanismSectionList[] CheckGreatestCommonDenominatorInput(
            IEnumerable<FailureMechanismSectionList> failureMechanismSectionLists,
            double assessmentSectionLength)
        {
            var errors = new List<AssemblyErrorMessage>();

            FailureMechanismSectionList[] mechanismSectionLists = null;

            if (double.IsNaN(assessmentSectionLength))
            {
                errors.Add(new AssemblyErrorMessage(nameof(assessmentSectionLength),
                    EAssemblyErrors.ValueMayNotBeNull));
            }

            if (assessmentSectionLength <= 0.0)
            {
                errors.Add(new AssemblyErrorMessage(nameof(assessmentSectionLength),
                    EAssemblyErrors.SectionLengthOutOfRange));
            }

            if (failureMechanismSectionLists == null)
            {
                errors.Add(new AssemblyErrorMessage(nameof(failureMechanismSectionLists),
                    EAssemblyErrors.ValueMayNotBeNull));
            }
            else
            {
                mechanismSectionLists = failureMechanismSectionLists as FailureMechanismSectionList[] ??
                                                            failureMechanismSectionLists.ToArray();
                if (!mechanismSectionLists.Any())
                {
                    errors.Add(new AssemblyErrorMessage(nameof(mechanismSectionLists),
                        EAssemblyErrors.EmptyResultsList));
                }
            }

            if (errors.Count > 0)
            {
                throw new AssemblyException(errors);
            }

            return mechanismSectionLists;
        }

        private static EInterpretationCategory DetermineCombinedCategory(EInterpretationCategory combinedCategory,
            EInterpretationCategory currentCategory,
            bool partialAssembly)
        {
            if (partialAssembly && (currentCategory == EInterpretationCategory.Dominant || currentCategory == EInterpretationCategory.NoResult))
            {
                return combinedCategory;
            }

            return currentCategory > combinedCategory ? currentCategory : combinedCategory;
        }
    }
}