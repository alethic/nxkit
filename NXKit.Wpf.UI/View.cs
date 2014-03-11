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

        static readonly DependencyPropertyKey RootVisualPropertyKey =
            DependencyProperty.RegisterReadOnly("RootVisual", typeof(Visual), typeof(View),
                new PropertyMetadata(RootVisual_PropertyChanged));

        public static readonly DependencyProperty RootVisualProperty =
            RootVisualPropertyKey.DependencyProperty;

        static void Source_PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs args)
        {
            ((View)d).OnSourceChanged((IDocumentSource)args.OldValue, (IDocumentSource)args.NewValue);
        }

        static void RootVisual_PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs args)
        {

        }

        static View()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(View), new FrameworkPropertyMetadata(typeof(View)));
        }

        public IDocumentSource Source
        {
            get { return (IDocumentSource)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        void OnSourceChanged(IDocumentSource oldValue, IDocumentSource newValue)
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
            private set { SetValue(RootVisualPropertyKey, value); }
        }

    }

}
