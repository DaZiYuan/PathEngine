using PathEngine.Pipelines.GetterMiddles;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace PathEngine.Pipelines
{
    public class Payload
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
                else
                {
                    //根据内容生成命令
                    URN = source;
                    List<string> commands = new();
                    if (URN.StartsWith("HKEY_"))
                    {
                        commands.Add(GetRegistrysContentMiddle.Command);
                    }
                    else
                    {
                        commands.Add(GetPathMiddle.Command);
                    }
                    Schemas = new ReadOnlyCollection<string>(commands);
                }
            }

            public ReadOnlyCollection<string> Schemas { get; set; }
            public string URN { get; set; }
        }

        public Payload(string command, string? content = null)
        {
            Command = new InnerCommand(command);
            Data = new string[] { content ?? Command.URN };
        }

        public void SetData(string[] data, [CallerFilePath] string callerFilePath = "", [CallerMemberName] string callerMemberName = "")
        {
            Data = data;
            callerFilePath = callerFilePath[callerFilePath.LastIndexOf(@"\")..];
            string caller = $"{callerFilePath} {callerMemberName}";
            Logger.Add($"{caller} ${data}");
        }
        public List<string> Logger { get; } = new List<string>();
        public string[] Data { get; private set; }
        public InnerCommand Command { get; private set; }
    }
}
