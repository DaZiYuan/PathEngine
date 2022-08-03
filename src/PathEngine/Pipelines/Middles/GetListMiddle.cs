using PathEngine.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace PathEngine.Pipelines.Middles
{
    /// <summary>
    /// 获取文件，注册表，嵌入资源等列表
    /// </summary>
    internal class GetListMiddle : IMiddle
    {
        public const string Command = "list";
        public Payload Input(Payload payload)
        {
            if (payload.Command.Schemas.Contains(Command))
            {
                List<PayloadData> res = new();
                foreach (var item in payload.Data)
                {
                    List<string> tmpRes = new();
                    //不是path，直接返回原有值
                    if (item.Content is not PathData pData)
                    {
                        res.Add(item);
                        continue;
                    }

                    switch (pData.Type)
                    {
                        case PathDataType.File:
                            tmpRes = FileHelper.Instance.GetList(pData.Path);
                            break;
                        case PathDataType.Registry:
                            tmpRes = RegistryHelper.Instance.GetList(pData.Path);
                            break;
                        case PathDataType.Embedded:
                            tmpRes = EmbeddedResourceHelper.Instance.GetList(pData.Path);
                            break;
                    }

                    res.AddRange(tmpRes.Select(x => new PayloadData(x)));
                }
                payload.SetData(res.ToArray());
            }

            return payload;
        }
    }
}
