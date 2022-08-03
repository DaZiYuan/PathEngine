using PathEngine.Helpers;
using System;
using System.Collections.Generic;

namespace PathEngine.Pipelines.Middles
{
    internal class SetContentMiddle : IMiddle
    {
        public const string Command = "content";
        public Payload Input(Payload payload)
        {
            if (payload.Command.Schemas.Contains(Command))
            {
                List<PayloadData> res = new();
                foreach (var item in payload.Data)
                {
                    string path = item.GetValue();
                    PathDataType type = PathDataType.File;
                    if (item.Content is PathData pData)
                    {
                        path = pData.Path;
                        type = pData.Type;
                    }

                    try
                    {
                        object? content = null;
                        switch (type)
                        {
                            case PathDataType.File:
                                content = FileHelper.Instance.SetContent(path, payload.Value);
                                break;
                            case PathDataType.Registry:
                                content = RegistryHelper.Instance.SetContent(path, payload.Value);
                                break;
                            case PathDataType.Embedded:
                                content = EmbeddedResourceHelper.Instance.SetContent(path, payload.Value);
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
