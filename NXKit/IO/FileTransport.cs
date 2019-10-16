using System;

using NXKit.Composition;

namespace NXKit.IO
{

    /// <summary>
    /// Handles submissions of the 'file' scheme.
    /// </summary>
    [Export(typeof(IIOTransport))]
    public class FileTransport :
        WebTransport
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public FileTransport() : base()
        {

        }

        /// <summary>
        /// Returns <c>true</c> if the processor can handle this request.
        /// </summary>
        /// <param name="submit"></param>
        /// <returns></returns>
        public override Priority CanSend(IORequest submit)
        {
            if (submit.ResourceUri.Scheme == Uri.UriSchemeFile)
                return Priority.Default;
            else
                return Priority.Ignore;
        }

    }

}
