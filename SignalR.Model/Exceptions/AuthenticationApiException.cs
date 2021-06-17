using System;
using System.Net;
using SignalR.Model.Authorization;

namespace SignalR.Model.Exceptions
{
    public class AuthenticationApiException : Exception
    {
        public string StatusCode { get; set; }
        public string StatusMessage { get; set; }
        public string MessageResponse { get; set; }
        public string OriginalMessage { get; set; }
        public ApiError ApiError { get; set; }

        public AuthenticationApiException(string message, HttpStatusCode statusCode, string statusDescription) : this(message, statusCode, statusDescription, string.Empty)
        {
        }        
        
        public AuthenticationApiException(string message, HttpStatusCode statusCode, string statusDescription, string originalMessage) : this(message, statusCode, statusDescription, originalMessage, null)
        {
        }

        public AuthenticationApiException(string message, HttpStatusCode statusCode, string statusDescription, string originalMessage, ApiError apiError) : base(message)
        {
            OriginalMessage = originalMessage;
            ApiError = apiError;
        }
    }
}