using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace PathEngine.Pipelines.Middles
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
                    if (Content is PathData pathData)
                        return (T?)Convert.ChangeType(pathData.Path.ToString(), type);
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
                    Schemas = new List<string>(matches.Groups[1].Value.Split('_'));
                    URN = matches.Groups[2].Value;
                }
                else
                {
                    URN = source;
                    List<string> commands = new()
                    {
                        //默认path命令
                        GetPathMiddle.Command
                    };
                    Schemas = new List<string>(commands);
                }
            }

            public List<string> Schemas { get; set; }
            public string URN { get; set; }
        }

        public Payload(string command, string? content = null, object? value = null)
        {
            Command = new InnerCommand(command);
            Data = new PayloadData[] { new PayloadData(content ?? Command.URN) };
            Value = value;
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
        public object? Value { get; private set; }
        public InnerCommand Command { get; private set; }
    }
}
