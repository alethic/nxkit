using System.Xml.Linq;

namespace NXKit.XForms
{

    /// <summary>
    /// Provides access to the bound model item.
    /// </summary>
    [Remote]
    public interface IModelItemBinding :
        IModelItemValue
    {

        [Remote]
        XName DataType { get; }

        [Remote]
        bool Relevant { get; }

        [Remote]
        bool ReadOnly { get; }

        [Remote]
        bool Required { get; }

        [Remote]
        bool Valid { get; }

    }

}
