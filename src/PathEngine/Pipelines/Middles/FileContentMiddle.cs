using System;
using System.Collections.Generic;
using System.IO;

namespace PathEngine.Middles
{
    /// <summary>
    /// 获取文件内容
    /// </summary>
    internal class FileContentMiddle : IPathEngineMiddle
    {
        PathEnginePayload IPathEngineMiddle.Input(PathEnginePayload payload)
        {
            string protocol = "file";
            if (payload.Command.Schemas.Contains(protocol))
            {
                List<string> res = new List<string>();
                foreach (var item in payload.Data)
                {
                    try
                    {
                        using (var reader = new StreamReader(item))
                        {
                            var content = reader.ReadToEnd();
                            res.Add(content);
                        }
                    }
                    catch (Exception)
                    {
                        res.Add(null);
                    }
                }
                payload.SetData(res.ToArray());

            }
            return payload;
        }
    }
}
