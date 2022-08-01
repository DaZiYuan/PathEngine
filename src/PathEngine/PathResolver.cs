using Newtonsoft.Json;
using PathEngine.Pipelines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace PathEngine
{
    public class PathResolver
    {
        private readonly GetterPipeline _getterPipeline = new();
        static PathResolver()
        {
            EntryAssembly = Assembly.GetEntryAssembly()!;
        }
        /// <summary>
        /// 一般不用改，只是为了方便单元测试
        /// </summary>
        public static Assembly EntryAssembly { get; set; }

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
                return GetAll(configPath);
            });
        }

        public string? Get(string path)
        {
            var res = GetAll(path);
            return res.Length > 0 ? res[0] : null;
        }
        public T? Get<T>(string path)
        {
            var res = GetAll<T>(path);
            return res.Length > 0 ? res[0] : default;
        }

        public string?[] GetAll(string path)
        {
            var res = GetAll<string>(path);
            return res;
        }

        public T?[] GetAll<T>(string path)
        {
            var res = _getterPipeline.Handle(path);
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
