using System.Xml.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Newtonsoft.Json.Linq;

using NXKit.Web.Serialization;

namespace NXKit.Web.Tests.Serialization
{

    [TestClass]
    public class RemoteObjectJsonConverterTests
    {


        [TestMethod]
        public void Test_ReadFrom()
        {
            var obj = new JObject(
                new JProperty("Test", 1));

            RemoteJson.SetJson(obj.CreateReader(), new XElement("Element"));
        }

    }

}
