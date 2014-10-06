using System;
using System.Web;

[assembly: PreApplicationStartMethod(typeof(NXKit.View.Web.UI.ViewModuleHttpModule), "Start")]

namespace NXKit.View.Web.UI
{

    /// <summary>
    /// Maps NXKit.axd requests to the <see cref="ViewModuleHttpHandler"/>.
    /// </summary>
    public class ViewModuleHttpModule :
        IHttpModule
    {

        static readonly string PREFIX = @"~/NXKit.axd/";

        /// <summary>
        /// PreApplicationStartMethod
        /// </summary>
        public static void Start()
        {
            HttpApplication.RegisterModule(typeof(ViewModuleHttpModule));
        }
        
        public void Init(HttpApplication context)
        {
            context.PostAuthenticateRequest += context_PostAuthenticateRequest;
        }

        void context_PostAuthenticateRequest(object sender, EventArgs args)
        {
            var context = (HttpApplication)sender;
            if (context == null)
                return;

            // send to proper handler if under prefix
            if (context.Request.AppRelativeCurrentExecutionFilePath.StartsWith(PREFIX))
                context.Context.RemapHandler(new ViewModuleHttpHandler());
        }

        public void Dispose()
        {

        }

    }

}
