using PathEngine.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PathEngine.Pipelines.Middles
{
    internal class SetContentMiddle : IMiddle
    {
        public Payload Input(Payload payload)
        {
            if (payload.Command.Schemas.Contains("content"))
            {
                List<PayloadData> res = new();
                foreach (var item in payload.Data)
                {
                    //不是path，直接返回原有值
                    if (item.Content is not PathData pData)
                    {
                        res.Add(item);
                        continue;
                    }
                    try
                    {
                        object? content = null;
                        switch (pData.Type)
                        {
                            case PathDataType.File:
                                content = FileHelper.Instance.GetContent(pData.Path);
                                break;
                            case PathDataType.Registry:
                                content = RegistryHelper.Instance.GetContent(pData.Path);
                                break;
                            case PathDataType.Embedded:
                                content = EmbeddedResourceHelper.Instance.GetContent(pData.Path);
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
