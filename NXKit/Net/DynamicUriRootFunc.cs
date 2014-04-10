using System;
using System.Diagnostics.Contracts;
using System.IO;
using System.Net;

namespace NXKit.Net
{

    public class DynamicUriRootFunc :
        DynamicUriAuthority
    {

        class FuncResponse :
            DynamicWebResponse
        {

            readonly Func<Stream> getFunc;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="request"></param>
            /// <param name="getFunc"></param>
            public FuncResponse(DynamicWebRequest request, Func<Stream> getFunc)
                : base(request)
            {
                Contract.Requires<ArgumentNullException>(request != null);
                Contract.Requires<ArgumentNullException>(getFunc != null);

                this.getFunc = getFunc;
            }

            public override Stream GetResponseStream()
            {
                return getFunc();
            }

        }

        readonly Func<Stream> getFunc;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="getFunc"></param>
        public DynamicUriRootFunc(Func<Stream> getFunc)
            : base()
        {
            Contract.Requires<ArgumentNullException>(getFunc != null);

            this.getFunc = getFunc;
        }

        public override WebResponse GetResponse(DynamicWebRequest request)
        {
            return new FuncResponse(request, getFunc);
        }

    }

}
