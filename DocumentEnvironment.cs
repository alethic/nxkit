using System;
using System.ComponentModel.Composition;
using System.Diagnostics.Contracts;

using NXKit.Composition;

namespace NXKit
{

    /// <summary>
    /// Provides access to document scope variables.
    /// </summary>
    [ScopeExport(typeof(DocumentEnvironment), Scope.Host)]
    public class DocumentEnvironment
    {

        NXDocumentHost host;

        /// <summary>
        /// Gets the configured host.
        /// </summary>
        /// <returns></returns>
        [ScopeExport(typeof(Func<NXDocumentHost>), Scope.Host)]
        public NXDocumentHost GetHost()
        {
            return host;
        }

        internal void SetHost(NXDocumentHost host)
        {
            Contract.Requires<ArgumentNullException>(host != null);

            this.host = host;
        }

    }

}
