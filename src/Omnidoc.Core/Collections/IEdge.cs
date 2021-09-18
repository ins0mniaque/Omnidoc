namespace Omnidoc.Collections
{
    public interface IEdge < TVertex > where TVertex : notnull
    {
        TVertex Source { get; }
        TVertex Target { get; }
    }
}