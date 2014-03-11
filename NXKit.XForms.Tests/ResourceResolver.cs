using System;
using System.Diagnostics.Contracts;
using System.IO;

namespace NXKit.XForms.Tests
{

    class ResourceResolver :
        IResolver
    {

        readonly Func<Uri, Stream> getFunc;
        readonly Func<Uri, Stream, Stream> putFunc;

        public ResourceResolver(
            Func<Uri, Stream> getFunc,
            Func<Uri, Stream, Stream> putFunc)
        {
            Contract.Requires<ArgumentNullException>(getFunc != null);
            Contract.Requires<ArgumentNullException>(putFunc != null);

            this.getFunc = getFunc;
            this.putFunc = putFunc;
        }

        public Stream Get(Uri href)
        {
            return getFunc(href);
        }

        public Stream Put(Uri href, Stream stream)
        {
            return putFunc(href, stream);
        }

    }

}
