using System;
using System.IO;
using System.Windows;

namespace NXKit.Wpf
{

    /// <summary>
    /// Implements a resolver for WPF.
    /// </summary>
    public class Resolver :
        DefaultResolver
    {

        public override Stream Get(Uri href)
        {
            if (!href.IsAbsoluteUri || href.Scheme == "pack")
            {
                var r = Application.GetResourceStream(href);
                return r != null ? r.Stream : null;
            }

            return base.Get(href);
        }

        public override Stream Put(Uri href, Stream stream)
        {
            if (!href.IsAbsoluteUri || href.Scheme == "pack")
                throw new NotSupportedException("PUT is not supported against pack");

            return base.Get(href);
        }

    }

}
