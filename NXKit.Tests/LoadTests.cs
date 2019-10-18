using System.IO;

using NUnit.Framework;

namespace NXKit.Tests
{

    public class LoadTests
    {

        /// <summary>
        /// Opens a new instance of the sample XML.
        /// </summary>
        /// <returns></returns>
        Stream OpenSampleXml()
        {
            return typeof(LoadTests).Assembly.GetManifestResourceStream("NXKit.Tests.SampleXml.xml");
        }

        [Test]
        public void TestMethod1()
        {

        }

    }

}
