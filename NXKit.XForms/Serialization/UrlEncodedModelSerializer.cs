using System;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Xml.Linq;

using NXKit.IO.Media;

namespace NXKit.XForms.Serialization
{

    [Export(typeof(IModelSerializer))]
    public class UrlEncodedModelSerializer :
        IModelSerializer
    {

        static readonly MediaRange[] ACCEPT = new MediaRange[]
        {
            "application/x-www-form-urlencoded",
        };

        public Priority CanSerialize(XNode node, MediaRange mediaType)
        {
            return ACCEPT.Any(i => i.Matches(mediaType)) && (node is XDocument || node is XElement) ? Priority.Default : Priority.Ignore;
        }

        public void Serialize(TextWriter writer, XNode node, MediaRange mediaType)
        {
            var element = node is XDocument ? ((XDocument)node).Root : node as XElement;
            if (element == null)
                throw new InvalidOperationException();

            // Each element node is visited in document order, except non-relevant elements are skipped if the relevant
            // setting of the submission is true. Each visited element that has no child element nodes (i.e., each leaf
            // element node) is selected for inclusion, including those that have no value (no text node). Note that
            // attribute information is not preserved.

            var separ = false;
            var items = element
                .DescendantsAndSelf()
                .Where(i => !i.HasElements);

            foreach (var item in items)
            {
                // write separator on subsequent nodes
                if (separ)
                    writer.Write(';');

                // encode values
                writer.Write(Encode(item.Name.LocalName));
                writer.Write('=');
                writer.Write(Encode(item.Value));
                separ = true;
            }
        }

        string Encode(string value)
        {
            return Uri.EscapeDataString(value);
        }

    }

}
