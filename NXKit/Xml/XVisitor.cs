using System;
using System.Xml.Linq;

namespace NXKit.Xml
{

    /// <summary>
    /// Visits each object in an XLinq hierarchy to produce a new XLinq hierarchy.
    /// </summary>
    public class XVisitor
    {

        /// <summary>
        /// Invoked for each <see cref="XObject"/>.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public virtual void Visit(XObject obj)
        {
            if (obj == null)
                throw new ArgumentNullException(nameof(obj));

            if (obj is XNode)
                Visit((XNode)obj);
            else if (obj is XAttribute)
                Visit((XAttribute)obj);
            else
                throw new InvalidOperationException();
        }

        /// <summary>
        /// Invoked for each <see cref="XNode"/>.
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public virtual void Visit(XNode node)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));

            if (node is XDocumentType)
                Visit((XDocumentType)node);
            else if (node is XContainer)
                Visit((XContainer)node);
            else if (node is XText)
                Visit((XText)node);
            else if (node is XComment)
                Visit((XComment)node);
            else if (node is XCData)
                Visit((XCData)node);
            else if (node is XProcessingInstruction)
                Visit((XProcessingInstruction)node);
            else
                throw new InvalidOperationException();
        }

        /// <summary>
        /// Invoked for each <see cref="XContainer"/>.
        /// </summary>
        /// <param name="container"></param>
        public virtual void Visit(XContainer container)
        {
            if (container == null)
                throw new ArgumentNullException(nameof(container));

            if (container is XDocument)
                Visit((XDocument)container);
            else if (container is XElement)
                Visit((XElement)container);
            else
                throw new InvalidOperationException();
        }

        /// <summary>
        /// Invoked for each <see cref="XDocument"/>.
        /// </summary>
        /// <param name="document"></param>
        public virtual void Visit(XDocument document)
        {
            if (document == null)
                throw new ArgumentNullException(nameof(document));

            if (document.Declaration != null)
                Visit(document.Declaration);

            foreach (XObject node in document.Nodes())
                Visit(node);
        }

        /// <summary>
        /// Invoked for each <see cref="XDeclaration"/>
        /// </summary>
        /// <param name="declaration"></param>
        public virtual void Visit(XDeclaration declaration)
        {
            if (declaration == null)
                throw new ArgumentNullException(nameof(declaration));
        }

        /// <summary>
        /// Invoked for each <see cref="XDocumentType"/>
        /// </summary>
        /// <param name="documentType"></param>
        public virtual void Visit(XDocumentType documentType)
        {
            if (documentType == null)
                throw new ArgumentNullException(nameof(documentType));
        }

        /// <summary>
        /// Invoked for each <see cref="XText"/>
        /// </summary>
        /// <param name="text"></param>
        public virtual void Visit(XText text)
        {
            if (text == null)
                throw new ArgumentNullException(nameof(text));
        }

        /// <summary>
        /// Invoked for each <see cref="XComment"/>
        /// </summary>
        /// <param name="comment"></param>
        public virtual void Visit(XComment comment)
        {
            if (comment == null)
                throw new ArgumentNullException(nameof(comment));
        }

        /// <summary>
        /// Invoked for each <see cref="XCData"/>
        /// </summary>
        /// <param name="cdata"></param>
        public virtual void Visit(XCData cdata)
        {
            if (cdata == null)
                throw new ArgumentNullException(nameof(cdata));
        }

        /// <summary>
        /// Invoked for each <see cref="XProcessingInstruction"/>
        /// </summary>
        /// <param name="processingInstruction"></param>
        public virtual void Visit(XProcessingInstruction processingInstruction)
        {
            if (processingInstruction == null)
                throw new ArgumentNullException(nameof(processingInstruction));
        }

        /// <summary>
        /// Invoked for each <see cref="XElement"/>.
        /// </summary>
        /// <param name="element"></param>
        public virtual void Visit(XElement element)
        {
            if (element == null)
                throw new ArgumentNullException(nameof(element));

            foreach (XObject attr in element.Attributes())
                Visit(attr);

            foreach (XObject node in element.Nodes())
                Visit(node);
        }

        /// <summary>
        /// Invoked for each <see cref="XAttribute"/>.
        /// </summary>
        /// <param name="attribute"></param>
        public virtual void Visit(XAttribute attribute)
        {
            if (attribute == null)
                throw new ArgumentNullException(nameof(attribute));
        }

    }

}
