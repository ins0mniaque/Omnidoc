using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;

namespace Omnidoc.Dynamic
{
    public class DynamicDictionary : DynamicObject, IDictionary < string, object? >
    {
        private readonly IDictionary < string, object? > dictionary = new Dictionary < string, object? > ( StringComparer.InvariantCultureIgnoreCase );

        public override bool TryGetMember ( GetMemberBinder binder, out object? result )
        {
            if ( binder is null )
                throw new ArgumentNullException ( nameof ( binder ) );

            if ( ! dictionary.TryGetValue ( binder.Name, out result ) )
                result = null;

            return true;
        }

        public override bool TrySetMember ( SetMemberBinder binder, object? value )
        {
            if ( binder is null )
                throw new ArgumentNullException ( nameof ( binder ) );

            dictionary [ binder.Name ] = value;

            return true;
        }

        public override bool TryInvokeMember ( InvokeMemberBinder binder, object [ ] args, out object? result )
        {
            if ( binder is null )
                throw new ArgumentNullException ( nameof ( binder ) );

            if ( dictionary.TryGetValue ( binder.Name, out var value ) && value is Delegate @delegate )
            {
                result = @delegate.DynamicInvoke ( args );

                return true;
            }

            return base.TryInvokeMember ( binder, args, out result );
        }

        public int  Count      => dictionary.Count;
        public bool IsReadOnly => dictionary.IsReadOnly;

        public ICollection < string  > Keys   => dictionary.Keys;
        public ICollection < object? > Values => dictionary.Values;

        public object? this [ string key ]
        {
            get => dictionary.TryGetValue ( key, out var value ) ? value : null;
            set => dictionary [ key ] = value;
        }

        public void Add         ( string key, object? value )     => dictionary.Add         ( key, value );
        public bool TryGetValue ( string key, out object? value ) => dictionary.TryGetValue ( key, out value );
        public bool ContainsKey ( string key ) => dictionary.ContainsKey ( key );
        public bool Remove      ( string key ) => dictionary.Remove      ( key );
        public void Clear       ( )            => dictionary.Clear       ( );

        public void Add      ( KeyValuePair < string, object? > item ) => dictionary.Add      ( item );
        public bool Contains ( KeyValuePair < string, object? > item ) => dictionary.Contains ( item );
        public bool Remove   ( KeyValuePair < string, object? > item ) => dictionary.Remove   ( item );
        public void CopyTo   ( KeyValuePair < string, object? > [ ] array, int arrayIndex ) => dictionary.CopyTo ( array, arrayIndex );

        public IEnumerator < KeyValuePair < string, object? > > GetEnumerator ( ) => dictionary.GetEnumerator ( );
        IEnumerator                                 IEnumerable.GetEnumerator ( ) => dictionary.GetEnumerator ( );
    }
}