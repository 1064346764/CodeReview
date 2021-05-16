using Analyzer.Rule;
using Analyzer.Test.Data;
using Analyzer.Test.Helper;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Xunit;
using DiagnosticVerifier = WiqunAnalyzer.Test.Verifiers.DiagnosticVerifier;

namespace Analyzer.Test.Rlue
{
    public class AsyncMethodEndWithAsync1001RuleTests : DiagnosticVerifier
    {
        /// <summary>
        /// 此文件应该报错
        /// </summary>
        [Fact]
        public void AsyncMethodNotEndWithAsync1()
        {
            var test = DataHelper.GetEmbeddedResource(DataMap.Test1001ValidClass);
            DiagnosticResult expected = CreateResult("Test0.cs", 10);
            VerifyCSharpDiagnostic(test, expected);
        }

        /// <summary>
        /// 此文件应该通过
        /// </summary>
        [Fact]
        public void AsyncMethodNotEndWithAsync2()
        {
            var test = DataHelper.GetEmbeddedResource(DataMap.Test1001FaildClass);
            // DiagnosticResult expected = CreateResult("Test0.cs", 10);
            VerifyCSharpDiagnostic(test);
        }



        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
        {
            return new AsyncMethodEndWithAsync1001Rule();
        }

        private DiagnosticResult CreateResult(string fileName, int lines)
        {
            return new DiagnosticResult
            {
                Id = AsyncMethodEndWithAsync1001Rule.DiagnosticId,
                Message = string.Format(
                    AsyncMethodEndWithAsync1001Rule.MessageFormat.ToString(),
                    fileName,
                    lines),
                Severity = DiagnosticSeverity.Warning,
                Locations = new[]
                {
                    new DiagnosticResultLocation("Test0.cs", lines, 27)
                }
            };
        }
    }
}
