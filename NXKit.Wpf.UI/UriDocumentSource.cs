using System;
using System.IO;
using System.Windows;

namespace NXKit.Wpf.UI
{

    public class UriDocumentSource :
        FrameworkElement,
        IDocumentSource,
        IResolver
    {

        public static readonly DependencyProperty UriProperty =
            DependencyProperty.Register("Uri", typeof(Uri), typeof(UriDocumentSource),
                new PropertyMetadata(Uri_PropertyChanged));

        static readonly DependencyPropertyKey DocumentPropertyKey =
            DependencyProperty.RegisterReadOnly("Document", typeof(Engine), typeof(UriDocumentSource),
                new PropertyMetadata(Document_PropertyChanged));

        public static DependencyProperty DocumentProperty =
            DocumentPropertyKey.DependencyProperty;

        static void Uri_PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs args)
        {
            ((UriDocumentSource)d).OnUriChanged();
        }

        static void Document_PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs args)
        {
            ((UriDocumentSource)d).OnDocumentChanged();
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public UriDocumentSource()
        {
            Load();
        }

        /// <summary>
        /// Gets or sets the <see cref="Uri"/> used to specify the document.
        /// </summary>
        public Uri Uri
        {
            get { return (Uri)GetValue(UriProperty); }
            set { SetValue(UriProperty, value); }
        }

        public event EventHandler UriChanged;

        void OnUriChanged()
        {
            Load();

            if (UriChanged != null)
                UriChanged(this, EventArgs.Empty);
        }

        /// <summary>
        /// Gets the <see cref="Engine"/> loaded from the <see cref="Uri"/>.
        /// </summary>
        public Engine Document
        {
            get { return (Engine)GetValue(DocumentProperty); }
            private set { SetValue(DocumentPropertyKey, value); }
        }

        public event EventHandler DocumentChanged;

        void OnDocumentChanged()
        {
            if (DocumentChanged != null)
                DocumentChanged(this, EventArgs.Empty);
        }

        /// <summary>
        /// Initializes the <see cref="Engine"/>.
        /// </summary>
        void Load()
        {
            var stream = Uri != null ? Application.GetResourceStream(Uri) : null;
        }

        Stream IResolver.Get(string href, string baseUri)
        {
            return null;
        }

        Stream IResolver.Put(string href, string baseUri, Stream stream)
        {
            return null;
        }

    }

}
