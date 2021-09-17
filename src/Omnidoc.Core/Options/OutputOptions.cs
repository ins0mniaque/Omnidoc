using Omnidoc.IO;

namespace Omnidoc
{
    public class OutputOptions : Options
    {
        public OutputOptions ( FileFormat format )
        {
            Format = format;
        }

        public FileFormat Format { get; }
    }
}