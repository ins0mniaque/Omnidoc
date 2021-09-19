using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

using Omnidoc.Model;

namespace Omnidoc.Core.Internal
{
    public static class ThrowError
    {
        [ DoesNotReturn ] public static void ArchiveEntryNotFound  ( int     index   ) => throw new ArgumentException ( string.Format ( CultureInfo.InvariantCulture, Strings.Error_ArchiveEntryNotFound, index ), nameof ( index ) );
        [ DoesNotReturn ] public static void ArchiveEntryNotFound  ( string  path    ) => throw new ArgumentException ( string.Format ( CultureInfo.InvariantCulture, Strings.Error_ArchiveEntryNotFound, path ), nameof ( path ) );
        [ DoesNotReturn ] public static void FailedToAddElement    ( Element element ) => throw new ArgumentException ( string.Format ( CultureInfo.InvariantCulture, Strings.Error_FailedToAddElement, element?.GetType ( ).Name ), nameof ( element ) );
        [ DoesNotReturn ] public static void FailedToRemoveElement ( Element element ) => throw new ArgumentException ( string.Format ( CultureInfo.InvariantCulture, Strings.Error_FailedToRemoveElement, element?.GetType ( ).Name ), nameof ( element ) );
    }
}