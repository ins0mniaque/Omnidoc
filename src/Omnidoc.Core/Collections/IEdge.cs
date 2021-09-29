namespace Omnidoc.Collections
{
    public interface IEdge < out TVertex > where TVertex : notnull
    {
        TVertex Source { get; }
        TVertex Target { get; }
    }
}