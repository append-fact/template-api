using System.Globalization;
using System.Net;

namespace Application.Common.Exceptions
{
    public class ApiException : Exception
    {
        private const int DefaultStatusCode = (int)HttpStatusCode.BadRequest;

        public int StatusCode { get; } = DefaultStatusCode;
        public ApiException() : base() { }
        public ApiException(string message) : base(message) { }
        public ApiException(string message, params object[] args) : base(string.Format(CultureInfo.CurrentCulture, message, args)) { }
        

        public ApiException(string message, int statusCode)
            : base(message)
        {
            StatusCode = statusCode;
        }
    }



}