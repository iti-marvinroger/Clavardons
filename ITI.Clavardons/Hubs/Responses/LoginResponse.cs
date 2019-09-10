using System;
namespace ITI.Clavardons.Hubs.Responses
{
    public struct LoginResponse
    {
        public bool Success;
        public string Token;
        public string Name;
        public string UserId;
    }
}
