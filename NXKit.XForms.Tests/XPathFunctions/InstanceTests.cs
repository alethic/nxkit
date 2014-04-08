using System.IO;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NXKit.Util;

namespace NXKit.XForms.Tests.XPathFunctions
{

    [TestClass]
    public class InstanceTests
    {

        static DynamicUriAuthority RegisterXml(string code)
        {
            return new DynamicUriRootFunc(() => new MemoryStream(Encoding.UTF8.GetBytes(code)));
        }

        [TestMethod]
        public void Test_default_instance_resolution()
        {
            var e = NXDocumentHost.Load(RegisterXml(@"
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
</unknown>").BaseUri);

            var input = e.Root
                .Descendants()
                .OfType<Input>()
                .FirstOrDefault();

            Assert.AreEqual("node1", input.Binding.Value);
        }

        [TestMethod]
        public void Test_specified_instance_resolution()
        {
            var e = NXDocumentHost.Load(RegisterXml(@"
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
                </unknown>").BaseUri);

            var input = e.Root
                .Descendants()
                .OfType<Input>()
                .FirstOrDefault();

            Assert.AreEqual("node2", input.Binding.Value);
        }

    }

}
