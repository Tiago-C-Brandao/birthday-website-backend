using System.Net;

namespace BirthdayWebsiteAPI.Exceptions
{
    public abstract class BaseApiException : Exception
    {
        public string ErrorCode { get; }
        public HttpStatusCode StatusCode { get; }

        protected BaseApiException(
            string errorCode,
            string message,
            HttpStatusCode statusCode)
            : base(message)
        {
            ErrorCode = errorCode;
            StatusCode = statusCode;
        }
    }
}
