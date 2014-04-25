using System.ComponentModel.Composition;
using System.Linq;

using NXKit.IO;

namespace NXKit.XForms.Examples
{

    [Export(typeof(IIOTransport))]
    public class ExampleIOHandler :
        IOTransport
    {

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
                return null;

            return new IOResponse(request, IOStatus.Success, stm, "application/xml");
        }

    }

}
