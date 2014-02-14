using System.IO;
using System.Xml.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NXKit.Tests
{

    [TestClass]
    public class EngineTests
    {

        /// <summary>
        /// Opens a new instance of the sample XML.
        /// </summary>
        /// <returns></returns>
        Stream OpenSampleXml()
        {
            return typeof(EngineTests).Assembly.GetManifestResourceStream("NXKit.Tests.SampleXml.xml");
        }

        [TestMethod]
        public void TestMethod1()
        {
            var engine = new Engine(XDocument.Load(OpenSampleXml()), null);
        }

    }

}
