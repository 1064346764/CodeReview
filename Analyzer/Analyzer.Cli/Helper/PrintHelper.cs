using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Analyzer.Cli.Helper
{
    /// <summary>
    ///     输出帮助类
    /// </summary>
    public static class PrintHelper
    {
        public static void Print(string tile, Action innerPrint, char splitChar = '=', int splitCharCount = 100)
        {
            splitCharCount = (splitCharCount - tile.Length) / 2;
            var splitStr = new string(splitChar, splitCharCount);
            Console.WriteLine($"{splitStr} {tile} {splitStr}");
            innerPrint();
            // Console.WriteLine(endStr);
        }
        
        public static void PrintSolutionInfo(this Solution solution)
        {
            Print("Load Info", () =>
            {
                Console.WriteLine($"Load solution from :{solution.FilePath}");
                Console.WriteLine($"Load Projects Count: {solution.Projects.Count()}");
                Console.WriteLine($"Load Document Count: {solution.Projects.Sum(p => p.DocumentIds.Count)}");
            });
        }
        
        public static void PrintReport(this Dictionary<string, Dictionary<string, IList<string>>> reportsDict)
        {
            Print("Reports", () =>
            {
                foreach (var reports in reportsDict)
                {
                    Console.WriteLine($"Level: {reports.Key}");
                    Console.WriteLine();
                    foreach (var report in reports.Value)
                    {
                        Console.WriteLine($"\tRules {report.Key}: {report.Value.Count}");
                        Console.WriteLine();
                        foreach (var value in report.Value)
                            Console.WriteLine($"\t\t{value}");
                    }
                }
            });
        }
        
        public static void PrintAnalyzersInfo(this ImmutableArray<DiagnosticAnalyzer> analyzers)
        {
            Print("Analyzer Info", () =>
            {
                Console.WriteLine($"Load Analyzers: {analyzers.Length}");
                foreach (var analyzer in analyzers)
                {
                    Console.WriteLine($"\t{analyzer}");
                }
            });
        }
    }
}
