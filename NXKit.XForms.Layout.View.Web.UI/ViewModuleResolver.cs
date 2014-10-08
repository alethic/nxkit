using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Web;
using System.Linq;
using NXKit.View.Web.UI;
using System.IO;

namespace NXKit.XForms.Layout.View.Web.UI
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
                "nxkit-xforms-layout", 
                _ => typeof(ViewModuleResolver).Assembly.GetManifestResourceStream("NXKit.XForms.Layout.View.Web.UI.Scripts.nxkit-xforms-layout.js").CopyTo(_), 
                "application/javascript",
                GetLastModifiedTime(),
                GetETag()),

            new ViewModuleInfo(
                "nxkit-xforms-layout.css",
                _ => typeof(ViewModuleResolver).Assembly.GetManifestResourceStream("NXKit.XForms.Layout.View.Web.UI.Content.nxkit-xforms-layout.css").CopyTo(_), 
                "text/css",
                GetLastModifiedTime(),
                GetETag()),

            new ViewModuleInfo(
                "nxkit-xforms-layout.html",
                _ => typeof(ViewModuleResolver).Assembly.GetManifestResourceStream("NXKit.XForms.Layout.View.Web.UI.Content.nxkit-xforms-layout.html").CopyTo(_), 
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
