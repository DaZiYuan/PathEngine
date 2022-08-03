using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PathEngine.Pipelines.Middles
{
    /// <summary>
    /// 转换通配符 %xxx%
    /// </summary>
    internal class GetPathMiddle : IGetterMiddle
    {
        public const string Command = "path";

        public GetterPipelinePayload Input(GetterPipelinePayload payload)
        {
            if (payload.Command.Schemas.Contains(Command))
            {
                List<GetterPipelinePayloadData> res = new();
                foreach (var dataItem in payload.Data)
                {
                    string path = Environment.ExpandEnvironmentVariables(dataItem.GetValue());
                    //替换自定义特殊key
                    foreach (var item in PathResolver.Variables)
                    {
                        if (path.Contains(item.Key))
                        {
                            path = path.Replace(item.Key, item.Value);
                        }
                    }

                    res.Add(new GetterPipelinePayloadData(new PathData(path)));
                }
                payload.SetData(res.ToArray());
            }

            return payload;
        }

        private List<GetterPipelinePayloadData> InnerGetAllPath(params string[] paths)
        {
            List<GetterPipelinePayloadData> result = new();
            foreach (var path in paths)
            {
                if (!path.Contains('*'))
                {
                    //不包含通配符
                    result.Add(new GetterPipelinePayloadData(path));
                    continue;
                }

                var pathSlices = path.Split('\\');
                string? currentPath = null;
                for (int i = 0; i < pathSlices.Length; i++)
                {
                    bool isLastOne = i == pathSlices.Length - 1;
                    var pathItem = pathSlices[i];

                    if (pathItem.Contains('*') && Directory.Exists(currentPath))
                    {
                        try
                        {
                            DirectoryInfo dInfo = new(currentPath);
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
                                result.AddRange(allPath.Select(x => new GetterPipelinePayloadData(x)).ToList());
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
