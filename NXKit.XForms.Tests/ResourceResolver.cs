using System;
using System.Diagnostics.Contracts;
using System.IO;

namespace NXKit.XForms.Tests
{

    class ResourceResolver :
        IResolver
    {

        readonly Func<string, Stream> getFunc;
        readonly Func<string, Stream, Stream> putFunc;

        public ResourceResolver(
            Func<string, Stream> getFunc,
            Func<string, Stream, Stream> putFunc)
        {
            Contract.Requires<ArgumentNullException>(getFunc != null);
            Contract.Requires<ArgumentNullException>(putFunc != null);
        }

        public Stream Get(string href)
        {
            return getFunc(href);
        }

        public Stream Put(string href, Stream stream)
        {
            return putFunc(href, stream);
        }

    }

}
