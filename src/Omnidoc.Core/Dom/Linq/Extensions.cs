using System;
using System.Collections.Generic;
using System.Linq;

using Omnidoc.Dom.Abstractions;

namespace Omnidoc.Dom.Linq
{
    public static partial class Extensions
    {
        // TODO: XName => Of<T> where T : IElement
        public static IEnumerable<IElement> AncestorsAndSelf(this IEnumerable<IElement> source) { throw new NotImplementedException(); }
        public static IEnumerable<IElement> AncestorsAndSelf(this IEnumerable<IElement> source, System.Xml.Linq.XName name) { throw new NotImplementedException(); }
        public static IEnumerable<IElement> Ancestors<T>(this IEnumerable<T> source) where T : System.Xml.Linq.XNode { throw new NotImplementedException(); }
        public static IEnumerable<IElement> Ancestors<T>(this IEnumerable<T> source, System.Xml.Linq.XName name) where T : System.Xml.Linq.XNode { throw new NotImplementedException(); }
        public static IEnumerable<IElement> DescendantsAndSelf(this IEnumerable<IElement> source) { throw new NotImplementedException(); }
        public static IEnumerable<IElement> DescendantsAndSelf(this IEnumerable<IElement> source, System.Xml.Linq.XName name) { throw new NotImplementedException(); }
        public static IEnumerable<IElement> Descendants<T>(this IEnumerable<T> source) where T : System.Xml.Linq.XContainer { throw new NotImplementedException(); }
        public static IEnumerable<IElement> Descendants<T>(this IEnumerable<T> source, System.Xml.Linq.XName name) where T : System.Xml.Linq.XContainer { throw new NotImplementedException(); }
        public static IEnumerable<IElement> Elements<T>(this IEnumerable<T> source) where T : System.Xml.Linq.XContainer { throw new NotImplementedException(); }
        public static IEnumerable<IElement> Elements<T>(this IEnumerable<T> source, System.Xml.Linq.XName name) where T : System.Xml.Linq.XContainer { throw new NotImplementedException(); }
    }
}