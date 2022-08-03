using PathEngine.Pipelines.Middles;
using System;

namespace PathEngine.Pipelines
{
    internal class GetterPipeline
    {
        readonly IMiddle[] _middles;
        internal GetterPipeline()
        {
            _middles = new IMiddle[] { new GetPathMiddle(), new GetListMiddle(), new GetContentMiddle(), new GetVersionMiddle() };
        }

        internal PayloadData[] Handle(Payload payload)
        {
            try
            {
                foreach (var item in _middles)
                {
                    payload = item.Input(payload);
                }
                return payload.Data;
            }
            catch (Exception)
            {
                return Array.Empty<PayloadData>();
            }
        }
    }
}
