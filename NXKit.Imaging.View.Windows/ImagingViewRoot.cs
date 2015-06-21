using System.Windows;
using System.Windows.Controls;
using System.Xml.Linq;

namespace NXKit.Imaging.View.Windows
{

    public class ImagingViewRoot :
        Control
    {

        /// <summary>
        /// Initializes the static instance.
        /// </summary>
        static ImagingViewRoot()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ImagingViewRoot), new FrameworkPropertyMetadata(typeof(ImagingViewRoot)));
        }

        /// <summary>
        /// Registers a dependency property as backing store for the Element property.
        /// </summary>
        public static readonly DependencyProperty ElementProperty = DependencyProperty.Register(
            "Element",
            typeof(XElement),
            typeof(ImagingViewRoot),
            new FrameworkPropertyMetadata());

        /// <summary>
        /// Gets or sets the <see cref="XElement"/> to be presented.
        /// </summary>
        public XElement Element
        {
            get { return (XElement)GetValue(ElementProperty); }
            set { SetValue(ElementProperty, value); }
        }

    }

}
