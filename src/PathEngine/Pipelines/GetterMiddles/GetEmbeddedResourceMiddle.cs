using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace PathEngine.Pipelines.GetterMiddles
{
    /// <summary>
    /// 获取嵌入资源内容
    /// </summary>
    internal class GetEmbeddedResourceMiddle : GetterMiddle
    {
        Payload GetterMiddle.Input(Payload payload)
        {
            if (payload.Command.Schemas.Contains("embedded"))
            {
                List<string> res = new List<string>();
                foreach (var item in payload.Data)
                {
                    var assembly = PathResolver.EntryAssembly;
                    var tmpPath = $"{assembly.GetName().Name}.{item.Replace(@"\", ".")}";
                    using (var reader = new StreamReader(PathResolver.EntryAssembly.GetManifestResourceStream(tmpPath)))
                    {
                        string tmp = reader.ReadToEnd();
                        res.Add(tmp);
                    }
                }
                payload.SetData(res.ToArray());
            }
            return payload;
        }
    }
}
