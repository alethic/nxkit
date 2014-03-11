using System.IO;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NXKit.Tests
{

    [TestClass]
    public class NXDocumentTests
    {

        /// <summary>
        /// Opens a new instance of the sample XML.
        /// </summary>
        /// <returns></returns>
        Stream OpenSampleXml()
        {
            return typeof(NXDocumentTests).Assembly.GetManifestResourceStream("NXKit.Tests.SampleXml.xml");
        }

        [TestMethod]
        public void TestMethod1()
        {
            var engine = new NXDocument(OpenSampleXml(), null);
        }

    }

}
