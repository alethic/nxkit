using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NXKit.Composition;

namespace NXKit.Tests
{

    [TestClass]
    public class DefaultInterfaceProviderTests
    {

        class Object1
        {

        }

        class Object2
        {

        }

        class Object3
        {

        }

        static readonly XNamespace NS = XNamespace.Get("http://tempuri.org");

        static readonly List<IExtensionPredicate> predicates = new List<IExtensionPredicate>()
        {

        };

        static readonly List<InterfaceDescriptor> descriptors = new List<InterfaceDescriptor>()
        {
            new InterfaceDescriptor(XmlNodeType.Element, NS.NamespaceName, "element1", null, typeof(Object1)),
            new InterfaceDescriptor(XmlNodeType.Element, (string)null, "element2", null, typeof(Object2)),
            new InterfaceDescriptor(XmlNodeType.Element, NS.NamespaceName, (string)null, null, typeof(Object3) ),
        };

        [TestMethod]
        public void Test_full_element_predicate()
        {
            Assert.Fail();
            //var i = descriptors[0].IsMatch(new XElement(NS + "element1"));
            //Assert.IsTrue(i);
        }

        [TestMethod]
        public void Test_local_only_element_predicate()
        {
            Assert.Fail();
            //var i = descriptors[1].IsMatch(new XElement(NS + "element2"));
            //Assert.IsTrue(i);
        }

        [TestMethod]
        public void Test_ns_only_element_predicate()
        {
            Assert.Fail();
            //var i = descriptors[2].IsMatch(new XElement(NS + "element3"));
            //Assert.IsTrue(i);
        }

        [TestMethod]
        public void Test_full_element()
        {
            var p = new DefaultInterfaceProvider(descriptors);
            var i = p.GetExtensions(new XElement(NS + "element1")).ToArray();
            Assert.IsTrue(i.Length == 2);
            Assert.IsInstanceOfType(i[0], typeof(Object1));
        }

        [TestMethod]
        public void Test_local_only_element()
        {
            var p = new DefaultInterfaceProvider(predicates, descriptors);
            var i = p.GetExtensions(new XElement(NS + "element2")).ToArray();
            Assert.IsTrue(i.Length == 2);
            Assert.IsInstanceOfType(i[0], typeof(Object2));
        }

        [TestMethod]
        public void Test_ns_only_element()
        {
            var p = new DefaultInterfaceProvider(predicates, descriptors);
            var i = p.GetExtensions(new XElement(NS + "element3")).ToArray();
            Assert.IsTrue(i.Length == 1);
            Assert.IsInstanceOfType(i[0], typeof(Object3));
        }

    }

}
