using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace Omnidoc.Collections
{
    public static class NumberListParser
    {
        public delegate bool TryParseDelegate < T > ( ReadOnlySpan < char > span, NumberStyles style, IFormatProvider provider, [ NotNullWhen ( true ) ] out T number );

        public static bool TryParse < T > ( TryParseDelegate < T > tryParse, string source, ref int index, NumberStyles style, IFormatProvider provider, [ NotNullWhen ( true ) ] out T numberA, [ NotNullWhen ( true ) ] out T numberB )
        {
            if ( tryParse == null ) throw new ArgumentNullException ( nameof ( tryParse ) );
            if ( source   == null ) throw new ArgumentNullException ( nameof ( source   ) );

            var separator = NumberFormatInfo.GetInstance ( provider ).NumberDecimalSeparator != "," ? ',' : ';';

            index   = source.IndexOf ( separator, index );
            numberA = numberB = default!;

            return index > 0 &&
                   tryParse ( source.AsSpan ( 0, index ),  style, provider, out numberA ) &&
                   tryParse ( source.AsSpan ( index + 1 ), style, provider, out numberB );
        }

        public static string Format < T > ( IFormatProvider? provider, string? format, T numberA, T numberB )
        {
            provider ??= CultureInfo.CurrentCulture;
            format   ??= string.Empty;

            var listFormat = string.Format ( CultureInfo.InvariantCulture,
                                             "{{0:{0}}}{1}{{1:{0}}}",
                                             format,
                                             provider.GetSeparator ( ) );

            return string.Format ( provider, listFormat, numberA, numberB );
        }

        public static string Format < T > ( IFormatProvider? provider, string? format, T numberA, T numberB, T numberC, T numberD )
        {
            provider ??= CultureInfo.CurrentCulture;
            format   ??= string.Empty;

            var listFormat = string.Format ( CultureInfo.InvariantCulture,
                                             "{{0:{0}}}{1}{{1:{0}}}{1}{{2:{0}}}{1}{{3:{0}}}",
                                             format,
                                             provider.GetSeparator ( ) );

            return string.Format ( provider, listFormat, numberA, numberB, numberC, numberD );
        }

        private static char GetSeparator ( this IFormatProvider provider ) => NumberFormatInfo.GetInstance ( provider ).NumberDecimalSeparator != "," ? ',' : ';';
    }
}