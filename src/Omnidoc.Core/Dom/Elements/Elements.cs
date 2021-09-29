using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Omnidoc.Dom.Abstractions;
using Omnidoc.Dom.Rendering;

// Rename?
namespace Omnidoc.Dom.Decorators
{
    // TODO: Rename... move to Decorators?
    public class Drawing : List < IRenderable >, IAsyncRenderable
    {
        public virtual bool IsLoadedFor ( ISurface surface, Rect bounds ) => this.OfType<IAsyncRenderable>().All ( r => r.IsLoadedFor ( surface, bounds ) );

        public virtual async Task LoadAsync ( ISurface surface, Rect bounds, CancellationToken cancellationToken = default )
        {
            foreach ( var asyn in this.OfType<IAsyncRenderable>() )
                await asyn.LoadAsync ( surface, bounds, cancellationToken );
        }

        public virtual void Render ( ISurface surface, Rect bounds )
        {
            foreach ( var asyn in this )
                asyn.Render ( surface, bounds );
        }
    }

    public abstract class Decorator : IAsyncRenderable
    {
        protected Decorator ( IRenderable renderable )
        {
            Renderable      = renderable;
            AsyncRenderable = renderable as IAsyncRenderable;
        }

        protected IRenderable       Renderable      { get; }
        protected IAsyncRenderable? AsyncRenderable { get; }

        public virtual bool IsLoadedFor ( ISurface surface, Rect bounds ) => AsyncRenderable?.IsLoadedFor ( surface, bounds ) ?? true;
        public virtual Task LoadAsync ( ISurface surface, Rect bounds, CancellationToken cancellationToken = default ) => AsyncRenderable?.LoadAsync ( surface, bounds, cancellationToken ) ??
                                                                                                                  Task.CompletedTask;
        public virtual void Render ( ISurface surface, Rect bounds ) => Renderable.Render ( surface, bounds );
    }

    public class Margin : Decorator
    {
        public Margin ( IAsyncRenderable renderable ) : base ( renderable ) { }

        public override bool IsLoadedFor ( ISurface surface, Rect bounds )                                                => base.IsLoadedFor ( surface, ApplyMargin ( bounds ) );
        public override Task LoadAsync   ( ISurface surface, Rect bounds, CancellationToken cancellationToken = default ) => base.LoadAsync   ( surface, ApplyMargin ( bounds ), cancellationToken );
        public override void Render      ( ISurface surface, Rect bounds )                                                => base.Render      ( surface, ApplyMargin ( bounds ) );

        // TODO: Margin property; supports negatives
        private Rect ApplyMargin ( Rect bounds )
        {
            var adjusted = bounds;

            return adjusted;
        }
    }
}

namespace Omnidoc.Dom.Elements
{
    public static class Factory < T > where T : class, new ( )
    {
        private static readonly T? singleton = typeof ( T ).GetProperties ( ).Length == 0 ? new T ( ) : default;

        public static T Create ( ) => singleton ?? new T ( );
    }

    // TODO: Rename...
    public class TextElement : Element
    {
        private readonly Implementation text;

        public TextElement ( ) : this ( new Implementation ( ) ) { }
        protected TextElement ( TextElement text ) : this ( text.text ) { }
        private TextElement ( Implementation implementation )
        {
            text = implementation;
        }

        public virtual string? Text { get => text.Text; set => text.Text = value; }

        public override IElement<TOtherLayout, TOtherStyle> Cast<TOtherLayout, TOtherStyle> ( )
        {
            return new TextElement<TOtherLayout, TOtherStyle>(this);
        }

        protected override object GetImplementation ( ) => text;

        private class Implementation
        {
            public string? Text { get; set; }
        }
    }

    public class TextElement < TLayout, TStyle > : TextElement, IElement < TLayout, TStyle > where TLayout : class, new ( ) where TStyle : class, new ( )
    {
        public TextElement ( ) : this ( new TextElement ( ) ) { }
        public TextElement ( TextElement element ) : this ( element, default, default ) { }
        private TextElement ( TextElement element, TLayout? layout, TStyle? style ) : base ( element )
        {
            Layout = layout ?? Factory < TLayout >.Create ( );
            Style  = style  ?? Factory < TStyle  >.Create ( );
        }

        public TLayout Layout { get; }
        public TStyle  Style  { get; }

        public override IElement<TOtherLayout, TOtherStyle> Cast<TOtherLayout, TOtherStyle> ( )
        {
            return this as TextElement<TOtherLayout, TOtherStyle> ??
                   new TextElement<TOtherLayout, TOtherStyle>(this, Layout as TOtherLayout, Style as TOtherStyle);
        }
    }

    public class Container : Implementable, IContainer
    {
        // TODO: Create impl that sets Element.Parent...
        private readonly IList < IElement > impl;

        // TODO: Capacity...
        public Container ( )
        {
            impl = new List < IElement > ( );
        }
        protected Container ( Container container )
        {
            impl = new List < IElement > ( container.impl.Select ( a => OnElementAdded ( a ) ) );
        }

        public IElement this [ int index ] { get => impl [ index ]; set => impl [ index ] = OnElementAdded ( value ); }

        public int Count => impl.Count;

        public bool IsReadOnly => impl.IsReadOnly;

        public void Add ( IElement item ) => impl.Add ( OnElementAdded ( item ) );
        public void Clear ( ) { var copy = new IElement[ impl.Count]; impl.CopyTo ( copy, 0 ); impl.Clear ( ); foreach ( var x in copy ) OnElementRemoved ( x ); }
        public bool Contains ( IElement item ) => impl.Contains ( item );
        public void CopyTo ( IElement [ ] array, int arrayIndex ) => impl.CopyTo ( array, arrayIndex );
        public IEnumerator<IElement> GetEnumerator ( ) => impl.GetEnumerator ( );
        public int IndexOf ( IElement item ) => impl.IndexOf ( item );
        public void Insert ( int index, IElement item ) => impl.Insert ( index, OnElementAdded ( item ) );
        public bool Remove ( IElement item ) { var x = impl.Remove ( item ); if ( x ) OnElementRemoved ( item ); return x; }
        public void RemoveAt ( int index ) { var x = impl [ index ]; impl.RemoveAt ( index ); OnElementRemoved ( x ); }
        IEnumerator IEnumerable.GetEnumerator ( ) => ( (IEnumerable) impl ).GetEnumerator ( );

        protected virtual IElement OnElementAdded ( IElement element )
        {
            if ( element.Parent is IContainer cc )
                cc.Remove ( element );

            if ( element is IContainable c )
                c.SetParent ( this );

            return element;
        }

        protected virtual void OnElementRemoved ( IElement element )
        {
            if ( element is IContainable c )
                c.SetParent ( null );
        }

        public virtual IRoot<TOtherLayout, TOtherStyle> Cast<TOtherLayout, TOtherStyle> ( )
            where TOtherLayout : class, new()
            where TOtherStyle : class, new()
        {
            return new Root<TOtherLayout, TOtherStyle>(this);
        }

        public virtual IContainer<TOtherContentLayout, TOtherContentStyle> CastContent<TOtherContentLayout, TOtherContentStyle> ( )
            where TOtherContentLayout : class, new()
            where TOtherContentStyle : class, new()
        {
            return new Container<TOtherContentLayout, TOtherContentStyle>(this);
        }
        public virtual IRoot<TOtherLayout, TOtherStyle, TOtherContentLayout, TOtherContentStyle> CastContent<TOtherLayout, TOtherStyle, TOtherContentLayout, TOtherContentStyle> ( )
            where TOtherLayout : class, new()
            where TOtherStyle : class, new()
            where TOtherContentLayout : class, new()
            where TOtherContentStyle : class, new()
        {
            return new Root<TOtherLayout, TOtherStyle, TOtherContentLayout, TOtherContentStyle>(this);
        }

        protected override object GetImplementation ( ) => impl;
    }

    public class Container<TContentLayout, TContentStyle> : Container, IContainer<TContentLayout, TContentStyle> where TContentLayout : class, new() where TContentStyle : class, new()
    {
        public Container ( ) : this ( new Container ( ) ) { }
        public Container ( Container container ) : base ( container ) { }

        public new IElement<TContentLayout, TContentStyle> this [ int index ]
        {
            get => (IElement<TContentLayout, TContentStyle>) base [ index ];
            set => base [ index ] = value;
        }

        // TODO: IElement<TContentLayout, TContentStyle> Add ( IElement ); + Insert
        public void Add ( IElement<TContentLayout, TContentStyle> item ) => base.Add ( item );
        public bool Contains ( IElement<TContentLayout, TContentStyle> item ) => base.Contains ( item );
        public void CopyTo ( IElement<TContentLayout, TContentStyle> [ ] array, int arrayIndex ) => base.CopyTo ( array, arrayIndex ); // TODO: Fix this...
        public int IndexOf ( IElement<TContentLayout, TContentStyle> item ) => base.IndexOf ( item );
        public void Insert ( int index, IElement<TContentLayout, TContentStyle> item )=> base.Insert ( index, item );
        public bool Remove ( IElement<TContentLayout, TContentStyle> item )=> base.Remove ( item );
        public new IEnumerator<IElement<TContentLayout, TContentStyle>> GetEnumerator ( ) => this.Cast<IElement<TContentLayout, TContentStyle>>().GetEnumerator();

        protected override IElement OnElementAdded ( IElement element ) => base.OnElementAdded ( element.Cast < TContentLayout, TContentStyle > ( ) );

        public new virtual IRoot < TOtherLayout, TOtherStyle, TContentLayout, TContentStyle > Cast < TOtherLayout, TOtherStyle > ( ) where TOtherLayout : class, new ( ) where TOtherStyle : class, new ( )
        {
            return this as Root< TOtherLayout, TOtherStyle, TContentLayout, TContentStyle > ??
                   new Root< TOtherLayout, TOtherStyle, TContentLayout, TContentStyle >(this);
        }

        public override IContainer<TOtherContentLayout, TOtherContentStyle> CastContent<TOtherContentLayout, TOtherContentStyle> ( )
        {
            return this as Container<TOtherContentLayout, TOtherContentStyle> ??
                   new Container<TOtherContentLayout, TOtherContentStyle>(this);
        }
        
    }

    public class Root<TLayout, TStyle> : Container, IRoot<TLayout, TStyle> where TLayout : class, new() where TStyle : class, new()
    {
        public Root ( ) : this ( new Container ( ) ) { }
        public Root ( Container container ) : this ( container, default, default ) { }
        protected Root ( Container container, TLayout? layout, TStyle? style ) : base ( container )
        {
            Layout = layout ?? Factory < TLayout >.Create ( );
            Style  = style  ?? Factory < TStyle  >.Create ( );
        }

        public TLayout Layout { get; }
        public TStyle  Style  { get; }

        public override IRoot<TOtherLayout, TOtherStyle> Cast<TOtherLayout, TOtherStyle> ( )
        {
            return this as Root<TOtherLayout, TOtherStyle> ??
                   new Root < TOtherLayout, TOtherStyle > ( this, Layout as TOtherLayout, Style as TOtherStyle );
        }

        public new IRoot<TLayout, TStyle, TOtherContentLayout, TOtherContentStyle> CastContent<TOtherContentLayout, TOtherContentStyle> ( )
            where TOtherContentLayout : class, new()
            where TOtherContentStyle : class, new()
        {
            return this as Root<TLayout, TStyle, TOtherContentLayout, TOtherContentStyle> ??
                   new Root < TLayout, TStyle, TOtherContentLayout, TOtherContentStyle > ( this );
        }

        public override IRoot<TOtherLayout, TOtherStyle, TOtherContentLayout, TOtherContentStyle> CastContent<TOtherLayout, TOtherStyle, TOtherContentLayout, TOtherContentStyle> ( )
        {
            return this as Root<TOtherLayout, TOtherStyle, TOtherContentLayout, TOtherContentStyle> ??
                   new Root < TOtherLayout, TOtherStyle, TOtherContentLayout, TOtherContentStyle > ( this as Root<TOtherLayout, TOtherStyle> ??
                   new Root < TOtherLayout, TOtherStyle > ( this, Layout as TOtherLayout, Style as TOtherStyle ) );
        }
    }

    public class Root<TLayout, TStyle, TContentLayout, TContentStyle> : Container<TContentLayout, TContentStyle>, IRoot<TLayout, TStyle, TContentLayout, TContentStyle> where TLayout : class, new() where TStyle : class, new() where TContentLayout : class, new() where TContentStyle : class, new()
    {
        public Root ( ) : this ( new Container ( ) ) { }
        public Root ( Container container ) : this ( container, default, default ) { }
        public Root ( Root<TLayout, TStyle> root ) : this ( root, root.Layout, root.Style ) { }
        private Root ( Container container, TLayout? layout, TStyle? style ) : base ( container )
        {
            Layout = layout ?? Factory < TLayout >.Create ( );
            Style  = style  ?? Factory < TStyle  >.Create ( );
        }

        public TLayout Layout { get; }
        public TStyle  Style  { get; }

        public override IRoot<TOtherLayout, TOtherStyle, TContentLayout, TContentStyle> Cast<TOtherLayout, TOtherStyle> ( )
        {
            return this as Root<TOtherLayout, TOtherStyle, TContentLayout, TContentStyle> ??
                   new Root < TOtherLayout, TOtherStyle, TContentLayout, TContentStyle > ( this, Layout as TOtherLayout, Style as TOtherStyle );
        }

        public new IRoot<TLayout, TStyle, TOtherContentLayout, TOtherContentStyle> CastContent<TOtherContentLayout, TOtherContentStyle> ( )
            where TOtherContentLayout : class, new()
            where TOtherContentStyle : class, new()
        {
            return this as Root<TLayout, TStyle, TOtherContentLayout, TOtherContentStyle> ??
                   new Root < TLayout, TStyle, TOtherContentLayout, TOtherContentStyle > ( this, Layout, Style );
        }
    }

    // TODO: Implementation + <TLayout>...
    // TODO: Shape like geometry? + Shape Equality... Yes.
    //       Equality requires knowing the bounds... So same type only; no global equals.

    public class Path : Shape
    {
        // TODO: Always stretch path, and add ViewBox to stretch rendering?
        //       Means image can't stretch? But could if Pos supported AspectRatios...
        //       Height = Self.Width * Self.AspectRatio
        //       AspecctRatio => Element.Size... on IRenderable?
        //       Text has no size... DesiredSize? IShape+ITexture => ISized if ISized => AspectRatio.
        //       Does surface cares about size? No. IResizable => OriginalSize
        public enum StretchMode { None, Geometry, Stroke, Both };

        public Geometry? Geometry { get; set; }
        public StretchMode Stretch { get; set; }

        protected override Geometry? GetGeometry ( Rect bounds ) => Geometry; // TODO: Stretch
    }

    public class Image : Data, IAsyncRenderable
    {
        private ITexture? Texture { get; set; }

        public bool IsLoadedFor ( ISurface surface, Rect bounds )
        {
            return Texture != null && surface.CanDraw ( Texture );
        }

        public async Task LoadAsync ( ISurface surface, Rect bounds, CancellationToken cancellationToken = default )
        {
            if ( ! IsLoadedFor ( surface, bounds ) )
            {
                using var stream = await OpenAsync ( cancellationToken );

                Texture = await surface.LoadAsync ( stream, cancellationToken );
            }
        }

        public void Render ( ISurface surface, Rect bounds )
        {
            if ( Texture == null || surface.CanDraw ( Texture ) )
                throw new InvalidOperationException ( "IAsyncRenderable was not loaded for this surface" );

            surface.Draw ( Texture, bounds );
        }
    }

    public class Link { }
    public class Data : IData
    {
        // TODO: WriteMode...
        public Task<Stream> OpenAsync ( CancellationToken cancellationToken ) => throw new NotImplementedException ( );
    }

    public class Edge
    {

    }

    public abstract partial class Shape : IShape, IAsyncRenderable
    {
        public Pen?   Stroke { get; set; }
        public Brush? Fill   { get; set; }

        public bool IsLoadedFor ( ISurface surface, Rect bounds )
        {
            if ( GetStroke ( bounds ) is Pen pen && pen.Stroke is IAsyncRenderable asyncStroke && ! asyncStroke.IsLoadedFor ( surface, bounds )  )
                return false;

            if ( GetFill ( bounds ) is IAsyncRenderable asyncFill && ! asyncFill.IsLoadedFor ( surface, bounds ) )
                return false;

            return true;
        }

        public async Task LoadAsync ( ISurface surface, Rect bounds, CancellationToken cancellationToken = default )
        {
            if ( GetStroke ( bounds ) is Pen pen && pen.Stroke is IAsyncRenderable asyncStroke )
                await asyncStroke.LoadAsync ( surface, bounds, cancellationToken );

            if ( GetFill ( bounds ) is IAsyncRenderable asyncFill )
                await asyncFill.LoadAsync ( surface, bounds, cancellationToken );
        }

        public void Render ( ISurface surface, Rect bounds )
        {
            surface.Draw ( this, bounds );
        }

        protected virtual  Pen?      GetStroke   ( Rect bounds ) => Stroke;
        protected virtual  Brush?    GetFill     ( Rect bounds ) => Fill;
        protected abstract Geometry? GetGeometry ( Rect bounds );

        Pen?      IShape.GetStroke   ( Rect bounds ) => GetStroke   ( bounds );
        Brush?    IShape.GetFill     ( Rect bounds ) => GetFill     ( bounds );
        Geometry? IShape.GetGeometry ( Rect bounds ) => GetGeometry ( bounds );
    }

        // Add to Shape... + use PathBuilder + add IsCircle methods?
    public partial class Shape
    {
        /**
         * Get path data for a rounded rectangle. Allows for different radius on each corner.
         * @param  {Number} w   Width of rounded rectangle
         * @param  {Number} h   Height of rounded rectangle
         * @param  {Number} tlr Top left corner radius
         * @param  {Number} trr Top right corner radius
         * @param  {Number} brr Bottom right corner radius
         * @param  {Number} blr Bottom left corner radius
         * @return {String}     Rounded rectangle SVG path data
         */

        // var roundedRectData = function (w, h, tlr, trr, brr, blr) {
        //   return 'M 0 ' + tlr
        //     + ' A ' + tlr + ' ' + tlr + ' 0 0 1 ' + tlr + ' 0'
        //     + ' L ' + (w - trr) + ' 0'
        //     + ' A ' + trr + ' ' + trr + ' 0 0 1 ' + w + ' ' + trr
        //     + ' L ' + w + ' ' + (h - brr)
        //     + ' A ' + brr + ' ' + brr + ' 0 0 1 ' + (w - brr) + ' ' + h
        //     + ' L ' + blr + ' ' + h
        //     + ' A ' + blr + ' ' + blr + ' 0 0 1 0 ' + (h - blr)
        //     + ' Z';
        // };
    }
}