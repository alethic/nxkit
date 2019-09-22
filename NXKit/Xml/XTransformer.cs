using System;
using System.Linq;
using System.Xml.Linq;

namespace NXKit.Xml
{

    /// <summary>
    /// Visits each object in an XLinq hierarchy to produce a new XLinq hierarchy.
    /// </summary>
    public class XTransformer
    {

        /// <summary>
        /// Invoked for each <see cref="XObject"/>.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public virtual XObject Visit(XObject obj)
        {
            if (obj == null)
                throw new ArgumentNullException(nameof(obj));

            if (obj is XNode)
                return Visit((XNode)obj);
            if (obj is XAttribute)
                return Visit((XAttribute)obj);

            throw new InvalidOperationException();
        }

        /// <summary>
        /// Invoked for each <see cref="XNode"/>.
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public virtual XNode Visit(XNode node)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));

            if (node is XDocumentType)
                return Visit((XDocumentType)node);
            if (node is XContainer)
                return Visit((XContainer)node);
            if (node is XText)
                return Visit((XText)node);
            if (node is XComment)
                return Visit((XComment)node);
            if (node is XCData)
                return Visit((XCData)node);
            if (node is XProcessingInstruction)
                return Visit((XProcessingInstruction)node);

            throw new InvalidOperationException();
        }

        /// <summary>
        /// Invoked for each <see cref="XContainer"/>.
        /// </summary>
        /// <param name="container"></param>
        /// <returns></returns>
        public virtual XContainer Visit(XContainer container)
        {
            if (container == null)
                throw new ArgumentNullException(nameof(container));

            if (container is XDocument)
                return Visit((XDocument)container);
            if (container is XElement)
                return Visit((XElement)container);

            throw new InvalidOperationException();
        }

        /// <summary>
        /// Invoked for each <see cref="XDocument"/>.
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        public virtual XDocument Visit(XDocument document)
        {
            if (document == null)
                throw new ArgumentNullException(nameof(document));

            return new XDocument(
                document.Declaration != null ? Visit(document.Declaration) : null,
                document.Nodes().Select(i => Visit((XObject)i)));
        }

        /// <summary>
        /// Invoked for each <see cref="XDeclaration"/>
        /// </summary>
        /// <param name="declaration"></param>
        /// <returns></returns>
        public virtual XDeclaration Visit(XDeclaration declaration)
        {
            if (declaration == null)
                throw new ArgumentNullException(nameof(declaration));

            return declaration;
        }

        /// <summary>
        /// Invoked for each <see cref="XDocumentType"/>
        /// </summary>
        /// <param name="documentType"></param>
        /// <returns></returns>
        public virtual XDocumentType Visit(XDocumentType documentType)
        {
            if (documentType == null)
                throw new ArgumentNullException(nameof(documentType));

            return documentType;
        }

        /// <summary>
        /// Invoked for each <see cref="XText"/>
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public virtual XText Visit(XText text)
        {
            if (text == null)
                throw new ArgumentNullException(nameof(text));

            return text;
        }

        /// <summary>
        /// Invoked for each <see cref="XComment"/>
        /// </summary>
        /// <param name="comment"></param>
        /// <returns></returns>
        public virtual XComment Visit(XComment comment)
        {
            if (comment == null)
                throw new ArgumentNullException(nameof(comment));

            return comment;
        }

        /// <summary>
        /// Invoked for each <see cref="XCData"/>
        /// </summary>
        /// <param name="cdata"></param>
        /// <returns></returns>
        public virtual XCData Visit(XCData cdata)
        {
            if (cdata == null)
                throw new ArgumentNullException(nameof(cdata));

            return cdata;
        }

        /// <summary>
        /// Invoked for each <see cref="XProcessingInstruction"/>
        /// </summary>
        /// <param name="processingInstruction"></param>
        /// <returns></returns>
        public virtual XProcessingInstruction Visit(XProcessingInstruction processingInstruction)
        {
            if (processingInstruction == null)
                throw new ArgumentNullException(nameof(processingInstruction));

            return processingInstruction;
        }

        /// <summary>
        /// Invoked for each <see cref="XElement"/>.
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public virtual XElement Visit(XElement element)
        {
            if (element == null)
                throw new ArgumentNullException(nameof(element));

            return new XElement(
                element.Name,
                element.Attributes().Select(i => Visit((XObject)i)),
                element.Nodes().Select(i => Visit((XObject)i)));
        }

        /// <summary>
        /// Invoked for each <see cref="XAttribute"/>.
        /// </summary>
        /// <param name="attribute"></param>
        /// <returns></returns>
        public virtual XAttribute Visit(XAttribute attribute)
        {
            if (attribute == null)
                throw new ArgumentNullException(nameof(attribute));

            return attribute;
        }

    }

}
