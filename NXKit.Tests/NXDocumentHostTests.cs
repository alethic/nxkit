using System.IO;
using System.Xml;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NXKit.Util;

namespace NXKit.Tests
{

    [TestClass]
    public class NXDocumentHostTests
    {

        [TestMethod]
        public void Test_basic_load()
        {
            using (var uri = DynamicUriUtil.GetUriFor(@"<unknown />"))
                NXDocumentHost.Load(uri);
        }

        [TestMethod]
        public void Test_basic_invoke()
        {
            using (var uri = DynamicUriUtil.GetUriFor(@"<unknown />"))
                NXDocumentHost.Load(uri).Invoke();
        }

        [TestMethod]
        public void Test_basic_save()
        {
            using (var uri = DynamicUriUtil.GetUriFor(@"<unknown />"))
            {
                var doc = NXDocumentHost.Load(uri);

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

        [TestMethod]
        public void Test_basic_invoke_save()
        {
            using (var uri = DynamicUriUtil.GetUriFor(@"<unknown />"))
            {
                var doc = NXDocumentHost.Load(uri);
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

}
