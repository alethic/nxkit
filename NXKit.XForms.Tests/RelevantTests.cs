using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using NXKit.DOMEvents;

namespace NXKit.XForms.Tests
{

    [TestClass]
    public class RelevantTests
    {

        static string sample = @"
<unknown xmlns:xf=""http://www.w3.org/2002/xforms"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
    <xf:model id=""data"">
        <xf:instance id=""instance1"">
            <data xmlns="""">true</data>
        </xf:instance>
        <xf:bind nodeset=""xf:instance('instance1')"" type=""xsd:boolean"" />
        <xf:instance id=""instance2"">
            <data xmlns="""">node2</data>
        </xf:instance>
        <xf:bind nodeset=""xf:instance('instance2')"" relevant=""xf:instance('instance1') = 'true'"" />
    </xf:model>
    <xf:input ref=""xf:instance('instance1')"" />
    <xf:input ref=""xf:instance('instance2')"" />
</unknown>";

        NXDocument GetSampleDocument()
        {
            var c = new NXDocumentConfiguration();
            c.AddModule<DOMEventsModule>();
            c.AddModule<XFormsModule>();

            return NXDocument.Parse(sample, c);
        }

        [TestMethod]
        public void Test_relevant_changes_on_input()
        {
            var d = GetSampleDocument();

            var inputs = d.Root
                .Descendants()
                .OfType<XFormsInputVisual>()
                .ToList();

            Assert.IsTrue(inputs[1].Relevant);
            inputs[0].Binding.SetValue("false");
            d.Invoke();
            Assert.IsFalse(inputs[1].Relevant);
        }

        [TestMethod]
        public void Test_disabled_event()
        {
            var d = GetSampleDocument();

            var inputs = d.Root
                .Descendants()
                .OfType<XFormsInputVisual>()
                .ToList();

            int c = 0;
            inputs[1].Interface<IEventTarget>().AddEventHandler("xforms-disabled", false, i => c++);
            inputs[0].Binding.SetValue("false");
            d.Invoke();
            Assert.AreEqual(1, c);
            Assert.IsFalse(inputs[1].Relevant);
        }

        [TestMethod]
        public void Test_enabled_event()
        {
            var d = GetSampleDocument();

            var inputs = d.Root
                .Descendants()
                .OfType<XFormsInputVisual>()
                .ToList();

            int c = 0;
            inputs[1].Interface<IEventTarget>().AddEventHandler("xforms-enabled", false, i => c++);
            inputs[0].Binding.SetValue("false");
            d.Invoke();
            inputs[0].Binding.SetValue("true");
            d.Invoke();
            Assert.AreEqual(1, c);
            Assert.IsTrue(inputs[1].Relevant);
        }

    }

}
