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
        private JWTFactory _jwtFactory = new JWTFactory(new byte[] { 0xDE, 0xAD, 0xBE, 0xEF });
        private static Dictionary<string, string> _userIdPerConnectionId = new Dictionary<string, string>();

        private void connectUser(HubCallerContext context, string userId)
        {
            _userIdPerConnectionId.Add(context.ConnectionId, userId);
        }

        private string getUserID(HubCallerContext context)
        {
            string userId;
            _userIdPerConnectionId.TryGetValue(context.ConnectionId, out userId);

            return userId;
        }

        public async Task<LoginResponse> LoginWithName(string name)
        {
            string userId = Guid.NewGuid().ToString();
            string jwtId = Guid.NewGuid().ToString();

            string jwt = _jwtFactory.Generate(new JWTPayload { JwtID = jwtId, Name = name, Subject = userId });

            connectUser(Context, userId);

            return new LoginResponse { Success = true, Token = jwt, Name = name, UserId = userId };
        }

        public async Task<LoginResponse> LoginWithToken(string jwt)
        {
            if (!_jwtFactory.Verify(jwt))
            {
                return new LoginResponse { Success = false };
            }

            var parsedToken = JWTFactory.Parse(jwt);

            connectUser(Context, parsedToken.Subject);

            return new LoginResponse { Success = true, Token = jwt, Name = parsedToken.Name, UserId = parsedToken.Subject };
        }

        public async Task RenewToken()
        {
        }

        public async Task Logout()
        {
        }

        public async Task SendMessage(string text)
        {
            var userId = getUserID(Context);

            if (userId == null)
            {
                return;
            }

            string messageId = Guid.NewGuid().ToString();

            await Clients.All.SendAsync("ReceiveMessage", new MessageEvent { Id = messageId, Text = text, UserId = userId });
        }

        public async Task UpdateIsWriting(bool writing)
        {
        }
    }
}
