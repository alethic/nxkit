namespace NXKit
{

    /// <summary>
    /// Specifies an interface that is invoked during the document invoke phase.
    /// </summary>
    public interface IOnInvoke
    {

        /// <summary>
        /// Invokes the specified interface. The invoke phase runs repeatidly until no invoations return <c>true</c>.
        /// </summary>
        /// <returns></returns>
        bool Invoke();

    }

}
