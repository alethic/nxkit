using System;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;

using NXKit.View.Js;

namespace NXKit.XForms.Layout.View.Js
{

    [Export(typeof(IViewModuleProvider))]
    public class ViewModuleProvider :
        IViewModuleProvider
    {

        static DateTime GetLastModifiedTime()
        {
            var file = new FileInfo(typeof(ViewModuleProvider).Assembly.Location);
            if (file.Exists)
                return file.LastWriteTimeUtc;

            return DateTime.UtcNow;
        }

        static string GetETag()
        {
            return Math.Abs(typeof(ViewModuleProvider).Assembly.GetName().GetHashCode()).ToString();
        }

        static readonly ViewModuleInfo[] infos = new[]
        {
            new ViewModuleInfo(
                "nxkit-xforms-layout", 
                _ => typeof(ViewModuleProvider).Assembly.GetManifestResourceStream("NXKit.XForms.Layout.View.Js.nxkit-xforms-layout.js").CopyTo(_), 
                "application/javascript",
                GetLastModifiedTime(),
                GetETag()),

            new ViewModuleInfo(
                "nxkit-xforms-layout.css",
                _ => typeof(ViewModuleProvider).Assembly.GetManifestResourceStream("NXKit.XForms.Layout.View.Js.nxkit-xforms-layout.css").CopyTo(_), 
                "text/css",
                GetLastModifiedTime(),
                GetETag()),

            new ViewModuleInfo(
                "nxkit-xforms-layout.html",
                _ => typeof(ViewModuleProvider).Assembly.GetManifestResourceStream("NXKit.XForms.Layout.View.Js.nxkit-xforms-layout.html").CopyTo(_), 
                "text/html",
                GetLastModifiedTime(),
                GetETag()),
        };

        public IQueryable<ViewModuleInfo> GetViewModules()
        {
            return infos.AsQueryable();
        }

    }

}
