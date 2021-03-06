﻿#region Copyright (C) Rijkswaterstaat 2019. All rights reserved
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
using System.IO;
using System.Linq;
using assembly.kernel.benchmark.tests.data.Result;

namespace assembly.kernel.benchmark.tests
{
    /// <summary>
    /// Writer to write benchmark test report.
    /// </summary>
    public static class BenchmarkTestReportWriter
    {
        /// <summary>
        /// Writes the report.
        /// </summary>
        /// <param name="order">The order.</param>
        /// <param name="result">The result.</param>
        /// <param name="reportDirectory">The report directory.</param>
        public static void WriteReport(int order, BenchmarkTestResult result, string reportDirectory)
        {
            string template = GetReportTemplate();

            template = template.Replace("$BenchmarkTestName$", result.TestName.Replace("_", @"\_"));
            template = template.Replace("$Order$", order.ToString());

            template = ReplaceFailureMechanismsTableWithResult(template, result);
            template = ReplaceCategoriesKeywordsWithResult(template, result);
            template = ReplaceFinalVerdictKeywordsWithResult(template, result);
            template = ReplaceCommonSectionsKeywordsWithResult(template, result);

            WriteReportToDestination(template, reportDirectory, GetTargetFileNameFromInputName(result.FileName));
        }

        /// <summary>
        /// Writes the summary.
        /// </summary>
        /// <param name="destinationFileName">The name of the destination file.</param>
        /// <param name="testResults">the test results.</param>
        public static void WriteSummary(string destinationFileName, Dictionary<string, BenchmarkTestResult> testResults)
        {
            string str =
                "\\section{Samenvatting van de testresultaten per methode} \n      \\label{sec:summary} \n In deze paragraaf zijn de resultaten tijdens alle benchmarktests weergegeven per methode, zie \\autoref{tab:ResultatenPerMethode}. \n\n";
            str += @"\begin{longtable}[]{| l | " + string.Concat(Enumerable.Repeat("cc |", testResults.Count)) + @" }" + "\n";
            str +=
                @"   \caption{Samenvatting van de resultaten van alle benchmarktests per methode bij volledig assembleren (V) en tussentijds assembleren (T).  \label{tab:ResultatenPerMethode}} \\" +
                "\n";
            str += @"   \hline \T" + "\n";
            str += @"    " + string.Concat(testResults.Select(
                                               t => string.Format(@" & \multicolumn{{2}}{{c|}}{{\rotatebox{{90}}{{{0}  }}}}",
                                                                  t.Value.TestName.Replace("_", @"\_")))) + @" \\" + "\n";
            str += @"   Methode " + string.Concat(Enumerable.Repeat("& V & T ", testResults.Count)) + @"\B \\" + "\n";
            str += @"   \hline" + "\n";
            str += @"   \endhead" + "\n";
            str += @"   \T" + "\n";

            str += "   " + @"WBI-0-1 " +
                   string.Concat(testResults.Select(t => " & " + ToResultText(t.Value.MethodResults.Wbi01) +
                                                         @" & \cellcolor{lightbluegray}")) + @" \\" + "\n";
            str += "   " + @"WBI-0-2 " +
                   string.Concat(testResults.Select(t => " & " + ToResultText(t.Value.MethodResults.Wbi02) +
                                                         @" & \cellcolor{lightbluegray}")) + @" \\" + "\n";
            str += "   " + @"WBI-1-1 " +
                   string.Concat(testResults.Select(t => " & " + ToResultText(t.Value.MethodResults.Wbi11) +
                                                         @" & \cellcolor{lightbluegray}")) + @" \\" + "\n";
            str += "   " + @"WBI-2-1 " +
                   string.Concat(testResults.Select(t => " & " + ToResultText(t.Value.MethodResults.Wbi21) +
                                                         @" & \cellcolor{lightbluegray}")) + @" \\" + "\n";
            str += "   " + @"\grayhline" + "\n";
            str += "   " + @"WBI-0E-1 " +
                   string.Concat(testResults.Select(t => " & " + ToResultText(t.Value.MethodResults.Wbi0E1) +
                                                         @" & \cellcolor{lightbluegray}")) + @" \\" + "\n";
            str += "   " + @"WBI-0E-2 " +
                   string.Concat(testResults.Select(t => " & " + ToResultText(t.Value.MethodResults.Wbi0E2) +
                                                         @" & \cellcolor{lightbluegray}")) + @" \\" + "\n";
            str += "   " + @"WBI-0E-3 " +
                   string.Concat(testResults.Select(t => " & " + ToResultText(t.Value.MethodResults.Wbi0E3) +
                                                         @" & \cellcolor{lightbluegray}")) + @" \\" + "\n";
            str += "   " + @"WBI-0E-4 " +
                   string.Concat(testResults.Select(t => " & " + ToResultText(t.Value.MethodResults.Wbi0E4) +
                                                         @" & \cellcolor{lightbluegray}")) + @" \\" + "\n";
            str += "   " + @"\grayhline" + "\n";
            str += "   " + @"WBI-0G-1 " +
                   string.Concat(testResults.Select(t => " & " + ToResultText(t.Value.MethodResults.Wbi0G1) +
                                                         @" & \cellcolor{lightbluegray}")) + @" \\" + "\n";
            str += "   " + @"WBI-0G-2 " +
                   string.Concat(testResults.Select(t => " & " + ToResultText(t.Value.MethodResults.Wbi0G2) +
                                                         @" & \cellcolor{lightbluegray}")) + @" \\" + "\n";
            str += "   " + @"WBI-0G-3 " +
                   string.Concat(testResults.Select(t => " & " + ToResultText(t.Value.MethodResults.Wbi0G3) +
                                                         @" & \cellcolor{lightbluegray}")) + @" \\" + "\n";
            str += "   " + @"WBI-0G-4 " +
                   string.Concat(testResults.Select(t => " & " + ToResultText(t.Value.MethodResults.Wbi0G4) +
                                                         @" & \cellcolor{lightbluegray}")) + @" \\" + "\n";
            str += "   " + @"WBI-0G-5 " +
                   string.Concat(testResults.Select(t => " & " + ToResultText(t.Value.MethodResults.Wbi0G5) +
                                                         @" & \cellcolor{lightbluegray}")) + @" \\" + "\n";
            str += "   " + @"WBI-0G-6 " +
                   string.Concat(testResults.Select(t => " & " + ToResultText(t.Value.MethodResults.Wbi0G6) +
                                                         @" & \cellcolor{lightbluegray}")) + @" \\" + "\n";
            str += "   " + @"WBI-0T-1 " +
                   string.Concat(testResults.Select(t => " & " + ToResultText(t.Value.MethodResults.Wbi0T1) +
                                                         @" & \cellcolor{lightbluegray}")) + @" \\" + "\n";
            str += "   " + @"WBI-0T-2 " +
                   string.Concat(testResults.Select(t => " & " + ToResultText(t.Value.MethodResults.Wbi0T2) +
                                                         @" & \cellcolor{lightbluegray}")) + @" \\" + "\n";
            str += "   " + @"WBI-0T-3 " +
                   string.Concat(testResults.Select(t => " & " + ToResultText(t.Value.MethodResults.Wbi0T3) +
                                                         @" & \cellcolor{lightbluegray}")) + @" \\" + "\n";
            str += "   " + @"WBI-0T-4 " +
                   string.Concat(testResults.Select(t => " & " + ToResultText(t.Value.MethodResults.Wbi0T4) +
                                                         @" & \cellcolor{lightbluegray}")) + @" \\" + "\n";
            str += "   " + @"WBI-0T-5 " +
                   string.Concat(testResults.Select(t => " & " + ToResultText(t.Value.MethodResults.Wbi0T5) +
                                                         @" & \cellcolor{lightbluegray}")) + @" \\" + "\n";
            str += "   " + @"WBI-0T-6 " +
                   string.Concat(testResults.Select(t => " & " + ToResultText(t.Value.MethodResults.Wbi0T6) +
                                                         @" & \cellcolor{lightbluegray}")) + @" \\" + "\n";
            str += "   " + @"WBI-0T-7 " +
                   string.Concat(testResults.Select(t => " & " + ToResultText(t.Value.MethodResults.Wbi0T7) +
                                                         @" & \cellcolor{lightbluegray}")) + @" \\" + "\n";
            str += "   " + @"\grayhline" + "\n";
            str += "   " + @"WBI-0A-1 " +
                   string.Concat(testResults.Select(t => " & " + ToResultText(t.Value.MethodResults.Wbi0A1) +
                                                         @" & \cellcolor{lightbluegray}")) + @" \\" + "\n";
            str += "   " + @"\grayhline" + "\n";
            str += "   " + @"WBI-1A-1 " +
                   string.Concat(testResults.Select(t => " & " + ToResultText(t.Value.MethodResults.Wbi1A1) + " & " +
                                                         ToResultText(t.Value.MethodResults.Wbi1A1T))) + @" \\" + "\n";
            str += "   " + @"WBI-1A-2 " +
                   string.Concat(testResults.Select(t => " & " + ToResultText(t.Value.MethodResults.Wbi1A2) + " & " +
                                                         ToResultText(t.Value.MethodResults.Wbi1A2T))) + @" \\" + "\n";
            str += "   " + @"WBI-1B-1 " +
                   string.Concat(testResults.Select(t => " & " + ToResultText(t.Value.MethodResults.Wbi1B1) + " & " +
                                                         ToResultText(t.Value.MethodResults.Wbi1B1T))) + @" \\" + "\n";
            str += "   " + @"\grayhline " + "\n";
            str += "   " + @"WBI-2A-1 " +
                   string.Concat(testResults.Select(t => " & " + ToResultText(t.Value.MethodResults.Wbi2A1) + " & " +
                                                         ToResultText(t.Value.MethodResults.Wbi2A1T))) + @" \\" + "\n";
            str += "   " + @"WBI-2B-1 " +
                   string.Concat(testResults.Select(t => " & " + ToResultText(t.Value.MethodResults.Wbi2B1) + " & " +
                                                         ToResultText(t.Value.MethodResults.Wbi2B1T))) + @" \\" + "\n";
            str += "   " + @"WBI-2C-1 " +
                   string.Concat(testResults.Select(t => " & " + ToResultText(t.Value.MethodResults.Wbi2C1) + " & " +
                                                         ToResultText(t.Value.MethodResults.Wbi2C1T))) + @" \\" + "\n";
            str += "   " + @"\grayhline " + "\n";
            str += "   " + @"WBI-3A-1 " +
                   string.Concat(testResults.Select(t => " & " + ToResultText(t.Value.MethodResults.Wbi3A1) +
                                                         @" & \cellcolor{lightbluegray}")) + @" \\" + "\n";
            str += "   " + @"WBI-3B-1 " +
                   string.Concat(testResults.Select(t => " & " + ToResultText(t.Value.MethodResults.Wbi3B1) +
                                                         @" & \cellcolor{lightbluegray}")) + @" \\" + "\n";
            str += "   " + @"WBI-3C-1 " +
                   string.Concat(testResults.Select(t => " & " + ToResultText(t.Value.MethodResults.Wbi3C1) + " & " +
                                                         ToResultText(t.Value.MethodResults.Wbi3C1T))) + @" \\" + "\n";

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
                var m = result.FailureMechanismResults[index];
                str += m.Name + " & " + m.Type.ToString("G") + " & " + m.Group + " & " +
                       ToResultText(m.AreEqualSimpleAssessmentResults) + " & " +
                       ToResultText(m.AreEqualDetailedAssessmentResults) + " & " +
                       ToResultText(m.AreEqualTailorMadeAssessmentResults) + " & " +
                       ToResultText(m.AreEqualCombinedAssessmentResultsPerSection) + " & " +
                       ToResultText(m.AreEqualAssessmentResultPerAssessmentSection) + " & " +
                       ToResultText(m.AreEqualAssessmentResultPerAssessmentSectionTemporal) + " & " +
                       ToResultText(m.AreEqualCombinedResultsCombinedSections) + " & " +
                       ToResultText(m.AreEqualCategoryBoundaries);
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
            template = template.Replace("$AreEqualCategoriesListGroup1and2$",
                                        ToResultText(result.AreEqualCategoriesListGroup1and2));
            return template;
        }

        private static string ReplaceFinalVerdictKeywordsWithResult(string template, BenchmarkTestResult result)
        {
            template = template.Replace("$AreEqualAssemblyResultGroup1and2$",
                                        ToResultText(result.AreEqualAssemblyResultGroup1and2));
            template = template.Replace("$AreEqualAssemblyResultGroup1and2Temporal$",
                                        ToResultText(result.AreEqualAssemblyResultGroup1and2Temporal));
            template = template.Replace("$AreEqualAssemblyResultGroup3and4$",
                                        ToResultText(result.AreEqualAssemblyResultGroup3and4));
            template = template.Replace("$AreEqualAssemblyResultGroup3and4Temporal$",
                                        ToResultText(result.AreEqualAssemblyResultGroup3and4Temporal));
            template = template.Replace("$AreEqualAssemblyResultFinalVerdict$",
                                        ToResultText(result.AreEqualAssemblyResultFinalVerdict));
            template = template.Replace("$AreEqualAssemblyResultFinalVerdictTemporal$",
                                        ToResultText(result.AreEqualAssemblyResultFinalVerdictTemporal));
            return template;
        }

        private static string ReplaceCommonSectionsKeywordsWithResult(string template, BenchmarkTestResult result)
        {
            template = template.Replace("$AreEqualAssemblyResultCombinedSections$",
                                        ToResultText(result.AreEqualAssemblyResultCombinedSections));
            template = template.Replace("$AreEqualAssemblyResultCombinedSectionsResults$",
                                        ToResultText(result.AreEqualAssemblyResultCombinedSectionsResults));
            template = template.Replace("$AreEqualAssemblyResultCombinedSectionsResultsTemporal$",
                                        ToResultText(result.AreEqualAssemblyResultCombinedSectionsResultsTemporal));
            return template;
        }

        private static void WriteReportToDestination(string template, string reportDirectory, string fileName)
        {
            var destinationFileName = Path.Combine(reportDirectory, fileName.Replace(" ", "_"));
            if (File.Exists(destinationFileName))
            {
                throw new ArgumentException();
            }

            File.WriteAllText(destinationFileName, template);
        }

        private static string GetTargetFileNameFromInputName(string resultFileName)
        {
            return Path.GetFileNameWithoutExtension(resultFileName) + ".tex";
        }

        private static string GetReportTemplate()
        {
            var assembly = System.Reflection.Assembly.GetExecutingAssembly();
            var resourceName = "assembly.kernel.benchmark.tests.Resources.reporttemplate.tex";

            string templateString;
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                using (StreamReader reader = new StreamReader(stream))
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