using System;
using System.Xml.Linq;

namespace NXKit.XForms
{

    /// <summary>
    /// Provides the XForms 'submission' attributes.
    /// </summary>
    [Extension(typeof(SubmissionAttributes), "{http://www.w3.org/2002/xforms}submission")]
    public class SubmissionAttributes :
        CommonAttributes
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public SubmissionAttributes(XElement element)
            : base(element)
        {
            if (element == null)
                throw new ArgumentNullException(nameof(element));
        }

        /// <summary>
        /// Gets the 'ref' attribute value.
        /// </summary>
        public string Ref
        {
            get { return GetAttributeValue("ref"); }
        }


        /// <summary>
        /// Gets the 'relevant' attribute value.
        /// </summary>
        public string Relevant
        {
            get { return GetAttributeValue("relevant"); }
        }

        /// <summary>
        /// Gets the 'validate' attribute value.
        /// </summary>
        public string Validate
        {
            get { return GetAttributeValue("validate"); }
        }

        /// <summary>
        /// Gets the 'resource' attribute value.
        /// </summary>
        public string Resource
        {
            get { return GetAttributeValue("resource"); }
        }

        /// <summary>
        /// Gets the 'action' attribute value.
        /// </summary>
        public string Action
        {
            get { return GetAttributeValue("action"); }
        }

        /// <summary>
        /// Gets the 'mode' attribute value.
        /// </summary>
        public string Mode
        {
            get { return GetAttributeValue("mode"); }
        }

        /// <summary>
        /// Gets the 'Method' attribute value.
        /// </summary>
        public string Method
        {
            get { return GetAttributeValue("method"); }
        }

        /// <summary>
        /// Gets the 'serialization' attribute value.
        /// </summary>
        public string Serialization
        {
            get { return GetAttributeValue("serialization"); }
        }

        /// <summary>
        /// Gets the 'mediatype' attribute value.
        /// </summary>
        public string MediaType
        {
            get { return GetAttributeValue("mediatype"); }
        }

        /// <summary>
        /// Gets the 'encoding' attribute value.
        /// </summary>
        public string Encoding
        {
            get { return GetAttributeValue("encoding"); }
        }

        /// <summary>
        /// Gets the 'replace' attribute value.
        /// </summary>
        public string Replace
        {
            get { return GetAttributeValue("replace"); }
        }

        /// <summary>
        /// Gets the 'instance' attribute value.
        /// </summary>
        public string Instance
        {
            get { return GetAttributeValue("instance"); }
        }

        /// <summary>
        /// Gets the 'targetref' attribute value.
        /// </summary>
        public string TargetRef
        {
            get { return GetAttributeValue("targetref"); }
        }

        /// <summary>
        /// Gets the 'separator' attribute value.
        /// </summary>
        public string Separator
        {
            get { return GetAttributeValue("separator"); }
        }

        /// <summary>
        /// Gets the 'version' attribute value.
        /// </summary>
        public string Version
        {
            get { return GetAttributeValue("version"); }
        }

        /// <summary>
        /// Gets the 'indent' attribute value.
        /// </summary>
        public string Indent
        {
            get { return GetAttributeValue("indent"); }
        }

        /// <summary>
        /// Gets the 'omit-xml-declaration' attribute value.
        /// </summary>
        public string OmitXmlDeclaration
        {
            get { return GetAttributeValue("omit-xml-declaration"); }
        }

        /// <summary>
        /// Gets the 'standalone' attribute value.
        /// </summary>
        public string Standalone
        {
            get { return GetAttributeValue("standalone"); }
        }

        /// <summary>
        /// Gets the 'cdata-section-elements' attribute value.
        /// </summary>
        public string CDataSectionElements
        {
            get { return GetAttributeValue("cdata-section-elements"); }
        }

        /// <summary>
        /// Gets the 'includenamespaceprefixes' attribute value.
        /// </summary>
        public string IncludeNamespacePrefixes
        {
            get { return GetAttributeValue("includenamespaceprefixes"); }
        }

    }

}