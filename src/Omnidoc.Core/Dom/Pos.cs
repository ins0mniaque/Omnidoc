using System;
using System.Collections.Generic;
using System.Linq;

using Omnidoc.Dom.Abstractions;
using Omnidoc.Dom.Elements;

namespace Omnidoc.Dom
{
    // TODO: Serialization
    public abstract class Relationship
    {
        public static Relationship Self   { get; } = new SelfRelationship   ( );
        public static Relationship Parent { get; } = new ParentRelationship ( );

        public abstract IElement? Relate ( IElement element );

        private class SelfRelationship : Relationship
        {
            public override IElement? Relate ( IElement element ) => element;

            public override string ToString ( ) => string.Empty;
        }

        private class ParentRelationship : Relationship
        {
            public override IElement? Relate ( IElement element ) => element.Parent;

            public override string ToString ( ) => "Parent";
        }
    }

    public abstract class RelatedProperty
    {
        public static RelatedProperty X      { get; } = new FixedRectProperty ( rect => rect.X      );
        public static RelatedProperty Y      { get; } = new FixedRectProperty ( rect => rect.Y      );
        public static RelatedProperty Width  { get; } = new FixedRectProperty ( rect => rect.Width  );
        public static RelatedProperty Height { get; } = new FixedRectProperty ( rect => rect.Height );
        public static RelatedProperty Left   { get; } = new FixedRectProperty ( rect => rect.Left   );
        public static RelatedProperty Top    { get; } = new FixedRectProperty ( rect => rect.Top    );
        public static RelatedProperty Right  { get; } = new FixedRectProperty ( rect => rect.Right  );
        public static RelatedProperty Bottom { get; } = new FixedRectProperty ( rect => rect.Bottom );

        public abstract double GetValue ( IElement? element, Rect fixedRect );

        // TODO: Serializable
        private class FixedRectProperty : RelatedProperty
        {
            private readonly Func < Rect, double > selector;
            public FixedRectProperty ( Func < Rect, double > selector )
            {
                this.selector = selector;
            }

            public override double GetValue ( IElement? element, Rect fixedRect ) => selector ( fixedRect );
        }
    }

    public abstract class RelatedProperty < TLayout > : RelatedProperty where TLayout : class, new ( )
    {
        public override double GetValue ( IElement? element, Rect fixedRect )
        {
            // TODO: IElement < TLayout > ?
            return Relate ( element, GetLayout, fixedRect );
        }

        protected abstract double Relate ( IElement? element, Func < IElement?, TLayout > layout, Rect fixedRect );

        private static TLayout GetLayout ( IElement? element )
        {
            return ( element as IElement < TLayout, object > )?.Layout ?? Factory < TLayout >.Create ( );
        }
    }

    public abstract class FlowRelatedProperty : RelatedProperty < Layouts.Flow >
    {
        // public static FlowRelatedProperty FontSize { get; } = ...
    }

    public class PosRect
    {
        public Pos Left   { get; set; } = Pos.Empty;
        public Pos Top    { get; set; } = Pos.Empty;
        public Pos Right  { get; set; } = Pos.Empty;
        public Pos Bottom { get; set; } = Pos.Empty;

        public Pos X
        {
            get => Left;
            set => Left = value;
        }

        public Pos Y
        {
            get => Top;
            set => Top = value;
        }

        public Pos Width
        {
            get => Right - Left;
            set
            {
                if ( Left.IsEmpty && ! Right.IsEmpty ) Left  = Right - value;
                else                                   Right = Left  + value;
            }
        }

        public Pos Height
        {
            get => Bottom - Top;
            set
            {
                if ( Top.IsEmpty && ! Bottom.IsEmpty ) Top    = Bottom - value;
                else                                   Bottom = Top    + value;
            }
        }
    }

    // TODO: Add FlowRelated...
    public static class Related
    {
        // TODO: Add Property ( margin ) helpers?
        public static Pos X      ( this Relationship relationship ) => Pos.Relative ( relationship, RelatedProperty.X      );
        public static Pos Y      ( this Relationship relationship ) => Pos.Relative ( relationship, RelatedProperty.Y      );
        public static Pos Width  ( this Relationship relationship ) => Pos.Relative ( relationship, RelatedProperty.Width  );
        public static Pos Height ( this Relationship relationship ) => Pos.Relative ( relationship, RelatedProperty.Height );
        public static Pos Left   ( this Relationship relationship ) => Pos.Relative ( relationship, RelatedProperty.Left   );
        public static Pos Top    ( this Relationship relationship ) => Pos.Relative ( relationship, RelatedProperty.Top    );
        public static Pos Right  ( this Relationship relationship ) => Pos.Relative ( relationship, RelatedProperty.Right  );
        public static Pos Bottom ( this Relationship relationship ) => Pos.Relative ( relationship, RelatedProperty.Bottom );
    }

    public abstract class Pos
    {
        // TODO: Rename default?
        public static Pos Empty { get; } = new None ( );

        public bool IsEmpty => this is None;

        public static Pos Absolute ( double value ) => new Constant ( value );
        public static Pos Relative ( Relationship relationship, RelatedProperty property ) => new Related ( relationship, property );

        private Pos ( ) { }

        public abstract IEnumerable < IElement > Relate ( IElement element );

        public abstract double Resolve ( IElement element, Func < IElement?, Rect > layout );

        public static implicit operator Pos ( double value ) => new Constant ( value );

        public static Pos operator + ( Pos left,    Pos right ) => Combine ( OpCode.Add,       left, right );
        public static Pos operator - ( Pos left,    Pos right ) => Combine ( OpCode.Substract, left, right );
        public static Pos operator * ( Pos left,    Pos right ) => Combine ( OpCode.Multiply,  left, right );
        public static Pos operator / ( Pos left,    Pos right ) => Combine ( OpCode.Divide,    left, right );
        public static Pos operator + ( double left, Pos right ) => Combine ( OpCode.Add,       new Constant ( left ), right );
        public static Pos operator - ( double left, Pos right ) => Combine ( OpCode.Substract, new Constant ( left ), right );
        public static Pos operator * ( double left, Pos right ) => Combine ( OpCode.Multiply,  new Constant ( left ), right );
        public static Pos operator / ( double left, Pos right ) => Combine ( OpCode.Divide,    new Constant ( left ), right );
        public static Pos operator + ( Pos left, double right ) => Combine ( OpCode.Add,       left, new Constant ( right ) );
        public static Pos operator - ( Pos left, double right ) => Combine ( OpCode.Substract, left, new Constant ( right ) );
        public static Pos operator * ( Pos left, double right ) => Combine ( OpCode.Multiply,  left, new Constant ( right ) );
        public static Pos operator / ( Pos left, double right ) => Combine ( OpCode.Divide,    left, new Constant ( right ) );

        private static Pos Combine ( OpCode opCode, Pos left, Pos right )
        {
            if ( left.IsEmpty && right.IsEmpty ) return Empty;
            if ( left .IsEmpty ) return right;
            if ( right.IsEmpty ) return left;

            if ( left is Constant leftConstant && right is Constant rightConstant )
                return new Constant ( Evaluate ( opCode, leftConstant.Value, rightConstant.Value ) );

            return new Combined ( opCode, left, right );
        }

        private static double Evaluate ( OpCode opCode, double left, double right )
        {
            return opCode switch
            {
                OpCode.Add       => left + right,
                OpCode.Substract => left - right,
                OpCode.Multiply  => left * right,
                OpCode.Divide    => left / right,
                _                => throw new InvalidOperationException ( ),
            };
        }

        private class None : Pos
        {
            public override IEnumerable < IElement > Relate ( IElement element )
            {
                return Enumerable.Empty < IElement > ( );
            }

            public override double Resolve ( IElement element, Func < IElement?, Rect > layout )
            {
                return double.NaN;
            }

            public override string ToString ( ) => "Empty";
        }

        private class Constant : Pos
        {
            public Constant ( double value )
            {
                Value = value;
            }

            public double Value { get; }

            public override IEnumerable < IElement > Relate ( IElement element )
            {
                return Enumerable.Empty < IElement > ( );
            }

            public override double Resolve ( IElement element, Func < IElement?, Rect > layout )
            {
                return Value;
            }

            public override string ToString ( ) => Value.ToString ( );
        }

        private class Related : Pos
        {
            public Related ( Relationship value, RelatedProperty property )
            {
                Relationship = value;
                Property     = property;
            }

            public Relationship    Relationship { get; }
            public RelatedProperty Property     { get; }

            public override IEnumerable < IElement > Relate ( IElement element )
            {
                if ( Relationship.Relate ( element ) is IElement related )
                    yield return related;
            }

            public override double Resolve ( IElement element, Func<IElement?, Rect> layout )
            {
                var rel = Relationship.Relate  ( element );
                return Property.GetValue ( rel, layout ( rel ) );
            }

            public override string ToString ( )
            {
                var rel = Relationship.ToString ( );
                if ( ! string.IsNullOrEmpty ( rel ) )
                    return $"{rel}.{Property}";

                return Property.ToString ( );
            }
        }

        private class Combined : Pos
        {
            public Combined ( OpCode op, Pos left, Pos right )
            {
                Op    = op;
                Left  = left;
                Right = right;
            }

            public OpCode Op    { get; }
            public Pos    Left  { get; }
            public Pos    Right { get; }

            public override IEnumerable < IElement > Relate ( IElement element )
            {
                // TODO: Distinct if root...
                return Left.Relate ( element ).Concat ( Right.Relate ( element ) );
            }

            public override double Resolve ( IElement element, Func<IElement?, Rect> layout )
            {
                var left  = Left .Resolve ( element, layout );
                var right = Right.Resolve ( element, layout );

                return Evaluate ( Op, left, right );
            }

            public override string ToString ( )
            {
                var left  = Left .ToString ( );
                var right = Right.ToString ( );
                var op    = Op switch
                {
                    OpCode.Add       => '+',
                    OpCode.Substract => '-',
                    OpCode.Multiply  => '*',
                    OpCode.Divide    => '/',
                    _                => '?'
                };

                // TODO: Add parenthesis only when necessary (priority)
                if ( Left  is Combined ) left  = '(' + left  + ')';
                if ( Right is Combined ) right = '(' + right + ')';

                return left + op + right;
            }
        }

        private enum OpCode
        {
            Add,
            Substract,
            Multiply,
            Divide
        }
    }
}