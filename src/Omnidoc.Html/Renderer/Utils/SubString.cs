using System;
using System.Globalization;

namespace Omnidoc.Html.Renderer.Core.Utils
{
    /// <summary>
    /// Represents sub-string of a full string starting at specific location with a specific length.
    /// </summary>
    internal sealed class SubString
    {
        public static SubString Empty { get; } = new SubString(string.Empty);

        /// <summary>
        /// Init sub-string that is the full string.
        /// </summary>
        /// <param name="fullString">the full string that this sub-string is part of</param>
        public SubString(string fullString)
        {
            ArgChecker.AssertArgNotNull(fullString, "fullString");

            FullString = fullString;
            StartIdx = 0;
            Length = fullString.Length;
        }

        /// <summary>
        /// Init.
        /// </summary>
        /// <param name="fullString">the full string that this sub-string is part of</param>
        /// <param name="startIdx">the start index of the sub-string</param>
        /// <param name="length">the length of the sub-string starting at <paramref name="startIdx"/></param>
        /// <exception cref="ArgumentNullException"><paramref name="fullString"/> is null</exception>
        public SubString(string fullString, int startIdx, int length)
        {
            ArgChecker.AssertArgNotNull(fullString, "fullString");
            if (startIdx < 0 || startIdx >= fullString.Length)
                throw new ArgumentOutOfRangeException(nameof(startIdx), "Must within fullString boundries");
            if (length < 0 || startIdx + length > fullString.Length)
                throw new ArgumentOutOfRangeException(nameof(length), "Must within fullString boundries");

            FullString = fullString;
            StartIdx = startIdx;
            Length = length;
        }

        /// <summary>
        /// the full string that this sub-string is part of
        /// </summary>
        public string FullString { get; }

        /// <summary>
        /// the start index of the sub-string
        /// </summary>
        public int StartIdx { get; }

        /// <summary>
        /// the length of the sub-string starting at <see cref="StartIdx"/>
        /// </summary>
        public int Length { get; }

        /// <summary>
        /// Get string char at specific index.
        /// </summary>
        /// <param name="idx">the idx to get the char at</param>
        /// <returns>char at index</returns>
        public char this[int idx]
        {
            get
            {
                if (idx < 0 || idx > Length )
                    throw new ArgumentOutOfRangeException(nameof(idx), "must be within the string range");
                return FullString[StartIdx + idx];
            }
        }

        /// <summary>
        /// Is the sub-string is empty string.
        /// </summary>
        /// <returns>true - empty string, false - otherwise</returns>
        public bool IsEmpty() => Length < 1;

        /// <summary>
        /// Is the sub-string is empty string or contains only whitespaces.
        /// </summary>
        /// <returns>true - empty or whitespace string, false - otherwise</returns>
        public bool IsEmptyOrWhitespace()
        {
            for (var i = 0; i < Length; i++)
            {
                if (!char.IsWhiteSpace(FullString, StartIdx + i))
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Is the sub-string contains only whitespaces (at least one).
        /// </summary>
        /// <returns>true - empty or whitespace string, false - otherwise</returns>
        public bool IsWhitespace()
        {
            if ( Length < 1)
                return false;
            for (var i = 0; i < Length; i++)
            {
                if (!char.IsWhiteSpace(FullString, StartIdx + i))
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Get a string of the sub-string.<br/>
        /// This will create a new string object!
        /// </summary>
        /// <returns>new string that is the sub-string represented by this instance</returns>
        public string CutSubstring() => Length > 0 ? FullString.Substring(StartIdx, Length ) : string.Empty;

        /// <summary>
        /// Retrieves a substring from this instance. The substring starts at a specified character position and has a specified length.
        /// </summary>
        /// <param name="startIdx">The zero-based starting character position of a substring in this instance.</param>
        /// <param name="length">The number of characters in the substring. </param>
        /// <returns>A String equivalent to the substring of length that begins at startIndex in this instance, or
        /// Empty if startIndex is equal to the length of this instance and length is zero. </returns>
        public string Substring(int startIdx, int length)
        {
            if (startIdx < 0 || startIdx > Length )
                throw new ArgumentOutOfRangeException(nameof(startIdx));
            if (length > Length )
                throw new ArgumentOutOfRangeException(nameof(length));
            if (startIdx + length > Length )
                throw new ArgumentOutOfRangeException(nameof(length));

            return FullString.Substring(StartIdx + startIdx, length);
        }

        public override string ToString() => string.Format(CultureInfo.InvariantCulture, "Sub-string: {0}", Length > 0 ? FullString.Substring(StartIdx, Length ) : string.Empty);
    }
}