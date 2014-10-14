using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;

using NXKit.Composition;

namespace NXKit.View.Js
{

    [Export(typeof(IViewModuleProvider))]
    public class ViewModuleProvider :
        IViewModuleProvider
    {

        /// <summary>
        /// Resolves the <see cref="ViewModule"/> with the specified name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static ViewModuleInfo ResolveViewModule(string name)
        {
            return CompositionUtil.CreateContainer(CompositionUtil.DefaultGlobalCatalog)
                .GetExportedValues<IViewModuleProvider>()
                .SelectMany(i => i.GetViewModules().Where(j => j.Name == name))
                .FirstOrDefault(i => i != null);
        }

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
                "nx-js",
                _ => typeof(ViewModuleProvider).Assembly.GetManifestResourceStream("NXKit.View.Js.nx-js.js").CopyTo(_), 
                "application/javascript",
                GetLastModifiedTime(),
                GetETag()),

            new ViewModuleInfo(
                "nx-html",
                _ => typeof(ViewModuleProvider).Assembly.GetManifestResourceStream("NXKit.View.Js.nx-html.js").CopyTo(_), 
                "application/javascript",
                GetLastModifiedTime(),
                GetETag()),

            new ViewModuleInfo(
                "nx-css",
                _ => typeof(ViewModuleProvider).Assembly.GetManifestResourceStream("NXKit.View.Js.nx-css.js").CopyTo(_), 
                "application/javascript",
                GetLastModifiedTime(),
                GetETag()),

            new ViewModuleInfo(
                "nxkit", 
                _ => typeof(ViewModuleProvider).Assembly.GetManifestResourceStream("NXKit.View.Js.nxkit.js").CopyTo(_), 
                "application/javascript",
                GetLastModifiedTime(),
                GetETag()),

            new ViewModuleInfo(
                "nxkit.css",
                _ => typeof(ViewModuleProvider).Assembly.GetManifestResourceStream("NXKit.View.Js.nxkit.css").CopyTo(_), 
                "text/css",
                GetLastModifiedTime(),
                GetETag()),

            new ViewModuleInfo(
                "nxkit.html",
                _ => typeof(ViewModuleProvider).Assembly.GetManifestResourceStream("NXKit.View.Js.nxkit.html").CopyTo(_), 
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
