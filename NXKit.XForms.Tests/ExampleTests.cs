using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;

using NUnit.Framework;

using NXKit.DOMEvents;
using NXKit.Testing;
using NXKit.Xml;

namespace NXKit.XForms.Tests
{

    public class ExampleTests : NXKitTestFixture
    {

        public ExampleTests(NXKitTestFixtureContext context) :
            base(context)
        {

        }

        [Test]
        public void Test_form()
        {
            var host = Context.Engine.Load(new Uri("nx-example:///form.xml"));
        }

        [Test]
        public void Test_form_save_load()
        {
            var host = Context.Engine.Load(new Uri("nx-example:///form.xml"));
            var save = new MemoryStream();
            host.Save(save);
            save = new MemoryStream(save.ToArray());
            var load = XDocument.Load(save);
            host = Context.Engine.Load(load);
        }

        [Test]
        public void Test_include()
        {
            var host = Context.Engine.Load(new Uri("nx-example:///include.xml"));
        }

        [Test]
        public void Test_script()
        {
            var host = Context.Engine.Load(new Uri("nx-example:///script.xml"));
        }

        [Test]
        public void Test_select1()
        {
            var host = Context.Engine.Load(new Uri("nx-example:///select1.xml"));
        }

        [Test]
        public void Test_send()
        {
            var host = Context.Engine.Load(new Uri("nx-example:///trigger.xml"));

            var stm = new StringWriter();
            host.Save(stm);
            host = Context.Engine.Load(new StringReader(stm.ToString()));

            var trigger = host.Xml.Descendants(XForms.Constants.XForms_1_0 + "trigger").First();
            trigger.Interface<EventTarget>().Dispatch("DOMActivate");
        }

    }

}
