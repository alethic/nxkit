using System;
using System.Linq;
using System.ComponentModel.Composition.Hosting;
using System.Web;
using NXKit.Composition;

[assembly: PreApplicationStartMethod(typeof(NXKit.Web.UI.ViewModuleHttpModule), "Start")]

namespace NXKit.Web.UI
{

    public class ViewModuleHttpModule :
        IHttpModule
    {

        static readonly string PREFIX = @"~/NXKit.axd/";

        public static void Start()
        {
            HttpApplication.RegisterModule(typeof(ViewModuleHttpModule));
        }
        
        public void Init(HttpApplication context)
        {
            context.BeginRequest += context_BeginRequest;
        }

        void context_BeginRequest(object sender, EventArgs args)
        {
            var context = (HttpApplication)sender;
            if (context == null)
                return;

            if (context.Request.AppRelativeCurrentExecutionFilePath.StartsWith(PREFIX))
            {
                var name = context.Request.QueryString["m"];
                var file = CompositionUtil.CreateContainer(CompositionUtil.DefaultGlobalCatalog)
                    .GetExportedValues<IViewModuleResolver>()
                    .Select(i => i.Resolve(name))
                    .FirstOrDefault(i => i != null);
                if (file != null)
                {
                    context.Response.StatusCode = 200;
                    file(context.Response);
                    context.Context.Response.End();
                }
                else
                {
                    context.Response.StatusCode = 404;
                    context.Context.Response.End();
                }
            }
        }

        public void Dispose()
        {

        }

    }

}
