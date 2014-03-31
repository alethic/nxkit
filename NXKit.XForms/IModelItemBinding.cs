using System.Xml.Linq;

namespace NXKit.XForms
{

    /// <summary>
    /// Provides access to the bound model item.
    /// </summary>
    [Public]
    public interface IModelItemBinding :
        IModelItemValue
    {

        [Public]
        XName DataType { get; }

        [Public]
        bool Relevant { get; }

        [Public]
        bool ReadOnly { get; }

        [Public]
        bool Required { get; }

        [Public]
        bool Valid { get; }

    }

}
