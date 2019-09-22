using System;
using System.Net;

namespace NXKit.Net
{

    abstract class DynamicWebResponse :
         WebResponse
    {

        readonly DynamicWebRequest request;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public DynamicWebResponse(DynamicWebRequest request)
            : base()
        {
            this.request = request ?? throw new ArgumentNullException(nameof(request));
        }

    }

}
