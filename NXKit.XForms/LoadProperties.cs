using System;
using System.ComponentModel.Composition;
using System.Diagnostics.Contracts;
using System.Xml.Linq;

using NXKit.Composition;
using NXKit.Xml;

namespace NXKit.XForms
{

    /// <summary>
    /// Provides the XForms 'load' properties.
    /// </summary>
    [Extension("{http://www.w3.org/2002/xforms}load")]
    [PartMetadata(ScopeCatalog.ScopeMetadataKey, Scope.Object)]
    public class LoadProperties :
        ElementExtension
    {

        readonly LoadAttributes attributes;
        readonly Lazy<EvaluationContextResolver> context;
        readonly Lazy<LoadShow> show;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="context"></param>
        public LoadProperties(
            XElement element,
            Lazy<EvaluationContextResolver> context)
            : base(element)
        {
            Contract.Requires<ArgumentNullException>(element != null);
            Contract.Requires<ArgumentNullException>(context != null);

            this.attributes = element.AnnotationOrCreate<LoadAttributes>(() => new LoadAttributes(element));
            this.context = context;

            this.show = new Lazy<LoadShow>(() =>
                !string.IsNullOrEmpty(attributes.Show) ? (LoadShow)Enum.Parse(typeof(LoadShow), attributes.Show, true) : LoadShow.Replace);
        }

        public string Resource
        {
            get { return attributes.Resource; }
        }

        public LoadShow Show
        {
            get { return show.Value; }
        }

        public IdRef TargetId
        {
            get { return attributes.TargetId; }
        }

    }

}