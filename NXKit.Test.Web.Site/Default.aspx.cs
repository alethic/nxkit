using System;
using System.Xml.Linq;

using NXKit.Web.UI;

namespace NXKit.Test.Web.Site
{

    public partial class Default :
        System.Web.UI.Page
    {

        protected override void OnLoad(EventArgs args)
        {
            base.OnLoad(args);
        }

        protected void View_Load(object sender, EventArgs args)
        {
            if (!IsPostBack)
            {
                var c = new NXDocumentConfiguration();
                c.AddModule<NXKit.XForms.XFormsModule>();
                c.AddModule<NXKit.XForms.Layout.LayoutModule>();
                var d = XDocument.Load(typeof(Default).Assembly.GetManifestResourceStream("NXKit.Test.Web.Site.Resources.form.xml"));
                View.Configure(c, d);
            }
        }

        protected void View_ResourceAction(object sender, ResourceActionEventArgs args)
        {
            if (args.Method != ResourceActionMethod.Get)
                return;

            if (args.Uri.IsAbsoluteUri)
                return;

            // attempt to load requested resource as embedded resource
            args.Result = typeof(Default).Assembly.GetManifestResourceStream("NXKit.Test.Web.Site.Resources." + args.Uri.ToString());
        }

    }

}