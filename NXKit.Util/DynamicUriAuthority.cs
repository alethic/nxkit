using System;
using System.Diagnostics.Contracts;
using System.Net;

namespace NXKit.Util
{

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


        readonly Guid id;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        protected DynamicUriAuthority()
        {
            this.id = Guid.NewGuid();

            DynamicUriRegistry.Register(this);
        }

        /// <summary>
        /// Gets the unique registration id.
        /// </summary>
        public Guid Id
        {
            get { return id; }
        }

        /// <summary>
        /// Gets the base URI for this authority.
        /// </summary>
        public Uri BaseUri
        {
            get { return new Uri(DynamicUriHelper.UriSchemeDynamic + "://" + id.ToString("N")); }
        }

        /// <summary>
        /// Disposes of the instance.
        /// </summary>
        public void Dispose()
        {
            DynamicUriRegistry.Unregister(this);
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
