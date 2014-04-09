using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace NXKit.IO
{

    /// <summary>
    /// Wraps an existing <see cref="XElement"/> modifying the body contents to include persisted annotation data.
    /// </summary>
    public class XAnnotationElement :
        XElement
    {

        internal static readonly XNamespace NX_NS = "http://schemas.nxkit.org/2014/save";
        internal static readonly XName NX_ANNOTATION = "annotation";
        internal static readonly XName NX_TARGET = "target";
        internal static readonly string NX_TARGET_DOCUMENT = "document";
        internal static readonly string NX_TARGET_ELEMENT = "element";
        internal static readonly string NX_TARGET_ATTRIBUTE = "attribute";
        internal static readonly string NX_TARGET_NODE = "node";
        internal static readonly XName NX_ATTRIBUTE = "attribute";
        internal static readonly XName NX_FORMAT = "format";
        internal static readonly string NX_FORMAT_XML = "xml";
        internal static readonly string NX_FORMAT_BINARY = "binary";
        internal static readonly XName NX_TYPE = "type";

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public XAnnotationElement(XElement element)
            : base(element.Name)
        {
            Contract.Requires<ArgumentNullException>(element != null);

            var l = GetContents(element).ToList();

            Add(l);
        }

        /// <summary>
        /// Gets the contents of the specified <see cref="XElement"/>.
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        IEnumerable<object> GetContents(XElement element)
        {
            Contract.Requires<ArgumentNullException>(element != null);

            // produces attributes of element
            foreach (var attr in element.Attributes())
                yield return attr;

            // produce saved data of element
            foreach (var save in GetSaveContents(element))
                if (save != null)
                    yield return save;

            // produce nodes of element
            foreach (var node in element.Nodes())
                if (node is XElement)
                    // replace original element with annotating element.
                    yield return new XAnnotationElement((XElement)node);
                else
                    yield return node;
        }

        /// <summary>
        /// Gets the saved state of the <see cref="XElement"/>.
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        protected virtual IEnumerable<object> GetSaveContents(XElement element)
        {
            Contract.Requires<ArgumentNullException>(element != null);

            if (element == element.Document.Root)
                foreach (var i in GetAnnotations(element.Document))
                    if (i != null)
                        yield return i;

            foreach (var i in GetAnnotations(element))
                if (i != null)
                    yield return i;
        }

        /// <summary>
        /// Produces a series of <see cref="XElement"/> nodes which describe annotation data for the specified <see
        /// cref="XObject"/>.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        protected virtual IEnumerable<XElement> GetAnnotations(XObject obj)
        {
            Contract.Requires<ArgumentNullException>(obj != null);

            var document = obj as XDocument;
            if (document != null)
                return GetAnnotations(document);

            // object is an element
            var element = obj as XElement;
            if (element != null)
                return GetAnnotations(element);

            // object is attribute
            var attribute = obj as XAttribute;
            if (attribute != null)
                return GetAnnotations(attribute);

            return null;
        }

        /// <summary>
        /// Get's the annotation elements for a <see cref="XDocument"/>.
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        protected virtual IEnumerable<XElement> GetAnnotations(XDocument document)
        {
            Contract.Requires<ArgumentNullException>(document != null);

            // emit annotations configured on the object itself
            foreach (var annotation in document.Annotations<object>())
                yield return GetAnnotation(document, annotation);
        }

        /// <summary>
        /// Get's the annotation elements for a <see cref="XElement"/>.
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        protected virtual IEnumerable<XElement> GetAnnotations(XElement element)
        {
            Contract.Requires<ArgumentNullException>(element != null);

            // emit annotations configured on the object itself
            foreach (var annotation in element.Annotations<object>())
                yield return GetAnnotation(element, annotation);

            // emit annotations on the attributes
            foreach (var attribute in element.Attributes())
                foreach (var i in GetAnnotations(attribute))
                    yield return i;
        }

        /// <summary>
        /// Gets's the annotation elements for a <see cref="XAttribute"/>.
        /// </summary>
        /// <param name="attribute"></param>
        /// <returns></returns>
        protected virtual IEnumerable<XElement> GetAnnotations(XAttribute attribute)
        {
            Contract.Requires<ArgumentNullException>(attribute != null);

            // skip namespace attribute
            if (attribute.Name.Namespace == XNamespace.Xmlns)
                yield break;

            // emit annotations on the attribute
            foreach (var i in GetAnnotations(attribute))
                yield return i;
        }

        /// <summary>
        /// Gets an element which described the serialized annotation on the given <see cref="XObject"/>.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="annotation"></param>
        /// <returns></returns>
        protected virtual XElement GetAnnotation(XObject obj, object annotation)
        {
            Contract.Requires<ArgumentNullException>(obj != null);
            Contract.Requires<ArgumentNullException>(annotation != null);

            return TrySerializeAsXml(obj, annotation);
        }

        /// <summary>
        /// Attempts to serialize the annotation as XML.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="annotation"></param>
        /// <returns></returns>
        XElement TrySerializeAsXml(XObject obj, object annotation)
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

            return XmlSerializeToElement(obj, annotation, CreateElement(obj, annotation));
        }

        /// <summary>
        /// Serializes the annotation as XML to the body of hte given element.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="annotation"></param>
        /// <param name="element"></param>
        XElement XmlSerializeToElement(XObject obj, object annotation, XElement element)
        {
            Contract.Requires<ArgumentNullException>(obj != null);
            Contract.Requires<ArgumentNullException>(annotation != null);
            Contract.Requires<ArgumentNullException>(element != null);

            // configure element
            element.SetAttributeValue(NX_FORMAT, NX_FORMAT_XML);
            element.SetAttributeValue(NX_TYPE, annotation.GetType().AssemblyQualifiedName);
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
            foreach (var i in element.Nodes().OfType<XComment>())
                i.Remove();

            return element;
        }

        /// <summary>
        /// Creates a new <see cref="XElement"/> for holding the specified annotation.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="annotation"></param>
        /// <returns></returns>
        protected XElement CreateElement(XObject obj, object annotation)
        {
            Contract.Requires<ArgumentNullException>(obj != null);
            Contract.Requires<ArgumentNullException>(annotation != null);
            Contract.Ensures(Contract.Result<XElement>() != null);

            if (obj is XDocument)
                return new XElement(NX_ANNOTATION,
                    new XAttribute(XNamespace.Xmlns + "nx", NX_NS),
                    new XAttribute(NX_TARGET_ELEMENT, NX_TARGET_DOCUMENT));

            if (obj is XElement)
                return new XElement(NX_ANNOTATION,
                    new XAttribute(XNamespace.Xmlns + "nx", NX_NS),
                    new XAttribute(NX_TARGET, NX_TARGET_ELEMENT));

            if (obj is XAttribute)
                return new XElement(NX_ANNOTATION,
                    new XAttribute(NX_TARGET, NX_TARGET_ATTRIBUTE),
                    new XAttribute(XNamespace.Xmlns + "nx", NX_NS),
                    new XAttribute(NX_ATTRIBUTE, ((XAttribute)obj).Name));

            if (obj is XNode)
                return new XElement(NX_ANNOTATION,
                    new XAttribute(XNamespace.Xmlns + "nx", NX_NS),
                    new XAttribute(NX_TARGET, NX_TARGET_NODE));

            // cannot serialize unknown object type
            throw new InvalidOperationException();
        }

        public override string ToString()
        {
            return null;
        }

    }

}
