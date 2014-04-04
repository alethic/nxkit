using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NXKit.XForms
{

    /// <summary>
    /// Describes an element that returns a binding.
    /// </summary>
    public interface IBinding
    {

        /// <summary>
        /// Gets the binding defined by the element.
        /// </summary>
        Binding Binding { get; }

    }

}
