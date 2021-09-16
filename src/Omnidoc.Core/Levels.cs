using System;

namespace Omnidoc
{
    [ Flags ]
    public enum Levels
    {
        None = 0,

        Start = End >> 1,
        End   = 1   << 31,

        Document  = 1 << Level.Document,
        Page      = 1 << Level.Page,
        Link      = 1 << Level.Link,
        Block     = 1 << Level.Block,
        Table     = 1 << Level.Table,
        Header    = 1 << Level.Header,
        Footer    = 1 << Level.Footer,
        Row       = 1 << Level.Row,
        Cell      = 1 << Level.Cell,
        Vertex    = 1 << Level.Vertex,
        Edge      = 1 << Level.Edge,
        Paragraph = 1 << Level.Paragraph,
        Line      = 1 << Level.Line,
        Word      = 1 << Level.Word,
        All       = Document  |
                    Page      |
                    Link      |
                    Block     |
                    Table     |
                    Header    |
                    Footer    |
                    Row       |
                    Cell      |
                    Vertex    |
                    Edge      |
                    Paragraph |
                    Line      |
                    Word,

        DocumentStart  = Document  | Start,
        PageStart      = Page      | Start,
        LinkStart      = Link      | Start,
        BlockStart     = Block     | Start,
        TableStart     = Table     | Start,
        HeaderStart    = Header    | Start,
        FooterStart    = Footer    | Start,
        RowStart       = Row       | Start,
        CellStart      = Cell      | Start,
        VertexStart    = Vertex    | Start,
        EdgeStart      = Edge      | Start,
        ParagraphStart = Paragraph | Start,
        LineStart      = Line      | Start,
        WordStart      = Word      | Start,

        DocumentEnd    = Document  | End,
        PageEnd        = Page      | End,
        LinkEnd        = Link      | End,
        BlockEnd       = Block     | End,
        TableEnd       = Table     | End,
        HeaderEnd      = Header    | End,
        FooterEnd      = Footer    | End,
        RowEnd         = Row       | End,
        CellEnd        = Cell      | End,
        VertexEnd      = Vertex    | End,
        EdgeEnd        = Edge      | End,
        ParagraphEnd   = Paragraph | End,
        LineEnd        = Line      | End,
        WordEnd        = Word      | End
    }
}