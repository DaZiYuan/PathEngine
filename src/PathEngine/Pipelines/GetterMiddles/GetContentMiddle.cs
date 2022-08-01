using System;
using System.Collections.Generic;
using System.IO;

namespace PathEngine.Pipelines.GetterMiddles
{
    /// <summary>
    /// 获取文件内容
    /// </summary>
    internal class GetContentMiddle : IGetterMiddle
    {
        Payload IGetterMiddle.Input(Payload payload)
        {
            if (payload.Command.Schemas.Contains("content"))
            {
                List<PayloadData> res = new();
                foreach (var item in payload.Data)
                {
                    try
                    {
                        using var reader = new StreamReader(item.GetValue());
                        var content = reader.ReadToEnd();
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
