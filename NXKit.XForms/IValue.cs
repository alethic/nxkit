using System.Xml.Linq;

namespace NXKit.XForms
{

    /// <summary>
    /// Provides a single value.
    /// </summary>
    [Public]
    public interface IValue
    {

        [Public]
        object Value { get; }

    }

}
