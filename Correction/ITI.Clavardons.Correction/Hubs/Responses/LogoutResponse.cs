using System;
namespace ITI.Clavardons.Hubs.Responses
{
    /// <summary>
    /// A LogoutResponse is sent in response to a logout.
    /// </summary>
    public struct LogoutResponse
    {
        /// <summary>
        /// Is the logout a success?
        /// </summary>
        public bool Success;
    }
}
