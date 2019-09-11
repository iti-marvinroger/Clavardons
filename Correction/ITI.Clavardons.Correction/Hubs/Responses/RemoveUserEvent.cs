using System;
namespace ITI.Clavardons.Hubs.Responses
{
    /// <summary>
    /// The RemoveUserEvent is sent whenever a user is disconnected (logout or force close)
    /// </summary>
    public struct RemoveUserEvent
    {
        /// <summary>
        /// The ID of the removed user
        /// </summary>
        public string Id;
    }
}
