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
        private static Dictionary<string, JWTPayload> _jwtPayloadPerConnectionId = new Dictionary<string, JWTPayload>();

        private void connectUser(HubCallerContext context, JWTPayload payload)
        {
            _jwtPayloadPerConnectionId.Add(context.ConnectionId, payload);
        }

        private JWTPayload? getUserInfo(HubCallerContext context)
        {
            JWTPayload payload;
            _jwtPayloadPerConnectionId.TryGetValue(context.ConnectionId, out payload);

            return payload;
        }

        public async Task<LoginResponse> LoginWithName(string name)
        {
            string userId = Guid.NewGuid().ToString();
            string jwtId = Guid.NewGuid().ToString();

            var payload = new JWTPayload { JwtID = jwtId, Name = name, Subject = userId };
            string jwt = _jwtFactory.Generate(payload);

            connectUser(Context, payload);

            return new LoginResponse { Success = true, Token = jwt, Name = name, UserId = userId };
        }

        public async Task<LoginResponse> LoginWithToken(string jwt)
        {
            if (!_jwtFactory.Verify(jwt))
            {
                return new LoginResponse { Success = false };
            }

            var parsedToken = JWTFactory.Parse(jwt);

            connectUser(Context, parsedToken);

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
            var userInfo = getUserInfo(Context);

            if (userInfo == null)
            {
                return;
            }

            string messageId = Guid.NewGuid().ToString();

            await Clients.All.SendAsync("ReceiveMessage", new MessageEvent { Id = messageId, Text = text, UserId = userInfo.Value.Subject, UserName = userInfo.Value.Name });
        }

        public async Task UpdateIsWriting(bool writing)
        {
        }
    }
}
