using System.Xml.Linq;

namespace NXKit.XForms.XmlSchema
{

    /// <summary>
    /// Provides methods to convert a .NET object into a given schema type.
    /// </summary>
    public interface IXsdTypeConverter
    {

        /// <summary>
        /// Returns <c>true</c> if the converter can convert into the given XSD type.
        /// </summary>
        /// <param name="xsdType"></param>
        /// <returns></returns>
        bool CanConvertTo(XName xsdType);

        /// <summary>
        /// Converts the given string into the given XSD type.
        /// </summary>
        /// <param name="xsdType"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        string ConvertTo(XName xsdType, string value);

    }

}
