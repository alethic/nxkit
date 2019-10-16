using System;
using System.Xml.Linq;

using NXKit.Composition;

namespace NXKit.XForms.Converters
{

    [Export(typeof(IXsdTypeConverter), CompositionScope.Host)]
    public class DateTimeXsdTypeConverter :
        IXsdTypeConverter
    {

        static readonly XName xsdName1 = "{http://www.w3.org/2001/XMLSchema}dateTime";
        static readonly XName xsdName2 = "{http://www.w3.org/2001/XMLSchema-datatypes}dateTime";

        public bool CanConvertTo(XName xsdType)
        {
            return xsdType == xsdName1 || xsdType == xsdName2;
        }

        public string ConvertTo(XName xsdType, string value)
        {
            DateTimeOffset dateTime;
            if (DateTimeOffset.TryParse(value, out dateTime))
                if (dateTime.Offset == TimeSpan.Zero)
                    return dateTime.ToString("yyyy-MM-ddThh:mm:ssZ");
                else
                    return dateTime.ToString("yyyy-MM-ddThh:mm:sszzz");
            else
                return null;
        }

    }

}
