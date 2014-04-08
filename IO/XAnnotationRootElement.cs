using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Xml.Linq;

namespace NXKit.IO
{

    /// <summary>
    /// Wraps an existing <see cref="XElement"/> modifying the body contents to include persisted annotation data.
    /// </summary>
    public class XAnnotationRootElement :
        XAnnotationElement
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public XAnnotationRootElement(XElement element)
            : base(element)
        {
            Contract.Requires<ArgumentNullException>(element != null);
        }

        protected override IEnumerable<object> GetSaveContents()
        {
            foreach (var i in base.GetAnnotations(Source.Document))
                yield return i;

            foreach (var i in base.GetSaveContents())
                yield return i;
        }

    }

}
