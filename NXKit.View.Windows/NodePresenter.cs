using System;
using System.Diagnostics.Contracts;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Linq;

namespace NXKit.View.Windows
{

    public class NodePresenter :
        Control
    {

        /// <summary>
        /// Initializes the static instance.
        /// </summary>
        static NodePresenter()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NodePresenter), new FrameworkPropertyMetadata(typeof(NodePresenter)));
        }

        /// <summary>
        /// Registers a dependency property as backing store for the Node property.
        /// </summary>
        public static readonly DependencyProperty NodeProperty = DependencyProperty.Register(
            "Node",
            typeof(XNode),
            typeof(NodePresenter),
            new FrameworkPropertyMetadata(Node_PropertyChanged));

        /// <summary>
        /// Gets or sets the <see cref="XNode"/> to be presented.
        /// </summary>
        public XNode Node
        {
            get { return (XNode)GetValue(NodeProperty); }
            set { SetValue(NodeProperty, value); }
        }

        /// <summary>
        /// Invoked when the value of Element is changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        static void Node_PropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            Contract.Requires<ArgumentNullException>(sender != null);
            Contract.Requires<ArgumentNullException>(args != null);

            var ctrl = (NodePresenter)sender;
            var node = ctrl.Node;
            if (node is XText)
                ctrl.SetValue(NodeTemplateResourceKeyPropertyKey, "NXKit:Text");
            else if (node is XElement)
                ctrl.SetValue(NodeTemplateResourceKeyPropertyKey, "NXKit:Element");
            else
                ctrl.SetValue(NodeTemplateResourceKeyPropertyKey, null);
        }

        static readonly DependencyPropertyKey NodeTemplateResourceKeyPropertyKey = DependencyProperty.RegisterReadOnly(
            "NodeTemplateResourceKey",
            typeof(string),
            typeof(NodePresenter),
            new FrameworkPropertyMetadata());

        public static readonly DependencyProperty NodeTemplateResourceKeyProperty = NodeTemplateResourceKeyPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets the <see cref="NodeTemplateResourceKey"/> that can be used to resolve the presented <see cref="XNode"/>'s template.
        /// </summary>
        public string NodeTemplateResourceKey
        {
            get { return (string)GetValue(NodeTemplateResourceKeyProperty); }
        }

    }

}
