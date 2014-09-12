﻿using System;
using System.ComponentModel.Composition;
using System.Diagnostics.Contracts;
using System.Xml.Linq;

using NXKit.Composition;

namespace NXKit.XForms
{

    [Extension("{http://www.w3.org/2002/xforms}var")]
    [PartMetadata(ScopeCatalog.ScopeMetadataKey, Scope.Object)]
    [Remote]
    public class Var :
        ElementExtension
    {

        readonly Lazy<VarProperties> properties;
        readonly Lazy<EvaluationContextResolver> context;
        readonly Lazy<Binding> value;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="properties"></param>
        /// <param name="context"></param>
        public Var(
            XElement element,
            Lazy<VarProperties> properties,
            Lazy<EvaluationContextResolver> context)
            : base(element)
        {
            Contract.Requires<ArgumentNullException>(element != null);
            Contract.Requires<ArgumentNullException>(properties != null);
            Contract.Requires<ArgumentNullException>(context != null);

            this.properties = properties;
            this.context = context;
            this.value = new Lazy<Binding>(() => properties.Value.Value != null ? new Binding(Element, context.Value.Context, properties.Value.Value) : null);
        }

        [Remote]
        public string Name
        {
            get { return properties.Value.Name; }
        }

        public object Value
        {
            get { return value.Value != null ? value.Value.Result : null; }
        }

    }

}