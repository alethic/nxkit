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
                UriTextBox.Text = new Uri(Request.Url, "../Examples/form.xml").ToString();
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