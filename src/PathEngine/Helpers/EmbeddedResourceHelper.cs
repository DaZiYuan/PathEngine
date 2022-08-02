using PathEngine.Pipelines;
using System;
using System.Collections.Generic;
using System.IO;

namespace PathEngine.Helpers
{
    internal class EmbeddedResourceHelper
    {
        //格式 assemblypath:namespacepath/
        //格式 namespacepath/
        internal static string? GetContent(string path)
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

        internal static List<string> GetList(params string[] path)
        {
            throw new NotImplementedException();
        }
    }
}
