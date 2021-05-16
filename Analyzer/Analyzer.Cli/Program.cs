using Analyzer.Cli;
using Analyzer.Cli.Helper;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Analyzer.Cli
{
    class Program
    {
        public static async Task<int> Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.InputEncoding = Encoding.UTF8;
            var path = SlnHelper.GetSlnUrl(args.FirstOrDefault());
            var manager = new AnalyzerManager(path);
            await manager.Report();
            return 0;
        }
    }
}
