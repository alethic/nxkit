using System.Xml.Linq;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace NXKit.XForms.Tests
{

    [TestClass]
    public class UnitTest1
    {

        static readonly XNamespace NS_XFORMS = "http://www.w3.org/2002/xforms";
        static readonly XNamespace NS_LAYOUT = "http://www.isillc.com/XML/forms/Layout-1.0";

        [TestMethod]
        public void TestMethod1()
        {
            var c = new EngineConfiguration();
            c.AddModule<XFormsModule>();

            var e = new Engine(c,
@"
                <unknown xmlns:f=""http://www.isillc.com/XML/forms/Layout-1.0"" xmlns:xf=""http://www.w3.org/2002/xforms"">
                    <xf:model id=""data"">
                        <xf:instance id=""instance1"">
                            <data xmlns="""">node1</data>
                        </xf:instance>
                        <xf:instance id=""instance2"">
                            <data xmlns="""">node2</data>
                        </xf:instance>
                    </xf:model>
                    <xf:input ref=""xf:instance('instance2')/data"" />
                </unknown>
", 
                new ResourceSetResolver());

            var input = e.RootVisual
                .Descendants()
                .OfType<XFormsInputVisual>()
                .FirstOrDefault();

            Assert.AreEqual("node2", input.Binding.Value);
        }

    }

}
