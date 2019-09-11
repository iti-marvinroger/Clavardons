using System;
namespace ITI.Clavardons.Hubs.Responses
{
    /// <summary>
    /// A LoginResponse is sent in response to a login.
    /// </summary>
    public struct LoginResponse
    {
        /// <summary>
        /// Is the login a success?
        /// </summary>
        public bool Success;
        /// <summary>
        /// The JWT if the login is a success
        /// </summary>
        public string Token;
        /// <summary>
        /// The name of the logged in user
        /// </summary>
        public string Name;
        /// <summary>
        /// A unique ID for this user
        /// </summary>
        public string UserId;
    }
}
