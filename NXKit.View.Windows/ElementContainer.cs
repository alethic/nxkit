using System.Windows;
using System.Windows.Controls;

namespace NXKit.View.Windows
{

    public class ElementContainer :
        ContentControl
    {

        /// <summary>
        /// Initializes the static instance.
        /// </summary>
        static ElementContainer()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ElementContainer), new FrameworkPropertyMetadata(typeof(ElementContainer)));
        }

    }

}
