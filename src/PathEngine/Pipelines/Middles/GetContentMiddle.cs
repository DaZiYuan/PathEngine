﻿using PathEngine.Helpers;
using System;
using System.Collections.Generic;
using System.IO;

namespace PathEngine.Pipelines.Middles
{
    /// <summary>
    /// 获取文件内容
    /// </summary>
    internal class GetContentMiddle : IGetterMiddle
    {
        GetterPipelinePayload IGetterMiddle.Input(GetterPipelinePayload payload)
        {
            if (payload.Command.Schemas.Contains("content"))
            {
                List<GetterPipelinePayloadData> res = new();
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
                        res.Add(new GetterPipelinePayloadData(content));
                    }
                    catch (Exception)
                    {
                        res.Add(new GetterPipelinePayloadData());
                    }
                }
                payload.SetData(res.ToArray());

            }
            return payload;
        }
    }
}