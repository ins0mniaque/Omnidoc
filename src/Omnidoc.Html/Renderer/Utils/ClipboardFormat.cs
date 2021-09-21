// "Therefore those skilled at the unorthodox
// are infinite as heaven and earth,
// inexhaustible as the great rivers.
// When they come to an end,
// they begin again,
// like the days and months;
// they die and are reborn,
// like the four seasons."
//
// - Sun Tsu,
// "The Art of War"

using System;
using System.Globalization;
using System.Text;

namespace Omnidoc.Html.Renderer.Core.Utils
{
    /// <summary>
    /// Helper to encode and set HTML fragment to clipboard.<br/>
    /// See http://theartofdev.wordpress.com/2012/11/11/setting-html-and-plain-text-formatting-to-clipboard/.<br/>
    /// </summary>
    /// <remarks>
    /// The MIT License (MIT) Copyright (c) 2014 Arthur Teplitzki.
    /// </remarks>
    public static class ClipboardFormat
    {
        /// <summary>
        /// The string contains index references to other spots in the string, so we need placeholders so we can compute the offsets. <br/>
        /// The <![CDATA[<<<<<<<]]>_ strings are just placeholders. We'll back-patch them actual values afterwards. <br/>
        /// The string layout (<![CDATA[<<<]]>) also ensures that it can't appear in the body of the html because the <![CDATA[<]]> <br/>
        /// character must be escaped. <br/>
        /// </summary>
        private const string Header = @"Version:0.9
StartHTML:<<<<<<<<1
EndHTML:<<<<<<<<2
StartFragment:<<<<<<<<3
EndFragment:<<<<<<<<4
StartSelection:<<<<<<<<3
EndSelection:<<<<<<<<4";

        /// <summary>
        /// html comment to point the beginning of html fragment
        /// </summary>
        public const string StartFragment = "<!--StartFragment-->";

        /// <summary>
        /// html comment to point the end of html fragment
        /// </summary>
        public const string EndFragment = @"<!--EndFragment-->";

        /// <summary>
        /// Used to calculate characters byte count in UTF-8
        /// </summary>
        private static readonly char[] _byteCount = new char[1];

        /// <summary>
        /// Generate HTML fragment data string with header that is required for the clipboard.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Builds the CF_HTML header correctly for all possible HTMLs<br/>
        /// If given html contains start/end fragments then it will use them in the header:
        /// <code><![CDATA[<html><body><!--StartFragment-->hello <b>world</b><!--EndFragment--></body></html>]]></code>
        /// If given html contains html/body tags then it will inject start/end fragments to exclude html/body tags:
        /// <code><![CDATA[<html><body>hello <b>world</b></body></html>]]></code>
        /// If given html doesn't contain html/body tags then it will inject the tags and start/end fragments properly:
        /// <code><![CDATA[hello <b>world</b>]]></code>
        /// In all cases creating a proper CF_HTML header:<br/>
        /// <code>
        /// <![CDATA[
        /// Version:1.0
        /// StartHTML:000000177
        /// EndHTML:000000329
        /// StartFragment:000000277
        /// EndFragment:000000295
        /// StartSelection:000000277
        /// EndSelection:000000277
        /// <!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN">
        /// <html><body><!--StartFragment-->hello <b>world</b><!--EndFragment--></body></html>
        /// ]]>
        /// </code>
        /// See format specification here: http://msdn.microsoft.com/library/default.asp?url=/workshop/networking/clipboard/htmlclipboard.asp
        /// </para>
        /// </remarks>
        /// <param name="html">the html to generate for</param>
        /// <returns>the resulted string</returns>
        public static string Encode(string html)
        {
            if(html is null)
                throw new ArgumentNullException(nameof(html));

            var sb = new StringBuilder();
            sb.AppendLine(Header);
            sb.AppendLine(@"<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.0 Transitional//EN"">");

            // if given html already provided the fragments we won't add them
            int fragmentStart, fragmentEnd;
            int fragmentStartIdx = html.IndexOf(StartFragment, StringComparison.OrdinalIgnoreCase);
            int fragmentEndIdx = html.LastIndexOf(EndFragment, StringComparison.OrdinalIgnoreCase);

            // if html tag is missing add it surrounding the given html (critical)
            int htmlOpenIdx = html.IndexOf("<html", StringComparison.OrdinalIgnoreCase);
            int htmlOpenEndIdx = htmlOpenIdx > -1 ? html.IndexOf('>', htmlOpenIdx) + 1 : -1;
            int htmlCloseIdx = html.LastIndexOf("</html", StringComparison.OrdinalIgnoreCase);

            if (fragmentStartIdx < 0 && fragmentEndIdx < 0)
            {
                int bodyOpenIdx = html.IndexOf("<body", StringComparison.OrdinalIgnoreCase);
                int bodyOpenEndIdx = bodyOpenIdx > -1 ? html.IndexOf('>', bodyOpenIdx) + 1 : -1;

                if (htmlOpenEndIdx < 0 && bodyOpenEndIdx < 0)
                {
                    // the given html doesn't contain html or body tags so we need to add them and place start/end fragments around the given html only
                    sb.Append("<html><body>");
                    sb.Append(StartFragment);
                    fragmentStart = GetByteCount(sb);
                    sb.Append(html);
                    fragmentEnd = GetByteCount(sb);
                    sb.Append(EndFragment);
                    sb.Append("</body></html>");
                }
                else
                {
                    // insert start/end fragments in the proper place (related to html/body tags if exists) so the paste will work correctly
                    int bodyCloseIdx = html.LastIndexOf("</body", StringComparison.OrdinalIgnoreCase);

                    if (htmlOpenEndIdx < 0)
                        sb.Append("<html>");
                    else
                        sb.Append(html, 0, htmlOpenEndIdx);

                    if (bodyOpenEndIdx > -1)
                        sb.Append(html, htmlOpenEndIdx > -1 ? htmlOpenEndIdx : 0, bodyOpenEndIdx - (htmlOpenEndIdx > -1 ? htmlOpenEndIdx : 0));

                    sb.Append(StartFragment);
                    fragmentStart = GetByteCount(sb);

                    var innerHtmlStart = bodyOpenEndIdx > -1 ? bodyOpenEndIdx : (htmlOpenEndIdx > -1 ? htmlOpenEndIdx : 0);
                    var innerHtmlEnd = bodyCloseIdx > -1 ? bodyCloseIdx : (htmlCloseIdx > -1 ? htmlCloseIdx : html.Length);
                    sb.Append(html, innerHtmlStart, innerHtmlEnd - innerHtmlStart);

                    fragmentEnd = GetByteCount(sb);
                    sb.Append(EndFragment);

                    if (innerHtmlEnd < html.Length)
                        sb.Append(html, innerHtmlEnd, html.Length - innerHtmlEnd);

                    if (htmlCloseIdx < 0)
                        sb.Append("</html>");
                }
            }
            else
            {
                // handle html with existing start\end fragments just need to calculate the correct bytes offset (surround with html tag if missing)
                if (htmlOpenEndIdx < 0)
                    sb.Append("<html>");
                int start = GetByteCount(sb);
                sb.Append(html);
                fragmentStart = start + GetByteCount(sb, start, start + fragmentStartIdx) + StartFragment.Length;
                fragmentEnd = start + GetByteCount(sb, start, start + fragmentEndIdx);
                if (htmlCloseIdx < 0)
                    sb.Append("</html>");
            }

            // Back-patch offsets (scan only the header part for performance)
            sb.Replace("<<<<<<<<1", Header.Length.ToString("D9", CultureInfo.InvariantCulture), 0, Header.Length);
            sb.Replace("<<<<<<<<2", GetByteCount(sb).ToString("D9", CultureInfo.InvariantCulture), 0, Header.Length);
            sb.Replace("<<<<<<<<3", fragmentStart.ToString("D9", CultureInfo.InvariantCulture), 0, Header.Length);
            sb.Replace("<<<<<<<<4", fragmentEnd.ToString("D9", CultureInfo.InvariantCulture), 0, Header.Length);

            return sb.ToString();
        }

        /// <summary>
        /// Calculates the number of bytes produced by encoding the string in the string builder in UTF-8 and not .NET default string encoding.
        /// </summary>
        /// <param name="sb">the string builder to count its string</param>
        /// <param name="start">optional: the start index to calculate from (default - start of string)</param>
        /// <param name="end">optional: the end index to calculate to (default - end of string)</param>
        /// <returns>the number of bytes required to encode the string in UTF-8</returns>
        private static int GetByteCount(StringBuilder sb, int start = 0, int end = -1)
        {
            int count = 0;
            end = end > -1 ? end : sb.Length;
            for (int i = start; i < end; i++)
            {
                _byteCount[0] = sb[i];
                count += Encoding.UTF8.GetByteCount(_byteCount);
            }
            return count;
        }
    }
}