using System.Xml.Linq;

namespace NXKit.XForms
{

    [Interface("{http://www.w3.org/2002/xforms}range")]
    [Remote]
    public class Range 
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public Range(XElement element)
        {

        }

        //[Public]
        //public string Start
        //{
        //    get { return Module.GetAttributeValue(Xml, "start"); }
        //}

        //[Public]
        //public string End
        //{
        //    get { return Module.GetAttributeValue(Xml, "end"); }
        //}

        //[Public]
        //public string Step
        //{
        //    get { return Module.GetAttributeValue(Xml, "step"); }
        //}

    }

}
