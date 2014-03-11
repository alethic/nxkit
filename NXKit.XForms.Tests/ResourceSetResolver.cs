using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace NXKit.XForms.Tests
{

    class ResourceSetResolver :
        Dictionary<string, string>,
        IResolver
    {

        readonly Dictionary<string, string> map;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public ResourceSetResolver()
        {
            this.map = new Dictionary<string, string>();
        }

        public Stream Get(string href, string baseUri)
        {
            return new MemoryStream(Encoding.UTF8.GetBytes(map[href]));
        }

        public Stream Put(string href, string baseUri, Stream stream)
        {
            throw new NotImplementedException();
        }

    }

}
