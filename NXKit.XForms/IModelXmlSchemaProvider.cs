using System.Collections.Generic;
using System.Xml.Schema;

namespace NXKit.XForms
{

    /// <summary>
    /// Provides a default set of schemas that should be made available within a model.
    /// </summary>
    public interface IModelXmlSchemaProvider
    {

        /// <summary>
        /// Provides a default set of schemas that should be made available within a model.
        /// </summary>
        /// <returns></returns>
        IEnumerable<XmlSchema> GetSchemas(Model model);

    }

}
