using System;
using System.IO;

namespace Analyzer.Cli.Helper
{
    public static class SlnHelper
    {
        /// <summary>
        ///     获取解决方案Url
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string GetSlnUrl(string url)
        {
            if (url == null || Directory.Exists(url) == false)
                throw new FileNotFoundException($"找不到{url}");
            DirectoryInfo theFolder = new DirectoryInfo(url);
            foreach (var file in theFolder.GetFileSystemInfos())
            {
                if (file.Name.Substring(file.Name.LastIndexOf(".", StringComparison.Ordinal) + 1) == "sln")
                {
                    url = url + @"\" + file.Name;
                    break;
                }
            }
            return url;
        }
    }
}
