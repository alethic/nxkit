using System;
using System.Xml.Linq;

namespace NXKit.XForms
{

    /// <summary>
    /// Serializable storage for a <see cref="UIBinding"/>'s state.
    /// </summary>
    [Serializable]
    public class UIBindingState
    {

        XName itemType;
        bool relevant;
        bool readOnly;
        bool required;
        bool valid;
        string value;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public UIBindingState()
        {

        }

        public XName ItemType
        {
            get { return itemType; }
            set { itemType = value; }
        }

        public bool Relevant
        {
            get { return relevant; }
            set { relevant = value; }
        }

        public bool ReadOnly
        {
            get { return readOnly; }
            set { readOnly = value; }
        }

        public bool Required
        {
            get { return required; }
            set { required = value; }
        }

        public bool Valid
        {
            get { return valid; }
            set { valid = value; }
        }

        public string Value
        {
            get { return value; }
            set { this.value = value; }
        }

    }

}
