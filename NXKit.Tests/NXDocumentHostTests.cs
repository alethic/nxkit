using System.IO;
using System.Xml;
using System.Xml.Linq;

using Autofac;

using Cogito.Autofac;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using NXKit.Autofac;
using NXKit.Composition;

namespace NXKit.Tests
{

    [TestClass]
    public class NXDocumentHostTests
    {

        ICompositionContext CreateCompositionContext()
        {
            var bld = new ContainerBuilder();
            bld.RegisterAllAssemblyModules();
            var cnt = bld.Build();
            return cnt.Resolve<ICompositionContext>();
        }

        [TestMethod]
        public void Test_basic_load()
        {
            Document.Load(XDocument.Parse(@"<unknown />"), CreateCompositionContext());
        }

        [TestMethod]
        public void Test_basic_save()
        {
            var doc = Document.Load(XDocument.Parse(@"<unknown />"), CreateCompositionContext());

            using (var str = new StringWriter())
            using (var wrt = XmlWriter.Create(str))
            {
                doc.Xml.AddAnnotation(new object());
                doc.Save(wrt);
                wrt.Flush();

                var xml = str.ToString();
            }
        }

        [TestMethod]
        public void Test_basic_invoke_save()
        {
            var doc = Document.Load(XDocument.Parse(@"<unknown />"), CreateCompositionContext());

            using (var str = new StringWriter())
            using (var wrt = XmlWriter.Create(str))
            {
                doc.Xml.AddAnnotation(new object());
                doc.Save(wrt);
                wrt.Flush();

                var xml = str.ToString();
            }
        }

    }

}
