using System;

using NXKit.Composition;

namespace NXKit.Net
{

    [Export(typeof(UriParser))]
    public class DynamicUriParser :
        GenericUriParser
    {

        static bool registered;

        /// <summary>
        /// Ensures the <see cref="UriParser"/> is registered.
        /// </summary>
        public static void Register()
        {
            if (!registered)
            {
                UriParser.Register(new DynamicUriParser(), DynamicUriHelper.UriSchemeDynamic, -1);
                registered = true;
            }

            DynamicWebRequestFactory.Register();
        }

        /// <summary>
        /// Initializes the static instance.
        /// </summary>
        static DynamicUriParser()
        {
            Register();
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public DynamicUriParser()
            : base(
                GenericUriParserOptions.NoFragment | 
                GenericUriParserOptions.NoPort | 
                GenericUriParserOptions.NoQuery | 
                GenericUriParserOptions.NoUserInfo)
        {

        }

    }

}
