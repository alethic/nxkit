using System;

using NXKit.Composition;

namespace NXKit.Net
{

    [Export(typeof(UriParser))]
    public class ResourceUriParser :
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
                UriParser.Register(new ResourceUriParser(), "resource", -1);
                registered = true;
            }
        }

        /// <summary>
        /// Initializes the static instance.
        /// </summary>
        static ResourceUriParser()
        {
            Register();
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public ResourceUriParser()
            : base(GenericUriParserOptions.NoFragment | GenericUriParserOptions.NoPort | GenericUriParserOptions.NoQuery | GenericUriParserOptions.NoUserInfo)
        {

        }

    }

}
