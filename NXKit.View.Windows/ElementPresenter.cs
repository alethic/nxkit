using System;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Linq;

namespace NXKit.View.Windows
{

    public class ElementPresenter :
        Control
    {

        /// <summary>
        /// Initializes the static instance.
        /// </summary>
        static ElementPresenter()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ElementPresenter), new FrameworkPropertyMetadata(typeof(ElementPresenter)));
        }

        /// <summary>
        /// Registers a dependency property as backing store for the Element property.
        /// </summary>
        public static readonly DependencyProperty ElementProperty = DependencyProperty.Register(
            "Element",
            typeof(XElement),
            typeof(ElementPresenter),
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
            if (sender is null)
                throw new ArgumentNullException(nameof(sender));

            var ctrl = (ElementPresenter)sender;
            var element = ctrl.Element;
            if (element != null)
            {
                ctrl.SetValue(ElementTemplateResourceKeyPropertyKey, "NXKit:" + element.Name.ToString());
            }
            else
            {
                ctrl.SetValue(ElementTemplateResourceKeyPropertyKey, null);
            }
        }

        static readonly DependencyPropertyKey ElementTemplateResourceKeyPropertyKey = DependencyProperty.RegisterReadOnly(
            "ElementTemplateResourceKey",
            typeof(string),
            typeof(ElementPresenter),
            new FrameworkPropertyMetadata());

        public static readonly DependencyProperty ElementTemplateResourceKeyProperty = ElementTemplateResourceKeyPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets the <see cref="ElementTemplateResourceKey"/> that can be used to resolve the presented <see cref="XElement"/>'s template.
        /// </summary>
        public string ElementTemplateResourceKey
        {
            get { return (string)GetValue(ElementTemplateResourceKeyProperty); }
        }

    }

}
