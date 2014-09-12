using System;
using System.ComponentModel.Composition;
using System.Xml.Linq;

using NXKit.Composition;

namespace NXKit.XForms.XmlSchema
{

    [Export(typeof(IXsdTypeConverter))]
    [PartMetadata(ScopeCatalog.ScopeMetadataKey, Scope.Host)]
    public class BooleanXsdTypeConverter :
        IXsdTypeConverter
    {

        static readonly XName xsdName1 = "{http://www.w3.org/2001/XMLSchema}boolean";
        static readonly XName xsdName2 = "{http://www.w3.org/2001/XMLSchema-datatypes}boolean";

        public bool CanConvertTo(XName xsdType)
        {
            return xsdType == xsdName1 || xsdType == xsdName2;
        }

        public string ConvertTo(XName xsdType, string value)
        {
            bool boolean;
            if (bool.TryParse(value, out boolean))
                return boolean ? "true" : "false";
            else if (value == "0")
                return "false";
            else if (value == "1")
                return "true";
            else
                return null;
        }

    }

}
