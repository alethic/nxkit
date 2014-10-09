using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;

using NXKit.View.Js;

namespace NXKit.XForms.View.Js
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
                "nxkit-xforms", 
                _ => typeof(ViewModuleResolver).Assembly.GetManifestResourceStream("NXKit.XForms.View.Js.nxkit-xforms.js").CopyTo(_), 
                "application/javascript",
                GetLastModifiedTime(),
                GetETag()),

            new ViewModuleInfo(
                "nxkit-xforms.css",
                _ => typeof(ViewModuleResolver).Assembly.GetManifestResourceStream("NXKit.XForms.View.Js.nxkit-xforms.css").CopyTo(_), 
                "text/css",
                GetLastModifiedTime(),
                GetETag()),

            new ViewModuleInfo(
                "nxkit-xforms.html",
                _ => typeof(ViewModuleResolver).Assembly.GetManifestResourceStream("NXKit.XForms.View.Js.nxkit-xforms.html").CopyTo(_), 
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
