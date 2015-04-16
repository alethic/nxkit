using System.Windows;
using System.Windows.Controls;

namespace NXKit.View.Windows
{

    public class NodeContainer :
        ContentControl
    {

        /// <summary>
        /// Initializes the static instance.
        /// </summary>
        static NodeContainer()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NodeContainer), new FrameworkPropertyMetadata(typeof(NodeContainer)));
        }

    }

}
