using System.Linq;
using Autofac;
using Cogito.Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NXKit.Composition;
using NXKit.Xml;

namespace NXKit.XForms.Tests.XPathFunctions
{

    [TestClass]
    public class InstanceTests
    {

        ICompositionContext CreateCompositionContext()
        {
            var bld = new ContainerBuilder();
            bld.RegisterAllAssemblyModules();
            var cnt = bld.Build();
            return cnt.Resolve<ICompositionContext>();
        }

        [TestMethod]
        public void Test_default_instance_resolution()
        {
            var e = Document.Parse(@"
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
                CreateCompositionContext());

            var input = e.Root
                .Descendants()
                .Where(i => i.Interfaces<Input>().Any())
                .Select(i => i.Interface<IUIBindingNode>())
                .FirstOrDefault();

            Assert.AreEqual("node1", input.UIBinding.Value);
        }

        [TestMethod]
        public void Test_specified_instance_resolution()
        {
            var e = Document.Parse(@"
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
                </unknown>",
                CreateCompositionContext());

            var input = e.Root
                .Descendants()
                .Where(i => i.Interfaces<Input>().Any())
                .Select(i => i.Interface<IUIBindingNode>())
                .FirstOrDefault();

            Assert.AreEqual("node2", input.UIBinding.Value);
        }

    }

}
