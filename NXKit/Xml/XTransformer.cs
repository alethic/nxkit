using System;
using System.Diagnostics.Contracts;
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
            Contract.Requires<ArgumentNullException>(obj != null);

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
            Contract.Requires<ArgumentNullException>(node != null);

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
            Contract.Requires<ArgumentNullException>(container != null);

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
            Contract.Requires<ArgumentNullException>(document != null);

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
            Contract.Requires<ArgumentNullException>(declaration != null);

            return declaration;
        }

        /// <summary>
        /// Invoked for each <see cref="XDocumentType"/>
        /// </summary>
        /// <param name="documentType"></param>
        /// <returns></returns>
        public virtual XDocumentType Visit(XDocumentType documentType)
        {
            Contract.Requires<ArgumentNullException>(documentType != null);

            return documentType;
        }

        /// <summary>
        /// Invoked for each <see cref="XText"/>
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public virtual XText Visit(XText text)
        {
            Contract.Requires<ArgumentNullException>(text != null);

            return text;
        }

        /// <summary>
        /// Invoked for each <see cref="XComment"/>
        /// </summary>
        /// <param name="comment"></param>
        /// <returns></returns>
        public virtual XComment Visit(XComment comment)
        {
            Contract.Requires<ArgumentNullException>(comment != null);

            return comment;
        }

        /// <summary>
        /// Invoked for each <see cref="XCData"/>
        /// </summary>
        /// <param name="cdata"></param>
        /// <returns></returns>
        public virtual XCData Visit(XCData cdata)
        {
            Contract.Requires<ArgumentNullException>(cdata != null);

            return cdata;
        }

        /// <summary>
        /// Invoked for each <see cref="XProcessingInstruction"/>
        /// </summary>
        /// <param name="processingInstruction"></param>
        /// <returns></returns>
        public virtual XProcessingInstruction Visit(XProcessingInstruction processingInstruction)
        {
            Contract.Requires<ArgumentNullException>(processingInstruction != null);

            return processingInstruction;
        }

        /// <summary>
        /// Invoked for each <see cref="XElement"/>.
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public virtual XElement Visit(XElement element)
        {
            Contract.Requires<ArgumentNullException>(element != null);

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
            Contract.Requires<ArgumentNullException>(attribute != null);

            return attribute;
        }

    }

}
