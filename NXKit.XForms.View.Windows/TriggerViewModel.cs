using System;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Windows.Input;
using System.Xml.Linq;

using NXKit.DOMEvents;
using NXKit.View.Windows;

namespace NXKit.XForms.View.Windows
{

    public class TriggerViewModel :
        ElementViewModel
    {

        readonly ICommand domActivateCommand;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public TriggerViewModel(XElement element)
            : base(element)
        {
            Contract.Requires<ArgumentNullException>(element != null);

            domActivateCommand = new DelegateCommand(i => DOMActivate_Execute());
        }

        public XElement Label
        {
            get { return GetLabel(); }
        }

        XElement GetLabel()
        {
            return Element.Elements("{http://www.w3.org/2002/xforms}label").FirstOrDefault();
        }

        public ICommand DOMActivateCommand
        {
            get { return domActivateCommand; }
        }

        void DOMActivate_Execute()
        {
            Element.DispatchEvent(UIEvents.DOMActivate);
        }

    }

}
