using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace PathEngine.Helpers
{
    internal class RegistryHelper : FileHelper
    {
        internal new static RegistryHelper Instance { get; set; } = new RegistryHelper();
        //格式 path:key
        internal new object? GetContent(string path)
        {
            var tmp = path.Split(':');
            if (tmp.Length > 1)
            {
                using var registryKey = GetRegistryKey(tmp[0]);
                var registryData = tmp[1];
                if (registryData.Contains('*'))
                {
                    var names = registryKey?.GetValueNames();
                    registryData = names?.FirstOrDefault(m => SearchStr(m, registryData));
                }

                var tmpRes = registryKey?.GetValue(registryData);
                return tmpRes;
            }
            return null;
        }

        //搜索文件
        protected override List<string> SearchFile(string path, string searchKey)
        {
            List<string> result = new();
            using var registryKey = GetRegistryKey(path);
            if (registryKey != null)
            {
                result = registryKey.GetValueNames().ToList();
                result = result.Where(m => SearchStr(m, searchKey)).ToList();
            }
            result = result.Select(x => $"{path}:{x}").ToList();
            return result;
        }

        //搜索目录
        protected override List<string> SearchFolder(string path, string searchKey)
        {
            List<string> result = new();
            using var registryKey = GetRegistryKey(path);
            if (registryKey != null)
            {
                result = registryKey.GetSubKeyNames().ToList();
                result = result.Where(m => SearchStr(m, searchKey)).ToList();
            }
            result = result.Select(x => Path.Combine(path, x)).ToList();
            return result;
        }

        internal override bool SetContent(string path, object? value)
        {
            try
            {
                var tmp = path.Split(':');
                if (tmp.Length > 1)
                {
                    using var registryKey = GetRegistryKey(tmp[0], true, true);
                    var registryName = tmp[1];
                    if (registryName.Contains('*'))
                    {
                        var names = registryKey?.GetValueNames();
                        registryName = names?.FirstOrDefault(m => SearchStr(m, registryName));
                    }

                    registryKey?.SetValue(registryName, value ?? string.Empty);
                }
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
                return false;
            }
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

        internal static RegistryKey? GetRegistryKey(string path, bool canWrite = false, bool autoCreate = false)
        {
            using RegistryKey? rootKey = GetRootRegistry(path, out string keyPath);
            RegistryKey? key = rootKey?.OpenSubKey(keyPath, canWrite);
            if (key == null && autoCreate)
                key = rootKey?.CreateSubKey(keyPath, RegistryKeyPermissionCheck.ReadWriteSubTree);
            return key;
        }

        internal static bool SearchStr(string str, string searchKey)
        {
            str = str.ToLower();
            searchKey = searchKey.ToLower();
            string searchText = searchKey.Replace("*", "");
            if (string.IsNullOrEmpty(searchText))
                return true;

            //*abc
            if (searchKey.StartsWith("*"))
            {
                string pattern = $".+{searchText}$";
                var res = Regex.Match(str, pattern);
                return res.Success;
            }
            //abc*
            else if (searchKey.EndsWith("*"))
            {
                string pattern = $"^{searchText}.+";
                var res = Regex.Match(str, pattern);
                return res.Success;
            }
            //ab*c
            else
            {
                var tmps = searchKey.Split('*').ToList();
                StringBuilder sb = new();
                for (int i = 0; i < tmps.Count; i++)
                {
                    var item = tmps[i];
                    sb.Append(item);
                    if (i < tmps.Count - 1)
                        sb.Append(".*?");
                }
                string pattern = sb.ToString();
                var res = Regex.Match(str, pattern);
                return res.Success;
            }
        }
    }
}
