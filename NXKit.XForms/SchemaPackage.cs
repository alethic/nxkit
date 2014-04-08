using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;

namespace NXKit.XForms
{

    public sealed class SchemaPackage : NXKit.SchemaPackage
    {

        public override IEnumerable<XNamespace> Namespaces
        {
            get
            {
                yield return Constants.XForms_1_0;
            }
        }

        public override string ResolveSchema(XNamespace ns)
        {
            if (ns == Constants.XForms_1_0)
                return Constants.XForms_1_0_HREF;
            else
                return null;
        }

        public override Stream OpenSchema(string location)
        {
            throw new NotImplementedException();
        }

    }

}
