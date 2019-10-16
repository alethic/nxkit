using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;

using Autofac;

using Cogito.Autofac;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using NXKit.Autofac;
using NXKit.Composition;
using NXKit.DOMEvents;
using NXKit.Xml;

namespace NXKit.XForms.Tests
{

    [TestClass]
    public class ExampleTests
    {

        ICompositionContext CreateCompositionContext()
        {
            var bld = new ContainerBuilder();
            bld.RegisterAllAssemblyModules();
            var cnt = bld.Build();
            return cnt.Resolve<ICompositionContext>();
        }

        [TestMethod]
        public void Test_form()
        {
            var host = Document.Load(new Uri("nx-example:///form.xml"), CreateCompositionContext());
        }

        [TestMethod]
        public void Test_form_save_load()
        {
            var host = Document.Load(new Uri("nx-example:///form.xml"), CreateCompositionContext());
            var save = new MemoryStream();
            host.Save(save);
            save = new MemoryStream(save.ToArray());
            var load = XDocument.Load(save);
            host = Document.Load(load, CreateCompositionContext());
        }

        [TestMethod]
        public void Test_include()
        {
            var host = Document.Load(new Uri("nx-example:///include.xml"), CreateCompositionContext());
        }

        [TestMethod]
        public void Test_script()
        {
            var host = Document.Load(new Uri("nx-example:///script.xml"), CreateCompositionContext());
        }

        [TestMethod]
        public void Test_select1()
        {
            var host = Document.Load(new Uri("nx-example:///select1.xml"), CreateCompositionContext());
        }

        [TestMethod]
        public void Test_send()
        {
            var host = Document.Load(new Uri("nx-example:///trigger.xml"), CreateCompositionContext());

            var stm = new StringWriter();
            host.Save(stm);
            host = Document.Load(new StringReader(stm.ToString()), CreateCompositionContext());

            var trigger = host.Xml.Descendants(XForms.Constants.XForms_1_0 + "trigger").First();
            trigger.Interface<EventTarget>().Dispatch("DOMActivate");
        }

    }

}
