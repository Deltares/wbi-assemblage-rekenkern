﻿// Copyright (C) Rijkswaterstaat 2022. All rights reserved.
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

        /// <inheritdoc />
        public FailureMechanismSectionList FindGreatestCommonDenominatorSectionsBoi3A1(
            IEnumerable<FailureMechanismSectionList> failureMechanismSectionLists,
            double assessmentSectionLength)
        {
            if (failureMechanismSectionLists == null)
            {
                throw new ArgumentNullException(nameof(failureMechanismSectionLists));
            }

            ValidateGreatestCommonDenominatorInput(failureMechanismSectionLists, assessmentSectionLength);

            return new FailureMechanismSectionList(GetCommonSectionLimitsIgnoringSmallDifferences(failureMechanismSectionLists));
        }

        /// <inheritdoc />
        public FailureMechanismSectionList TranslateFailureMechanismResultsToCommonSectionsBoi3B1(
            FailureMechanismSectionList failureMechanismSections,
            FailureMechanismSectionList commonSections)
        {
            ValidateFailureMechanismResultsToCommonSectionsInput(failureMechanismSections, commonSections);

            var resultsToCommonSections = new List<FailureMechanismSection>();
            foreach (FailureMechanismSection commonSection in commonSections.Sections)
            {
                var section = (FailureMechanismSectionWithCategory) failureMechanismSections.GetSectionAtPoint(commonSection.Center);

                resultsToCommonSections.Add(
                    new FailureMechanismSectionWithCategory(
                        commonSection.Start, commonSection.End, section.Category));
            }

            return new FailureMechanismSectionList(resultsToCommonSections);
        }

        /// <inheritdoc />
        public IEnumerable<FailureMechanismSectionWithCategory> DetermineCombinedResultPerCommonSectionBoi3C1(
            IEnumerable<FailureMechanismSectionList> failureMechanismResultsForCommonSections, bool partialAssembly)
        {
            if (failureMechanismResultsForCommonSections == null)
            {
                throw new ArgumentNullException(nameof(failureMechanismResultsForCommonSections));
            }

            ValidateCombinedResultPerCommonSectionInput(failureMechanismResultsForCommonSections);

            IEnumerable<FailureMechanismSectionWithCategory>[] failureMechanismSectionLists =
                failureMechanismResultsForCommonSections.Select(resultsList => resultsList.Sections.Cast<FailureMechanismSectionWithCategory>())
                                                        .ToArray();

            return failureMechanismSectionLists.First()
                                               .Select(section => new FailureMechanismSectionWithCategory(
                                                           section.Start, section.End,
                                                           GetCategory(section, failureMechanismSectionLists, partialAssembly)))
                                               .ToArray();
        }

        #region 3B1

        /// <summary>
        /// Validates the failure mechanism results to common sections input. 
        /// </summary>
        /// <param name="failureMechanismSections">The list of failure mechanism sections.</param>
        /// <param name="commonSections">The list of common failure mechanism sections.</param>
        /// <returns>A <see cref="FailureMechanismSectionList"/> with the assembly result per common denominator section.</returns>
        /// <exception cref="ArgumentNullException">Thrown when any parameter is <c>null</c>.</exception>
        /// <exception cref="AssemblyException">Thrown when:
        /// <list type="bullet">
        /// <item>The length of the <paramref name="commonSections"/> is not equal to the lenght of the <paramref name="failureMechanismSections"/>;</item>
        /// <item>The elements of <paramref name="failureMechanismSections"/> are not of type <see cref="FailureMechanismSectionWithCategory"/>.</item>
        /// </list>
        /// </exception>
        private static void ValidateFailureMechanismResultsToCommonSectionsInput(
            FailureMechanismSectionList failureMechanismSections, FailureMechanismSectionList commonSections)
        {
            if (failureMechanismSections == null)
            {
                throw new ArgumentNullException(nameof(failureMechanismSections));
            }

            if (commonSections == null)
            {
                throw new ArgumentNullException(nameof(commonSections));
            }

            double commonSectionsLength = commonSections.Sections.Last().End;
            double failureMechanismSectionsLength = failureMechanismSections.Sections.Last().End;
            if (Math.Abs(commonSectionsLength - failureMechanismSectionsLength) > verySmallLengthDifference)
            {
                throw new AssemblyException(nameof(commonSections),
                                            EAssemblyErrors.CommonFailureMechanismSectionsInvalid);
            }

            if (failureMechanismSections.Sections.Any(s => s.GetType() != typeof(FailureMechanismSectionWithCategory)))
            {
                throw new AssemblyException(nameof(failureMechanismSections),
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
        /// <item><paramref name="failureMechanismSectionLists"/> is <c>empty</c>;</item>
        /// <item><paramref name="assessmentSectionLength"/> is <see cref="double.NaN"/> or not &gt; 0;</item>
        /// <item>The sum of the failure mechanism section lengths is not equal to the <paramref name="assessmentSectionLength"/>.</item>
        /// </list>
        /// </exception>
        private static void ValidateGreatestCommonDenominatorInput(
            IEnumerable<FailureMechanismSectionList> failureMechanismSectionLists,
            double assessmentSectionLength)
        {
            var errors = new List<AssemblyErrorMessage>();

            if (double.IsNaN(assessmentSectionLength) || assessmentSectionLength <= 0.0)
            {
                errors.Add(new AssemblyErrorMessage(nameof(assessmentSectionLength),
                                                    EAssemblyErrors.SectionLengthOutOfRange));
            }

            if (!failureMechanismSectionLists.Any())
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
        /// <item><paramref name="failureMechanismResultsForCommonSections"/> is <c>empty</c>;</item>
        /// <item>The elements of <paramref name="failureMechanismResultsForCommonSections"/> are not of type <see cref="FailureMechanismSectionWithCategory"/>;</item>
        /// <item>The elements of <paramref name="failureMechanismResultsForCommonSections"/> do not have equal sections.</item>
        /// </list>
        /// </exception>
        private static void ValidateCombinedResultPerCommonSectionInput(
            IEnumerable<FailureMechanismSectionList> failureMechanismResultsForCommonSections)
        {
            IEnumerable<FailureMechanismSection> failureMechanismSectionsPerFailureMechanism = failureMechanismResultsForCommonSections.SelectMany(fm => fm.Sections);
            if (!failureMechanismResultsForCommonSections.Any()
                || failureMechanismSectionsPerFailureMechanism.Any(s => s.GetType() != typeof(FailureMechanismSectionWithCategory)))
            {
                throw new AssemblyException(nameof(failureMechanismResultsForCommonSections), EAssemblyErrors.CommonSectionsWithoutCategoryValues);
            }

            IEnumerable<int> numberOfSectionsPerFailureMechanism = failureMechanismResultsForCommonSections.Select(fmr => fmr.Sections.Count());
            if (numberOfSectionsPerFailureMechanism.Distinct().Count() > 1)
            {
                throw new AssemblyException(nameof(failureMechanismResultsForCommonSections), EAssemblyErrors.UnequalCommonFailureMechanismSectionLists);
            }

            if (failureMechanismSectionsPerFailureMechanism.Select(s => s.Start).Distinct().Count() != numberOfSectionsPerFailureMechanism.First()
                || failureMechanismSectionsPerFailureMechanism.Select(s => s.End).Distinct().Count() != numberOfSectionsPerFailureMechanism.First())
            {
                throw new AssemblyException(nameof(failureMechanismResultsForCommonSections),
                                            EAssemblyErrors.CommonFailureMechanismSectionsDoNotHaveEqualSections);
            }
        }

        private static EInterpretationCategory GetCategory(FailureMechanismSection currentSection,
                                                           IEnumerable<IEnumerable<FailureMechanismSectionWithCategory>> failureMechanismSectionLists,
                                                           bool partialAssembly)
        {
            var category = EInterpretationCategory.NotRelevant;
            foreach (IEnumerable<FailureMechanismSectionWithCategory> failureMechanismSectionList in failureMechanismSectionLists)
            {
                FailureMechanismSectionWithCategory section = failureMechanismSectionList.Single(s => AreEqualSections(s, currentSection));
                category = DetermineCombinedCategory(category, section.Category, partialAssembly);

                if (category == EInterpretationCategory.NoResult)
                {
                    break;
                }
            }

            return category;
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
            if (partialAssembly
                && (currentCategory == EInterpretationCategory.Dominant
                    || currentCategory == EInterpretationCategory.NoResult))
            {
                return combinedCategory;
            }

            return currentCategory > combinedCategory
                       ? currentCategory
                       : combinedCategory;
        }

        #endregion
    }
}