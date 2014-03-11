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

        readonly Dictionary<Uri, string> map;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public ResourceSetResolver()
        {
            this.map = new Dictionary<Uri, string>();
        }

        public Stream Get(Uri href)
        {
            return new MemoryStream(Encoding.UTF8.GetBytes(map[href]));
        }

        public Stream Put(Uri uri, Stream stream)
        {
            throw new NotImplementedException();
        }

    }

}
