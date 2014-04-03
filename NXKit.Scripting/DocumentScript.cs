using System;
using System.Diagnostics.Contracts;

namespace NXKit.Scripting
{

    /// <summary>
    /// Provides an interface towards executing scripts within the document.
    /// </summary>
    public class DocumentScript :
        IDocumentScript
    {

        readonly NXDocument document;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="document"></param>
        public DocumentScript(NXDocument document)
        {
            Contract.Requires<ArgumentNullException>(document != null);

            this.document = document;
        }

        IScriptDispatcher ScriptDispatcher
        {
            get { return document.Container.GetExportedValue<IScriptDispatcher>(); }
        }

        public object Execute(string type, string code)
        {
            return ScriptDispatcher.Execute(type, code);
        }

    }

}
