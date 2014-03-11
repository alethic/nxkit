using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;

namespace NXKit.Wpf
{

    public class UriDocumentSource :
        FrameworkElement,
        IDocumentSource
    {

        public static readonly DependencyProperty UriProperty =
            DependencyProperty.Register("Uri", typeof(Uri), typeof(UriDocumentSource),
                new PropertyMetadata(Uri_PropertyChanged));

        public static readonly DependencyProperty ModulesProperty =
            DependencyProperty.Register("Modules", typeof(IEnumerable<Type>), typeof(UriDocumentSource),
                new PropertyMetadata(Modules_PropertyChanged));

        static readonly DependencyPropertyKey DocumentPropertyKey =
            DependencyProperty.RegisterReadOnly("Document", typeof(NXDocument), typeof(UriDocumentSource),
                new PropertyMetadata(Document_PropertyChanged));

        public static DependencyProperty DocumentProperty =
            DocumentPropertyKey.DependencyProperty;

        static void Uri_PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs args)
        {
            ((UriDocumentSource)d).OnUriChanged();
        }

        static void Modules_PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs args)
        {
            ((UriDocumentSource)d).OnModulesChanged();
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

        public IEnumerable<Type> Modules
        {
            get { return (IEnumerable<Type>)GetValue(ModulesProperty); }
            set { SetValue(ModulesProperty, value); }
        }

        void OnModulesChanged()
        {
            Load();
        }

        /// <summary>
        /// Gets the <see cref="Document"/> loaded from the <see cref="Uri"/>.
        /// </summary>
        public NXDocument Document
        {
            get { return (NXDocument)GetValue(DocumentProperty); }
            private set { SetValue(DocumentPropertyKey, value); }
        }

        public event EventHandler DocumentChanged;

        void OnDocumentChanged()
        {
            if (DocumentChanged != null)
                DocumentChanged(this, EventArgs.Empty);
        }

        /// <summary>
        /// Initializes the <see cref="Document"/>.
        /// </summary>
        void Load()
        {
            if (Uri != null && Modules != null)
            {
                var cfg = new NXDocumentConfiguration();
                foreach (var moduleType in Modules)
                    cfg.AddModule(moduleType);

                Document = new NXDocument(cfg, Uri, new Resolver());
            }
            else
                Document = null;
        }

    }

}
