using System.IO;
using System.Linq;
using System.Xml.Linq;

using Autofac;

using NUnit.Framework;

using NXKit.Testing;
using NXKit.Xml;

namespace NXKit.XForms.Tests
{

    public class NXDocumentHostTests : NXKitTestFixture
    {

        static XDocument Sample = XDocument.Parse(@"
<unknown xmlns:xf=""http://www.w3.org/2002/xforms"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
    <xf:model id=""data"">
        <xf:instance id=""instance1"">
            <data xmlns="""">true</data>
        </xf:instance>
        <xf:bind ref=""xf:instance('instance1')"" type=""xsd:boolean"" />
        <xf:instance id=""instance2"">
            <data xmlns="""">node2</data>
        </xf:instance>
        <xf:bind ref=""xf:instance('instance2')"" relevant=""xf:instance('instance1') = 'true'"" />
    </xf:model>
    <xf:group>
        <xf:input ref=""xf:instance('instance1')"" />
        <xf:input ref=""xf:instance('instance2')"" />
    </xf:group>
</unknown>");

        public NXDocumentHostTests(NXKitTestFixtureContext context) :
            base(context)
        {

        }

        Document GetSampleDocument()
        {
            return Context.Engine.Load(Sample);
        }


        [Test]
        public void Test_save_after_events()
        {
            var doc = GetSampleDocument();

            var inputs = doc.Root
                .Descendants()
                .Where(i => i.Interfaces<Input>().Any())
                .Select(i => i.Interface<IUIBindingNode>())
                .ToList();

            Assert.IsTrue(inputs[1].UIBinding.Relevant);

            inputs[0].UIBinding.Value = "false";
            Assert.IsFalse(inputs[1].UIBinding.Relevant);

            inputs[0].UIBinding.Value = "true";
            Assert.IsTrue(inputs[1].UIBinding.Relevant);

            inputs[0].UIBinding.Value = "false";
            Assert.IsFalse(inputs[1].UIBinding.Relevant);

            inputs[0].UIBinding.Value = "true";
            Assert.IsTrue(inputs[1].UIBinding.Relevant);

            using (var str = new StringWriter())
            {
                doc.Save(str);
                var xml = str.ToString();
            }
        }

        [Test]
        public void Test_save_load_after_events()
        {
            var doc = GetSampleDocument();

            var inputs = doc.Root
                .Descendants()
                .Where(i => i.Interfaces<Input>().Any())
                .Select(i => i.Interface<IUIBindingNode>())
                .ToList();

            Assert.IsTrue(inputs[1].UIBinding.Relevant);

            inputs[0].UIBinding.Value = "false";
            Assert.IsFalse(inputs[1].UIBinding.Relevant);

            inputs[0].UIBinding.Value = "true";
            Assert.IsTrue(inputs[1].UIBinding.Relevant);

            inputs[0].UIBinding.Value = "false";
            Assert.IsFalse(inputs[1].UIBinding.Relevant);

            inputs[0].UIBinding.Value = "true";
            Assert.IsTrue(inputs[1].UIBinding.Relevant);

            using (var str = new StringWriter())
            {
                doc.Save(str);
                var xml = str.ToString();
                Context.Engine.Load(new StringReader(xml));
            }
        }

    }

}
