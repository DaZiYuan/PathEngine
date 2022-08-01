﻿using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace PathEngine.Pipelines.GetterMiddles
{
    /// <summary>
    /// 获取嵌入资源内容
    /// </summary>
    internal class GetEmbeddedResourceMiddle : IGetterMiddle
    {
        Payload IGetterMiddle.Input(Payload payload)
        {
            if (payload.Command.Schemas.Contains("embedded"))
            {
                List<PayloadData> res = new();
                foreach (var item in payload.Data)
                {
                    var assembly = PathResolver.EntryAssembly;
                    var tmpPath = $"{assembly.GetName().Name}.{item.GetValue().Replace(@"\", ".")}";
                    using StreamReader? reader = new(PathResolver.EntryAssembly.GetManifestResourceStream(tmpPath)!);
                    string tmp = reader.ReadToEnd();
                    res.Add(new PayloadData(tmp));
                }
                payload.SetData(res.ToArray());
            }
            return payload;
        }
    }
}
