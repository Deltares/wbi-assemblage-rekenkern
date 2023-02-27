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
using System.IO;
using System.Linq;
using Assembly.Kernel.Acceptance.TestUtil.Data.Result;

namespace Assembly.Kernel.Acceptance.Test
{
    /// <summary>
    /// Writer to write benchmark test report.
    /// </summary>
    public static class BenchmarkTestReportWriter
    {
        /// <summary>
        /// Writes the reports.
        /// </summary>
        /// <param name="results">The results.</param>
        /// <param name="reportDirectory">The report directory.</param>
        public static void WriteReports(IEnumerable<BenchmarkTestResult> results, string reportDirectory)
        {
            string template = GetReportTemplate();

            for (var i = 0; i < results.Count(); i++)
            {
                BenchmarkTestResult result = results.ElementAt(i);

                string resultTemplate = template.Replace("$BenchmarkTestName$", result.TestName.Replace("_", @"\_"));
                resultTemplate = resultTemplate.Replace("$Order$", i.ToString());

                resultTemplate = ReplaceFailureMechanismsTableWithResult(resultTemplate, result);
                resultTemplate = ReplaceCategoriesKeywordsWithResult(resultTemplate, result);
                resultTemplate = ReplaceFinalVerdictKeywordsWithResult(resultTemplate, result);
                resultTemplate = ReplaceCommonSectionsKeywordsWithResult(resultTemplate, result);

                WriteReportToDestination(resultTemplate, reportDirectory, GetTargetFileNameFromInputName(result.FileName));
            }
        }

        /// <summary>
        /// Writes the summary.
        /// </summary>
        /// <param name="destinationFileName">The name of the destination file.</param>
        /// <param name="testResults">The test results.</param>
        public static void WriteSummary(string destinationFileName, IDictionary<string, BenchmarkTestResult> testResults)
        {
            var str = "\\section{Samenvatting van de testresultaten per methode} \n      \\label{sec:summary} \n In deze paragraaf zijn de resultaten tijdens alle benchmarktests weergegeven per methode, zie \\autoref{tab:ResultatenPerMethode}. \n\n";
            str += @"\begin{longtable}[]{| l | " + string.Concat(Enumerable.Repeat("cc |", testResults.Count)) + @" }" + "\n";
            str += @"   \caption{Samenvatting van de resultaten van alle benchmarktests per methode bij volledig assembleren (V) en tussentijds assembleren (T).  \label{tab:ResultatenPerMethode}} \\" + "\n";
            str += @"   \hline \T" + "\n";
            str += @"    " + string.Concat(testResults.Select(t => string.Format(@" & \multicolumn{{2}}{{c|}}{{\rotatebox{{90}}{{{0}  }}}}",
                                                                                 t.Value.TestName.Replace("_", @"\_")))) + @" \\" + "\n";
            str += @"   Methode " + string.Concat(Enumerable.Repeat("& V & T ", testResults.Count)) + @"\B \\" + "\n";
            str += @"   \hline" + "\n";
            str += @"   \endhead" + "\n";
            str += @"   \T" + "\n";
            str += "   " + @"BOI-0-1 " + string.Concat(testResults.Select(t => " & " + ToResultText(t.Value.MethodResults.Boi01) +
                                                                               @" & \cellcolor{lightbluegray}")) + @" \\" + "\n";
            str += "   " + @"BOI-2-1 " + string.Concat(testResults.Select(t => " & " + ToResultText(t.Value.MethodResults.Boi21) +
                                                                               @" & \cellcolor{lightbluegray}")) + @" \\" + "\n";
            str += "   " + @"\grayhline" + "\n";
            str += "   " + @"BOI-0A-1" + string.Concat(testResults.Select(t => " & " + ToResultText(t.Value.MethodResults.Boi0A1) +
                                                                               @" & \cellcolor{lightbluegray}")) + @" \\" + "\n";
            str += "   " + @"BOI-0A-2" + string.Concat(testResults.Select(t => " & " + ToResultText(t.Value.MethodResults.Boi0A2) +
                                                                               @" & \cellcolor{lightbluegray}")) + @" \\" + "\n";
            str += "   " + @"BOI-0B-1" + string.Concat(testResults.Select(t => " & " + ToResultText(t.Value.MethodResults.Boi0B1) +
                                                                               @" & \cellcolor{lightbluegray}")) + @" \\" + "\n";
            str += "   " + @"BOI-0C-1" + string.Concat(testResults.Select(t => " & " + ToResultText(t.Value.MethodResults.Boi0C1) +
                                                                               @" & \cellcolor{lightbluegray}")) + @" \\" + "\n";
            str += "   " + @"BOI-0C-2" + string.Concat(testResults.Select(t => " & " + ToResultText(t.Value.MethodResults.Boi0C2) +
                                                                               @" & \cellcolor{lightbluegray}")) + @" \\" + "\n";
            str += "   " + @"BOI-0D-1" + string.Concat(testResults.Select(t => " & " + ToResultText(t.Value.MethodResults.Boi0D1) +
                                                                               @" & \cellcolor{lightbluegray}")) + @" \\" + "\n";
            str += "   " + @"BOI-0D-2" + string.Concat(testResults.Select(t => " & " + ToResultText(t.Value.MethodResults.Boi0D2) +
                                                                               @" & \cellcolor{lightbluegray}")) + @" \\" + "\n";
            str += "   " + @"\grayhline" + "\n";
            str += "   " + @"BOI-1A-1 " + string.Concat(testResults.Select(t => " & " + ToResultText(t.Value.MethodResults.Boi1A1) + " & " +
                                                                                ToResultText(t.Value.MethodResults.Boi1A1P))) + @" \\" + "\n";
            str += "   " + @"BOI-1A-2 " + string.Concat(testResults.Select(t => " & " + ToResultText(t.Value.MethodResults.Boi1A2) + " & " +
                                                                                ToResultText(t.Value.MethodResults.Boi1A2P))) + @" \\" + "\n";
            str += "   " + @"\grayhline " + "\n";
            str += "   " + @"BOI-2A-1 " + string.Concat(testResults.Select(t => " & " + ToResultText(t.Value.MethodResults.Boi2A1) + " & " +
                                                                                ToResultText(t.Value.MethodResults.Boi2A1P))) + @" \\" + "\n";
            str += "   " + @"BOI-2B-1 " + string.Concat(testResults.Select(t => " & " + ToResultText(t.Value.MethodResults.Boi2B1) +
                                                                                @" & \cellcolor{lightbluegray}")) + @" \\" + "\n";
            str += "   " + @"\grayhline " + "\n";
            str += "   " + @"BOI-3A-1 " + string.Concat(testResults.Select(t => " & " + ToResultText(t.Value.MethodResults.Boi3A1) +
                                                                                @" & \cellcolor{lightbluegray}")) + @" \\" + "\n";
            str += "   " + @"BOI-3B-1 " + string.Concat(testResults.Select(t => " & " + ToResultText(t.Value.MethodResults.Boi3B1) +
                                                                                @" & \cellcolor{lightbluegray}")) + @" \\" + "\n";
            str += "   " + @"BOI-3C-1 " + string.Concat(testResults.Select(t => " & " + ToResultText(t.Value.MethodResults.Boi3C1) + " & " +
                                                                                ToResultText(t.Value.MethodResults.Boi3C1P))) + @" \\" + "\n";
            str += @"   \hline" + "\n";
            str += @"\end{longtable}" + "\n";

            str += "\n";

            str += "De tekens die in deze tabel zijn opgenomen hebben de volgende betekenis:" + "\n";
            str += @"\begin{itemize}" + "\n";
            str += @"   \item[\cmark] Alle tests van deze methode zijn succesvol uitgevoerd" + "\n";
            str += @"   \item[\xmark] E\'en of meerdere tests van deze methode is/zijn niet succesvol uitgevoerd" + "\n";
            str += @"   \item[\nmark] Deze methode is niet getest als onderdeel van de betreffende benchmarktest" + "\n";
            str += @"\end{itemize}" + "\n";

            File.WriteAllText(destinationFileName, str);
        }

        private static string ReplaceFailureMechanismsTableWithResult(string template, BenchmarkTestResult result)
        {
            var str = "";
            for (var index = 0; index < result.FailureMechanismResults.Count; index++)
            {
                BenchmarkFailureMechanismTestResult m = result.FailureMechanismResults[index];
                str += m.Name + " & " + m.MechanismId + " & " + m.HasLengthEffect + " & " + m.AssemblyMethod + " & " +
                       ToResultText(m.AreEqualFailureMechanismSectionsResults) + " & " +
                       ToResultText(m.AreEqualFailureMechanismSectionsResults) + " & " +
                       ToResultText(m.AreEqualFailureMechanismResult) + " & " +
                       ToResultText(m.AreEqualFailureMechanismResultPartial) + " & " +
                       ToResultText(m.AreEqualFailureMechanismTheoreticalBoundaries) + " & " +
                       ToResultText(m.AreEqualFailureMechanismTheoreticalBoundariesPartial) + " & " +
                       ToResultText(m.AreEqualCombinedResultsCombinedSections);
                if (index != result.FailureMechanismResults.Count - 1)
                {
                    str += @" \B \\ \T" + "\n";
                }
            }

            return template.Replace("$FailureMechanismResultsTable$", str);
        }

        private static string ReplaceCategoriesKeywordsWithResult(string template, BenchmarkTestResult result)
        {
            template = template.Replace("$AreEqualCategoriesListAssessmentSection$",
                                        ToResultText(result.AreEqualCategoriesListAssessmentSection));
            template = template.Replace("$AreEqualCategoriesListInterpretationCategories$",
                                        ToResultText(result.AreEqualCategoriesListInterpretationCategories));
            return template;
        }

        private static string ReplaceFinalVerdictKeywordsWithResult(string template, BenchmarkTestResult result)
        {
            template = template.Replace("$AreEqualAssemblyResultFinalVerdict$",
                                        ToResultText(result.AreEqualAssemblyResultFinalVerdict));
            template = template.Replace("$AreEqualAssemblyResultFinalVerdictProbability$",
                                        ToResultText(result.AreEqualAssemblyResultFinalVerdictProbability));
            template = template.Replace("$AreEqualAssemblyResultFinalVerdictProbabilityPartial$",
                                        ToResultText(result.AreEqualAssemblyResultFinalVerdictProbabilityPartial));
            template = template.Replace("$AreEqualAssemblyResultFinalVerdictPartial$",
                                        ToResultText(result.AreEqualAssemblyResultFinalVerdictPartial));
            return template;
        }

        private static string ReplaceCommonSectionsKeywordsWithResult(string template, BenchmarkTestResult result)
        {
            template = template.Replace("$AreEqualAssemblyResultCombinedSections$",
                                        ToResultText(result.AreEqualAssemblyResultCombinedSections));
            template = template.Replace("$AreEqualAssemblyResultCombinedSectionsResults$",
                                        ToResultText(result.AreEqualAssemblyResultCombinedSectionsResults));
            template = template.Replace("$AreEqualAssemblyResultCombinedSectionsResultsPartial$",
                                        ToResultText(result.AreEqualAssemblyResultCombinedSectionsResultsPartial));
            return template;
        }

        private static void WriteReportToDestination(string template, string reportDirectory, string fileName)
        {
            string destinationFileName = Path.Combine(reportDirectory, Path.GetFileName(fileName).Replace(" ", "_"));
            if (File.Exists(destinationFileName))
            {
                throw new ArgumentException();
            }

            File.WriteAllText(destinationFileName, template);
        }

        private static string GetTargetFileNameFromInputName(string resultFileName)
        {
            return Path.ChangeExtension(resultFileName, "tex");
        }

        private static string GetReportTemplate()
        {
            var assembly = System.Reflection.Assembly.GetExecutingAssembly();
            const string resourceName = "Assembly.Kernel.Acceptance.Test.Resources.reporttemplate.tex";

            string templateString;
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                using (var reader = new StreamReader(stream))
                {
                    templateString = reader.ReadToEnd();
                }
            }

            return templateString;
        }

        private static string ToResultText(bool? result)
        {
            if (result == null)
            {
                return @"\nmark";
            }

            return (bool) result ? @"\cmark" : @"\xmark";
        }
    }
}