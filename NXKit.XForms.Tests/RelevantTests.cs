using System.Linq;
using System.Xml.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using NXKit.DOMEvents;
using NXKit.Xml;

namespace NXKit.XForms.Tests
{

    [TestClass]
    public class RelevantTests
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

        NXDocumentHost GetSampleDocument()
        {
            return NXDocumentHost.Load(Sample);
        }

        [TestMethod]
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
            d.Invoke();
            Assert.IsFalse(inputs[1].UIBinding.Relevant);

            inputs[0].UIBinding.Value = "true";
            d.Invoke();
            Assert.IsTrue(inputs[1].UIBinding.Relevant);

            inputs[0].UIBinding.Value = "false";
            d.Invoke();
            Assert.IsFalse(inputs[1].UIBinding.Relevant);

            inputs[0].UIBinding.Value = "true";
            d.Invoke();
            Assert.IsTrue(inputs[1].UIBinding.Relevant);
        }

        [TestMethod]
        public void Test_value_changed_event()
        {
            var d = GetSampleDocument();

            var inputs = d.Root
                .Descendants(Constants.XForms_1_0 + "input")
                .Select(i => new
                {
                    Input = i.Interface<Input>(),
                    BindingNode = i.Interface<IUIBindingNode>(),
                    Target = i.Interface<INXEventTarget>(),
                })
                .Where(i => i.Input != null)
                .ToList();

            int c = 0;
            inputs[0].Target.AddEventHandler("xforms-value-changed", i => c++);
            inputs[0].BindingNode.UIBinding.Value = "false";
            inputs[0].BindingNode.UIBinding.Value = "false";
            d.Invoke();
            Assert.AreEqual(1, c);

            inputs[0].BindingNode.UIBinding.Value = "true";
            inputs[0].BindingNode.UIBinding.Value = "true";
            d.Invoke();
            Assert.AreEqual(2, c);
        }

        [TestMethod]
        public void Test_disabled_event()
        {
            var d = GetSampleDocument();

            var inputs = d.Root
                .Descendants(Constants.XForms_1_0 + "input")
                .Select(i => new
                {
                    Input = i.Interface<Input>(),
                    BindingNode = i.Interface<IUIBindingNode>(),
                    Target = i.Interface<INXEventTarget>(),
                })
                .Where(i => i.Input != null)
                .ToList();

            int c = 0;
            inputs[1].Target.AddEventHandler("xforms-disabled", i => c++);
            inputs[0].BindingNode.UIBinding.Value = "false";
            inputs[0].BindingNode.UIBinding.Value = "false";
            d.Invoke();
            Assert.AreEqual(1, c);
            Assert.IsFalse(inputs[1].BindingNode.UIBinding.Relevant);
        }

        [TestMethod]
        public void Test_enabled_event()
        {
            var d = GetSampleDocument();

            var inputs = d.Root
                .Descendants(Constants.XForms_1_0 + "input")
                .Select(i => new
                {
                    Input = i.Interface<Input>(),
                    BindingNode = i.Interface<IUIBindingNode>(),
                    Target = i.Interface<INXEventTarget>(),
                })
                .Where(i => i.Input != null)
                .ToList();

            int c = 0;
            inputs[1].Target.AddEventHandler("xforms-enabled", i => c++);
            inputs[0].BindingNode.UIBinding.Value = "false";
            d.Invoke();
            inputs[0].BindingNode.UIBinding.Value = "true";
            d.Invoke();
            Assert.AreEqual(1, c);
            Assert.IsTrue(inputs[1].BindingNode.UIBinding.Relevant);
        }

    }

}
