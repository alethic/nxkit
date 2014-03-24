namespace NXKit
{

    public class NXObjectChangeEventArgs :
        NXObjectEventArgs
    {

        readonly NXObjectChange change;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="object"></param>
        public NXObjectChangeEventArgs(NXObject @object, NXObjectChange change)
            : base(@object)
        {
            this.change = change;
        }

        public NXObjectChange Change
        {
            get { return change; }
        }

    }

}
