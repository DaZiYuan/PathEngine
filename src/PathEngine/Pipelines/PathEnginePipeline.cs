using PathEngine.Middles;
using System;

namespace PathEngine.Pipelines
{
    internal class PathEnginePipeline
    {
        readonly IPathEngineMiddle[] _middles;
        internal PathEnginePipeline()
        {
            _middles = new IPathEngineMiddle[] { new PathMiddle(), new FileContentMiddle(), new EmbeddedResMiddle(), new RegistrysMiddle(), new VersionMiddle() };
        }

        internal string[] Handle(string input)
        {
            try
            {
                var payload = new PathEnginePayload(input);
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
