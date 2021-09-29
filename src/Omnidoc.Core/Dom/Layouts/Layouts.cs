using System;

namespace Omnidoc.Dom.Layouts
{
    // TODO: Cell wrapper like Fixed
    public class Grid
    {
        public Cell Cell { get; set; }
    }

    public class Graph < TAlgorithm > : Graph where TAlgorithm : class, new ( )
    {
        public TAlgorithm Algorithm { get; } = Elements.Factory < TAlgorithm >.Create ( );
    }

    public class Graph { }

    public class Relative : PosRect
    {

    }

    // TODO: Units... px/pt absolute... but relative?
    //       Relative units: em (FontSize) % (parent size) + viewport size + root size
    // TODO: Solve style inheritence: Font and Brushes... Fixed should also have that...
    public class Flow : Relative
    {
        // TODO: Move inside? and rename...
        [ Flags ]
        public enum Modes // Merge with positions?
        {
            Inherits = 0,
            Inline   = 1 << 0,
            Block    = 1 << 1,
            Break    = 1 << 2,
            Stack    = 1 << 3,
            None     = 1 << 4 // Not sure if before or after mskes sense... Most common usage is borders
        }

        public Modes Mode { get; set; }
    }

    // TODO: Flow < TStyle >

    // NOTE: <Glyphs Indices... if present, split per glyph and done...
    public class Fixed
    {
        private Rect rect; // Or add non-sealed Rect called bounds for this...

        // public double Left { get => rect.Left... }
    }

    public class Archive
    {
        public string? Path { get; set; }
    }

    public class Document
    {
        public int PageIndex { get; set; }
    }
}