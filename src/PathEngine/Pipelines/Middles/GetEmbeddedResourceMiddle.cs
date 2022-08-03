using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace PathEngine.Pipelines.GetterMiddles
{
    /// <summary>
    /// 获取嵌入资源内容
    /// </summary>
    [Obsolete]
    internal class GetEmbeddedResourceMiddle : IGetterMiddle
    {
        GetterPipelinePayload IGetterMiddle.Input(GetterPipelinePayload payload)
        {
            if (payload.Command.Schemas.Contains("embedded"))
            {
                List<GetterPipelinePayloadData> res = new();
                foreach (var item in payload.Data)
                {
                    var assembly = PathResolver.EntryAssembly;
                    var tmpPath = $"{assembly.GetName().Name}.{item.GetValue().Replace(@"\", ".")}";
                    using StreamReader? reader = new(PathResolver.EntryAssembly.GetManifestResourceStream(tmpPath)!);
                    string tmp = reader.ReadToEnd();
                    res.Add(new GetterPipelinePayloadData(tmp));
                }
                payload.SetData(res.ToArray());
            }
            return payload;
        }
    }
}
