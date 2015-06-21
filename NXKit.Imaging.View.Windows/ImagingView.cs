using System.Windows;
using System.Windows.Controls;

namespace NXKit.Imaging.View.Windows
{

    /// <summary>
    /// Defines a view layer that draws overlayed region indicators.
    /// </summary>
    public class ImagingView :
        ContentControl
    {

        /// <summary>
        /// Initializes the static instance.
        /// </summary>
        static ImagingView()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ImagingView), new FrameworkPropertyMetadata(typeof(ImagingView)));
        }

        /// <summary>
        /// Registers a dependency property as backing store for the Document property.
        /// </summary>
        public static readonly DependencyProperty DocumentProperty = DependencyProperty.Register(
            "Document",
            typeof(Document),
            typeof(ImagingView),
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
