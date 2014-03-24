using System.IO;
using System.Xml.Linq;

namespace NXKit
{

    /// <summary>
    /// Represents a simple text node.
    /// </summary>
    public class NXText :
        NXNode
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public NXText()
            : base()
        {

        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="text"></param>
        public NXText(XText text)
            : base(text)
        {

        }

        public XText Xml
        {
            get { return (XText)Xml; }
        }

        [Interactive]
        public string Text
        {
            get { return Xml.Value; }
        }

        public void WriteText(TextWriter writer)
        {
            writer.Write(Text);
        }

    }

}
