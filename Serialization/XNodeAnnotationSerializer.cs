using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Serialization;

using NXKit.Xml;
using NXKit.Util;

namespace NXKit.Serialization
{

    /// <summary>
    /// Provides methods to serialize or deserialize an XLinq object graph including annotations.
    /// </summary>
    public static class XNodeAnnotationSerializer
    {

        internal static readonly XNamespace NX_NS = "http://schemas.nxkit.org/2014/serialization";
        internal static readonly XName NX_ANNOTATION = NX_NS + "annotation";
        internal static readonly XName NX_FOR = "for";
        internal const string NX_FOR_DOCUMENT = "document";
        internal const string NX_FOR_ELEMENT = "element";
        internal const string NX_FOR_ATTRIBUTE = "attribute";
        internal const string NX_FOR_NODE = "node";
        internal static readonly XName NX_ATTRIBUTE = "attribute";
        internal static readonly XName NX_FORMAT = "format";
        internal const string NX_FORMAT_XML = "xml";
        internal const string NX_FORMAT_BINARY = "binary";
        internal static readonly XName NX_TYPE = "type";

        #region Serialize

        /// <summary>
        /// Serialize a normal <see cref="XDocument"/> into a new <see cref="XDocument"/>, integrating the annotations
        /// into the elements.
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        public static XDocument Serialize(XDocument document)
        {
            Contract.Requires<ArgumentNullException>(document != null);

            return new XDocument(SerializeContents(document));
        }

        /// <summary>
        /// Gets the contents of the given <see cref="XDocument"/>.
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        static IEnumerable<object> SerializeContents(XDocument document)
        {
            Contract.Requires<ArgumentNullException>(document != null);

            if (document.DocumentType != null)
                yield return document.DocumentType;

            foreach (var node in document.Nodes())
                if (node is XElement)
                    // replace original element with annotating element.
                    yield return new XStreamingElement(((XElement)node).Name, SerializeContents((XElement)node));
                else
                    yield return node;
        }

        /// <summary>
        /// Gets the contents of the specified <see cref="XElement"/>.
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        static IEnumerable<object> SerializeContents(XElement element)
        {
            Contract.Requires<ArgumentNullException>(element != null);

            // produces attributes of element
            foreach (var attr in element.Attributes())
                yield return attr;

            // produce saved data of element
            foreach (var save in GetSerializationBody(element))
                if (save != null)
                    yield return save;

            // produce nodes of element
            foreach (var node in element.Nodes())
                if (node is XElement)
                    // replace original element with annotating element.
                    yield return new XStreamingElement(((XElement)node).Name, SerializeContents((XElement)node));
                else
                    yield return node;
        }

        /// <summary>
        /// Gets the saved state of the <see cref="XElement"/>.
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        static IEnumerable<object> GetSerializationBody(XElement element)
        {
            Contract.Requires<ArgumentNullException>(element != null);

            if (element == element.Document.Root)
            {
                // add annotation namespace to root element to prevent duplication
                yield return new XAttribute(XNamespace.Xmlns + "nx", NX_NS);

                // attach annotation for the document to the root element
                foreach (var i in SerializeObject(element.Document))
                    if (i != null)
                        yield return i;
            }

            // yield annotations for the element itself
            foreach (var i in SerializeObject(element))
                if (i != null)
                    yield return i;
        }

        /// <summary>
        /// Produces a series of <see cref="XElement"/> nodes which describe annotation data for the specified <see
        /// cref="XObject"/>.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        static IEnumerable<XElement> SerializeObject(XObject obj)
        {
            Contract.Requires<ArgumentNullException>(obj != null);

            var document = obj as XDocument;
            if (document != null)
                return SerializeDocument(document);

            // object is an element
            var element = obj as XElement;
            if (element != null)
                return SerializeElements(element);

            // object is attribute
            var attribute = obj as XAttribute;
            if (attribute != null)
                return SerializeAttributes(attribute);

            return null;
        }

        /// <summary>
        /// Get's the annotation elements for a <see cref="XDocument"/>.
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        static IEnumerable<XElement> SerializeDocument(XDocument document)
        {
            Contract.Requires<ArgumentNullException>(document != null);

            // emit annotations configured on the object itself
            foreach (var annotation in document.Annotations<object>())
            {
                var obj = SerializeAnnotation(document, annotation);
                if (obj != null)
                    obj.SetAttributeValue(NX_FOR, NX_FOR_DOCUMENT);

                yield return obj;
            }
        }

        /// <summary>
        /// Get's the annotation elements for a <see cref="XElement"/>.
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        static IEnumerable<XElement> SerializeElements(XElement element)
        {
            Contract.Requires<ArgumentNullException>(element != null);

            // emit annotations configured on the object itself
            foreach (var annotation in element.Annotations<object>())
            {
                var obj = SerializeAnnotation(element, annotation);
                if (obj != null)
                    obj.SetAttributeValue(NX_FOR, NX_FOR_ELEMENT);

                yield return obj;
            }

            // emit annotations on the attributes
            foreach (var attribute in element.Attributes())
                foreach (var i in SerializeAttributes(attribute))
                    yield return i;
        }

        /// <summary>
        /// Gets's the annotation elements for a <see cref="XAttribute"/>.
        /// </summary>
        /// <param name="attribute"></param>
        /// <returns></returns>
        static IEnumerable<XElement> SerializeAttributes(XAttribute attribute)
        {
            Contract.Requires<ArgumentNullException>(attribute != null);

            // skip namespace attribute
            if (attribute.Name.Namespace == XNamespace.Xmlns)
                yield break;

            // emit annotations on the attribute
            foreach (var annotation in attribute.Annotations<object>())
            {
                var obj = SerializeAnnotation(attribute, attribute);
                if (obj != null)
                    obj.SetAttributeValue(NX_FOR, NX_FOR_ATTRIBUTE);

                yield return obj;
            }
        }

        /// <summary>
        /// Gets an element which described the serialized annotation on the given <see cref="XObject"/>.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="annotation"></param>
        /// <returns></returns>
        static XElement SerializeAnnotation(XObject obj, object annotation)
        {
            Contract.Requires<ArgumentNullException>(obj != null);
            Contract.Requires<ArgumentNullException>(annotation != null);

            return SerializeToXml(obj, annotation);
        }

        /// <summary>
        /// Attempts to serialize the annotation as XML.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="annotation"></param>
        /// <returns></returns>
        static XElement SerializeToXml(XObject obj, object annotation)
        {
            Contract.Requires<ArgumentNullException>(obj != null);
            Contract.Requires<ArgumentNullException>(annotation != null);

            var type = annotation.GetType();

            // unsupported types
            if (!type.IsPublic || type.IsAbstract)
                return null;

            // find default constructor
            var ctor = type.GetConstructor(new Type[] { });
            if (ctor == null)
                return null;

            return SerializeToXml(obj, annotation, CreateAnnotationElement(obj, annotation));
        }

        /// <summary>
        /// Serializes the annotation as XML to the body of hte given element.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="annotation"></param>
        /// <param name="element"></param>
        static XElement SerializeToXml(XObject obj, object annotation, XElement element)
        {
            Contract.Requires<ArgumentNullException>(obj != null);
            Contract.Requires<ArgumentNullException>(annotation != null);
            Contract.Requires<ArgumentNullException>(element != null);

            // configure element
            element.SetAttributeValue(NX_FORMAT, NX_FORMAT_XML);
            element.SetAttributeValue(NX_TYPE, annotation.GetType().FullName + ", " + annotation.GetType().Assembly.GetName().Name);
            element.RemoveNodes();

            // serialize anntation into body
            using (var wrt = element.CreateWriter())
            {
                wrt.WriteComment(""); // fixes a bug in XmlSerializer that tries to WriteStartDocument

                var srs = new XmlSerializer(annotation.GetType());
                var ens = new XmlSerializerNamespaces();
                ens.Add("", "");
                srs.Serialize(wrt, annotation, ens);
            }

            // remove bug comment
            element.Nodes()
                .OfType<XComment>()
                .Remove();

            return element;
        }

        /// <summary>
        /// Creates a new <see cref="XElement"/> for holding the specified annotation.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="annotation"></param>
        /// <returns></returns>
        static XElement CreateAnnotationElement(XObject obj, object annotation)
        {
            Contract.Requires<ArgumentNullException>(obj != null);
            Contract.Requires<ArgumentNullException>(annotation != null);
            Contract.Ensures(Contract.Result<XElement>() != null);

            if (obj is XDocument)
                return new XElement(NX_ANNOTATION,
                    new XAttribute(XNamespace.Xmlns + "nx", NX_NS),
                    new XAttribute(NX_FOR_ELEMENT, NX_FOR_DOCUMENT));

            if (obj is XElement)
                return new XElement(NX_ANNOTATION,
                    new XAttribute(XNamespace.Xmlns + "nx", NX_NS),
                    new XAttribute(NX_FOR, NX_FOR_ELEMENT));

            if (obj is XAttribute)
                return new XElement(NX_ANNOTATION,
                    new XAttribute(NX_FOR, NX_FOR_ATTRIBUTE),
                    new XAttribute(XNamespace.Xmlns + "nx", NX_NS),
                    new XAttribute(NX_ATTRIBUTE, ((XAttribute)obj).Name));

            if (obj is XNode)
                return new XElement(NX_ANNOTATION,
                    new XAttribute(XNamespace.Xmlns + "nx", NX_NS),
                    new XAttribute(NX_FOR, NX_FOR_NODE));

            // cannot serialize unknown object type
            throw new InvalidOperationException();
        }

        #endregion

        #region Deserialize

        /// <summary>
        /// Deserialize a <see cref="XDocument"/> which includes integrated annotations into a new <see
        /// cref="XDocument"/> with applied annotations.
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        public static XDocument Deserialize(XDocument document)
        {
            Contract.Requires<ArgumentNullException>(document != null);

            // copy element and deserialize contents 
            var xml = new XDocument(document);

            // document being read expresses a base URI, set on element
            if (!string.IsNullOrEmpty(document.BaseUri))
                xml.SetBaseUri(document.BaseUri);

            // apply serialied contents
            DeserializeContents(xml);

            // strip out NX namespaces
            xml.Root.DescendantsAndSelf()
                .SelectMany(i => i.Attributes())
                .Where(i => i.IsNamespaceDeclaration)
                .Where(i => i.Value == NX_NS)
                .Remove();

            return xml;
        }

        /// <summary>
        /// Deserializes the contents of the <see cref="XDocument"/>.
        /// </summary>
        /// <param name="document"></param>
        static void DeserializeContents(XDocument document)
        {
            Contract.Requires<ArgumentNullException>(document != null);

            // deserializes the entire document hierarchy
            foreach (var element in document.Descendants())
                DeserializeContents(element);
        }

        /// <summary>
        /// Deserializes the contents of the <see cref="XElement"/>.
        /// </summary>
        /// <param name="element"></param>
        static void DeserializeContents(XElement element)
        {
            Contract.Requires<ArgumentNullException>(element != null);

            // extract annotation elements
            var annotations = element
                .Elements(NX_ANNOTATION)
                .ToList();

            // detach annotations from hierarchy (so indexed nodes operate)
            annotations.Remove();

            // deserialize each annotation element
            foreach (var annotation in annotations)
                DeserializeAnnotationElement(element, annotation);
        }

        /// <summary>
        /// Deserializes the specified annotation element and applies it to the given target element.
        /// </summary>
        /// <param name="element">The parent element of the annotation node.</param>
        /// <param name="annotation">The annotation element.</param>
        static void DeserializeAnnotationElement(XElement element, XElement annotation)
        {
            Contract.Requires<ArgumentNullException>(element != null);
            Contract.Requires<ArgumentNullException>(annotation != null);

            // deserialize anotation data
            var obj = DeserializeAnnotationElement(annotation);
            if (obj == null)
                return;

            // annotation specifies how it should be applied
            switch ((string)annotation.Attribute(NX_FOR))
            {
                case NX_FOR_DOCUMENT:
                    ApplyAnnotationToDocument(element, annotation, obj);
                    break;
                case NX_FOR_ELEMENT:
                    ApplyAnnotationToElement(element, annotation, obj);
                    break;
                case NX_FOR_ATTRIBUTE:
                    ApplyAnnotationToAttribute(element, annotation, obj);
                    break;
                case NX_FOR_NODE:
                    ApplyAnnotationToNode(element, annotation, obj);
                    break;
                default:
                    throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// Deserializes the specified annotation element.
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        static object DeserializeAnnotationElement(XElement element)
        {
            Contract.Requires<ArgumentNullException>(element != null);

            switch ((string)element.Attribute(NX_FORMAT))
            {
                case NX_FORMAT_XML:
                    return DeserializeAnnotationFromXml(element);
            }

            throw new InvalidOperationException();
        }

        /// <summary>
        /// Deserializes the specified annotation element stored as XML.
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        static object DeserializeAnnotationFromXml(XElement element)
        {
            Contract.Requires<ArgumentNullException>(element != null);

            var root = element.FirstNode;
            if (root == null)
                throw new InvalidOperationException();

            var typeName = (string)element.Attribute(NX_TYPE);
            if (typeName == null)
                throw new InvalidOperationException();

            var type = Type.GetType(typeName);
            if (type == null)
                throw new InvalidOperationException();

            // serialize anntation into body
            using (var rdr = root.CreateReader())
                return new XmlSerializer(type).Deserialize(rdr);
        }

        static void ApplyAnnotationToDocument(XElement element, XElement annotation, object obj)
        {
            Contract.Requires<ArgumentNullException>(element != null);
            Contract.Requires<ArgumentNullException>(annotation != null);
            Contract.Requires<ArgumentNullException>(obj != null);

            var document = element.Document;
            if (document == null)
                throw new InvalidOperationException();

            document.AddAnnotation(obj);
        }

        static void ApplyAnnotationToElement(XElement element, XElement annotation, object obj)
        {
            Contract.Requires<ArgumentNullException>(element != null);
            Contract.Requires<ArgumentNullException>(annotation != null);
            Contract.Requires<ArgumentNullException>(obj != null);

            element.AddAnnotation(obj);
        }

        static void ApplyAnnotationToAttribute(XElement element, XElement annotation, object obj)
        {
            Contract.Requires<ArgumentNullException>(element != null);
            Contract.Requires<ArgumentNullException>(annotation != null);
            Contract.Requires<ArgumentNullException>(obj != null);

            var attributeName = (XName)(string)annotation.Attribute(NX_ATTRIBUTE);
            if (attributeName == null)
                throw new InvalidOperationException();

            var attribute = element.Attribute(attributeName);
            if (attribute == null)
                throw new InvalidOperationException();

            attribute.AddAnnotation(obj);
        }

        static void ApplyAnnotationToNode(XElement element, XElement annotation, object obj)
        {
            Contract.Requires<ArgumentNullException>(element != null);
            Contract.Requires<ArgumentNullException>(annotation != null);
            Contract.Requires<ArgumentNullException>(obj != null);

            throw new NotImplementedException();
        }

        #endregion

    }

}
