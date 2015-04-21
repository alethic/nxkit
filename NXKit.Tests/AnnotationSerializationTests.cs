using System.Xml.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NXKit.Serialization;
using System.Linq;

using NXKit.Xml;

namespace NXKit.Tests
{

    [TestClass]
    public class AnnotationSerializationTests
    {

        [TestMethod]
        public void Test_no_annotations()
        {
            var d1 = new XDocument(
                new XElement("Hello"));
            var d2 = new AnnotationSerializer().Serialize(d1);
        }

        [TestMethod]
        public void Test_simple_round_trip()
        {
            var e1 = new XElement("Root");
            var d1 = new XDocument(e1);
            var di = d1.GetObjectId();
            var id = e1.GetObjectId();
            var d2 = new AnnotationSerializer().Serialize(d1);
            var d3 = new AnnotationSerializer().Deserialize(d2);
            Assert.IsTrue(d3.Root.GetObjectId() == id);
        }

        [TestMethod]
        public void Test_text_node()
        {
            var tx = new XText("This is some text");
            var e1 = new XElement("Root", tx);
            var d1 = new XDocument(e1);
            var di = d1.GetObjectId();
            var id = e1.GetObjectId();
            var ti = tx.GetObjectId();
            var d2 = new AnnotationSerializer().Serialize(d1);
            var d3 = new AnnotationSerializer().Deserialize(d2);
            Assert.IsTrue(d3.Root.FirstNode.GetObjectId() == ti);
        }

        [TestMethod]
        public void Test_formatted_text()
        {
            var d1 = XDocument.Parse(new XDocument(
                new XElement("root",
                    new XElement("data",
                        new XElement("item", "text")))).ToString());
            var l1 = d1.DescendantNodesAndSelf()
                .Select(i => i.GetObjectId())
                .ToArray();
            var d2 = new AnnotationSerializer().Serialize(d1);
            var d3 = new AnnotationSerializer().Deserialize(d2);
            var l2 = d3.DescendantNodesAndSelf()
                .Select(i => i.GetObjectId())
                .ToArray();
            Assert.IsTrue(l1.SequenceEqual(l2));
        }

        [TestMethod]
        public void Test_formatted_text_manipulation()
        {
            var d1 = XDocument.Parse(new XDocument(
                new XElement("root",
                    new XElement("data",
                        new XElement("item", "text")))).ToString());
            var l1 = d1.DescendantNodesAndSelf()
                .Select(i => i.GetObjectId())
                .ToArray();
            var d2 = new AnnotationSerializer().Serialize(d1);
            var d3 = new AnnotationSerializer().Deserialize(d2);
            var l2 = d3.DescendantNodesAndSelf()
                .Select(i => i.GetObjectId())
                .ToArray();
            d3.Element("root").Element("data").AddAfterSelf(d3.Element("root").Element("data"));
            var l3 = d3.DescendantNodesAndSelf()
                .Select(i => i.GetObjectId())
                .ToArray();
            var d4 = new AnnotationSerializer().Serialize(d3);
            var d5 = new AnnotationSerializer().Deserialize(d4);
            var l5 = d5.DescendantNodesAndSelf()
                .Select(i => i.GetObjectId())
                .ToArray();
        }

    }
}
