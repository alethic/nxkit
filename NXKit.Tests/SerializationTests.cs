using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NXKit.Tests
{
    [TestClass]
    public class SerializationTests
    {

        public class TestVisual :
            NXElement
        {

            public TestVisual()
                : base()
            {

            }

            public override string Id
            {
                get { return "id"; }
            }

        }

        [Serializable]
        public class TestVisualState
        {

            public string Value { get; set; }

        }

        [TestMethod]
        public void Test_VisualStateCollection_roundtrip()
        {
            var v = new TestVisual();
            var c = new VisualStateCollection();
            var s = c.Get<TestVisualState>(v);
            s.Value = "Value1";

            var f = new BinaryFormatter();
            var m = new MemoryStream();
            f.Serialize(m, c);

            m.Position = 0;

            var c2 = (VisualStateCollection)f.Deserialize(m);

            Assert.AreEqual("Value1", c2.Get<TestVisualState>(v).Value);
        }

        [TestMethod]
        public void Test_NXDocument_roundtrip()
        {
            var d1 = NXDocument.Parse(@"<unknown />");
            d1.Root.GetState<TestVisualState>().Value = "Value1";

            var f = new BinaryFormatter();
            var m = new MemoryStream();
            f.Serialize(m, d1.Save());
            m.Position = 0;

            var s = (NXDocumentState)f.Deserialize(m);
            var d2 = new NXDocument(d1.Resolver, s);

            Assert.AreEqual("Value1", d1.Root.GetState<TestVisualState>().Value);
        }

    }

}
