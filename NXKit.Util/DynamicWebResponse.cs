using System;
using System.Diagnostics.Contracts;
using System.Net;

namespace NXKit.Util
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
            Contract.Requires<ArgumentNullException>(request != null);

            this.request = request;
        }

    }

}
