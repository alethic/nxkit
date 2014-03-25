using System.Xml.Linq;

namespace NXKit.XForms
{

    [Element("instance")]
    public class InstanceElement :
        XFormsElement
    {

        InstanceElementState state;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="xml"></param>
        public InstanceElement(XElement xml)
            : base(xml)
        {

        }

        /// <summary>
        /// Gets the instance state associated with this instance visual.
        /// </summary>
        public InstanceElementState State
        {
            get { return state ?? (state = GetState<InstanceElementState>()); }
        }

        protected override void CreateNodes()
        {

        }

    }

}
