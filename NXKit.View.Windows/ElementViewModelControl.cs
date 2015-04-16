using System;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Linq;

using NXKit.View.Windows;

namespace NXKit.View.Windows
{

    /// <summary>
    /// Creates a dynamic instance of the specified <see cref="ElementViewModel"/> for the given <see cref="XElement"/>.
    /// </summary>
    public class ElementViewModelControl :
        Control
    {

        /// <summary>
        /// Initializes the static instance.
        /// </summary>
        static ElementViewModelControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ElementViewModelControl), new FrameworkPropertyMetadata(typeof(ElementViewModelControl)));
        }

        XElement element;
        Type elementViewModelType;

        /// <summary>
        /// Identifies the Element dependency property.
        /// </summary>
        public static readonly DependencyProperty ElementProperty = DependencyProperty.Register(
            "Element",
            typeof(XElement),
            typeof(ElementViewModelControl),
            new FrameworkPropertyMetadata(null, OnElementPropertyChanged));

        /// <summary>
        /// Gets or sets the <see cref="XElement"/> being rendered.
        /// </summary>
        public XElement Element
        {
            get { return (XElement)GetValue(ElementProperty); }
            set { SetValue(ElementProperty, value); }
        }

        static void OnElementPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs args)
        {
            ((ElementViewModelControl)d).RefreshViewModel();
        }

        /// <summary>
        /// Identifies the ElementViewModelType dependency property.
        /// </summary>
        public static readonly DependencyProperty ElementViewModelTypeProperty = DependencyProperty.Register(
            "ElementViewModelType",
            typeof(Type),
            typeof(ElementViewModelControl),
            new FrameworkPropertyMetadata(null, OnElementViewModelTypePropertyChanged));

        /// <summary>
        /// Gets or sets the <see cref="Type"/> of <see cref="ElementViewModel"/> to create.
        /// </summary>
        public Type ElementViewModelType
        {
            get { return (Type)GetValue(ElementViewModelTypeProperty); }
            set { SetValue(ElementViewModelTypeProperty, value); }
        }

        static void OnElementViewModelTypePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs args)
        {
            ((ElementViewModelControl)d).RefreshViewModel();
        }

        /// <summary>
        /// Key to the ElementViewModel dependency property.
        /// </summary>
        static readonly DependencyPropertyKey ElementViewModelPropertyKey = DependencyProperty.RegisterReadOnly(
            "ElementViewModel",
            typeof(ElementViewModel),
            typeof(ElementViewModelControl),
            new FrameworkPropertyMetadata());

        /// <summary>
        /// Identifies the ElementViewModel dependency property.
        /// </summary>
        public static DependencyProperty ElementViewModelProperty = ElementViewModelPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets the generated <see cref="ElementViewModel"/>.
        /// </summary>
        public ElementViewModel ElementViewModel
        {
            get { return (ElementViewModel)GetValue(ElementViewModelProperty); }
            set { SetValue(ElementViewModelPropertyKey, value); }
        }

        /// <summary>
        /// Refreshes the instance of the configured <see cref="ElementViewModel"/> <see cref="Type"/>.
        /// </summary>
        void RefreshViewModel()
        {
            if (Element != element ||
                ElementViewModelType != elementViewModelType)
                CreateViewModel();

            // store away old values
            element = Element;
            elementViewModelType = ElementViewModelType;
        }

        /// <summary>
        /// Creates a new instance of the configured <see cref="ElementViewModel"/> <see cref="Type/>.
        /// </summary>
        void CreateViewModel()
        {
            if (Element != null &&
                ElementViewModelType != null)
                ElementViewModel = (ElementViewModel)Activator.CreateInstance(ElementViewModelType, new[] { Element });
            else
                ElementViewModel = null;
        }

    }

}
