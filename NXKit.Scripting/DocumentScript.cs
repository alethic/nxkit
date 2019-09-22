using System;
using System.ComponentModel.Composition;
using System.Xml.Linq;

using NXKit.Composition;

namespace NXKit.Scripting
{

    /// <summary>
    /// Provides an interface towards executing scripts within the document.
    /// </summary>
    [Extension(ExtensionObjectType.Document)]
    [PartMetadata(ScopeCatalog.ScopeMetadataKey, Scope.Object)]
    public class DocumentScript :
        DocumentExtension,
        IDocumentScript,
        IOnLoad,
        IOnSave
    {

        readonly IScriptDispatcher dispatcher;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="document"></param>
        /// <param name="dispatcher"></param>
        [ImportingConstructor]
        public DocumentScript(
            XDocument document,
            IScriptDispatcher dispatcher)
            : base(document)
        {
            if (document == null)
                throw new ArgumentNullException(nameof(document));

            this.dispatcher = dispatcher ?? throw new ArgumentNullException(nameof(dispatcher));
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
