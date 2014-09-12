using System;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Primitives;
using System.Diagnostics.Contracts;

using NXKit.Composition;

namespace NXKit
{

    /// <summary>
    /// Provides access to document scope variables.
    /// </summary>
    [Export(typeof(DocumentEnvironment))]
    [PartMetadata(ScopeCatalog.ScopeMetadataKey, Scope.Host)]
    public class DocumentEnvironment
    {

        Document host;

        /// <summary>
        /// Gets the configured host.
        /// </summary>
        /// <returns></returns>
        [Export(typeof(Func<Document>))]
        public Document GetHost()
        {
            return host;
        }

        /// <summary>
        /// Sets the current <see cref="Document"/>.
        /// </summary>
        /// <param name="host"></param>
        internal void SetHost(Document host)
        {
            Contract.Requires<ArgumentNullException>(host != null);

            this.host = host;
        }

    }

}
