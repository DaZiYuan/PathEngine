using Newtonsoft.Json;
using PathEngine.Pipelines;
using PathEngine.Pipelines.Middles;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace PathEngine
{
    public class PathResolver
    {
        private readonly GetterPipeline _getterPipeline = new();
        private readonly SetterPipeline _setterPipeline = new();
        static PathResolver()
        {
            EntryAssembly = Assembly.GetEntryAssembly()!;
            Variables["%app_folder%"] = Path.GetDirectoryName(EntryAssembly.Location)!;
        }
        /// <summary>
        /// 一般不用改，只是为了方便单元测试
        /// </summary>
        public static Assembly EntryAssembly { get; set; }

        public static Dictionary<string, string> Variables { get; private set; } = new();

        /// <summary>
        /// 默认访问实例
        /// </summary>
        public static PathResolver Instance { get; private set; } = new PathResolver();

        #region public methods
        public Task<string?> GetAsync(string configPath)
        {
            return Task.Run(() =>
            {
                return Get(configPath);
            });
        }

        public Task<string?[]> GetAllAsync(string configPath)
        {
            return Task.Run(() =>
            {
                return List(configPath);
            });
        }

        public string? Get(string path)
        {
            return Get<string>(path);
        }


        public bool Set(string path, object value)
        {
            var payload = new Payload(path, null, value);
            if (!payload.Command.Schemas.Contains(SetContentMiddle.Command))
            {
                payload.Command.Schemas.Add(SetContentMiddle.Command);//自动补齐 content命令
            }
            var res = _setterPipeline.Handle(payload);
            return res.Length > 0 ? res[0].GetValue<bool>() : false;
        }


        public T? Get<T>(string path)
        {
            var payload = new Payload(path);
            var res = _getterPipeline.Handle(payload);
            return res.Length > 0 ? res[0].GetValue<T>() : default;
        }

        /// <summary>
        /// 搜索目录以 斜线结尾
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public string?[] List(string path)
        {
            var res = List<string>(path);
            return res;
        }

        public T?[] List<T>(string path)
        {
            var payload = new Payload(path);
            var res = _getterPipeline.Handle(payload);
            return res.Select(x => x.GetValue<T>()).ToArray();
        }

        public static async Task<T?> LoadJsonConfig<T>(string path)
        {
            string? json = await Instance.GetAsync(path);
            if (json == null)
                return default;
            var res = JsonConvert.DeserializeObject<T>(json);
            return res;
        }

        #endregion
    }
}
