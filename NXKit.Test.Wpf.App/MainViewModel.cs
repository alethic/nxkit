using System;

namespace NXKit.Test.Wpf.App
{

    public class MainViewModel
    {

        public MainViewModel()
        {
            FormUri = new Uri("pack://application:,,,/NXKit.Test.Wpf.App;component/Form.xaml");
        }

        public Uri FormUri { get; set; }

    }

}
