using System;
using System.Xml.Linq;

using NXKit.Composition;

namespace NXKit.XForms
{

    /// <summary>
    /// Provides the XForms 'load' properties.
    /// </summary>
    [Extension(typeof(LoadProperties), "{http://www.w3.org/2002/xforms}load")]
    public class LoadProperties :
        ElementExtension
    {

        readonly LoadAttributes attributes;
        readonly IExport<EvaluationContextResolver> context;
        readonly Lazy<LoadShow> show;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="attributes"></param>
        /// <param name="context"></param>
        public LoadProperties(
            XElement element,
            LoadAttributes attributes,
            IExport<EvaluationContextResolver> context)
            : base(element)
        {
            if (element == null)
                throw new ArgumentNullException(nameof(element));

            this.attributes = attributes ?? throw new ArgumentNullException(nameof(attributes));
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.show = new Lazy<LoadShow>(() => !string.IsNullOrEmpty(attributes.Show) ? (LoadShow)Enum.Parse(typeof(LoadShow), attributes.Show, true) : LoadShow.Replace);
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