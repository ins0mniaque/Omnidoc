using System;
using System.Diagnostics.CodeAnalysis;

using Omnidoc.Model;

namespace Omnidoc.Core.Errors
{
    public static class ThrowError
    {
        [ DoesNotReturn ] public static void ArchiveEntryNotFound  ( int     index   ) => throw new ArgumentException ( $"Archive does not contain entry { index }", nameof ( index ) );
        [ DoesNotReturn ] public static void ArchiveEntryNotFound  ( string  path    ) => throw new ArgumentException ( $"Archive does not contain entry { path }",  nameof ( path  ) );
        [ DoesNotReturn ] public static void FailedToAddElement    ( Element element ) => throw new ArgumentException ( $"Failed to add { element?.GetType ( ).Name } to page",      nameof ( element ) );
        [ DoesNotReturn ] public static void FailedToRemoveElement ( Element element ) => throw new ArgumentException ( $"Failed to remove { element?.GetType ( ).Name } from page", nameof ( element ) );
    }
}