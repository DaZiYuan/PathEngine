using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PathEngine.Pipelines.GetterMiddles
{
    public enum PathDataType
    {
        File,//文件路径
        Registry,//注册表路径
        Embedded,//内嵌路径
    }

    /// <summary>
    /// 路径信息
    /// </summary>
    internal class PathData
    {

        public PathData(string path)
        {
            Path = path;
            if (path.StartsWith("HKEY_"))
            {
                Type = PathDataType.Registry;
            }
            else if (path.IndexOf("\\") < 0)
            {
                Type = PathDataType.Embedded;
            }
            else
            {
                Type = PathDataType.File;
            }
        }

        public string Path { get; private set; }
        public PathDataType Type { get; private set; }
    }
}
