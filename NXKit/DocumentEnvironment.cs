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

        NXDocumentHost host;

        /// <summary>
        /// Gets the configured host.
        /// </summary>
        /// <returns></returns>
        [Export(typeof(Func<NXDocumentHost>))]
        public NXDocumentHost GetHost()
        {
            return host;
        }

        /// <summary>
        /// Sets the current <see cref="NXDocumentHost"/>.
        /// </summary>
        /// <param name="host"></param>
        internal void SetHost(NXDocumentHost host)
        {
            Contract.Requires<ArgumentNullException>(host != null);

            this.host = host;
        }

    }

}
