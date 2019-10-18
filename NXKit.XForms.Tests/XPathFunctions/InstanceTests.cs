using System.Linq;

using Autofac;

using NUnit.Framework;

using NXKit.Testing;
using NXKit.Xml;

namespace NXKit.XForms.Tests.XPathFunctions
{

    public class InstanceTests : NXKitTestFixture
    {

        public InstanceTests(NXKitTestFixtureContext context) :
            base(context)
        {

        }

        [Test]
        public void Test_default_instance_resolution()
        {
            var e = Context.Engine.Parse(@"
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
                </unknown>");

            var input = e.Root
                .Descendants()
                .Where(i => i.Interfaces<Input>().Any())
                .Select(i => i.Interface<IUIBindingNode>())
                .FirstOrDefault();

            Assert.AreEqual("node1", input.UIBinding.Value);
        }

        [Test]
        public void Test_specified_instance_resolution()
        {
            var e = Context.Engine.Parse(@"
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
                </unknown>");

            var input = e.Root
                .Descendants()
                .Where(i => i.Interfaces<Input>().Any())
                .Select(i => i.Interface<IUIBindingNode>())
                .FirstOrDefault();

            Assert.AreEqual("node2", input.UIBinding.Value);
        }

    }

}
