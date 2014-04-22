using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics.Contracts;

namespace NXKit.IO
{

    /// <summary>
    //  Contains protocol headers associated with a request or response.
    /// </summary>
    public class Headers :
        IEnumerable<KeyValuePair<string, string>>
    {

        Dictionary<string, string> items =
            new Dictionary<string, string>();

        /// <summary>
        //  Gets or sets the value associated with the specified key.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string this[string name]
        {
            get { return items[name]; }
            set { items[name] = value; }
        }

        /// <summary>
        /// Copies the header values from the given <see cref="Headers"/> collection.
        /// </summary>
        /// <param name="headers"></param>
        public void Add(Headers headers)
        {
            Contract.Requires<ArgumentNullException>(headers != null);

            foreach (var header in headers)
                items[header.Key] = header.Value;
        }

        /// <summary>
        /// Copies the header values from the given <see cref="NameValueCollection"/> collection.
        /// </summary>
        /// <param name="headers"></param>
        public void Add(NameValueCollection headers)
        {
            Contract.Requires<ArgumentNullException>(headers != null);

            foreach (string key in headers)
                items[key] = headers[key];
        }

        public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        {
            return items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

    }

}
