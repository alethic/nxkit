using System;
using System.Collections.Generic;
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
                UriTextBox.Text = new System.Uri(Request.Url, "../Resources/form.xml").ToString();
                View.Configure(UriTextBox.Text);
            }
        }

        protected void LoadButton_Click(object sender, EventArgs args)
        {
            View.Configure(UriTextBox.Text);
        }

    }

}