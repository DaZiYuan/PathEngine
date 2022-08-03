using PathEngine.Helpers;
using System;
using System.Collections.Generic;
using System.IO;

namespace PathEngine.Pipelines.Middles
{
    /// <summary>
    /// 获取文件内容
    /// </summary>
    internal class GetContentMiddle : IMiddle
    {
        Payload IMiddle.Input(Payload payload)
        {
            if (payload.Command.Schemas.Contains("content"))
            {
                List<PayloadData> res = new();
                foreach (var item in payload.Data)
                {
                    string path = item.GetValue();
                    PathDataType type = PathDataType.File;
                    if (item.Content is PathData pData)
                    {
                        path = pData.Path;
                        type = pData.Type;
                    }

                    try
                    {
                        object? content = null;
                        switch (type)
                        {
                            case PathDataType.File:
                                content = FileHelper.Instance.GetContent(path);
                                break;
                            case PathDataType.Registry:
                                content = RegistryHelper.Instance.GetContent(path);
                                break;
                            case PathDataType.Embedded:
                                content = EmbeddedResourceHelper.Instance.GetContent(path);
                                break;
                        }
                        res.Add(new PayloadData(content));
                    }
                    catch (Exception)
                    {
                        res.Add(new PayloadData());
                    }
                }
                payload.SetData(res.ToArray());

            }
            return payload;
        }
    }
}
