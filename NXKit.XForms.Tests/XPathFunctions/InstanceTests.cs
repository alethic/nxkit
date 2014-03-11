﻿using System.Linq;
using System.Xml.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NXKit.XForms.Tests.XPathFunctions
{

    [TestClass]
    public class InstanceTests
    {

        static readonly XNamespace NS_XFORMS = "http://www.w3.org/2002/xforms";

        [TestMethod]
        public void Test_default()
        {
            var c = new NXDocumentConfiguration();
            c.AddModule<XFormsModule>();

            var e = new NXDocument(c,
@"
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
                </unknown>
",
                new ResourceSetResolver());

            var input = e.RootVisual
                .Descendants()
                .OfType<XFormsInputVisual>()
                .FirstOrDefault();

            Assert.AreEqual("node1", input.Binding.Value);
        }

        [TestMethod]
        public void Test_specified_id()
        {
            var c = new NXDocumentConfiguration();
            c.AddModule<XFormsModule>();

            var e = new NXDocument(c,
@"
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