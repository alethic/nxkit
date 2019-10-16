using System;
using System.Xml.Linq;

using NXKit.Composition;

namespace NXKit.XForms.Converters
{

    [Export(typeof(IXsdTypeConverter), CompositionScope.Host)]
    public class DateXsdTypeConverter :
        IXsdTypeConverter
    {

        static readonly XName xsdName1 = "{http://www.w3.org/2001/XMLSchema}date";
        static readonly XName xsdName2 = "{http://www.w3.org/2001/XMLSchema-datatypes}date";

        public bool CanConvertTo(XName xsdType)
        {
            return xsdType == xsdName1 || xsdType == xsdName2;
        }

        public string ConvertTo(XName xsdType, string value)
        {
            DateTimeOffset dateTime;
            if (DateTimeOffset.TryParse(value, out dateTime))
                return dateTime.ToString("yyyy-MM-dd");
            else
                return null;
        }

    }

}
