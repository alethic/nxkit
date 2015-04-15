using System.Windows;
using System.Windows.Controls;

namespace NXKit.View.Windows
{

    /// <summary>
    /// Provides a Windows WPF <see cref="Control"/> that visualizes a NXKit <see cref="Document"/>.
    /// </summary>
    public class View :
        Control
    {

        /// <summary>
        /// Initializes the static instance.
        /// </summary>
        static View()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(View), new FrameworkPropertyMetadata(typeof(View)));
        }

        /// <summary>
        /// Registers a dependency property as backing store for the Document property.
        /// </summary>
        public static readonly DependencyProperty DocumentProperty = DependencyProperty.Register(
            "Document",
            typeof(Document),
            typeof(View),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsParentMeasure));

        /// <summary>
        /// Gets or sets the <see cref="Document"/> to be viewed.
        /// </summary>
        public Document Document
        {
            get { return (Document)GetValue(DocumentProperty); }
            set { SetValue(DocumentProperty, value); }
        }

    }

}
