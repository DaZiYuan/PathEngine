using PathEngine.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace PathEngine.Pipelines.GetterMiddles
{
    /// <summary>
    /// 获取文件，注册表，嵌入资源等列表
    /// </summary>
    internal class GetListMiddle : IGetterMiddle
    {
        public const string Command = "list";
        public GetterPipelinePayload Input(GetterPipelinePayload payload)
        {
            if (payload.Command.Schemas.Contains(Command))
            {
                List<GetterPipelinePayloadData> res = new();
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
                            tmpRes = FileHelper.GetList(pData.Path);
                            break;
                        case PathDataType.Registry:
                            tmpRes = RegistryHelper.GetList(pData.Path);
                            break;
                        case PathDataType.Embedded:
                            tmpRes = EmbeddedResourceHelper.GetList(pData.Path);
                            break;
                    }

                    res.AddRange(tmpRes.Select(x => new GetterPipelinePayloadData(x)));
                }
                payload.SetData(res.ToArray());
            }

            return payload;
        }
    }
}
