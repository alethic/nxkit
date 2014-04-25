using System;
using System.Diagnostics.Contracts;
using System.Xml;
using System.Xml.Linq;

using NXKit.Xml;

namespace NXKit.Scripting
{

    /// <summary>
    /// Provides an interface towards executing scripts within the document.
    /// </summary>
    [Interface(XmlNodeType.Document)]
    public class DocumentScript :
        IDocumentScript
    {

        readonly XDocument document;
        readonly IScriptDispatcher dispatcher;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="document"></param>
        /// <param name="dispatcher"></param>
        public DocumentScript(XDocument document, IScriptDispatcher dispatcher)
        {
            Contract.Requires<ArgumentNullException>(document != null);
            Contract.Requires<ArgumentNullException>(dispatcher != null);

            this.document = document;
            this.dispatcher = dispatcher;
        }

        public object Execute(string type, string code)
        {
            return dispatcher.Execute(type, code);
        }

    }

}
