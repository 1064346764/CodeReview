using System.Threading.Tasks;

namespace Analyzer.Test.Data.Test1001
{
    /// <summary>
    /// ������֤�첽�����Ƿ���Async��β
    /// </summary>
    public class ValidClass
    {
        public async Task TestAsyncMethodAsync()
        {
            await Task.CompletedTask;
        }
    }
}
