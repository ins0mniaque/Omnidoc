using System;

namespace Omnidoc.Model
{
    [ Flags ]
    public enum Levels
    {
        None = 0,

        Document  = 1 << Level.Document,
        Page      = 1 << Level.Page,
        Block     = 1 << Level.Block,
        Paragraph = 1 << Level.Paragraph,
        Line      = 1 << Level.Line,
        Link      = 1 << Level.Link,
        Table     = 1 << Level.Table,
        Header    = 1 << Level.Header,
        Footer    = 1 << Level.Footer,
        Row       = 1 << Level.Row,
        Cell      = 1 << Level.Cell,
        Vertex    = 1 << Level.Vertex,
        Edge      = 1 << Level.Edge,

        Start = End >> 1,
        End   = 1   << 31,

        DocumentStart  = Document  | Start,
        DocumentEnd    = Document  | End,
        PageStart      = Page      | Start,
        PageEnd        = Page      | End,
        BlockStart     = Block     | Start,
        BlockEnd       = Block     | End,
        ParagraphStart = Paragraph | Start,
        ParagraphEnd   = Paragraph | End,
        LineStart      = Line      | Start,
        LineEnd        = Line      | End,
        LinkStart      = Link      | Start,
        LinkEnd        = Link      | End,
        TableStart     = Table     | Start,
        TableEnd       = Table     | End,
        HeaderStart    = Header    | Start,
        HeaderEnd      = Header    | End,
        FooterStart    = Footer    | Start,
        FooterEnd      = Footer    | End,
        RowStart       = Row       | Start,
        RowEnd         = Row       | End,
        CellStart      = Cell      | Start,
        CellEnd        = Cell      | End,
        VertexStart    = Vertex    | Start,
        VertexEnd      = Vertex    | End,
        EdgeStart      = Edge      | Start,
        EdgeEnd        = Edge      | End
    }
}