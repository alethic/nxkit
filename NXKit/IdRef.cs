using System;
using System.Diagnostics.Contracts;

namespace NXKit
{

    public class IdRef
    {

        public static implicit operator string(IdRef idRef)
        {
            return idRef != null ? idRef.Id : null;
        }

        public static implicit operator IdRef(string id)
        {
            return string.IsNullOrWhiteSpace(id) ? null : new IdRef(id);
        }


        readonly string id;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="id"></param>
        public IdRef(string id)
        {
            Contract.Requires<ArgumentNullException>(id != null);
            Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(id));

            this.id = id;
        }

        public string Id
        {
            get { return id; }
        }

        public override string ToString()
        {
            return id;
        }

        public override bool Equals(object obj)
        {
            var idRef = obj as IdRef;
            if (idRef == null)
                return false;

            return id == idRef.id;
        }

        public override int GetHashCode()
        {
            return id.GetHashCode();
        }

    }

}
