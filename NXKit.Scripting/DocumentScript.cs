using System;
using System.Diagnostics.Contracts;
using System.Xml;

namespace NXKit.Scripting
{

    /// <summary>
    /// Provides an interface towards executing scripts within the document.
    /// </summary>
    [Interface(XmlNodeType.Document)]
    public class DocumentScript :
        IDocumentScript,
        IOnSave,
        IOnLoad
    {

        readonly IScriptDispatcher dispatcher;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="dispatcher"></param>
        public DocumentScript(IScriptDispatcher dispatcher)
        {
            Contract.Requires<ArgumentNullException>(dispatcher != null);

            this.dispatcher = dispatcher;
        }

        public object Execute(string type, string code)
        {
            return dispatcher.Evaluate(type, code);
        }

        public void Load()
        {
            dispatcher.Load();
        }

        public void Save()
        {
            dispatcher.Save();
        }

    }

}
