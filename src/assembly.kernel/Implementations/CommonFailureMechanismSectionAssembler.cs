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
using Assembly.Kernel.Model.AssessmentSection;
using Assembly.Kernel.Model.Categories;
using Assembly.Kernel.Model.FailureMechanismSections;

namespace Assembly.Kernel.Implementations
{
    /// <inheritdoc />
    public class CommonFailureMechanismSectionAssembler : ICommonFailureMechanismSectionAssembler
    {
        /// <inheritdoc />
        public AssemblyResult AssembleCommonFailureMechanismSections(
            IEnumerable<FailureMechanismSectionList> failureMechanismSectionLists, double assessmentSectionLength,
            bool partialAssembly)
        {
            FailureMechanismSectionList[] failureMechanismSections = failureMechanismSectionLists.ToArray();

            // step 1: create greatest common denominator list of the failure mechanism sections in the list.
            FailureMechanismSectionList commonSections =
                FindGreatestCommonDenominatorSectionsWbi3A1(failureMechanismSections, assessmentSectionLength);

            // step 2: determine assessment results per section for each failure mechanism.
            var failureMechanismResults = new List<FailureMechanismSectionList>();
            foreach (FailureMechanismSectionList failureMechanismSectionList in failureMechanismSections)
            {
                failureMechanismResults.Add(
                    TranslateFailureMechanismResultsToCommonSectionsWbi3B1(failureMechanismSectionList,
                        commonSections));
            }

            // step 3: determine combined result per common section
            var combinedSectionResult =
                DetermineCombinedResultPerCommonSectionWbi3C1(failureMechanismResults, partialAssembly);

            return new AssemblyResult(failureMechanismResults, combinedSectionResult);
        }

        /// <inheritdoc />
        public FailureMechanismSectionList FindGreatestCommonDenominatorSectionsWbi3A1(
            IEnumerable<FailureMechanismSectionList> failureMechanismSectionLists,
            double assessmentSectionLength)
        {
            var mechanismSectionLists =
                CheckGreatestCommonDenominatorInput(failureMechanismSectionLists, assessmentSectionLength);

            var sectionLimits = new List<double>();

            var minimumAssessmentSectionLength = double.PositiveInfinity;
            foreach (var failureMechanismSectionList in mechanismSectionLists)
            {
                foreach (var failureMechanismSection in failureMechanismSectionList.Sections)
                {
                    var sectionEnd = failureMechanismSection.SectionEnd;
                    if (!sectionLimits.Contains(sectionEnd))
                    {
                        sectionLimits.Add(sectionEnd);
                    }
                }

                if (failureMechanismSectionList.Sections.Last().SectionEnd < minimumAssessmentSectionLength)
                {
                    minimumAssessmentSectionLength = failureMechanismSectionList.Sections.Last().SectionEnd;
                }

                // compare calculated assessment section length with the provided length with a margin of 1 cm.
                if (Math.Abs(minimumAssessmentSectionLength - assessmentSectionLength) > 0.01)
                {
                    throw new AssemblyException("AssembleCommonFailureMechanismSection",
                        EAssemblyErrors.FailureMechanismSectionLengthInvalid);
                }
            }

            sectionLimits.Sort();
            var previousSectionEnd = 0.0;
            var commonSections = new List<FailureMechanismSection>();
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

                commonSections.Add(new FailureMechanismSection(previousSectionEnd, sectionLimit));
                previousSectionEnd = sectionLimit;
            }

            return new FailureMechanismSectionList(commonSections);
        }

        /// <inheritdoc />
        public FailureMechanismSectionList TranslateFailureMechanismResultsToCommonSectionsWbi3B1(
            FailureMechanismSectionList failureMechanismSectionList,
            FailureMechanismSectionList commonSections)
        {
            CheckResultsToCommonSectionsInput(commonSections, failureMechanismSectionList);

            var commonSectionsArray = commonSections.Sections as FailureMechanismSection[] ??
                                      commonSections.Sections.ToArray();

            var resultsToCommonSections = new List<FailureMechanismSection>();
            foreach (var commonSection in commonSectionsArray)
            {
                var section = failureMechanismSectionList.GetSectionAtPoint(
                    commonSection.SectionEnd - (commonSection.SectionEnd - commonSection.SectionStart) / 2.0);

                var sectionWithCategory = section as FailureMechanismSectionWithCategory;
                if (sectionWithCategory != null)
                {
                    resultsToCommonSections.Add(new FailureMechanismSectionWithCategory(
                        commonSection.SectionStart,
                        commonSection.SectionEnd,
                        sectionWithCategory.Category));
                }
            }

            return new FailureMechanismSectionList(resultsToCommonSections);
        }

        /// <inheritdoc />
        public IEnumerable<FailureMechanismSectionWithCategory> DetermineCombinedResultPerCommonSectionWbi3C1(
            IEnumerable<FailureMechanismSectionList> failureMechanismResults, bool partialAssembly)
        {
            FailureMechanismSectionWithCategory[][] failureMechanismSectionLists = CheckInputWbi3C1(failureMechanismResults);

            FailureMechanismSectionWithCategory[] firstSectionsList = failureMechanismSectionLists.First();
            var combinedSectionResults = new List<FailureMechanismSectionWithCategory>();

            for (var iSection = 0; iSection < firstSectionsList.Length; iSection++)
            {
                var newCombinedSection = new FailureMechanismSectionWithCategory(firstSectionsList[iSection].SectionStart,
                    firstSectionsList[iSection].SectionEnd,
                    EInterpretationCategory.NotDominant);

                foreach (var failureMechanismSectionList in failureMechanismSectionLists)
                {
                    var section = failureMechanismSectionList[iSection];
                    if (!AreEqualSections(section, newCombinedSection))
                    {
                        throw new AssemblyException("FailureMechanismSectionList",
                            EAssemblyErrors.CommonFailureMechanismSectionsInvalid);
                    }

                    newCombinedSection.Category = DetermineCombinedCategory(newCombinedSection.Category,
                        section.Category, partialAssembly);
                    if (newCombinedSection.Category == EInterpretationCategory.Gr)
                    {
                        break;
                    }
                }

                combinedSectionResults.Add(newCombinedSection);
            }

            return combinedSectionResults;
        }

        private static FailureMechanismSectionWithCategory[][] CheckInputWbi3C1(
            IEnumerable<FailureMechanismSectionList> failureMechanismResults)
        {
            if (failureMechanismResults == null)
            {
                throw new AssemblyException("FailureMechanismSectionList",
                    EAssemblyErrors.ValueMayNotBeNull);
            }

            var failureMechanismSectionLists = failureMechanismResults
                .Where(resultsList => resultsList.Sections.First().GetType() ==
                               typeof(FailureMechanismSectionWithCategory))
                .Select(resultsList =>
                    resultsList.Sections.OfType<FailureMechanismSectionWithCategory>().ToArray())
                .ToArray();

            if (!failureMechanismSectionLists.Any())
            {
                throw new AssemblyException("FailureMechanismSectionList",
                    EAssemblyErrors.ValueMayNotBeNull);
            }

            if (failureMechanismSectionLists.Select(l => l.Length).Distinct().Count() > 1)
            {
                throw new AssemblyException("FailureMechanismSectionList",
                    EAssemblyErrors.CommonFailureMechanismSectionsInvalid);
            }

            return failureMechanismSectionLists;
        }

        private static bool AreEqualSections(FailureMechanismSection section1,
            FailureMechanismSection section2)
        {
            return Math.Abs(section1.SectionStart - section2.SectionStart) < 1e-8 &&
                   Math.Abs(section1.SectionEnd - section2.SectionEnd) < 1e-8;
        }

        private static void CheckResultsToCommonSectionsInput(FailureMechanismSectionList commonSections,
            FailureMechanismSectionList failureMechanismSectionList)
        {
            if (commonSections == null || failureMechanismSectionList == null)
            {
                throw new AssemblyException("FailureMechanismSectionList",
                    EAssemblyErrors.ValueMayNotBeNull);
            }

            if (Math.Abs(commonSections.Sections.Last().SectionEnd -
                         failureMechanismSectionList.Sections.Last().SectionEnd) > 1e-8)
            {
                throw new AssemblyException("FailureMechanismSectionList",
                    EAssemblyErrors.CommonFailureMechanismSectionsInvalid);
            }

            var firstResult = failureMechanismSectionList.Sections.First();
            if (!(firstResult is FailureMechanismSectionWithCategory))
            {
                throw new AssemblyException("FailureMechanismSectionList",
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
                errors.Add(new AssemblyErrorMessage("AssessmentSectionLength",
                    EAssemblyErrors.ValueMayNotBeNull));
            }

            if (assessmentSectionLength <= 0.0)
            {
                errors.Add(new AssemblyErrorMessage("AssessmentSectionLength",
                    EAssemblyErrors.SectionLengthOutOfRange));
            }

            if (failureMechanismSectionLists == null)
            {
                errors.Add(new AssemblyErrorMessage("FailureMechanismSectionList",
                    EAssemblyErrors.ValueMayNotBeNull));
            }
            else
            {
                mechanismSectionLists = failureMechanismSectionLists as FailureMechanismSectionList[] ??
                                                            failureMechanismSectionLists.ToArray();
                if (!mechanismSectionLists.Any())
                {
                    errors.Add(new AssemblyErrorMessage("FailureMechanismSectionList",
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
            if (partialAssembly && (currentCategory == EInterpretationCategory.Dominant || currentCategory == EInterpretationCategory.Gr))
            {
                return combinedCategory;
            }

            return currentCategory > combinedCategory ? currentCategory : combinedCategory;
        }
    }
}