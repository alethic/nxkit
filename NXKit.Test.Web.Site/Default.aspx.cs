using System;
using System.Web.UI;

using NXKit.Web.UI;

namespace NXKit.Test.Web.Site
{

    public partial class Default :
        Page,
        IPostBackEventHandler
    {

        protected override void OnLoad(EventArgs args)
        {
            base.OnLoad(args);
        }

        protected void View_Load(object sender, EventArgs args)
        {
            if (!IsPostBack)
            {
                UriTextBox.Text = new Uri(Request.Url, "../Resources/trigger.xml").ToString();
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