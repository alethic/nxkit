using System;
using System.Net;

namespace NXKit.Net
{

    /// <summary>
    /// Base class for custom URI namespaces under the 'nx' scheme. Extend this class to implement your own custom
    /// namespaces.
    /// </summary>
    public abstract class DynamicUriAuthority :
        IDisposable
    {

        /// <summary>
        /// Initializes the static instance.
        /// </summary>
        static DynamicUriAuthority()
        {
            DynamicUriParser.Register();
        }


        readonly Guid id = Guid.NewGuid();

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        protected DynamicUriAuthority()
        {
            DynamicUriRegistry.Register(this);
        }

        /// <summary>
        /// Gets the unique registration id.
        /// </summary>
        public Guid Id => id;

        /// <summary>
        /// Gets the base URI for this authority.
        /// </summary>
        public Uri BaseUri => new Uri(DynamicUriHelper.UriSchemeDynamic + "://" + id.ToString("N"));

        /// <summary>
        /// Disposes of the instance.
        /// </summary>
        public void Dispose()
        {
            DynamicUriRegistry.Unregister(this);
        }

        /// <summary>
        /// Disposes of the instance.
        /// </summary>
        ~DynamicUriAuthority()
        {
            Dispose();
        }

        /// <summary>
        /// Gets the response for the given registered request.
        /// </summary>
        /// <returns></returns>
        public virtual WebResponse GetResponse(DynamicWebRequest request)
        {
            throw new InvalidOperationException();
        }

    }

}
