using System;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Xml.Linq;

using NXKit.View.Windows;
using NXKit.Xml;

namespace NXKit.XForms.View.Windows
{

    public class InputViewModel :
        ElementViewModel
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public InputViewModel(XElement element)
            : base(element)
        {
            Contract.Requires<ArgumentNullException>(element != null);
        }

        public XElement Label
        {
            get { return GetLabel(); }
        }

        XElement GetLabel()
        {
            return Element.Elements("{http://www.w3.org/2002/xforms}label").FirstOrDefault();
        }

        public string Value
        {
            get { return GetValue(); }
            set { SetValue(value); }
        }

        string GetValue()
        {
            return Element.Interface<IDataNode>().Value;
        }

        void SetValue(string value)
        {
            Element.Interface<IDataNode>().Value = value;
            RaisePropertyChanged("Valid");
        }

        public bool Valid
        {
            get { return GetValid(); }
        }

        bool GetValid()
        {
            return Element.Interface<IUINode>().Valid;
        }

    }

}
