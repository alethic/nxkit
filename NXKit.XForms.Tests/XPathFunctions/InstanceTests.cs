using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NXKit.XForms.Tests.XPathFunctions
{

    [TestClass]
    public class InstanceTests
    {

        [TestMethod]
        public void Test_default_instance_resolution()
        {
            var c = new NXDocumentConfiguration();
            c.AddModule<XFormsModule>();

            var e = NXDocument.Parse(@"
<unknown xmlns:xf=""http://www.w3.org/2002/xforms"">
    <xf:model id=""data"">
        <xf:instance id=""instance1"">
            <data xmlns="""">node1</data>
        </xf:instance>
        <xf:instance id=""instance2"">
            <data xmlns="""">node2</data>
        </xf:instance>
    </xf:model>
    <xf:input ref=""xf:instance()"" />
</unknown>",
                c);

            var input = e.RootVisual
                .Descendants()
                .OfType<XFormsInputVisual>()
                .FirstOrDefault();

            Assert.AreEqual("node1", input.Binding.Value);
        }

        [TestMethod]
        public void Test_specified_instance_resolution()
        {
            var c = new NXDocumentConfiguration();
            c.AddModule<XFormsModule>();

            var e = NXDocument.Parse(@"
                <unknown xmlns:xf=""http://www.w3.org/2002/xforms"">
                    <xf:model id=""data"">
                        <xf:instance id=""instance1"">
                            <data xmlns="""">node1</data>
                        </xf:instance>
                        <xf:instance id=""instance2"">
                            <data xmlns="""">node2</data>
                        </xf:instance>
                    </xf:model>
                    <xf:input ref=""xf:instance('instance2')"" />
                </unknown>", c);

            var input = e.RootVisual
                .Descendants()
                .OfType<XFormsInputVisual>()
                .FirstOrDefault();

            Assert.AreEqual("node2", input.Binding.Value);
        }

    }

}
