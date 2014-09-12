﻿using System;
using System.ComponentModel.Composition;
using System.Diagnostics.Contracts;
using System.Xml.Linq;

using NXKit.Composition;
using NXKit.DOMEvents;
using NXKit.Xml;
using NXKit.XMLEvents;

namespace NXKit.XForms
{

    [Extension("{http://www.w3.org/2002/xforms}load")]
    [PartMetadata(ScopeCatalog.ScopeMetadataKey, Scope.Object)]
    public class Load :
        ElementExtension,
        IEventHandler
    {

        readonly Lazy<LoadProperties> properties;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="element"></param>
        public Load(XElement element)
            : base(element)
        {
            Contract.Requires<ArgumentNullException>(element != null);

            this.properties = new Lazy<LoadProperties>(() => element.Interface<LoadProperties>());
        }

        public void HandleEvent(Event ev)
        {
            throw new NotImplementedException();
        }

    }

}
