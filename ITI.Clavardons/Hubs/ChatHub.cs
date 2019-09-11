using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ITI.Clavardons.Hubs.Responses;
using ITI.Clavardons.Libraries;
using Microsoft.AspNetCore.SignalR;

namespace ITI.Clavardons.Hubs
{
    public class ChatHub : Hub
    {
        private static JWTFactory _jwtFactory = new JWTFactory(new byte[] { 0xDE, 0xAD, 0xBE, 0xEF });

        /// <summary>
        /// Called when the client is disconnected, cleanly or not.
        /// </summary>
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await base.OnDisconnectedAsync(exception);
        }

        /// <summary>
        /// Login the current client with a name.
        /// </summary>
        /// <param name="name">The name to login with</param>
        /// <returns>A LoginResponse with all data</returns>
        public async Task<LoginResponse> LoginWithName(string name)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Login the current client with a JWT.
        /// </summary>
        /// <param name="jwt">The JWT to login with</param>
        /// <returns>A LoginResponse with all data</returns>
        public async Task<LoginResponse> LoginWithToken(string jwt)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Logout a user, revoking its JWT.
        /// </summary>
        /// <returns>A LogoutResponse with all data</returns>
        public async Task<LogoutResponse> Logout()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Send a messager to all users.
        /// </summary>
        /// <param name="text">The text of the message</param>
        public async Task SendMessage(string text)
        {
            throw new NotImplementedException();
        }
    }
}
