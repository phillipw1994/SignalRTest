using System;

namespace SignalR.Model.Interfaces.AuthorizationApi
{
    public interface IApiUser
    {
        Guid Uid { get; set; }
        string Email { get; set; }
        string AccessToken { get; set; }
        string RefreshToken { get; set; }
        DateTime TokenExpires { get; set; }
    }
}
