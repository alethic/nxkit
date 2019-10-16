using System;
using System.Net;

using NXKit.Composition;

namespace NXKit.Net
{

    [Export(typeof(IWebRequestCreate))]
    public class DynamicWebRequestFactory :
        IWebRequestCreate
    {

        static bool registered;

        /// <summary>
        /// Ensures the <see cref="IWebRequestCreate"/> is registered.
        /// </summary>
        public static void Register()
        {
            if (!registered)
            {
                WebRequest.RegisterPrefix(DynamicUriHelper.UriSchemeDynamic, Default);
                registered = true;
            }
        }

        /// <summary>
        /// Initializes the static instance.
        /// </summary>
        static DynamicWebRequestFactory()
        {
            Register();
        }

        static IWebRequestCreate _default;

        /// <summary>
        /// Gets a reference to the default factory instance.
        /// </summary>
        public static IWebRequestCreate Default
        {
            get { return _default ?? (_default = new DynamicWebRequestFactory()); }
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        DynamicWebRequestFactory()
        {

        }

        public WebRequest Create(Uri uri)
        {
            return new DynamicWebRequest(uri);
        }

    }

}
