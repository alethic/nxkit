using System.IO;
using System.Xml;
using System.Xml.Linq;

using NUnit.Framework;

using NXKit.Testing;

namespace NXKit.Tests
{

    public class NXDocumentHostTests : NXKitTestFixture
    {

        public NXDocumentHostTests(NXKitTestFixtureContext context) :
            base(context)
        {

        }

        [Test]
        public void Test_basic_load()
        {
            Context.Engine.Load(XDocument.Parse(@"<unknown />"));
        }

        [Test]
        public void Test_basic_save()
        {
            var doc = Context.Engine.Load(XDocument.Parse(@"<unknown />"));

            using (var str = new StringWriter())
            using (var wrt = XmlWriter.Create(str))
            {
                doc.Xml.AddAnnotation(new object());
                doc.Save(wrt);
                wrt.Flush();

                var xml = str.ToString();
            }
        }

        [Test]
        public void Test_basic_invoke_save()
        {
            var doc = Context.Engine.Load(XDocument.Parse(@"<unknown />"));

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
