using System;
using System.Diagnostics.Contracts;
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
            Contract.Requires<ArgumentNullException>(obj != null);

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
            Contract.Requires<ArgumentNullException>(node != null);

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

        /// 
        /// Invoked for each <see cref="XContainer"/>.
        /// </summary>
        /// <param name="container"></param>
        public virtual void Visit(XContainer container)
        {
            Contract.Requires<ArgumentNullException>(container != null);

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
            Contract.Requires<ArgumentNullException>(document != null);

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
            Contract.Requires<ArgumentNullException>(declaration != null);
        }

        /// <summary>
        /// Invoked for each <see cref="XDocumentType"/>
        /// </summary>
        /// <param name="documentType"></param>
        public virtual void Visit(XDocumentType documentType)
        {
            Contract.Requires<ArgumentNullException>(documentType != null);
        }

        /// <summary>
        /// Invoked for each <see cref="XText"/>
        /// </summary>
        /// <param name="text"></param>
        public virtual void Visit(XText text)
        {
            Contract.Requires<ArgumentNullException>(text != null);
        }

        /// <summary>
        /// Invoked for each <see cref="XComment"/>
        /// </summary>
        /// <param name="comment"></param>
        public virtual void Visit(XComment comment)
        {
            Contract.Requires<ArgumentNullException>(comment != null);
        }

        /// <summary>
        /// Invoked for each <see cref="XCData"/>
        /// </summary>
        /// <param name="cdata"></param>
        public virtual void Visit(XCData cdata)
        {
            Contract.Requires<ArgumentNullException>(cdata != null);
        }

        /// <summary>
        /// Invoked for each <see cref="XProcessingInstruction"/>
        /// </summary>
        /// <param name="processingInstruction"></param>
        public virtual void Visit(XProcessingInstruction processingInstruction)
        {
            Contract.Requires<ArgumentNullException>(processingInstruction != null);
        }

        /// <summary>
        /// Invoked for each <see cref="XElement"/>.
        /// </summary>
        /// <param name="element"></param>
        public virtual void Visit(XElement element)
        {
            Contract.Requires<ArgumentNullException>(element != null);

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
            Contract.Requires<ArgumentNullException>(attribute != null);
        }

    }

}
