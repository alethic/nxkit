using System;
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
                if (request == null)
                    throw new ArgumentNullException(nameof(request));

                this.getFunc = getFunc ?? throw new ArgumentNullException(nameof(getFunc));
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
            this.getFunc = getFunc ?? throw new ArgumentNullException(nameof(getFunc));
        }

        public override WebResponse GetResponse(DynamicWebRequest request)
        {
            return new FuncResponse(request, getFunc);
        }

    }

}
