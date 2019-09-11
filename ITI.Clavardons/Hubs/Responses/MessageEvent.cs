using System;
namespace ITI.Clavardons.Hubs.Responses
{
    /// <summary>
    /// A MessageEvent is sent to everyone whenever a message is sent.
    /// </summary>
    public struct MessageEvent
    {
        /// <summary>
        /// Unique ID of the message
        /// </summary>
        public string Id;
        /// <summary>
        /// User ID of the user sending the message
        /// </summary>
        public string UserId;
        /// <summary>
        /// User name of the user sending the message
        /// </summary>
        public string UserName;
        /// <summary>
        /// Text of the message
        /// </summary>
        public string Text;
    }
}
