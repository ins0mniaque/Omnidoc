using System;

namespace Omnidoc.Content
{
    [ Flags ]
    public enum DocumentMarkers
    {
        None = 0,

        StartBlock     = 1 << 0,
        StartParagraph = 1 << 1,
        StartLine      = 1 << 2,
        EndLine        = 1 << 3,
        EndParagraph   = 1 << 4,
        EndBlock       = 1 << 5
    }
}