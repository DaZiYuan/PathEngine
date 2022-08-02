using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PathEngine.Helpers
{
    internal class FileHelper
    {
        internal static string? GetContent(string path)
        {
            using var reader = new StreamReader(path);
            var content = reader.ReadToEnd();
            return content;
        }

        internal static List<string> GetList(params string[] paths)
        {
            List<string> result = new();
            foreach (var path in paths)
            {
                if (!path.Contains('*'))
                {
                    //不包含通配符
                    result.Add(path);
                    continue;
                }

                var pathSlices = path.Split('\\');
                string? currentPath = null;
                for (int i = 0; i < pathSlices.Length; i++)
                {
                    bool isLastOne = i == pathSlices.Length - 1;
                    var pathItem = pathSlices[i];

                    if (pathItem.Contains('*'))
                    {
                        if (isLastOne)
                        {
                            var tmp = SearchFile(currentPath, pathItem);
                            result.AddRange(tmp);
                        }
                        else
                        {
                            var tmp = SearchFolder(currentPath, pathItem);
                            string[] restSlices = pathSlices.Skip(i + 1).ToArray();
                            //拼接剩下的路径
                            tmp = tmp.Select(x =>
                            {
                                var r = Path.Combine(x, string.Join("\\", restSlices));
                                return r;
                            }).ToList();

                            tmp.ForEach(x =>
                            {
                                //递归获取子目录
                                var tmpRes = GetList(x);
                                result.AddRange(tmpRes);
                            });

                            //没有子目录，终止
                            if (result.Count == 0)
                                break;
                        }
                    }

                    currentPath += $"{pathItem}\\";
                }
            }
            return result;
        }

        //搜索文件
        protected static List<string> SearchFile(string? path, string searchKey)
        {
            List<string> result = new();
            if (!Directory.Exists(path))
                return result;

            try
            {
                DirectoryInfo dInfo = new(path);
                //文件名通配符
                var allPath = dInfo.GetFiles(searchKey).OrderByDescending(m => m.LastWriteTime).Select(m => m.FullName).ToArray();
                result.AddRange(allPath.ToList());
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }
            return result;
        }

        //搜索目录
        protected static List<string> SearchFolder(string? path, string searchKey)
        {
            List<string> result = new();
            if (!Directory.Exists(path))
                return result;
            
            try
            {
                DirectoryInfo dInfo = new(path);
                var allPath = dInfo.GetDirectories(searchKey).Select(m =>
                {
                    return m.FullName;
                }).ToArray();

                result.AddRange(allPath);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }
            return result;
        }
    }
}
