namespace Omnidoc.Core
{
    public interface IArchiveLoader < TArchive > : IService, IFileLoader < TArchive >
        where TArchive : IReadOnlyArchive { }
}