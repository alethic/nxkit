using System;
using System.IO;
using System.Net;

namespace NXKit
{

    /// <summary>
    /// Resolver implementation that supports HTTP.
    /// </summary>
    public class HttpResolver :
        IResolver
    {

        public Stream Get(Uri href)
        {
            var r = WebRequest.CreateHttp(href);
            r.Method = "GET";
            return r.GetResponse().GetResponseStream();
        }

        public Stream Put(Uri href, Stream stream)
        {
            var r = WebRequest.CreateHttp(href);
            r.Method = "PUT";
            stream.CopyTo(r.GetRequestStream());
            return r.GetResponse().GetResponseStream();
        }

    }

}
