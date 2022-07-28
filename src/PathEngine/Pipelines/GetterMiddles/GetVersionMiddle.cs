using System.Collections.Generic;
using System.Diagnostics;

namespace PathEngine.Middles
{
    /// <summary>
    /// 获取版本号
    /// </summary>
    internal class GetVersionMiddle : GetterMiddle
    {
        Payload GetterMiddle.Input(Payload payload)
        {
            if (payload.Command.Schemas.Contains("version"))
            {
                List<string> res = new List<string>();
                foreach (var dataItem in payload.Data)
                {
                    string tmpRes;
                    try
                    {
                        FileVersionInfo version = FileVersionInfo.GetVersionInfo(dataItem);
                        tmpRes = version.FileVersion;
                    }
                    catch (System.Exception)
                    {
                        tmpRes = null;
                    }
                    res.Add(tmpRes);
                }
                payload.SetData(res.ToArray());
            }
            return payload;
        }
    }
}
