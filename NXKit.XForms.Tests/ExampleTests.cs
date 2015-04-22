using System;
using System.IO;
using System.Xml.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NXKit.XForms.Tests
{

    [TestClass]
    public class ExampleTests
    {

        [TestMethod]
        public void Test_form()
        {
            var host = Document.Load(new Uri("nx-example:///form.xml"));
        }

        [TestMethod]
        public void Test_form_save_load()
        {
            var host = Document.Load(new Uri("nx-example:///form.xml"));
            var save = new MemoryStream();
            host.Save(save);
            save = new MemoryStream(save.ToArray());
            var load = XDocument.Load(save);
            host = Document.Load(load);
        }

        [TestMethod]
        public void Test_include()
        {
            var host = Document.Load(new Uri("nx-example:///include.xml"));
        }

        [TestMethod]
        public void Test_script()
        {
            var host = Document.Load(new Uri("nx-example:///script.xml"));
        }

        [TestMethod]
        public void Test_select1()
        {
            var host = Document.Load(new Uri("nx-example:///select1.xml"));
        }

    }

}
