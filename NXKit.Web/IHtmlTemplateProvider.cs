using System.Collections.Generic;

namespace NXKit.Web
{

    /// <summary>
    /// Provides a series of 
    /// </summary>
    public interface IHtmlTemplateProvider
    {

        /// <summary>
        /// Gets a series of HTML templates provided.
        /// </summary>
        /// <returns></returns>
        IEnumerable<HtmlTemplateInfo> GetTemplates();

    }

}
