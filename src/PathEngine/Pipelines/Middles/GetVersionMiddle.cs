using System.Collections.Generic;
using System.Diagnostics;

namespace PathEngine.Pipelines.Middles
{
    /// <summary>
    /// 获取版本号
    /// </summary>
    internal class GetVersionMiddle : IMiddle
    {
        Payload IMiddle.Input(Payload payload)
        {
            if (payload.Command.Schemas.Contains("version"))
            {
                List<PayloadData> res = new();
                foreach (var dataItem in payload.Data)
                {
                    string tmpRes;
                    try
                    {
                        FileVersionInfo version = FileVersionInfo.GetVersionInfo(dataItem.GetValue());
                        tmpRes = version.FileVersion!;
                    }
                    catch (System.Exception)
                    {
                        tmpRes = string.Empty;
                    }
                    res.Add(new(tmpRes));
                }
                payload.SetData(res.ToArray());
            }
            return payload;
        }
    }
}
