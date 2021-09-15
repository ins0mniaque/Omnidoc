using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

using Omnidoc.Collections;

namespace Omnidoc.Model
{
    [ SuppressMessage ( "Naming", "CA1710:Identifiers should have correct suffix", Justification = "Dynamic object" ) ]
    [ SuppressMessage ( "Naming", "CA1724:TypeNamesShouldNotMatchNamespaces",      Justification = "System.Runtime.Remoting is not available in .NET Standard" ) ]
    public class Metadata : DynamicDictionary { }

    [ SuppressMessage ( "Usage", "CA2227:Collection properties should be read only", Justification = "Dynamic object" ) ]
    public abstract class Content
    {
        public Point?    Position { get; set; }
        public Size?     Size     { get; set; }
        public Cell?     Cell     { get; set; }
        public Levels    Levels   { get; set; }
        public Metadata? Metadata { get; set; }
    }

    public enum Level
    {
        Document,
        Page,
        Block,
        Paragraph,
        Line,
        Link,
        Table,
        Header,
        Footer,
        Row,
        Cell,
        Vertex,
        Edge
    }

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

    public class Glyphs : Shape
    {
        public Glyphs ( string text )
        {
            Text = text;
        }

        public Font?  Font { get; set; }
        public string Text { get; }
    }

    public class Font
    {
        public string? Name   { get; set; }
        public double? Size   { get; set; }
        public int?    Weight { get; set; }
    }

    public class Pen
    {
        public string? Stroke      { get; set; }
        public double? StrokeWidth { get; set; }
    }

    public class Shape : Content
    {
        public Pen?    Pen  { get; set; }
        public string? Fill { get; set; }
    }

    public class Path : Shape
    {
        public Path ( string data )
        {
            Data = data;
        }

        public string Data { get; }
    }

    public enum EdgeType
    {
        Directed,
        Undirected
    }

    public class Edge : Content
    {
        public Edge ( EdgeType type, Content source, Content target, Content content )
        {
            Type    = type;
            Source  = source;
            Target  = target;
            Content = content;
        }

        public EdgeType Type    { get; }
        public Content  Source  { get; }
        public Content  Target  { get; }
        public Content  Content { get; }
    }

    public class Attachment
    {
        protected const string DefaultContentType = "application/octet-stream";

        public Attachment ( Stream content ) : this ( content, null ) { }
        public Attachment ( Stream content, string? contentType )
        {
            ContentType = contentType ?? DefaultContentType;
            Content     = content;
        }

        public Attachment ( Uri uri ) : this ( uri, null ) { }
        public Attachment ( Uri uri, string? contentType )
        {
            ContentType = contentType ?? ( DataUri.TryParse ( uri, out contentType ) ? contentType : DefaultContentType );
            Uri         = uri;
        }

        public    string? ContentType { get; }
        protected Stream? Content     { get; set; }
        protected Uri?    Uri         { get; set; }

        public virtual async Task < Stream > GetContentAsync ( CancellationToken cancellationToken = default )
        {
            if ( Content != null ) return Content;
            if ( Uri     == null ) throw new InvalidOperationException ( Strings.Error_InvalidAttachment );

            using var web = new WebClient ( );

            return await web.OpenReadTaskAsync ( Uri ).ConfigureAwait ( false );
        }

        public virtual async Task < Uri > GetUriAsync ( CancellationToken cancellationToken = default )
        {
            if ( Uri     != null ) return Uri;
            if ( Content == null ) throw new InvalidOperationException ( Strings.Error_InvalidAttachment );

            using var buffer = new MemoryStream ( );

            await Content.CopyToAsync ( buffer ).ConfigureAwait ( false );

            return Uri = DataUri.Generate ( buffer.ToArray ( ), ContentType );
        }
    }

    public class Link : Content
    {
        public Link ( Uri uri )
        {
            Uri = uri;
        }

        public Uri Uri { get; }
    }

    public class Image : Attachment
    {
        public Image ( Stream content )                      : base ( content )              { }
        public Image ( Stream content, string? contentType ) : base ( content, contentType ) { }
        public Image ( Uri uri )                             : base ( uri )                  { }
        public Image ( Uri uri, string? contentType )        : base ( uri, contentType )     { }

        public string? Text { get; set; }
    }
}