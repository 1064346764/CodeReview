using System.Threading.Tasks;

namespace Analyzer.Test.Data.Test1001
{
    /// <summary>
    /// 用于验证异步方法是否用Async结尾
    /// </summary>
    public class ValidClass
    {
        public async Task TestAsyncMethodAsync()
        {
            await Task.CompletedTask;
        }
    }
}
