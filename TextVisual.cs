using System.IO;
using System.Xml.Linq;

namespace NXKit
{

    public class TextVisual : Visual, ITextVisual
    {

        public string Text
        {
            get { return ((XText)Node).Value; }
        }

        public void WriteText(TextWriter w)
        {
            w.Write(Text);
        }

    }

}
