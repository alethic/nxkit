using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;

namespace NXKit.Web
{

    [Export(typeof(IHtmlTemplateProvider))]
    public class AssemblyHtmlTemplateProvider :
        IHtmlTemplateProvider
    {

        /// <summary>
        /// Gets the templates for the given <see cref="Assembly"/>.
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        static IEnumerable<HtmlTemplateInfo> GetTemplates(Assembly assembly)
        {
            Contract.Requires<ArgumentNullException>(assembly != null);

            return assembly.GetManifestResourceNames()
                .Where(j => j.EndsWith(".html"))
                .Select(j => new HtmlTemplateInfo(j, () => assembly.GetManifestResourceStream(j)))
                .ToList();
        }

        readonly Assembly assembly;
        readonly IEnumerable<HtmlTemplateInfo> templates;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="assembly"></param>
        public AssemblyHtmlTemplateProvider(Assembly assembly)
        {
            Contract.Requires<ArgumentNullException>(assembly != null);

            this.assembly = assembly;
            this.templates = GetTemplates(this.assembly);
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        [ImportingConstructor]
        public AssemblyHtmlTemplateProvider()
            : this(typeof(AssemblyHtmlTemplateProvider).Assembly)
        {
            this.assembly = GetType().Assembly;
            this.templates = GetTemplates(this.assembly);
        }

        public IEnumerable<HtmlTemplateInfo> GetTemplates()
        {
            return templates;
        }

    }

}
