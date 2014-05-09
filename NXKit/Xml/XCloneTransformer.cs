using System.Xml.Linq;

namespace NXKit.Xml
{

    public class XCloneTransformer :
        XTransformer
    {

        public readonly static XCloneTransformer Default = new XCloneTransformer();

        public override XAttribute Visit(XAttribute attribute)
        {
            return new XAttribute(attribute);
        }

        public override XComment Visit(XComment comment)
        {
            return new XComment(comment);
        }

        public override XDeclaration Visit(XDeclaration declaration)
        {
            return new XDeclaration(declaration);
        }

        public override XDocument Visit(XDocument document)
        {
            return new XDocument(document);
        }

        public override XElement Visit(XElement element)
        {
            return new XElement(element);
        }

        public override XProcessingInstruction Visit(XProcessingInstruction processingInstruction)
        {
            return new XProcessingInstruction(processingInstruction);
        }

        public override XText Visit(XText text)
        {
            return new XText(text);
        }

        public override XCData Visit(XCData cdata)
        {
            return new XCData(cdata);
        }

    }

}
