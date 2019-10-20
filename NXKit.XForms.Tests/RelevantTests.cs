using System.Linq;
using System.Xml.Linq;

using Autofac;

using NUnit.Framework;

using NXKit.DOMEvents;
using NXKit.Testing;
using NXKit.Xml;

namespace NXKit.XForms.Tests
{

    public class RelevantTests : NXKitTestFixture
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

        public RelevantTests(NXKitTestFixtureContext context) :
            base(context)
        {

        }

        Document GetSampleDocument()
        {
            return Context.Engine.Load(Sample);
        }

        [Test]
        public void Test_relevant_changes_on_input()
        {
            var d = GetSampleDocument();

            var inputs = d.Root
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
        }

        [Test]
        public void Test_value_changed_event()
        {
            var d = GetSampleDocument();

            var inputs = d.Root
                .Descendants(Constants.XForms + "input")
                .Select(i => new
                {
                    Input = i.Interface<Input>(),
                    BindingNode = i.Interface<IUIBindingNode>(),
                    Target = i.Interface<EventTarget>(),
                })
                .Where(i => i.Input != null)
                .ToList();

            int c = 0;
            inputs[0].Target.AddEventDelegate("xforms-value-changed", i => c++);
            inputs[0].BindingNode.UIBinding.Value = "false";
            inputs[0].BindingNode.UIBinding.Value = "false";
            Assert.AreEqual(1, c);

            inputs[0].BindingNode.UIBinding.Value = "true";
            inputs[0].BindingNode.UIBinding.Value = "true";
            Assert.AreEqual(2, c);
        }

        [Test]
        public void Test_disabled_event()
        {
            var d = GetSampleDocument();

            var inputs = d.Root
                .Descendants(Constants.XForms + "input")
                .Select(i => new
                {
                    Input = i.Interface<Input>(),
                    BindingNode = i.Interface<IUIBindingNode>(),
                    Target = i.Interface<EventTarget>(),
                })
                .Where(i => i.Input != null)
                .ToList();

            int c = 0;
            inputs[1].Target.AddEventDelegate("xforms-disabled", i => c++);
            inputs[0].BindingNode.UIBinding.Value = "false";
            inputs[0].BindingNode.UIBinding.Value = "false";
            Assert.AreEqual(1, c);
            Assert.IsFalse(inputs[1].BindingNode.UIBinding.Relevant);
        }

        [Test]
        public void Test_enabled_event()
        {
            var d = GetSampleDocument();

            var inputs = d.Root
                .Descendants(Constants.XForms + "input")
                .Select(i => new
                {
                    Input = i.Interface<Input>(),
                    BindingNode = i.Interface<IUIBindingNode>(),
                    Target = i.Interface<EventTarget>(),
                })
                .Where(i => i.Input != null)
                .ToList();

            int c = 0;
            inputs[1].Target.AddEventDelegate("xforms-enabled", i => c++);
            inputs[0].BindingNode.UIBinding.Value = "false";
            inputs[0].BindingNode.UIBinding.Value = "true";
            Assert.AreEqual(1, c);
            Assert.IsTrue(inputs[1].BindingNode.UIBinding.Relevant);
        }

    }

}
