using System;
using System.Windows;
using System.Windows.Controls;

namespace NXKit.Wpf.UI
{

    public class View :
        Control
    {

        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.Register("Source", typeof(IDocumentSource), typeof(View),
            new PropertyMetadata(Source_PropertyChanged));

        public static readonly DependencyProperty RootVisualProperty =
            DependencyProperty.Register("RootVisual", typeof(Visual), typeof(View));

        static View()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(View), new FrameworkPropertyMetadata(typeof(View)));
        }

        public IDocumentSource Source
        {
            get { return (IDocumentSource)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        static void Source_PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs args)
        {
            var v = (View)d;
            var o = (IDocumentSource)args.OldValue;
            var n = (IDocumentSource)args.NewValue;

            v.SourceChanged(o, n);
        }

        void SourceChanged(IDocumentSource oldValue, IDocumentSource newValue)
        {
            if (oldValue != null)
                oldValue.DocumentChanged -= Source_DocumentChanged;

            if (newValue != null)
                newValue.DocumentChanged += Source_DocumentChanged;

            SetRootVisual();
        }

        void Source_DocumentChanged(object sender, EventArgs args)
        {
            SetRootVisual();
        }

        void SetRootVisual()
        {
            RootVisual = Source != null && Source.Document != null ? Source.Document.RootVisual : null;
        }

        public Visual RootVisual
        {
            get { return (Visual)GetValue(RootVisualProperty); }
            set { SetValue(RootVisualProperty, value); }
        }

    }

}
