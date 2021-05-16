using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.MSBuild;

namespace Analyzer.Cli.Helper
{
    public static class RoslynHelper
    {
        public static async Task<(Solution, Workspace)> GetSolutionAndWorkspace(this string slnPath)
        {
            MSBuildWorkspace workspace = MSBuildWorkspace.Create();
            return (await workspace.OpenSolutionAsync(slnPath), workspace);
        }

        public static ImmutableArray<DiagnosticAnalyzer> GetAllAnalyzers(this Assembly assembly)
        {
            var diagnosticAnalyzerType = typeof(DiagnosticAnalyzer);

            var analyzers = ImmutableArray.CreateBuilder<DiagnosticAnalyzer>();

            foreach (var type in assembly.GetTypes())
            {
                if (type.IsSubclassOf(diagnosticAnalyzerType) && !type.IsAbstract)
                {
                    analyzers.Add((DiagnosticAnalyzer)Activator.CreateInstance(type));
                }
            }

            if (analyzers.Count < 1)
                throw new Exception($"程序集{assembly.GetName()} 当前无可用的检查规则");

            return analyzers.ToImmutable();
        }

        public static async Task<ImmutableDictionary<ProjectId, ImmutableArray<Diagnostic>>> GetProjectAnalyzerTasks(this Solution solution, ImmutableArray<DiagnosticAnalyzer> analyzers, CancellationToken cancellationToken)
        {
            List<KeyValuePair<ProjectId, Task<ImmutableArray<Diagnostic>>>> projectDiagnosticTasks = new List<KeyValuePair<ProjectId, Task<ImmutableArray<Diagnostic>>>>();

            // Make sure we analyze the projects in parallel
            foreach (var project in solution.Projects)
            {
                if (project.Language != LanguageNames.CSharp)
                {
                    continue;
                }
                // var projectTmp = project.AddMetadataReferences(new[] {
                //     MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
                //     MetadataReference.CreateFromFile(typeof(CSharpCompilation).Assembly.Location),
                //     MetadataReference.CreateFromFile(typeof(Compilation).Assembly.Location) });

                projectDiagnosticTasks.Add(new KeyValuePair<ProjectId, Task<ImmutableArray<Diagnostic>>>(project.Id, GetProjectAnalyzerDiagnosticsAsync(analyzers, project, cancellationToken)));
            }

            ImmutableDictionary<ProjectId, ImmutableArray<Diagnostic>>.Builder projectDiagnosticBuilder = ImmutableDictionary.CreateBuilder<ProjectId, ImmutableArray<Diagnostic>>();
            foreach (var task in projectDiagnosticTasks)
            {
                projectDiagnosticBuilder.Add(task.Key, await task.Value);
            }

            return projectDiagnosticBuilder.ToImmutable();
        }

        private static async Task<ImmutableArray<Diagnostic>> GetProjectAnalyzerDiagnosticsAsync(ImmutableArray<DiagnosticAnalyzer> analyzers, Project project, CancellationToken cancellationToken)
        {
            Console.WriteLine($"Project:{project.Name} Start Analysis");
            var supportedDiagnosticsSpecificOptions = new Dictionary<string, ReportDiagnostic>();
            // update the project compilation options
            // var modifiedSpecificDiagnosticOptions = supportedDiagnosticsSpecificOptions.ToImmutableDictionary().SetItems(project.CompilationOptions.SpecificDiagnosticOptions);
            var modifiedCompilationOptions = project.CompilationOptions.WithOutputKind(OutputKind.DynamicallyLinkedLibrary);
            var processedProject = project.WithCompilationOptions(modifiedCompilationOptions);

            Compilation compilation = await processedProject.GetCompilationAsync(cancellationToken);
            CompilationWithAnalyzers compilationWithAnalyzers = compilation.WithAnalyzers(analyzers, project.AnalyzerOptions, cancellationToken);

            var diagnostics = await compilationWithAnalyzers.GetAllDiagnosticsAsync(cancellationToken);
            Console.WriteLine($"Project:{project.Name} Analysis End");
            return diagnostics;
        }
        
        public static Dictionary<string, Dictionary<string, IList<string>>> ParseDiagnostics(this ImmutableDictionary<ProjectId, ImmutableArray<Diagnostic>> diagnostics)
        {
            var allDiagnostics = diagnostics.SelectMany(i => i.Value).ToImmutableArray();
            var reportsDict = new Dictionary<string, Dictionary<string, IList<string>>>();

            foreach (var group in allDiagnostics.GroupBy(i => i.Id).OrderBy(i => i.Key, StringComparer.OrdinalIgnoreCase))
            {
                if (!group.Key.StartsWith("Wiqun"))  //如果不是WIKI开头的解析，直接跳过本次
                {
                    continue;
                }
                var first = group.First();
                reportsDict.TryGetValue(first.DefaultSeverity.ToString(), out Dictionary<string, IList<string>> innerDict);
                if (innerDict == null)
                {
                    innerDict = new Dictionary<string, IList<string>>();
                    reportsDict.Add(first.DefaultSeverity.ToString(), innerDict);
                }
                innerDict.TryGetValue(first.DefaultSeverity.ToString(), out IList<string> infos);
                if (infos == null)
                    infos = new List<string>();
                foreach (var val in group.AsQueryable().ToList())
                {
                    infos.Add(val.ToString());
                }
                innerDict.Add(group.Key, infos);
            }
            return reportsDict;
        }
    }
}
