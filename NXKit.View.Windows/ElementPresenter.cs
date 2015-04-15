using System;
using System.Diagnostics.Contracts;
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
            Contract.Requires<ArgumentNullException>(sender != null);
            Contract.Requires<ArgumentNullException>(args != null);

            var ctrl = (ElementPresenter)sender;
            var element = ctrl.Element;
            if (element != null)
            {
                ctrl.SetValue(ElementModelItemPropertyKey, new ElementModelItem(element));
                ctrl.SetValue(ElementTemplateResourceKeyPropertyKey, new ElementTemplateResourceKey(element.Name));
            }
            else
            {
                ctrl.SetValue(ElementModelItemPropertyKey, null);
                ctrl.SetValue(ElementTemplateResourceKeyPropertyKey, null);
            }
        }

        static readonly DependencyPropertyKey ElementModelItemPropertyKey = DependencyProperty.RegisterReadOnly(
            "ElementModelItem",
            typeof(ElementModelItem),
            typeof(ElementPresenter),
            new FrameworkPropertyMetadata());

        public static readonly DependencyProperty ElementModelItemProperty = ElementModelItemPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets the <see cref="ElementModelItem"/> that will be set as the DataContext of the template.
        /// </summary>
        public ElementModelItem ElementModelItem
        {
            get { return (ElementModelItem)GetValue(ElementModelItemProperty); }
        }

        static readonly DependencyPropertyKey ElementTemplateResourceKeyPropertyKey = DependencyProperty.RegisterReadOnly(
            "ElementTemplateResourceKey",
            typeof(ElementTemplateResourceKey),
            typeof(ElementPresenter),
            new FrameworkPropertyMetadata());

        public static readonly DependencyProperty ElementTemplateResourceKeyProperty = ElementTemplateResourceKeyPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets the <see cref="ElementTemplateResourceKey"/> that can be used to resolve the presented <see cref="XElement"/>'s template.
        /// </summary>
        public ElementTemplateResourceKey ElementTemplateResourceKey
        {
            get { return (ElementTemplateResourceKey)GetValue(ElementTemplateResourceKeyProperty); }
        }

    }

}
