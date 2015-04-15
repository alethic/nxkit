using System;
using System.ComponentModel;
using System.Windows.Input;

namespace NXKit.View.Windows.Test.App
{

    public class MainWindowViewModel :
        INotifyPropertyChanged
    {

        Document document;
        string loadUri;
        ICommand loadCommand;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public MainWindowViewModel()
        {
            this.loadCommand = new DelegateCommand(i => ExecuteLoad(i), i => CanExecuteLoad(i));
            this.loadUri = "nx-example:///form.xml";
        }

        public string LoadUri
        {
            get { return loadUri; }
            set { loadUri = value; RaisePropertyChanged("LoadUri"); }
        }

        public ICommand LoadCommand
        {
            get { return loadCommand; }
        }

        bool CanExecuteLoad(object parameter)
        {
            return !string.IsNullOrWhiteSpace(LoadUri);
        }

        void ExecuteLoad(object parameter)
        {
            Document = Document.Load(new Uri(LoadUri));
        }

        public Document Document
        {
            get { return document; }
            set { document = value; RaisePropertyChanged("Document"); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, args);
        }

        void RaisePropertyChanged(string propertyName)
        {
            OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }

    }

}
