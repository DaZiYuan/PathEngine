using PathEngine.Pipelines.GetterMiddles;
using System;

namespace PathEngine.Pipelines
{
    internal class GetterPipeline
    {
        readonly IGetterMiddle[] _middles;
        internal GetterPipeline()
        {
            _middles = new IGetterMiddle[] { new GetPathMiddle(), new GetListMiddle(), new GetContentMiddle(), new GetVersionMiddle() };
        }

        internal GetterPipelinePayloadData[] Handle(string input)
        {
            try
            {
                var payload = new GetterPipelinePayload(input);
                foreach (var item in _middles)
                {
                    payload = item.Input(payload);
                }
                return payload.Data;
            }
            catch (Exception)
            {
                return Array.Empty<GetterPipelinePayloadData>();
            }
        }
    }
}
