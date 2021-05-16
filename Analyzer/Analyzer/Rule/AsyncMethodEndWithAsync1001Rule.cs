using System;
using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Analyzer.Rule
{

    /// <summary>
    /// 异步方法必须以Async结尾
    /// </summary>
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class AsyncMethodEndWithAsync1001Rule : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "Test1001";
        public static readonly LocalizableString Title = new LocalizableResourceString(nameof(Resources._1001Title), Resources.ResourceManager, typeof(Resources));
        public static readonly LocalizableString MessageFormat = new LocalizableResourceString(nameof(Resources._1001MessageFormat), Resources.ResourceManager, typeof(Resources));
        public static readonly LocalizableString Description = new LocalizableResourceString(nameof(Resources._1001Description), Resources.ResourceManager, typeof(Resources));
        public const string Category = "Test";

        public static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Warning, isEnabledByDefault: true, description: Description);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get { return ImmutableArray.Create(Rule); } }


        private static readonly Action<SyntaxNodeAnalysisContext> MethodDeclarationAction = HandleMethodDeclaration;


        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="context"></param>
        public override void Initialize(AnalysisContext context)
        {
            // 配置生成的代码分析
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);

            // 启用并发执行
            context.EnableConcurrentExecution();

            // 注册语法节点操作
            context.RegisterSyntaxNodeAction(MethodDeclarationAction, SyntaxKind.MethodDeclaration);
        }

        /// <summary>
        /// 句柄接口声明
        /// </summary>
        private static void HandleMethodDeclaration(SyntaxNodeAnalysisContext context)
        {
            var methodDeclarationSyntax = (MethodDeclarationSyntax)context.Node;
            var hasAsyncModifiers = methodDeclarationSyntax.Modifiers.Any(SyntaxKind.AsyncKeyword);
            Console.WriteLine($"{methodDeclarationSyntax.Identifier.ValueText}");
            var asyncMethodNotEndWithAsync = 
                !methodDeclarationSyntax.Identifier.ValueText.EndsWith("Async", StringComparison.Ordinal);
            if (hasAsyncModifiers && asyncMethodNotEndWithAsync)
                context.ReportDiagnostic(Diagnostic.Create(Rule, methodDeclarationSyntax.Identifier.GetLocation()));
        }
    }
}
