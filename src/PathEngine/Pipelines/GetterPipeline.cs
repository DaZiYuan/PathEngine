using PathEngine.Pipelines.GetterMiddles;
using System;

namespace PathEngine.Pipelines
{
    internal class GetterPipeline
    {
        readonly GetterMiddle[] _middles;
        internal GetterPipeline()
        {
            _middles = new GetterMiddle[] { new GetPathMiddle(), new GetContentMiddle(), new GetEmbeddedResourceMiddle(), new GetRegistrysContentMiddle(), new GetVersionMiddle() };
        }

        internal string[] Handle(string input)
        {
            try
            {
                var payload = new Payload(input);
                foreach (var item in _middles)
                {
                    payload = item.Input(payload);
                }
                return payload.Data;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
