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
    internal class SetTest
    {
        [TestMethod]
        public void SetEmbeddedResource()
        {
            PathResolver.EntryAssembly = Assembly.GetExecutingAssembly();
            var res = PathResolver.Instance.Set(@"embedded:\Configs\config3.txt","1");            
            var res1 = PathResolver.Instance.Set(@"embedded:\Configs\config3.txt", 1);            
        }
    }
}
