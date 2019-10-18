using System;
using System.Linq;

using NXKit.Composition;
using NXKit.IO;

namespace NXKit.XForms.Examples
{

    [Export(typeof(IIOTransport))]
    public class ExampleIOTransport :
        IOTransport
    {

        /// <summary>
        /// Initializes the static instance.
        /// </summary>
        static ExampleIOTransport()
        {
            if (UriParser.IsKnownScheme("nx-example") == false)
                UriParser.Register(new GenericUriParser(GenericUriParserOptions.Default | GenericUriParserOptions.AllowEmptyAuthority), "nx-example", -1);
        }

        public override Priority CanSend(IORequest request)
        {
            return request.ResourceUri.Scheme == "nx-example" ? Priority.High : Priority.Ignore;
        }

        public override IOResponse Submit(IORequest request)
        {
            var stm = typeof(Ref).Assembly
                .GetManifestResourceNames()
                .Where(i => i.EndsWith("." + request.ResourceUri.LocalPath.Substring(1)))
                .Select(i => typeof(Ref).Assembly.GetManifestResourceStream(i))
                .FirstOrDefault();

            if (stm == null)
                return new IOResponse(request, IOStatus.Unknown);
            else
                return new IOResponse(request, IOStatus.Success, stm, "application/xml");
        }

    }

}
