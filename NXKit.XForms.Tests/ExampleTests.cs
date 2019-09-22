using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using NXKit.DOMEvents;
using NXKit.Xml;

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

        [TestMethod]
        public void Test_send()
        {
            var host = Document.Load(new Uri("nx-example:///trigger.xml"));

            var stm = new StringWriter();
            host.Save(stm);
            host = Document.Load(new StringReader(stm.ToString()));

            var trigger = host.Xml.Descendants(XForms.Constants.XForms_1_0 + "trigger").First();
            trigger.Interface<EventTarget>().Dispatch("DOMActivate");
        }

    }

}
