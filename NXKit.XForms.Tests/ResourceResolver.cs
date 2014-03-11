using System;
using System.Diagnostics.Contracts;
using System.IO;

namespace NXKit.XForms.Tests
{

    class ResourceResolver :
        IResourceResolver
    {

        readonly Func<string, string, Stream> getFunc;
        readonly Func<string, string, Stream, Stream> putFunc;

        public ResourceResolver(
            Func<string, string, Stream> getFunc,
            Func<string, string, Stream, Stream> putFunc)
        {
            Contract.Requires<ArgumentNullException>(getFunc != null);
            Contract.Requires<ArgumentNullException>(putFunc != null);
        }

        public Stream Get(string href, string baseUri)
        {
            return getFunc(href, baseUri);
        }

        public Stream Put(string href, string baseUri, Stream stream)
        {
            return putFunc(href, baseUri, stream);
        }

    }

}
