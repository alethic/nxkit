﻿using System;
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
                var c = new NXDocumentConfiguration();
                c.AddModule<NXKit.XForms.XFormsModule>();
                c.AddModule<NXKit.XForms.Layout.LayoutModule>();
                View.Configure("form.xml", c);
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

        protected void NextButton_Click(object sender, EventArgs args)
        {
            var l = new LinkedList<FormNavigation>(View.Navigations);
            var n = l.Find(View.CurrentPage);
            if (n.Next != null)
                View.Navigate(n.Next.Value);
        }

        protected void PrevButton_Click(object sender, EventArgs args)
        {
            var l = new LinkedList<FormNavigation>(View.Navigations);
            var n = l.Find(View.CurrentPage);
            if (n.Previous != null)
                View.Navigate(n.Previous.Value);
        }

    }

}