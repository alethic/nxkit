using System;
using System.Diagnostics.Contracts;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Linq;

namespace NXKit.Imaging.View.Windows
{

    public class RegionView :
        Control
    {

        /// <summary>
        /// Initializes the static instance.
        /// </summary>
        static RegionView()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RegionView), new FrameworkPropertyMetadata(typeof(RegionView)));
        }

        /// <summary>
        /// Registers a dependency property as backing store for the Element property.
        /// </summary>
        public static readonly DependencyProperty ElementProperty = DependencyProperty.Register(
            "Element",
            typeof(XElement),
            typeof(RegionView),
            new FrameworkPropertyMetadata(Element_PropertyChanged));

        /// <summary>
        /// Gets or sets the <see cref="XElement"/> to be presented.
        /// </summary>
        public XElement Element
        {
            get { return (XElement)GetValue(ElementProperty); }
            set { SetValue(ElementProperty, value); }
        }

        /// <summary>
        /// Invoked when the value of Element is changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        static void Element_PropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            Contract.Requires<ArgumentNullException>(sender != null);
            Contract.Requires<ArgumentNullException>(args != null);
        }

    }

}
