using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace MvcMovie.Tests
{
    [TestClass]
    public class NameOfClassTests
    {
        [TestMethod]
        public void All_Controllers_Names_Must_Have_Controller_On_End()
        {
            Assembly assembly = Assembly.LoadFrom("MvcMovie.dll");
            var t = assembly.GetType();
            string nameSpace = "MvcMovie.Controllers";
            var classNames = Assembly.GetExecutingAssembly().GetTypes().Where(n => n.Namespace == nameSpace && n.IsClass).ToList();
        }
    }
}
