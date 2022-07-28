using Microsoft.Win32;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PathEngine.Middles
{
    /// <summary>
    /// 获取注册表内容
    /// </summary>
    internal class RegistrysMiddle : IPathEngineMiddle
    {
        PathEnginePayload IPathEngineMiddle.Input(PathEnginePayload payload)
        {
            string protocol = "registry";
            if (payload.Command.Schemas.Contains(protocol))
            {
                List<string> res = new List<string>();
                foreach (var item in payload.Data)
                {
                    using (var reader = new StringReader(item))
                    {
                        var tmp = item.Split(':');
                        if (tmp.Length > 1)
                        {
                            var registryKey = GetRegistryKey(tmp[0]);
                            var registryData = tmp[1];
                            if (registryData.Contains("*"))
                            {
                                var names = registryKey.GetValueNames();
                                registryData = names.FirstOrDefault(m => m.Contains(registryData.Replace("*", "")));
                            }

                            var tmpRes = registryKey?.GetValue(registryData);
                            string strRes = null;
                            if (tmpRes is byte[])
                                strRes = System.Text.Encoding.UTF8.GetString(tmpRes as byte[]);
                            else if (tmpRes is string)
                                strRes = tmpRes.ToString();
                            res.Add(strRes);
                        }
                        else
                            res.Add(null);
                    }
                }
                payload.SetData(res?.ToArray());
            }
            return payload;
        }

        internal static RegistryKey GetRootRegistry(string path, out string keyPath)
        {
            var rootEndIndex = path.IndexOf("\\") + 1;
            var root = path.Substring(0, rootEndIndex);
            keyPath = path.Substring(rootEndIndex);
            switch (root)
            {
                case "HKEY_CLASSES_ROOT\\":
                    return Registry.ClassesRoot;
                case "HKEY_CURRENT_USER\\":
                    return Registry.CurrentUser;
                case "HKEY_LOCAL_MACHINE\\":
                    return Registry.LocalMachine;
                case "HKEY_USERS\\":
                    return Registry.Users;
                case "HKEY_CURRENT_CONFIG\\":
                    return Registry.CurrentConfig;
            };
            return null;
        }

        internal static RegistryKey GetRegistryKey(string path, bool canWrite = false)
        {
            using (var rootKey = GetRootRegistry(path, out string keyPath))
            {
                RegistryKey key = rootKey.OpenSubKey(keyPath, canWrite);
                return key;
            }
        }
    }
}
