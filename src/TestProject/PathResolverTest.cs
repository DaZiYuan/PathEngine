using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Win32;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PathEngine;
using System.Diagnostics;
using System.Reflection;

namespace TestProject
{
    [TestClass]
    public class PathResolverTest
    {
        [TestMethod]
        public void GetPath()
        {
            var res = PathResolver.Instance.Get("%ProgramFiles(x86)%");
            Assert.IsTrue(res == @"C:\Program Files (x86)");

            var res1 = PathResolver.Instance.GetAll(@"path:\%ProgramData%\Microsoft\VisualStudio\Packages\Microsoft.CodeAnalysis*\");
            Assert.IsTrue(res1 != null);

            var res2 = PathResolver.Instance.GetAll(@"path:\%ProgramData%\*\*\*.txt");
            Assert.IsTrue(res2 != null);
        }

        [TestMethod]
        public void GetEmbeddedResource()
        {
            PathResolver.EntryAssembly = Assembly.GetExecutingAssembly();
            var res = PathResolver.Instance.Get(@"embedded:\Configs\config.txt");
            using var reader = new StreamReader(PathResolver.EntryAssembly.GetManifestResourceStream("TestProject.Configs.config.txt")!);
            string tmp = reader.ReadToEnd();
            Assert.IsTrue(res == tmp);
        }

        [TestMethod]
        public void GetContent()
        {
            PathResolver.EntryAssembly = Assembly.GetExecutingAssembly();
            var res = PathResolver.Instance.Get(@"path_content:\Configs\config2.txt");

            Assert.IsTrue(res == "hello world!!!");
        }

        [TestMethod]
        public void GetVersion()
        {
            string exe = @"C:\Windows\System32\cmd.exe";
            var res = PathResolver.Instance.Get(@$"version:\{exe}");
            FileVersionInfo version = FileVersionInfo.GetVersionInfo(exe)!;

            Assert.IsTrue(res == version.FileVersion!.ToString());
        }

        [TestMethod]
        public void GetRegistryContent()
        {
            var res = PathResolver.Instance.Get(@"registry:\HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\Microsoft\EdgeUpdate\Clients\{F3017226-FE2A-4295-8BDF-00C3A9A7E4C5}:pv");
            using var tmpKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\WOW6432Node\Microsoft\EdgeUpdate\Clients\{F3017226-FE2A-4295-8BDF-00C3A9A7E4C5}")!;
            var tmpValue = tmpKey.GetValue("pv")?.ToString();
            Assert.IsTrue(res == tmpValue);

            //无协议的情况
            res = PathResolver.Instance.Get(@"HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\Microsoft\EdgeUpdate\Clients\{F3017226-FE2A-4295-8BDF-00C3A9A7E4C5}:pv");
            Assert.IsTrue(res == tmpValue);
        }
    }
}
