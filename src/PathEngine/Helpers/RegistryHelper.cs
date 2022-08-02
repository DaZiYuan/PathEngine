using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PathEngine.Helpers
{
    internal class RegistryHelper : FileHelper
    {
        //格式 path:key
        internal new static object? GetContent(string path)
        {
            var tmp = path.Split(':');
            if (tmp.Length > 1)
            {
                var registryKey = GetRegistryKey(tmp[0]);
                var registryData = tmp[1];
                if (registryData.Contains('*'))
                {
                    var names = registryKey?.GetValueNames();
                    registryData = names?.FirstOrDefault(m => m.Contains(registryData.Replace("*", "")));
                }

                var tmpRes = registryKey?.GetValue(registryData);
                return tmpRes;
            }
            return null;
        }

        internal static RegistryKey? GetRootRegistry(string path, out string keyPath)
        {
            var rootEndIndex = path.IndexOf("\\") + 1;
            var root = path[..rootEndIndex];
            keyPath = path[rootEndIndex..];
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

        internal static RegistryKey? GetRegistryKey(string path, bool canWrite = false)
        {
            using RegistryKey? rootKey = GetRootRegistry(path, out string keyPath);
            RegistryKey? key = rootKey?.OpenSubKey(keyPath, canWrite);
            return key;
        }
    }
}
