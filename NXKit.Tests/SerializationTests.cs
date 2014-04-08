using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NXKit.Util;

namespace NXKit.Tests
{

    [TestClass]
    public class SerializationTests
    {

        [Serializable]
        public class TestVisualState
        {

            public string Value { get; set; }

        }

        [TestMethod]
        public void Test_NXDocument_roundtrip()
        {
            using (var uri = DynamicUriUtil.GetUriFor(@"<unknown />"))
            {
                var d1 = NXDocumentHost.Load(uri);
                d1.Root.GetState<TestVisualState>().Value = "Value1";

                var f = new BinaryFormatter();
                var m = new MemoryStream();
                f.Serialize(m, d1.Save());
                m.Position = 0;

                var s = (NXDocumentState)f.Deserialize(m);
                var d2 = new NXDocumentHost(CompositionUtil.CreateContainer(), s);

                Assert.AreEqual("Value1", d1.Root.GetState<TestVisualState>().Value);
            }
        }

    }

}
