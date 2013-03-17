using System.Xml.Linq;


namespace NXKit
{

    public class TextVisual : Visual
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="form"></param>
        /// <param name="parent"></param>
        /// <param name="node"></param>
        /// <param name="text"></param>
        public TextVisual(IEngine form, StructuralVisual parent, XText node)
            : base(form, parent, node)
        {

        }

        public string Text
        {
            get { return ((XText)Node).Value; }
        }

    }

}
