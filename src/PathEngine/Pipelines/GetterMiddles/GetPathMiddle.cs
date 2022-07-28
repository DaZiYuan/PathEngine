using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Linq;

namespace PathEngine.Middles
{
    /// <summary>
    /// 转换通配符 %xxx%
    /// </summary>
    internal class GetPathMiddle : GetterMiddle
    {
        public const string Command = "path";
        static readonly Dictionary<string, Func<string>> _dict = new Dictionary<string, Func<string>>()
        {
            {"%app_folder%", GetAppFolder }
        };

        private static string GetAppFolder()
        {
            var res = Path.GetDirectoryName(PathResolver.EntryAssembly.Location);
            return res;
        }

        public Payload Input(Payload payload)
        {
            if (payload.Command.Schemas.Contains(Command))
            {
                List<string> res = new List<string>();
                foreach (var dataItem in payload.Data)
                {
                    string path = Environment.ExpandEnvironmentVariables(dataItem);
                    //替换自定义特殊key
                    foreach (var item in _dict)
                    {
                        if (path.Contains(item.Key))
                        {
                            path = path.Replace(item.Key, item.Value());
                        }
                    }

                    res = InnerGetAllPath(path);
                }
                payload.SetData(res.ToArray());
            }

            return payload;
        }

        private List<string> InnerGetAllPath(params string[] paths)
        {
            List<string> result = new List<string>();
            foreach (var path in paths)
            {
                if (!path.Contains('*'))
                {
                    //不包含通配符
                    result.Add(path);
                    continue;
                }

                var pathSlices = path.Split('\\');
                string currentPath = null;
                for (int i = 0; i < pathSlices.Length; i++)
                {
                    bool isLastOne = i == pathSlices.Length - 1;
                    var pathItem = pathSlices[i];

                    if (pathItem.Contains('*') && Directory.Exists(currentPath))
                    {
                        try
                        {
                            DirectoryInfo dInfo = new DirectoryInfo(currentPath);
                            if (!isLastOne)
                            {
                                var allPath = dInfo.GetDirectories(pathItem).Select(m =>
                                {
                                    string[] tmp = pathSlices.Skip(i + 1).ToArray();
                                    var r = Path.Combine(m.FullName, string.Join("\\", tmp));
                                    return r;
                                }).ToArray();

                                var tmpRes = InnerGetAllPath(allPath);
                                result.AddRange(tmpRes);
                            }
                            else
                            {
                                //文件名通配符
                                var allPath = dInfo.GetFiles(pathItem).OrderByDescending(m => m.LastWriteTime).Select(m => m.FullName).ToArray();
                                result.AddRange(allPath);
                            }
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine(ex);
                            continue;
                        }
                    }

                    currentPath += $"{pathItem}\\";
                }
            }
            return result;
        }
    }
}
