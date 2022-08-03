using Microsoft.VisualStudio.TestTools.UnitTesting;
using PathEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TestProject
{
    [TestClass]
    public class SetTest
    {
        [TestMethod]
        public void SetContent()
        {
            PathResolver.EntryAssembly = Assembly.GetExecutingAssembly();

            //设置注册表
            var res = PathResolver.Instance.Set(@"path_content:\HKEY_LOCAL_MACHINE\SOFTWARE\MyApp:key", "1");
            var res1 = PathResolver.Instance.Set(@"path:\HKEY_LOCAL_MACHINE\SOFTWARE\MyApp:key2", 2);
        }
    }
}
