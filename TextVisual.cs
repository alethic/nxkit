using System.IO;
using System.Xml.Linq;

namespace NXKit
{

    /// <summary>
    /// Represents a simple text node.
    /// </summary>
    public class TextVisual : 
        Visual, 
        ITextVisual
    {

        [Interactive]
        public string Text
        {
            get { return ((XText)Node).Value; }
        }

        public void WriteText(TextWriter writer)
        {
            writer.Write(Text);
        }

    }

}
