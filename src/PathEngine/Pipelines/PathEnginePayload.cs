using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace PathEngine
{
    public class PathEnginePayload
    {
        public class InnerCommand
        {
            public InnerCommand(string source)
            {
                string pattern = @"(.*?):\\(.*)";
                var matches = Regex.Match(source, pattern);
                if (matches.Success && matches.Groups.Count > 2)
                {
                    Schemas = new ReadOnlyCollection<string>(matches.Groups[1].Value.Split('_'));
                    URN = matches.Groups[2].Value;
                }
            }

            public ReadOnlyCollection<string> Schemas { get; set; }
            public string URN { get; set; }
        }

        public PathEnginePayload(string command, string content = null)
        {
            Command = new InnerCommand(command);
            Data = new string[] { content ?? Command.URN };
        }

        public void SetData(string[] data, [CallerFilePath] string callerFilePath = "", [CallerMemberName] string callerMemberName = "")
        {
            Data = data;
            callerFilePath = callerFilePath.Substring(callerFilePath.LastIndexOf(@"\"));
            string caller = $"{callerFilePath} {callerMemberName}";
            Logger.Add($"{caller} ${data}");
        }
        public List<string> Logger { get; } = new List<string>();
        public string[] Data { get; private set; }
        public InnerCommand Command { get; private set; }
    }
}
