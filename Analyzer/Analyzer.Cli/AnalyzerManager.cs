using System;
using System.Collections.Immutable;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.MSBuild;
using Analyzer.Cli.Helper;

namespace Analyzer.Cli
{
    /// <summary>
    ///     分析器管理者
    /// </summary>
    public class AnalyzerManager
    {
        /// <summary>
        ///     工作空间
        /// </summary>
        public static Workspace Workspace { get; set; }

        /// <summary>
        ///     解决方案
        /// </summary>
        public static Solution Solution { get; set; }

        /// <summary>
        ///     分析器集合
        /// </summary>
        public static ImmutableArray<DiagnosticAnalyzer> Analyzers { get; set; }

        /// <summary>
        ///     取消信号
        /// </summary>
        public static CancellationTokenSource Cts = new CancellationTokenSource();
        
        /// <summary>
        ///     程序集
        /// </summary>
        private static Assembly _analyzerAssembly { get; set; }

        public AnalyzerManager(string slnPath)
        {
            using (MSBuildWorkspace workspace = MSBuildWorkspace.Create())
            {
                Workspace = workspace;
                Solution = workspace.OpenSolutionAsync(slnPath).Result;
                Solution.PrintSolutionInfo();
            }
            _analyzerAssembly = Assembly.Load("WiqunAnalyzer");
            Analyzers = _analyzerAssembly.GetAllAnalyzers();
            Analyzers.PrintAnalyzersInfo();
        }
        
        public async Task Report()
        {
            PrintHelper.Print("Analysis Log", () =>
            {
                Console.WriteLine("Analysing..........");
            });
            var diagnostics = await Solution.GetProjectAnalyzerTasks(Analyzers, Cts.Token);
            diagnostics.ParseDiagnostics().PrintReport();
        }
        
        
        
    }
}
