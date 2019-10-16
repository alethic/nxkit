namespace NXKit.Composition
{

    /// <summary>
    /// Defines a lazy export with the specified metadata instance.
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    /// <typeparam name="TMetadata"></typeparam>
    public interface IExport<out TValue, out TMetadata> :
        IExport<TValue>
        where TMetadata : IExportMetadata
    {

        TMetadata Metadata { get; }

    }

    /// <summary>
    /// Defines a lazy export.
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    public interface IExport<out TValue>
    {

        TValue Value { get; }

    }

}
