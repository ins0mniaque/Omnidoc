using System;
using System.Globalization;

namespace Omnidoc.Html.Renderer.Core.Entities
{
    /// <summary>
    /// Raised when an error occurred during html rendering.
    /// </summary>
    public sealed class HtmlRenderErrorEventArgs : EventArgs
    {

        /// <summary>
        /// the exception that occurred (can be null)
        /// </summary>
        private readonly Exception? _exception;

        /// <summary>
        /// Init.
        /// </summary>
        /// <param name="type">the type of error to report</param>
        /// <param name="message">the error message</param>
        /// <param name="exception">optional: the exception that occurred</param>
        public HtmlRenderErrorEventArgs(HtmlRenderErrorType type, string message, Exception? exception = null)
        {
            Type = type;
            Message = message;
            _exception = exception;
        }

        /// <summary>
        /// error type that is reported
        /// </summary>
        public HtmlRenderErrorType Type { get; }

        /// <summary>
        /// the error message
        /// </summary>
        public string Message { get; }

        /// <summary>
        /// the exception that occurred (can be null)
        /// </summary>
        public Exception? Exception => _exception;

        public override string ToString() => string.Format(CultureInfo.InvariantCulture, "Type: {0}", Type);
    }
}