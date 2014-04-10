using System.IO;
using System.Xml;
using System.Xml.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NXKit.Net;
using NXKit.Util;
using NXKit.Xml;

namespace NXKit.Tests
{

    [TestClass]
    public class NXDocumentHostTests
    {

        [TestMethod]
        public void Test_basic_load()
        {
            NXDocumentHost.Load(XDocument.Parse(@"<unknown />"));
        }

        [TestMethod]
        public void Test_basic_invoke()
        {
            NXDocumentHost.Load(XDocument.Parse(@"<unknown />")).Invoke();
        }

        [TestMethod]
        public void Test_basic_save()
        {
            var doc = NXDocumentHost.Load(XDocument.Parse(@"<unknown />"));

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
            var doc = NXDocumentHost.Load(XDocument.Parse(@"<unknown />"));
            doc.Invoke();

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
