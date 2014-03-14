namespace NXKit
{

    public interface INavigationVisual : IContentVisual
    {

        string UniqueId { get; }

        bool Relevant { get; }

        string Label { get; }

    }

}
