using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;

namespace NXKit.Xml
{

    /// <summary>
    /// Allows for the resolution of XML resources embedded within assemblies.
    /// </summary>
    public class AssemblyResourceXmlResolver :
        XmlResolver
    {

        /// <summary>
        /// Initializes the static instance.
        /// </summary>
        static AssemblyResourceXmlResolver()
        {
            if (!UriParser.IsKnownScheme("assembly"))
                UriParser.Register(new GenericUriParser(GenericUriParserOptions.Default), "assembly", 0);
        }

        readonly IEnumerable<Assembly> assemblies;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="assemblies"></param>
        public AssemblyResourceXmlResolver(IEnumerable<Assembly> assemblies)
        {
            this.assemblies = assemblies ?? throw new ArgumentNullException(nameof(assemblies));
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="assemblies"></param>
        public AssemblyResourceXmlResolver(params Assembly[] assemblies) :
            this(assemblies?.AsEnumerable())
        {

        }

        /// <summary>
        /// Searches for the assembly resource at the specified path.
        /// </summary>
        /// <param name="absoluteUri"></param>
        /// <returns></returns>
        public Stream LoadUri(Uri absoluteUri)
        {
            if (absoluteUri.Scheme == "assembly")
            {
                // resolve the assembly
                var assembly = assemblies.FirstOrDefault(i => string.Equals(i.GetName().Name, absoluteUri.Host, StringComparison.InvariantCultureIgnoreCase));
                if (assembly == null)
                    return null;

                // convert URI to an embedded resource name
                var resourceName = string.Join(".", PathToResourceName(absoluteUri.Segments.Skip(1).Select(i => i.TrimEnd('/'))));

                var resource = assembly.GetManifestResourceStream(resourceName);
                if (resource != null)
                    return resource;
            }

            return null;
        }

        public override object GetEntity(Uri absoluteUri, string role, Type ofObjectToReturn)
        {
            return LoadUri(absoluteUri);
        }

        /// <summary>
        /// Escapes a single path element.
        /// </summary>
        /// <param name="name"></param>
        string EscapePathElement(string name)
        {
            // path element is itself escaped by separator
            var s = name.Split('.');

            // fix each component
            for (int i = 0; i < s.Length; i++)
            {
                s[i] = s[i].Replace("-", "_");
                s[i] = char.IsNumber(s[i][0]) ? "_" + s[i] : s[i];
            }

            // return file result
            return string.Join(".", s);
        }

        /// <summary>
        /// Converts a series of path segments to a resource name following the default C# compiler rules.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        IEnumerable<string> PathToResourceName(IEnumerable<string> path)
        {
            // work on  copy
            var p = path.ToArray();

            // escape all but last element
            for (int i = 0; i < p.Length - 1; i++)
                yield return EscapePathElement(p[i]);

            // return new version
            yield return p[p.Length - 1];
        }

    }
}
