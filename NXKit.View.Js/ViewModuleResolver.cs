using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;

namespace NXKit.View.Js
{

    [Export(typeof(IViewModuleResolver))]
    public class ViewModuleResolver :
        IViewModuleResolver
    {

        static DateTime GetLastModifiedTime()
        {
            var file = new FileInfo(typeof(ViewModuleResolver).Assembly.Location);
            if (file.Exists)
                return file.LastWriteTimeUtc;

            return DateTime.UtcNow;
        }

        static string GetETag()
        {
            return Math.Abs(typeof(ViewModuleResolver).Assembly.GetName().GetHashCode()).ToString();
        }

        static readonly ViewModuleInfo[] infos = new[]
        {
            new ViewModuleInfo(
                "nx-html",
                _ => typeof(ViewModuleResolver).Assembly.GetManifestResourceStream("NXKit.View.Js.nx-html.js").CopyTo(_), 
                "application/javascript",
                GetLastModifiedTime(),
                GetETag()),

            new ViewModuleInfo(
                "nx-css",
                _ => typeof(ViewModuleResolver).Assembly.GetManifestResourceStream("NXKit.View.Js.nx-css.js").CopyTo(_), 
                "application/javascript",
                GetLastModifiedTime(),
                GetETag()),

            new ViewModuleInfo(
                "nxkit", 
                _ => typeof(ViewModuleResolver).Assembly.GetManifestResourceStream("NXKit.View.Js.nxkit.js").CopyTo(_), 
                "application/javascript",
                GetLastModifiedTime(),
                GetETag()),

            new ViewModuleInfo(
                "nxkit.css",
                _ => typeof(ViewModuleResolver).Assembly.GetManifestResourceStream("NXKit.View.Js.nxkit.css").CopyTo(_), 
                "text/css",
                GetLastModifiedTime(),
                GetETag()),

            new ViewModuleInfo(
                "nxkit.html",
                _ => typeof(ViewModuleResolver).Assembly.GetManifestResourceStream("NXKit.View.Js.nxkit.html").CopyTo(_), 
                "text/html",
                GetLastModifiedTime(),
                GetETag()),
        };

        public IEnumerable<ViewModuleInfo> Resolve(string name)
        {
            return infos.Where(i => i.Name == name);
        }

    }

}
