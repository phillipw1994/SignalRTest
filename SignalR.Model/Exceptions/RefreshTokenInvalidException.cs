using System;

namespace SignalR.Model.Exceptions
{
    public class RefreshTokenInvalidException : Exception
    {
        public RefreshTokenInvalidException(string message)
            : base(message)
        {
        }
    }
}
