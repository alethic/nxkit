using System;
using System.Web.UI;

namespace NXKit.View.Web.UI.Test.Site
{

    public partial class Default :
        Page,
        IPostBackEventHandler
    {

        protected void View_Load(object sender, EventArgs args)
        {
            if (!IsPostBack)
            {
                // default uri value
                var uri = new Uri(Request.Url, "../Examples/form.xml");

                // allow the user to specify the URI to load
                if (Request["Uri"] != null)
                    uri = new Uri(Request["Uri"], UriKind.RelativeOrAbsolute);

                // ensure the URI is absolute
                if (!uri.IsAbsoluteUri)
                    uri = new Uri(Request.Url, uri);

                UriTextBox.Text = uri.ToString();
                View.Open(UriTextBox.Text);
            }
        }

        public void RaisePostBackEvent(string eventArgument)
        {
            if (eventArgument == "Load")
                View.Open(UriTextBox.Text);
        }

    }

}