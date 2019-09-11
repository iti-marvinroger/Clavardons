using System;
namespace ITI.Clavardons.Hubs.Responses
{
    /// <summary>
    /// The NewUserEvent is sent whenever a user is connected
    /// </summary>
    public struct NewUserEvent
    {
        /// <summary>
        /// The ID of the newly connected user
        /// </summary>
        public string Id;
        /// <summary>
        /// THe name of the newly connected user
        /// </summary>
        public string Name;
    }
}
