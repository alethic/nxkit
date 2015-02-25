using System;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;

using NXKit.View.Js;

namespace NXKit.Html.View.Js
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
                "nxkit-html", 
                _ => typeof(ViewModuleProvider).Assembly.GetManifestResourceStream("NXKit.Html.View.Js.nxkit-html.js").CopyTo(_), 
                "application/javascript",
                GetLastModifiedTime(),
                GetETag()),

            new ViewModuleInfo(
                "nxkit-html.css",
                _ => typeof(ViewModuleProvider).Assembly.GetManifestResourceStream("NXKit.Html.View.Js.nxkit-html.css").CopyTo(_), 
                "text/css",
                GetLastModifiedTime(),
                GetETag()),

            new ViewModuleInfo(
                "nxkit-html.html",
                _ => typeof(ViewModuleProvider).Assembly.GetManifestResourceStream("NXKit.Html.View.Js.nxkit-html.html").CopyTo(_), 
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
