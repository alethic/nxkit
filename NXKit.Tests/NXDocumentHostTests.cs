using System.IO;
using System.Xml;
using System.Xml.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NXKit.Tests
{

    [TestClass]
    public class NXDocumentHostTests
    {

        [TestMethod]
        public void Test_basic_load()
        {
            Document.Load(XDocument.Parse(@"<unknown />"));
        }

        [TestMethod]
        public void Test_basic_save()
        {
            var doc = Document.Load(XDocument.Parse(@"<unknown />"));

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
            var doc = Document.Load(XDocument.Parse(@"<unknown />"));

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
