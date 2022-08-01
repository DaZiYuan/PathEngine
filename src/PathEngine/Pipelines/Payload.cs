using PathEngine.Pipelines.GetterMiddles;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace PathEngine.Pipelines
{
    public class PayloadData
    {
        public PayloadData()
        {
            
        }
        public PayloadData(object? content)
        {
            Content = content;
        }

        public object? Content { get; set; }

        internal string GetValue()
        {
            var res = GetValue<string>();
            return res ?? string.Empty;
        }
        internal T? GetValue<T>()
        {
            var type = typeof(T);
            switch (type)
            {
                case Type t when t == typeof(string):
                    return (T?)Convert.ChangeType(Content?.ToString(), type);
                case Type t when t == typeof(int):
                    return (T?)Convert.ChangeType((int?)Content, type);
                default:
                    break;
            }

            return default;
        }
    }
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
            Data = new PayloadData[] { new PayloadData(content ?? Command.URN) };
        }

        public void SetData(PayloadData[] data, [CallerFilePath] string callerFilePath = "", [CallerMemberName] string callerMemberName = "")
        {
            Data = data;
            callerFilePath = callerFilePath[callerFilePath.LastIndexOf(@"\")..];
            string caller = $"{callerFilePath} {callerMemberName}";
            Logger.Add($"{caller} ${data}");
        }
        public List<string> Logger { get; } = new List<string>();
        public PayloadData[] Data { get; private set; }
        public InnerCommand Command { get; private set; }
    }
}
