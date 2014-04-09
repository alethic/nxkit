using System;
using System.IO;
using System.Linq;
using System.Xml;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using NXKit.Util;

namespace NXKit.XForms.Tests
{

    [TestClass]
    public class SaveTests
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
            doc.Invoke();
            Assert.IsFalse(inputs[1].UIBinding.Relevant);

            inputs[0].UIBinding.Value = "true";
            doc.Invoke();
            Assert.IsTrue(inputs[1].UIBinding.Relevant);

            inputs[0].UIBinding.Value = "false";
            doc.Invoke();
            Assert.IsFalse(inputs[1].UIBinding.Relevant);

            inputs[0].UIBinding.Value = "true";
            doc.Invoke();
            Assert.IsTrue(inputs[1].UIBinding.Relevant);

            using (var str = new StringWriter())
            using (var wrt = XmlWriter.Create(str))
            {
                doc.Save(wrt);
                wrt.Flush();

                var xml = str.ToString();
            }
        }

    }

}
