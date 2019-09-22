using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;

namespace NXKit.Net
{

    class ResourceWebResponse :
        WebResponse
    {

        readonly ResourceWebRequest request;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public ResourceWebResponse(ResourceWebRequest request)
            : base()
        {
            this.request = request ?? throw new ArgumentNullException(nameof(request));
        }

        public override Stream GetResponseStream()
        {
            var assembly = Assembly.Load(new AssemblyName(request.RequestUri.Authority));
            if (assembly == null)
                throw new WebException("Cannot locate assembly.");

            var find = assembly.GetManifestResourceNames();
            var path = assembly.GetName().Name + request.RequestUri.AbsolutePath;
            var rslt = path.ToCharArray();
            while (!Array.Exists(find, _ => _ == new string(rslt)) && Array.Exists(rslt, _ => _ == '/'))
                rslt[Array.FindLastIndex(rslt, _ => _ == '/')] = '.';

            var name = new string(rslt);
            if (find.Contains(name))
                return assembly.GetManifestResourceStream(name);

            return null;
        }

    }

}
