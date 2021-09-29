using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using Omnidoc.Dom.Rendering;

namespace Omnidoc.Dom.Abstractions
{
    // NOTE: Find way to makee multi-styles easily
    // TODO: Add StylesheetTransformer?
    public interface IStylesheet<T>
    {
        IAsyncRenderable Style ( IElement element, T style );
    }

    public interface ILayoutEngine<TInputLayout, TOutputLayout> where TInputLayout : class, new ( ) where TOutputLayout : class, new ( )
    {
        IElement < TOutputLayout, TStyle > LayoutAsync < TStyle > ( IElement < TInputLayout, TStyle > element, CancellationToken cancellationToken )
             where TStyle : class, new ( );
    }

    // Should not be interface...
    public class LayoutExtractor<T> { }

    public class None { }

    public interface IContainable
    {
        void SetParent ( IElement? parent );
    }

    public interface IElement : IEquatable < IElement >
    {
        IElement? Parent { get; }

        // Extensions for other methods... (Restyle, Relayout) + Rename 'To' to... ? Change?
        // As ( (layout, style) => { } ) [NEW] && As ( (layout, style) => (layout, style) ) [OLD => NEW] (Add 4 parameters old => new?)
        IElement < TOtherLayout, TOtherStyle > Cast < TOtherLayout, TOtherStyle > ( ) where TOtherLayout : class, new ( ) where TOtherStyle : class, new ( );
    }

    public interface IElement < out TLayout, out TStyle > : IElement where TLayout : class, new ( ) where TStyle : class, new ( )
    {
        TLayout Layout { get; } // Layout.Create
        TStyle  Style  { get; } // Layout.Create
    }

    public interface IContainer : IList < IElement >, IElement
    {
        new IRoot < TOtherLayout, TOtherStyle > Cast < TOtherLayout, TOtherStyle > ( ) where TOtherLayout : class, new ( ) where TOtherStyle : class, new ( );
        IContainer < TOtherContentLayout, TOtherContentStyle > CastContent < TOtherContentLayout, TOtherContentStyle > ( ) where TOtherContentLayout : class, new ( ) where TOtherContentStyle : class, new ( );
        IRoot < TOtherLayout, TOtherStyle, TOtherContentLayout, TOtherContentStyle > CastContent < TOtherLayout, TOtherStyle, TOtherContentLayout, TOtherContentStyle > ( ) where TOtherLayout : class, new ( ) where TOtherStyle : class, new ( ) where TOtherContentLayout : class, new ( ) where TOtherContentStyle : class, new ( );

        IElement < TOtherLayout, TOtherStyle > IElement.Cast < TOtherLayout, TOtherStyle > ( )
        {
            return Cast < TOtherLayout, TOtherStyle > ( );
        }
    }

    public interface IReadOnlyContainer < out TContentLayout, out TContentStyle > : IReadOnlyList < IElement < TContentLayout, TContentStyle > >, IElement where TContentLayout : class, new ( ) where TContentStyle : class, new ( ) { }

    public interface IContainer < TContentLayout, TContentStyle > : IContainer, IReadOnlyContainer < TContentLayout, TContentStyle >, IList < IElement < TContentLayout, TContentStyle > >, IElement where TContentLayout : class, new ( ) where TContentStyle : class, new ( )
    {
        new IRoot < TOtherLayout, TOtherStyle, TContentLayout, TContentStyle > Cast < TOtherLayout, TOtherStyle > ( ) where TOtherLayout : class, new ( ) where TOtherStyle : class, new ( );

        IElement < TOtherLayout, TOtherStyle > IElement.Cast < TOtherLayout, TOtherStyle > ( )
        {
            return Cast < TOtherLayout, TOtherStyle > ( );
        }
    }

    public interface IRoot < out TLayout, out TStyle > : IContainer, IElement < TLayout, TStyle > where TLayout : class, new ( ) where TStyle : class, new ( )
    {
        new IRoot < TLayout, TStyle, TOtherContentLayout, TOtherContentStyle > CastContent < TOtherContentLayout, TOtherContentStyle > ( ) where TOtherContentLayout : class, new ( ) where TOtherContentStyle : class, new ( );

        IContainer < TOtherContentLayout, TOtherContentStyle > IContainer.CastContent < TOtherContentLayout, TOtherContentStyle > ( )
        {
            return CastContent < TOtherContentLayout, TOtherContentStyle > ( );
        }
    }

    public interface IRoot < out TLayout, out TStyle, TContentLayout, TContentStyle > : IRoot < TLayout, TStyle >, IContainer < TContentLayout, TContentStyle > where TLayout : class, new ( ) where TStyle : class, new ( ) where TContentLayout : class, new ( ) where TContentStyle : class, new ( )
    {

    }

    // *********************************************************************************
    // Only interfaces for elements that will be reused should be here...
    // Probably means no edge too...
    // *********************************************************************************

    public interface ILink : IElement
    {
        Uri? Uri { get; }
    }
    
    public interface IData // : IElement
    {
        Task < Stream > OpenAsync ( CancellationToken cancellationToken );
    }

    public interface IAsyncContainer : IContainer
    {
        bool IsLoaded { get; }
        Task LoadAsync ( CancellationToken cancellationToken );
    }

    public interface IAsyncRenderable : IRenderable
    {
        bool IsLoadedFor ( ISurface surface, Rect bounds );
        Task LoadAsync   ( ISurface surface, Rect bounds, CancellationToken cancellationToken = default );
    }

    public interface IViewport // : IElement
    {
        Point Offset { get; }
        Size Size { get; } // Needs Auto... double.Infinity? NaN?
    }

    public interface IEdge : Collections.IEdge < IElement > { }
}