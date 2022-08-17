using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PathEngine.Helpers
{
    internal class FileHelper
    {
        internal static FileHelper Instance { get; set; } = new FileHelper();
        internal string? GetContent(string path)
        {
            if (!File.Exists(path))
                return null;
            using var reader = new StreamReader(path);
            var content = reader.ReadToEnd();
            return content;
        }

        internal List<string> GetList(params string[] paths)
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

                    if (currentPath != null && pathItem.Contains('*'))
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
                                //System.Diagnostics.Debug.WriteLine(r);
                                return r;
                            }).ToList();

                            tmp.ForEach(x =>
                            {
                                //递归获取子目录
                                var tmpRes = GetList(x);
                                result.AddRange(tmpRes);
                            });

                            //包含通配符就终止了，递归会查询后面的路径
                            break;
                        }
                    }

                    currentPath += $"{pathItem}\\";
                }
            }
            return result;
        }

        internal virtual bool SetContent(string path, object? value)
        {
            throw new NotImplementedException();
        }

        //搜索文件
        protected virtual List<string> SearchFile(string path, string searchKey)
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
        protected virtual List<string> SearchFolder(string path, string searchKey)
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
