using System.Collections.Generic;
using System.IO;

namespace PathEngine.Helpers
{
    internal class EmbeddedResourceHelper : FileHelper
    {
        internal new static EmbeddedResourceHelper Instance { get; set; } = new EmbeddedResourceHelper();
        //格式 assemblypath:namespacepath/
        //格式 namespacepath/
        internal new string? GetContent(string path)
        {
            var tmp = path.Split(':');
            var assembly = PathResolver.EntryAssembly;
            string? assemblyName = null;
            if (tmp.Length > 1)
            {
                assemblyName = tmp[0];
                path = tmp[1];
            }

            if (assemblyName == null)
            {
                assemblyName = assembly.GetName().Name;
            }

            if (assembly == null)
                return null;

            if (!path.StartsWith(assemblyName))
                path = $"{assemblyName}.{path.Replace(@"\", ".")}";
            using StreamReader? reader = new(PathResolver.EntryAssembly.GetManifestResourceStream(path)!);
            string? res = reader.ReadToEnd();
            return res;
        }

        protected override List<string> SearchFile(string path, string searchKey)
        {
            //todo 有需求在做
            return base.SearchFile(path, searchKey);
        }

        protected override List<string> SearchFolder(string path, string searchKey)
        {
            //todo 有需求在做
            return base.SearchFolder(path, searchKey);
        }
    }
}
