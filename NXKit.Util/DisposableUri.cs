using System;
using System.Diagnostics.Contracts;

namespace NXKit.Util
{

    /// <summary>
    /// Base class for a URI which can be disposed.
    /// </summary>
    public abstract class DisposableUri :
        Uri,
        IDisposable
    {

        /// <summary>
        /// Wraps the existing URI in a new URI.
        /// </summary>
        /// <param name="uri"></param>
        protected DisposableUri(Uri uri)
            : base(uri.ToString())
        {
            Contract.Requires<ArgumentNullException>(uri != null);
        }

        /// <summary>
        /// Disposes of the instance.
        /// </summary>
        public abstract void Dispose();

    }

}
