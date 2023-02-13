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
using System.Linq;
using Assembly.Kernel.Exceptions;
using Assembly.Kernel.Interfaces;
using Assembly.Kernel.Model.Categories;
using Assembly.Kernel.Model.FailureMechanismSections;

namespace Assembly.Kernel.Implementations
{
    /// <summary>
    /// Assemble failure mechanism section results of multiple failure mechanisms to
    /// a greatest denominator section result.
    /// </summary>
    public class CommonFailureMechanismSectionAssembler : ICommonFailureMechanismSectionAssembler
    {
        private const double tooSmallSectionLength = 1e-4;
        private const double maximumAllowedAssessmentSectionLengthDifference = 0.01;
        private const double verySmallLengthDifference = 1e-8;

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

            List<FailureMechanismSectionList> failureMechanismResults = failureMechanismSections.Select(
                failureMechanismSectionList => TranslateFailureMechanismResultsToCommonSectionsBoi3B1(failureMechanismSectionList, commonSections)).ToList();

            IEnumerable<FailureMechanismSectionWithCategory> combinedSectionResult = DetermineCombinedResultPerCommonSectionBoi3C1(failureMechanismResults, partialAssembly);

            return new GreatestCommonDenominatorAssemblyResult(failureMechanismResults, combinedSectionResult);
        }

        public FailureMechanismSectionList FindGreatestCommonDenominatorSectionsBoi3A1(
            IEnumerable<FailureMechanismSectionList> failureMechanismSectionLists,
            double assessmentSectionLength)
        {
            ValidateGreatestCommonDenominatorInput(failureMechanismSectionLists, assessmentSectionLength);

            return new FailureMechanismSectionList(GetCommonSectionLimitsIgnoringSmallDifferences(failureMechanismSectionLists));
        }

        public FailureMechanismSectionList TranslateFailureMechanismResultsToCommonSectionsBoi3B1(
            FailureMechanismSectionList failureMechanismSectionList,
            FailureMechanismSectionList commonSections)
        {
            ValidateFailureMechanismResultsToCommonSectionsInput(failureMechanismSectionList, commonSections);

            var resultsToCommonSections = new List<FailureMechanismSection>();
            foreach (FailureMechanismSection commonSection in commonSections.Sections)
            {
                var section = (FailureMechanismSectionWithCategory) failureMechanismSectionList.GetSectionAtPoint(commonSection.Center);

                resultsToCommonSections.Add(
                    new FailureMechanismSectionWithCategory(
                        commonSection.Start, commonSection.End, section.Category));
            }

            return new FailureMechanismSectionList(resultsToCommonSections);
        }

        public IEnumerable<FailureMechanismSectionWithCategory> DetermineCombinedResultPerCommonSectionBoi3C1(
            IEnumerable<FailureMechanismSectionList> failureMechanismResultsForCommonSections, bool partialAssembly)
        {
            ValidateCombinedResultPerCommonSectionInput(failureMechanismResultsForCommonSections);

            FailureMechanismSectionWithCategory[][] failureMechanismSectionLists = 
                failureMechanismResultsForCommonSections.Select(resultsList => resultsList.Sections
                                                                                          .OfType<FailureMechanismSectionWithCategory>()
                                                                                          .ToArray())
                                                        .Where(l => l.Any())
                                                        .ToArray();

            FailureMechanismSectionWithCategory[] firstSectionsList = failureMechanismSectionLists.First();
            var combinedSectionResults = new List<FailureMechanismSectionWithCategory>();

            for (var iSection = 0; iSection < firstSectionsList.Length; iSection++)
            {
                var newCombinedSection = new FailureMechanismSectionWithCategory(firstSectionsList[iSection].Start,
                                                                                 firstSectionsList[iSection].End,
                                                                                 EInterpretationCategory.NotRelevant);

                foreach (FailureMechanismSectionWithCategory[] failureMechanismSectionList in failureMechanismSectionLists)
                {
                    FailureMechanismSectionWithCategory section = failureMechanismSectionList[iSection];
                    if (!AreEqualSections(section, newCombinedSection))
                    {
                        throw new AssemblyException(nameof(failureMechanismResultsForCommonSections),
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

        #region 3B1

        /// <summary>
        /// Validates the failure mechanism results to common sections input. 
        /// </summary>
        /// <param name="failureMechanismSectionLists">The list of failure mechanism sections.</param>
        /// <param name="commonSections">The list of common failure mechanism sections.</param>
        /// <returns>A <see cref="FailureMechanismSectionList"/> with the assembly result per common denominator section.</returns>
        /// <exception cref="AssemblyException">Thrown when:
        /// <list type="bullet">
        /// <item><paramref name="failureMechanismSectionList"/> is <c>null</c>;</item>
        /// <item><paramref name="commonSections"/> is <c>null</c>;</item>
        /// <item>The length of the <paramref name="commonSections"/> is not equal to the lenght of the <paramref name="failureMechanismSectionList"/>;</item>
        /// <item>The elements of <paramref name="failureMechanismSectionList"/> are not of type <see cref="FailureMechanismSectionWithCategory"/>.</item>
        /// </list>
        /// </exception>
        private static void ValidateFailureMechanismResultsToCommonSectionsInput(
            FailureMechanismSectionList failureMechanismSectionList, FailureMechanismSectionList commonSections)
        {
            if (failureMechanismSectionList == null)
            {
                throw new AssemblyException(nameof(failureMechanismSectionList),
                                            EAssemblyErrors.ValueMayNotBeNull);
            }

            if (commonSections == null)
            {
                throw new AssemblyException(nameof(commonSections),
                                            EAssemblyErrors.ValueMayNotBeNull);
            }

            double commonSectionsLength = commonSections.Sections.Last().End;
            double failureMechanismSectionsLength = failureMechanismSectionList.Sections.Last().End;
            if (Math.Abs(commonSectionsLength - failureMechanismSectionsLength) > verySmallLengthDifference)
            {
                throw new AssemblyException(nameof(failureMechanismSectionList),
                                            EAssemblyErrors.CommonFailureMechanismSectionsInvalid);
            }

            if (failureMechanismSectionList.Sections.Any(s => s.GetType() != typeof(FailureMechanismSectionWithCategory)))
            {
                throw new AssemblyException(nameof(failureMechanismSectionList),
                                            EAssemblyErrors.SectionsWithoutCategory);
            }
        }

        #endregion

        #region 3A1

        /// <summary>
        /// Validates the greatest common denominator input. 
        /// </summary>
        /// <param name="failureMechanismSectionLists">The list of failure mechanism section lists.</param>
        /// <param name="assessmentSectionLength">The total length of the assessment section.</param>
        /// <exception cref="AssemblyException">Thrown when:
        /// <list type="bullet">
        /// <item><paramref name="failureMechanismSectionLists"/> is <c>null</c> or <c>empty</c>;</item>
        /// <item><paramref name="assessmentSectionLength"/> is <see cref="double.NaN"/> or not &gt; 0;</item>
        /// <item>The sum of the failure mechanism section lengths is not equal to the <paramref name="assessmentSectionLength"/>.</item>
        /// </list>
        /// </exception>
        private static void ValidateGreatestCommonDenominatorInput(
            IEnumerable<FailureMechanismSectionList> failureMechanismSectionLists,
            double assessmentSectionLength)
        {
            var errors = new List<AssemblyErrorMessage>();

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
            else if (!failureMechanismSectionLists.Any())
            {
                errors.Add(new AssemblyErrorMessage(nameof(failureMechanismSectionLists),
                                                    EAssemblyErrors.EmptyResultsList));
            }

            if (!errors.Any())
            {
                foreach (FailureMechanismSectionList failureMechanismSectionList in failureMechanismSectionLists)
                {
                    if (Math.Abs(failureMechanismSectionList.Sections.Last().End - assessmentSectionLength) > maximumAllowedAssessmentSectionLengthDifference)
                    {
                        errors.Add(new AssemblyErrorMessage(nameof(failureMechanismSectionList),
                                                            EAssemblyErrors.FailureMechanismSectionLengthInvalid));
                        break;
                    }
                }
            }

            if (errors.Any())
            {
                throw new AssemblyException(errors);
            }
        }

        private static IEnumerable<FailureMechanismSection> GetCommonSectionLimitsIgnoringSmallDifferences(
            IEnumerable<FailureMechanismSectionList> failureMechanismSectionLists)
        {
            var commonSections = new List<FailureMechanismSection>();
            var previousSectionEnd = 0.0;

            foreach (double sectionLimit in GetSectionLimits(failureMechanismSectionLists))
            {
                if (Math.Abs(sectionLimit - previousSectionEnd) < tooSmallSectionLength)
                {
                    continue;
                }

                commonSections.Add(new FailureMechanismSection(previousSectionEnd, sectionLimit));
                previousSectionEnd = sectionLimit;
            }

            return commonSections;
        }

        private static IEnumerable<double> GetSectionLimits(IEnumerable<FailureMechanismSectionList> failureMechanismSectionLists)
        {
            return failureMechanismSectionLists.SelectMany(fm => fm.Sections.Select(s => s.End))
                                               .Distinct()
                                               .OrderBy(limit => limit)
                                               .ToArray();
        }

        #endregion

        #region 3C1

        /// <summary>
        /// Validates the combined result per common section input. 
        /// </summary>
        /// <param name="failureMechanismResultsForCommonSections">The list of common section results per failure mechanism.</param>
        /// <exception cref="AssemblyException">Thrown when:
        /// <list type="bullet">
        /// <item><paramref name="failureMechanismResultsForCommonSections"/> is <c>null</c> or <c>empty</c>;</item>
        /// <item>The elements of <paramref name="failureMechanismResultsForCommonSections"/> are not of type <see cref="FailureMechanismSectionWithCategory"/>;</item>
        /// <item>The elements of <paramref name="failureMechanismResultsForCommonSections"/> do not have equal sections.</item>
        /// </list>
        /// </exception>
        private static void ValidateCombinedResultPerCommonSectionInput(
            IEnumerable<FailureMechanismSectionList> failureMechanismResultsForCommonSections)
        {
            if (failureMechanismResultsForCommonSections == null)
            {
                throw new AssemblyException(nameof(failureMechanismResultsForCommonSections),
                                            EAssemblyErrors.ValueMayNotBeNull);
            }

            if (!failureMechanismResultsForCommonSections.Any()
                || failureMechanismResultsForCommonSections.SelectMany(fm => fm.Sections)
                                                           .Any(s => s.GetType() != typeof(FailureMechanismSectionWithCategory)))
            {
                throw new AssemblyException(nameof(failureMechanismResultsForCommonSections), EAssemblyErrors.CommonSectionsWithoutCategoryValues);
            }

            if (failureMechanismResultsForCommonSections.Select(fmr => fmr.Sections.Count()).Distinct().Count() > 1)
            {
                throw new AssemblyException(nameof(failureMechanismResultsForCommonSections), EAssemblyErrors.UnequalCommonFailureMechanismSectionLists);
            }
        }

        private static bool AreEqualSections(FailureMechanismSection section1,
                                             FailureMechanismSection section2)
        {
            return Math.Abs(section1.Start - section2.Start) < verySmallLengthDifference &&
                   Math.Abs(section1.End - section2.End) < verySmallLengthDifference;
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

        #endregion
    }
}