using System.Xml.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NXKit.Util.Tests
{

    [TestClass]
    public class XAttributeExtensionsTests
    {

        [TestMethod]
        public void Test_ValueAsXName_with_no_prefix()
        {
            var ns1 = XNamespace.Get("http://tempuri.org/");
            var ns2 = XNamespace.Get("http://tempuri.org/2");

            var d = new XDocument(
                new XElement(ns1 + "root",
                    new XElement(ns2 + "node",
                        new XAttribute(ns2 + "attr", "value"))));

            Assert.AreEqual(ns2 + "value", d.Element(ns1 + "root").Element(ns2 + "node").Attribute(ns2 + "attr").ValueAsXName());
        }

        [TestMethod]
        public void Test_ValueAsXName_with_prefix()
        {
            var ns1 = XNamespace.Get("http://tempuri.org/");
            var ns2 = XNamespace.Get("http://tempuri.org/2");

            var d = new XDocument(
                new XElement(ns1 + "root",
                    new XAttribute(XNamespace.Xmlns + "ns1", ns1),
                    new XAttribute(XNamespace.Xmlns + "ns2", ns2),
                    new XElement(ns2 + "node",
                        new XAttribute(ns2 + "attr", "ns1:value"))));

            Assert.AreEqual(ns1 + "value", d.Element(ns1 + "root").Element(ns2 + "node").Attribute(ns2 + "attr").ValueAsXName());
        }

        [TestMethod]
        public void Test_ValueAsXName_with_no_prefix_and_default_attribute()
        {
            var ns1 = XNamespace.Get("http://tempuri.org/");
            var ns2 = XNamespace.Get("http://tempuri.org/2");

            var d = new XDocument(
                new XElement(ns1 + "root",
                    new XAttribute(XNamespace.Xmlns + "ns1", ns1),
                    new XAttribute(XNamespace.Xmlns + "ns2", ns2),
                    new XElement(ns2 + "node",
                        new XAttribute("attr", "value"))));

            Assert.AreEqual(ns2 + "value", d.Element(ns1 + "root").Element(ns2 + "node").Attribute("attr").ValueAsXName());
        }

    }

}
