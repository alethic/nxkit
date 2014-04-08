using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using NXKit.DOMEvents;
using NXKit.Util;

namespace NXKit.XForms.Tests
{

    [TestClass]
    public class RelevantTests
    {

        static Uri SampleUri = DynamicUriUtil.GetUriFor(@"
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
            return NXDocumentHost.Load(SampleUri);
        }

        [TestMethod]
        public void Test_relevant_changes_on_input()
        {
            var d = GetSampleDocument();

            var inputs = d.Root
                .Descendants()
                .OfType<Input>()
                .ToList();

            Assert.IsTrue(inputs[1].Relevant);

            inputs[0].UIBinding.Value = "false";
            d.Invoke();
            Assert.IsFalse(inputs[1].Relevant);

            inputs[0].UIBinding.Value = "true";
            d.Invoke();
            Assert.IsTrue(inputs[1].Relevant);

            inputs[0].UIBinding.Value = "false";
            d.Invoke();
            Assert.IsFalse(inputs[1].Relevant);

            inputs[0].UIBinding.Value = "true";
            d.Invoke();
            Assert.IsTrue(inputs[1].Relevant);
        }

        [TestMethod]
        public void Test_disabled_event()
        {
            var d = GetSampleDocument();

            var inputs = d.Root
                .Descendants()
                .OfType<Input>()
                .ToList();

            int c = 0;
            inputs[1].Interface<INXEventTarget>().AddEventHandler("xforms-disabled", i => c++);
            inputs[0].UIBinding.Value = "false";
            inputs[0].UIBinding.Value = "false";
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
                .OfType<Input>()
                .ToList();

            int c = 0;
            inputs[1].Interface<INXEventTarget>().AddEventHandler("xforms-enabled", i => c++);
            inputs[0].UIBinding.Value = "false";
            d.Invoke();
            inputs[0].UIBinding.Value = "true";
            d.Invoke();
            Assert.AreEqual(1, c);
            Assert.IsTrue(inputs[1].Relevant);
        }

    }

}
