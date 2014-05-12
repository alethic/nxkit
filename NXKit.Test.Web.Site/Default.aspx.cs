using System;
using System.Web.UI;

using NXKit.Web.UI;

namespace NXKit.Test.Web.Site
{

    public partial class Default :
        Page,
        IPostBackEventHandler
    {

        protected void View_Load(object sender, EventArgs args)
        {
            if (!IsPostBack)
            {
                UriTextBox.Text = new Uri(Request.Url, "../Examples/insert.xml").ToString();
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