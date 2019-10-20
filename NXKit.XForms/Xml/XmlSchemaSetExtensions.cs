using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;

namespace NXKit.XForms.Xml
{

    public static class XmlSchemaSetExtensions
    {

        public static XmlSchemaType GetSchemaType(this XmlSchemaSet schemaSet, XName name)
        {
            return (XmlSchemaType)schemaSet.GlobalTypes[new XmlQualifiedName(name.LocalName, name.NamespaceName)];
        }

    }

}
