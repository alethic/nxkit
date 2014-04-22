using System;
using System.Diagnostics.Contracts;
using System.Text;
using System.Xml.Linq;

using NXKit.Util;
using NXKit.XForms.IO;
using NXKit.Xml;

namespace NXKit.XForms
{

    /// <summary>
    /// Provides the XForms 'submission' properties.
    /// </summary>
    public class SubmissionProperties :
        ElementExtension
    {

        readonly SubmissionAttributes attributes;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public SubmissionProperties(XElement element)
            : base(element)
        {
            Contract.Requires<ArgumentNullException>(element != null);

            this.attributes = new SubmissionAttributes(element);
        }

        /// <summary>
        /// Gets the 'ref' attribute value.
        /// </summary>
        public string Ref
        {
            get { return attributes.Ref ?? "/"; }
        }

        /// <summary>
        /// Gets the 'relevant' attribute value.
        /// </summary>
        public bool Relevant
        {
            get { return attributes.Relevant != null ? bool.Parse(attributes.Relevant) : (attributes.Serialization == "none" ? false : true); }
        }

        /// <summary>
        /// Gets the 'validate' attribute value.
        /// </summary>
        public bool Validate
        {
            get { return attributes.Validate != null ? bool.Parse(attributes.Validate) : (attributes.Serialization == "none" ? false : true); }
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
            var uri = attributes.Resource != null ? new Uri(attributes.Resource, UriKind.RelativeOrAbsolute) : null;
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
            var uri = attributes.Action != null ? new Uri(attributes.Action, UriKind.RelativeOrAbsolute) : null;
            if (uri != null)
                return uri.IsAbsoluteUri ? uri : new Uri(Element.GetBaseUri(), uri);

            return null;
        }

        /// <summary>
        /// Gets the 'mode' attribute value.
        /// </summary>
        public SubmissionMode Mode
        {
            get { return attributes.Mode == "synchronous" ? SubmissionMode.Synchronous : SubmissionMode.Asynchronous; }
        }

        /// <summary>
        /// Gets the 'method' attribute value.
        /// </summary>
        public ModelMethod Method
        {
            get { return attributes.Method; }
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
            if (attributes.Serialization == "none")
                return new SubmissionSerialization(true);
            else if (!string.IsNullOrEmpty(attributes.Serialization))
                return new SubmissionSerialization(attributes.Serialization);
            else
                return new SubmissionSerialization(false);
        }

        /// <summary>
        /// Gets the 'mediatype' attribute value.
        /// </summary>
        public MediaRange MediaType
        {
            get { return attributes.MediaType != "none" ? attributes.MediaType : null; }
        }

        /// <summary>
        /// Gets the 'encoding' attribute value.
        /// </summary>
        public Encoding Encoding
        {
            get { return Encoding.GetEncoding(attributes.Encoding ?? "UTF-8"); }
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
            switch (attributes.Replace)
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
            get { return attributes.Instance; }
        }

        /// <summary>
        /// Gets the 'targetref' attribute value.
        /// </summary>
        public string TargetRef
        {
            get { return attributes.TargetRef; }
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
            var separator = attributes.Separator.Trim();
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
            get { return attributes.Version ?? "1.0"; }
        }

        /// <summary>
        /// Gets the 'indent' attribute value.
        /// </summary>
        public bool Indent
        {
            get { return attributes.Indent == "true" ? true : false; }
        }

        /// <summary>
        /// Gets the 'omit-xml-declaration' attribute value.
        /// </summary>
        public bool OmitXmlDeclaration
        {
            get { return attributes.OmitXmlDeclaration == "true" ? true : false; }
        }

        /// <summary>
        /// Gets the 'standalone' attribute value.
        /// </summary>
        public bool Standalone
        {
            get { return OmitXmlDeclaration ? false : (attributes.Standalone == "true" ? true : false); }
        }

        /// <summary>
        /// Gets the 'cdata-section-elements' attribute value.
        /// </summary>
        public string CDataSectionElements
        {
            get { return attributes.CDataSectionElements ?? ""; }
        }

        /// <summary>
        /// Gets the 'includenamespaceprefixes' attribute value.
        /// </summary>
        public string IncludeNamespacePrefixes
        {
            get { return attributes.IncludeNamespacePrefixes ?? ""; }
        }

    }

}