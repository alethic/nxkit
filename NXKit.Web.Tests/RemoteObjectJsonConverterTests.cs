using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NXKit.Web.Serialization;
using NXKit.Xml;

namespace NXKit.Web.Tests
{

    [TestClass]
    public class RemoteObjectJsonConverterTests
    {

        [Interface("element")]
        [Remote]
        public class Remote1Test
        {

            [Remote]
            public string RemoteValue { get; set; }

            public string NonRemoteValue { get; set; }

            [Remote]
            public void RemoteMethod()
            {

            }

            public void NonRemoteMethod()
            {

            }

        }

        [Interface("element")]
        [Remote]
        public class Remote2Test
        {

            [Remote]
            public string RemoteValue { get; set; }

            public string NonRemoteValue { get; set; }

            [Remote]
            public void RemoteMethod()
            {

            }

            public void NonRemoteMethod()
            {

            }

        }

        [TestMethod]
        public void Test_GetInterfaceTypes()
        {
            RemoteObjectJsonConverter.GetRemoteInterfaces(new[] { new Remote1Test() }).ToList();
        }

        [TestMethod]
        public void Test_GetRemoteProperties()
        {
            Assert.AreEqual(1, RemoteObjectJsonConverter.GetRemoteProperties(typeof(Remote1Test)).ToList().Count);
        }

        [TestMethod]
        public void Test_GetRemoteMethods()
        {
            Assert.AreEqual(1, RemoteObjectJsonConverter.GetRemoteMethods(typeof(Remote1Test)).ToList().Count);
        }

        [TestMethod]
        public void Test_ToObject()
        {
            var jo1 = new JObject(
                new JProperty(typeof(Remote1Test).FullName,
                    new JObject(
                        new JProperty("RemoteValue", null),
                        new JProperty("@RemoteMethod", new JArray()))));
            var js1 = jo1.ToString();

            var jo2 = RemoteObjectJsonConverter.RemoteToObject(new Remote1Test(), new JsonSerializer());
            var js2 = jo2.ToString();

            Assert.AreEqual(js1, js2);
        }

        [TestMethod]
        public void Test_ToObject_multiple()
        {
            var e = new System.Xml.Linq.XElement("element");
            var i = e.Interfaces(NXKit.CompositionUtil.CreateContainer()).ToList();

            var jo1 = RemoteXNodeJsonConverter.RemotesToObject(i, new JsonSerializer());
            var js1 = jo1.ToString();
        }

    }

}
