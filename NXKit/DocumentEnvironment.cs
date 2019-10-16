using System;

using NXKit.Composition;

namespace NXKit
{

    /// <summary>
    /// Provides access to document scope variables.
    /// </summary>
    [Export(typeof(DocumentEnvironment), CompositionScope.Host)]
    public class DocumentEnvironment
    {

        Document host;

        /// <summary>
        /// Gets the configured host.
        /// </summary>
        /// <returns></returns>
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
            this.host = host ?? throw new ArgumentNullException(nameof(host));
        }

    }

}
