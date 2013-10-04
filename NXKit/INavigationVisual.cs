namespace NXKit
{

    public interface INavigationVisual : IStructuralVisual
    {

        string UniqueId { get; }

        bool Relevant { get; }

        string Label { get; }

    }

}
