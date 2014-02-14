using System.Windows;
using System.Windows.Controls;

namespace NXKit.Wpf.UI
{

    public class View :
        Control
    {

        static View()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(View), new FrameworkPropertyMetadata(typeof(View)));
        }

    }

}
