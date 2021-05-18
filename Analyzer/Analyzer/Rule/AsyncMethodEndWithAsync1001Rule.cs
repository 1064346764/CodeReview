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
        //唯一诊断标识符(此id与resources文件中的前缀需保持一致)
        public const string DiagnosticId = "Test1001";
        //标题
        public static readonly LocalizableString Title = 
            new LocalizableResourceString(nameof(Resources.Test1001Title), Resources.ResourceManager, typeof(Resources));
        //提示信息
        public static readonly LocalizableString MessageFormat = 
            new LocalizableResourceString(nameof(Resources.Test1001MessageFormat), Resources.ResourceManager, typeof(Resources));
        //描述信息
        public static readonly LocalizableString Description = 
            new LocalizableResourceString(nameof(Resources.Test1001Description), Resources.ResourceManager, typeof(Resources));
        //类别
        public const string Category = "Test";

        public static readonly DiagnosticDescriptor Rule = 
            new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Error,  true, Description);

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
        /// 规则描述
        /// </summary>
        private static void HandleMethodDeclaration(SyntaxNodeAnalysisContext context)
        {
            var methodDeclarationSyntax = (MethodDeclarationSyntax)context.Node;
            var hasAsyncModifiers = methodDeclarationSyntax.Modifiers.Any(SyntaxKind.AsyncKeyword);
            var asyncMethodNotEndWithAsync = 
                !methodDeclarationSyntax.Identifier.ValueText.EndsWith("Async", StringComparison.Ordinal);
            if (hasAsyncModifiers && asyncMethodNotEndWithAsync)
                context.ReportDiagnostic(Diagnostic.Create(Rule, methodDeclarationSyntax.Identifier.GetLocation()));
        }
    }
}
