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
using System.Linq;
using Assembly.Kernel.Exceptions;
using Assembly.Kernel.Interfaces;
using Assembly.Kernel.Model;
using Assembly.Kernel.Model.FailurePathSectionResults;

namespace Assembly.Kernel.Implementations
{
    /// <inheritdoc />
    public class CommonFailurePathSectionAssembler : ICommonFailurePathSectionAssembler
    {
        /// <inheritdoc />
        public AssemblyResult AssembleCommonFailurePathSections(
            IEnumerable<FailurePathSectionList> failurePathSectionLists, double assessmentSectionLength,
            bool partialAssembly)
        {
            FailurePathSectionList[] failurePathSections = failurePathSectionLists.ToArray();

            // step 1: create greatest common denominator list of the failure path sections in the list.
            FailurePathSectionList commonSections =
                FindGreatestCommonDenominatorSectionsWbi3A1(failurePathSections, assessmentSectionLength);

            // step 2: determine assessment results per section for each failure path.
            var failurePathResults = new List<FailurePathSectionList>();
            foreach (FailurePathSectionList failurePathSectionList in failurePathSections)
            {
                failurePathResults.Add(
                    TranslateFailurePathResultsToCommonSectionsWbi3B1(failurePathSectionList,
                        commonSections));
            }

            // step 3: determine combined result per common section
            var combinedSectionResult =
                DetermineCombinedResultPerCommonSectionWbi3C1(failurePathResults, partialAssembly);

            return new AssemblyResult(failurePathResults, combinedSectionResult);
        }

        /// <inheritdoc />
        public FailurePathSectionList FindGreatestCommonDenominatorSectionsWbi3A1(
            IEnumerable<FailurePathSectionList> failurePathSectionLists,
            double assessmentSectionLength)
        {
            var pathSectionLists =
                CheckGreatestCommonDenominatorInput(failurePathSectionLists, assessmentSectionLength);

            var sectionLimits = new List<double>();

            var minimumAssessmentSectionLength = double.PositiveInfinity;
            foreach (var failurePathSectionList in pathSectionLists)
            {
                foreach (var failurePathSection in failurePathSectionList.Sections)
                {
                    var sectionEnd = failurePathSection.SectionEnd;
                    if (!sectionLimits.Contains(sectionEnd))
                    {
                        sectionLimits.Add(sectionEnd);
                    }
                }

                if (failurePathSectionList.Sections.Last().SectionEnd < minimumAssessmentSectionLength)
                {
                    minimumAssessmentSectionLength = failurePathSectionList.Sections.Last().SectionEnd;
                }

                // compare calculated assessment section length with the provided length with a margin of 1 cm.
                if (Math.Abs(minimumAssessmentSectionLength - assessmentSectionLength) > 0.01)
                {
                    throw new AssemblyException("AssembleCommonFailurePathSection",
                        EAssemblyErrors.FpSectionLengthInvalid);
                }
            }

            sectionLimits.Sort();
            var previousSectionEnd = 0.0;
            var commonSections = new List<FailurePathSection>();
            foreach (var sectionLimit in sectionLimits)
            {
                if (sectionLimit > minimumAssessmentSectionLength)
                {
                    break;
                }

                if (Math.Abs(sectionLimit - previousSectionEnd) < 1e-4)
                {
                    continue;
                }

                commonSections.Add(new FailurePathSection(previousSectionEnd, sectionLimit));
                previousSectionEnd = sectionLimit;
            }

            return new FailurePathSectionList("Common", commonSections);
        }

        /// <inheritdoc />
        public FailurePathSectionList TranslateFailurePathResultsToCommonSectionsWbi3B1(
            FailurePathSectionList failurePathSectionList,
            FailurePathSectionList commonSections)
        {
            CheckResultsToCommonSectionsInput(commonSections, failurePathSectionList);

            var commonSectionsArray = commonSections.Sections as FailurePathSection[] ??
                                      commonSections.Sections.ToArray();

            var resultsToCommonSections = new List<FailurePathSection>();
            foreach (var commonSection in commonSectionsArray)
            {
                var section = failurePathSectionList.GetSectionResultForPoint(
                    commonSection.SectionEnd - (commonSection.SectionEnd - commonSection.SectionStart) / 2.0);

                var sectionWithCategory = section as FailurePathSectionWithCategory;
                if (sectionWithCategory != null)
                {
                    resultsToCommonSections.Add(new FailurePathSectionWithCategory(
                        commonSection.SectionStart,
                        commonSection.SectionEnd,
                        sectionWithCategory.Category));
                }
            }

            return new FailurePathSectionList(failurePathSectionList.FailurePathId, resultsToCommonSections);
        }

        /// <inheritdoc />
        public IEnumerable<FailurePathSectionWithCategory> DetermineCombinedResultPerCommonSectionWbi3C1(
            IEnumerable<FailurePathSectionList> failurePathResults, bool partialAssembly)
        {
            FailurePathSectionWithCategory[][] failurePathSectionLists = CheckInputWbi3C1(failurePathResults);

            FailurePathSectionWithCategory[] firstSectionsList = failurePathSectionLists.First();
            var combinedSectionResults = new List<FailurePathSectionWithCategory>();

            for (var iSection = 0; iSection < firstSectionsList.Length; iSection++)
            {
                var newCombinedSection = new FailurePathSectionWithCategory(firstSectionsList[iSection].SectionStart,
                    firstSectionsList[iSection].SectionEnd,
                    EInterpretationCategory.Gr);

                foreach (var failurePathSectionList in failurePathSectionLists)
                {
                    var section = failurePathSectionList[iSection];
                    if (!AreEqualSections(section, newCombinedSection))
                    {
                        throw new AssemblyException("FailurePathSectionList",
                            EAssemblyErrors.CommonFailurePathSectionsInvalid);
                    }

                    newCombinedSection.Category = DetermineCombinedCategory(newCombinedSection.Category,
                        section.Category, partialAssembly);
                    if (newCombinedSection.Category == EInterpretationCategory.D)
                    {
                        break;
                    }
                }

                combinedSectionResults.Add(newCombinedSection);
            }

            return combinedSectionResults;
        }

        private static FailurePathSectionWithCategory[][] CheckInputWbi3C1(
            IEnumerable<FailurePathSectionList> failurePathResults)
        {
            if (failurePathResults == null)
            {
                throw new AssemblyException("FailurePathSectionList",
                    EAssemblyErrors.ValueMayNotBeNull);
            }

            var failurePathSectionLists = failurePathResults
                .Where(fmrl => fmrl.Sections.First().GetType() ==
                               typeof(FailurePathSectionWithCategory))
                .Select(fmrl =>
                    fmrl.Sections.OfType<FailurePathSectionWithCategory>().ToArray())
                .ToArray();

            if (!failurePathSectionLists.Any())
            {
                throw new AssemblyException("FailurePathSectionList",
                    EAssemblyErrors.ValueMayNotBeNull);
            }

            if (failurePathSectionLists.Select(l => l.Length).Distinct().Count() > 1)
            {
                throw new AssemblyException("FailurePathSectionList",
                    EAssemblyErrors.CommonFailurePathSectionsInvalid);
            }

            return failurePathSectionLists;
        }

        private static bool AreEqualSections(FailurePathSectionWithCategory section1,
            FailurePathSectionWithCategory section2)
        {
            return Math.Abs(section1.SectionStart - section2.SectionStart) < 1e-8 &&
                   Math.Abs(section1.SectionEnd - section2.SectionEnd) < 1e-8;
        }

        private static void CheckResultsToCommonSectionsInput(FailurePathSectionList commonSections,
            FailurePathSectionList failurePathSectionList)
        {
            if (commonSections == null || failurePathSectionList == null)
            {
                throw new AssemblyException("FailurePathSectionList",
                    EAssemblyErrors.ValueMayNotBeNull);
            }

            if (Math.Abs(commonSections.Sections.Last().SectionEnd -
                         failurePathSectionList.Sections.Last().SectionEnd) > 1e-8)
            {
                throw new AssemblyException("FailurePathSectionList",
                    EAssemblyErrors.CommonFailurePathSectionsInvalid);
            }

            var firstResult = failurePathSectionList.Sections.First();
            if (!(firstResult is FailurePathSectionWithCategory))
            {
                throw new AssemblyException("FailurePathSectionList",
                    EAssemblyErrors.SectionsWithoutCategory);
            }
        }

        private static FailurePathSectionList[] CheckGreatestCommonDenominatorInput(
            IEnumerable<FailurePathSectionList> failurePathSectionLists,
            double assessmentSectionLength)
        {
            if (double.IsNaN(assessmentSectionLength))
            {
                throw new AssemblyException("AssessmentSectionLength",
                    EAssemblyErrors.ValueMayNotBeNull);
            }

            if (assessmentSectionLength <= 0.0)
            {
                throw new AssemblyException("AssessmentSectionLength",
                    EAssemblyErrors.SectionLengthOutOfRange);
            }

            if (failurePathSectionLists == null)
            {
                throw new AssemblyException("FailurePathSectionList",
                    EAssemblyErrors.ValueMayNotBeNull);
            }

            var pathSectionLists = failurePathSectionLists as FailurePathSectionList[] ??
                                   failurePathSectionLists.ToArray();
            if (!pathSectionLists.Any())
            {
                throw new AssemblyException("FailurePathSectionList",
                    EAssemblyErrors.ValueMayNotBeNull);
            }

            return pathSectionLists;
        }

        private static EInterpretationCategory DetermineCombinedCategory(EInterpretationCategory combinedCategory,
            EInterpretationCategory currentCategory,
            bool partialAssembly)
        {
            switch (currentCategory)
            {
                case EInterpretationCategory.Gr:
                case EInterpretationCategory.D
                    : // TODO: This should maybe not result in Gr? Does D prevail above any other value?
                    if (!partialAssembly)
                    {
                        return EInterpretationCategory.Gr;
                    }

                    break;
                case EInterpretationCategory.ND:
                    break;
                case EInterpretationCategory.III:
                case EInterpretationCategory.II:
                case EInterpretationCategory.I:
                case EInterpretationCategory.ZeroPlus:
                case EInterpretationCategory.Zero:
                case EInterpretationCategory.IMin:
                case EInterpretationCategory.IIMin:
                case EInterpretationCategory.IIIMin:
                    if (currentCategory > combinedCategory)
                    {
                        combinedCategory = currentCategory;
                    }

                    break;
            }

            return combinedCategory;
        }
    }
}