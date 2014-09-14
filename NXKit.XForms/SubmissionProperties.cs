using System;
using System.ComponentModel.Composition;
using System.Diagnostics.Contracts;
using System.Text;
using System.Xml.Linq;

using NXKit.Composition;
using NXKit.Util;
using NXKit.XForms.IO;
using NXKit.Xml;

namespace NXKit.XForms
{

    /// <summary>
    /// Provides the XForms 'submission' properties.
    /// </summary>
    [Extension("{http://www.w3.org/2002/xforms}submission")]
    [PartMetadata(ScopeCatalog.ScopeMetadataKey, Scope.Object)]
    public class SubmissionProperties :
        ElementExtension
    {

        readonly Extension<SubmissionAttributes> attributes;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="attributes"></param>
        [ImportingConstructor]
        public SubmissionProperties(
            XElement element,
            Extension<SubmissionAttributes> attributes)
            : base(element)
        {
            Contract.Requires<ArgumentNullException>(element != null);
            Contract.Requires<ArgumentNullException>(attributes != null);

            this.attributes = attributes;
        }

        public SubmissionAttributes Attributes
        {
            get { return Attributes; }
        }

        /// <summary>
        /// Gets the 'ref' attribute value.
        /// </summary>
        public string Ref
        {
            get { return Attributes.Ref ?? "/"; }
        }

        /// <summary>
        /// Gets the 'relevant' attribute value.
        /// </summary>
        public bool Relevant
        {
            get { return Attributes.Relevant != null ? bool.Parse(Attributes.Relevant) : (Attributes.Serialization == "none" ? false : true); }
        }

        /// <summary>
        /// Gets the 'validate' attribute value.
        /// </summary>
        public bool Validate
        {
            get { return Attributes.Validate != null ? bool.Parse(Attributes.Validate) : (Attributes.Serialization == "none" ? false : true); }
        }

        /// <summary>
        /// Gets the 'resource' URI value.
        /// </summary>
        public Uri Resource
        {
            get { return GetResource(); }
        }

        Uri GetResource()
        {
            var uri = Attributes.Resource != null ? new Uri(Attributes.Resource, UriKind.RelativeOrAbsolute) : null;
            if (uri != null)
                return uri.IsAbsoluteUri ? uri : new Uri(Element.GetBaseUri(), uri);

            return null;
        }

        /// <summary>
        /// Gets the 'action' URI value.
        /// </summary>
        public Uri Action
        {
            get { return GetAction(); }
        }

        Uri GetAction()
        {
            var uri = Attributes.Action != null ? new Uri(Attributes.Action, UriKind.RelativeOrAbsolute) : null;
            if (uri != null)
                return uri.IsAbsoluteUri ? uri : new Uri(Element.GetBaseUri(), uri);

            return null;
        }

        /// <summary>
        /// Gets the 'mode' attribute value.
        /// </summary>
        public SubmissionMode Mode
        {
            get { return Attributes.Mode == "synchronous" ? SubmissionMode.Synchronous : SubmissionMode.Asynchronous; }
        }

        /// <summary>
        /// Gets the 'method' attribute value.
        /// </summary>
        public ModelMethod Method
        {
            get { return Attributes.Method; }
        }

        /// <summary>
        /// Gets the 'serialization' attribute value.
        /// </summary>
        public SubmissionSerialization Serialization
        {
            get { return GetSerialization(); }
        }

        SubmissionSerialization GetSerialization()
        {
            if (Attributes.Serialization == "none")
                return new SubmissionSerialization(true);
            else if (!string.IsNullOrEmpty(Attributes.Serialization))
                return new SubmissionSerialization(Attributes.Serialization);
            else
                return new SubmissionSerialization(false);
        }

        /// <summary>
        /// Gets the 'mediatype' attribute value.
        /// </summary>
        public MediaRange MediaType
        {
            get { return Attributes.MediaType != "none" ? Attributes.MediaType : null; }
        }

        /// <summary>
        /// Gets the 'encoding' attribute value.
        /// </summary>
        public Encoding Encoding
        {
            get { return Encoding.GetEncoding(Attributes.Encoding ?? "UTF-8"); }
        }

        /// <summary>
        /// Gets the 'replace' attribute value.
        /// </summary>
        public SubmissionReplace Replace
        {
            get { return GetReplace(); }
        }

        SubmissionReplace GetReplace()
        {
            switch (Attributes.Replace)
            {
                case "none":
                    return SubmissionReplace.None;
                case "instance":
                    return SubmissionReplace.Instance;
                case "text":
                    return SubmissionReplace.Text;
                case "all":
                case "":
                case null:
                    return SubmissionReplace.All;
            }

            throw new FormatException();
        }

        /// <summary>
        /// Gets the 'instance' attribute value.
        /// </summary>
        public IdRef Instance
        {
            get { return Attributes.Instance; }
        }

        /// <summary>
        /// Gets the 'targetref' attribute value.
        /// </summary>
        public string TargetRef
        {
            get { return Attributes.TargetRef; }
        }

        /// <summary>
        /// Gets the 'separator' attribute value.
        /// </summary>
        public char Separator
        {
            get { return GetSeparator(); }
        }

        char GetSeparator()
        {
            var separator = Attributes.Separator.Trim();
            if (string.IsNullOrEmpty(separator))
                return '&';
            else
                return separator[0];
        }

        /// <summary>
        /// Gets the 'version' attribute value.
        /// </summary>
        public string Version
        {
            get { return Attributes.Version ?? "1.0"; }
        }

        /// <summary>
        /// Gets the 'indent' attribute value.
        /// </summary>
        public bool Indent
        {
            get { return Attributes.Indent == "true" ? true : false; }
        }

        /// <summary>
        /// Gets the 'omit-xml-declaration' attribute value.
        /// </summary>
        public bool OmitXmlDeclaration
        {
            get { return Attributes.OmitXmlDeclaration == "true" ? true : false; }
        }

        /// <summary>
        /// Gets the 'standalone' attribute value.
        /// </summary>
        public bool Standalone
        {
            get { return OmitXmlDeclaration ? false : (Attributes.Standalone == "true" ? true : false); }
        }

        /// <summary>
        /// Gets the 'cdata-section-elements' attribute value.
        /// </summary>
        public string CDataSectionElements
        {
            get { return Attributes.CDataSectionElements ?? ""; }
        }

        /// <summary>
        /// Gets the 'includenamespaceprefixes' attribute value.
        /// </summary>
        public string IncludeNamespacePrefixes
        {
            get { return Attributes.IncludeNamespacePrefixes ?? ""; }
        }

    }

}