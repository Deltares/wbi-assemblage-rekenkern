#region Copyright (c) 2018 Technolution BV. All Rights Reserved. 

// // Copyright (C) Technolution BV. 2018. All rights reserved.
// //
// // This file is part of the Assembly kernel.
// //
// // Assembly kernel is free software: you can redistribute it and/or modify
// // it under the terms of the GNU Lesser General Public License as published by
// // the Free Software Foundation, either version 3 of the License, or
// // (at your option) any later version.
// // 
// // This program is distributed in the hope that it will be useful,
// // but WITHOUT ANY WARRANTY; without even the implied warranty of
// // MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// // GNU Lesser General Public License for more details.
// //
// // You should have received a copy of the GNU Lesser General Public License
// // along with this program. If not, see <http://www.gnu.org/licenses/>.
// //
// // All names, logos, and references to "Technolution BV" are registered trademarks of
// // Technolution BV and remain full property of Technolution BV at all times.
// // All rights reserved.

#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using Assembly.Kernel.Exceptions;
using Assembly.Kernel.Interfaces;
using Assembly.Kernel.Model;
using Assembly.Kernel.Model.FmSectionTypes;

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
                DeterminCombinedResultPerCommonSectionWbi3C1(failureMechanismResults, partialAssembly);

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

            foreach (var failureMechanismSectionList in mechanismSectionLists)
            {
                var totalFailureMechanismSectionLength = 0.0;
                foreach (var fmSection in failureMechanismSectionList.Sections)
                {
                    var sectionEnd = fmSection.SectionEnd;
                    if (!sectionLimits.Contains(sectionEnd))
                    {
                        sectionLimits.Add(sectionEnd);
                    }

                    totalFailureMechanismSectionLength += sectionEnd - fmSection.SectionStart;
                }

                // compare calculated assessment section length with the provided length with a margin of 1 cm.
                if (Math.Abs(totalFailureMechanismSectionLength - assessmentSectionLength) > 0.01)
                {
                    throw new AssemblyException("AssembleCommonFailureMechanismSection",
                        EAssemblyErrors.FmSectionLengthInvalid);
                }
            }

            sectionLimits.Sort();
            var previousSectionEnd = 0.0;
            var commonSections = new List<FailureMechanismSection>();
            foreach (var sectionLimit in sectionLimits)
            {
                commonSections.Add(new FailureMechanismSection(previousSectionEnd, sectionLimit));
                previousSectionEnd = sectionLimit;
            }

            return new FailureMechanismSectionList("Common", commonSections);
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
                var section = failureMechanismSectionList.GetSectionCategoryForPoint(commonSection.SectionEnd);

                var sectionWithDirectCategory = section as FmSectionWithDirectCategory;
                if (sectionWithDirectCategory != null)
                {
                    resultsToCommonSections.Add(new FmSectionWithDirectCategory(commonSection.SectionStart,
                        commonSection.SectionEnd,
                        sectionWithDirectCategory.Category));
                }
                else
                {
                    resultsToCommonSections.Add(new FmSectionWithIndirectCategory(commonSection.SectionStart,
                        commonSection.SectionEnd,
                        ((FmSectionWithIndirectCategory) section).Category));
                }
            }

            return new FailureMechanismSectionList(failureMechanismSectionList.FailureMechanismId,
                resultsToCommonSections);
        }

        /// <inheritdoc />
        public IEnumerable<FmSectionWithDirectCategory> DeterminCombinedResultPerCommonSectionWbi3C1(
            IEnumerable<FailureMechanismSectionList> failureMechanismResults, bool partialAssembly)
        {
            FmSectionWithDirectCategory[][] directFailureMechanismSectionLists = CheckInputWbi3C1(failureMechanismResults);

            FmSectionWithDirectCategory[] firstSectionsList = directFailureMechanismSectionLists.First();
            var combinedSectionResults = new List<FmSectionWithDirectCategory>();

            for (var iSection = 0; iSection < firstSectionsList.Length; iSection++)
            {
                var newCombinedSection = new FmSectionWithDirectCategory(firstSectionsList[iSection].SectionStart,
                    firstSectionsList[iSection].SectionEnd, EFmSectionCategory.NotApplicable);

                foreach (var failureMechanismSectionList in directFailureMechanismSectionLists)
                {
                    var section = failureMechanismSectionList[iSection];
                    if (!AreEqualSections(section, newCombinedSection))
                    {
                        throw new AssemblyException("FailureMechanismSectionList",
                            EAssemblyErrors.CommonFailureMechanismSectionsInvalid);
                    }

                    newCombinedSection.Category = DetermineCombinedCategory(partialAssembly, newCombinedSection, section.Category);

                    if (newCombinedSection.Category == EFmSectionCategory.Gr)
                    {
                        break;
                    }
                }

                combinedSectionResults.Add(newCombinedSection);
            }

            return combinedSectionResults;
        }

        private static FmSectionWithDirectCategory[][] CheckInputWbi3C1(IEnumerable<FailureMechanismSectionList> failureMechanismResults)
        {
            if (failureMechanismResults == null)
            {
                throw new AssemblyException("FailureMechanismSectionList",
                    EAssemblyErrors.ValueMayNotBeNull);
            }

            var directFailureMechanismSectionLists = failureMechanismResults
                .Where(fmrl => fmrl.Sections.First().GetType() == typeof(FmSectionWithDirectCategory))
                .Select(fmrl => fmrl.Sections.OfType<FmSectionWithDirectCategory>().ToArray())
                .ToArray();

            if (!directFailureMechanismSectionLists.Any())
            {
                throw new AssemblyException("FailureMechanismSectionList",
                    EAssemblyErrors.ValueMayNotBeNull);
            }

            if (directFailureMechanismSectionLists.Select(l => l.Length).Distinct().Count() > 1)
            {
                throw new AssemblyException("FailureMechanismSectionList",
                    EAssemblyErrors.CommonFailureMechanismSectionsInvalid);
            }

            return directFailureMechanismSectionLists;
        }

        private static bool AreEqualSections(FmSectionWithDirectCategory section1, FmSectionWithDirectCategory section2)
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
                throw new AssemblyException("FailureMEchanismSectionList",
                    EAssemblyErrors.CommonFailureMechanismSectionsInvalid);
            }

            var firstResult = failureMechanismSectionList.Sections.First();
            if (!(firstResult is FmSectionWithIndirectCategory) && !(firstResult is FmSectionWithDirectCategory))
            {
                throw new AssemblyException("FailureMEchanismSectionList",
                    EAssemblyErrors.SectionsWithoutCategory);
            }
        }

        private static FailureMechanismSectionList[] CheckGreatestCommonDenominatorInput(
            IEnumerable<FailureMechanismSectionList> failureMechanismSectionLists,
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

            if (failureMechanismSectionLists == null)
            {
                throw new AssemblyException("FailureMEchanismSectionList",
                    EAssemblyErrors.ValueMayNotBeNull);
            }

            var mechanismSectionLists = failureMechanismSectionLists as FailureMechanismSectionList[] ??
                                        failureMechanismSectionLists.ToArray();
            if (!mechanismSectionLists.Any())
            {
                throw new AssemblyException("FailureMEchanismSectionList",
                    EAssemblyErrors.ValueMayNotBeNull);
            }

            return mechanismSectionLists;
        }

        private static EFmSectionCategory DetermineCombinedCategory(bool partialAssembly,
            FmSectionWithDirectCategory combinedFailureMechanismSectionResult,
            EFmSectionCategory currentCategory)
        {
            EFmSectionCategory combinedCategory = combinedFailureMechanismSectionResult.Category;
            switch (currentCategory)
            {
                case EFmSectionCategory.NotApplicable:
                    // ignore not applicable, will not be greater than combinedCategory ever.
                    break;
                case EFmSectionCategory.Iv:
                case EFmSectionCategory.IIv:
                case EFmSectionCategory.IIIv:
                case EFmSectionCategory.IVv:
                case EFmSectionCategory.Vv:
                case EFmSectionCategory.VIv:
                    if (currentCategory > combinedCategory)
                    {
                        combinedCategory = currentCategory;
                    }

                    break;
                case EFmSectionCategory.VIIv:
                    if (!partialAssembly)
                    {
                        combinedCategory = currentCategory;
                    }

                    break;
                case EFmSectionCategory.Gr:
                    combinedCategory = EFmSectionCategory.Gr;
                    break;
            }

            return combinedCategory;
        }
    }
}