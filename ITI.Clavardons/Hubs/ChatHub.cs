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
        const string CONNECTED_GROUP = "connected";

        private static JWTFactory _jwtFactory = new JWTFactory(new byte[] { 0xDE, 0xAD, 0xBE, 0xEF });
        private static Dictionary<string, JWTPayload> _jwtPayloadPerConnectionId = new Dictionary<string, JWTPayload>();
        private static HashSet<string> _revokedTokenIds = new HashSet<string>();

        private async Task connectUser(HubCallerContext context, JWTPayload payload)
        {
            await Clients.Group(CONNECTED_GROUP).SendAsync("AddUser", new NewUserEvent { Id = payload.Subject, Name = payload.Name });

            foreach (var item in _jwtPayloadPerConnectionId)
            {
                await Clients.Caller.SendAsync("AddUser", new NewUserEvent { Id = item.Value.Subject, Name = item.Value.Name });
            }

            _jwtPayloadPerConnectionId.Add(context.ConnectionId, payload);

            await Groups.AddToGroupAsync(Context.ConnectionId, CONNECTED_GROUP);
        }

        private JWTPayload? getUserInfo(HubCallerContext context)
        {
            JWTPayload payload;
            _jwtPayloadPerConnectionId.TryGetValue(context.ConnectionId, out payload);

            return payload;
        }

        public async Task<LoginResponse> LoginWithName(string name)
        {
            var userInfo = getUserInfo(Context);

            if (userInfo.Value.JwtID != null)
            {
                return new LoginResponse { Success = false };
            }

            string userId = Guid.NewGuid().ToString();
            string jwtId = Guid.NewGuid().ToString();

            var payload = new JWTPayload { JwtID = jwtId, Name = name, Subject = userId };
            string jwt = _jwtFactory.Generate(payload);

            await connectUser(Context, payload);

            return new LoginResponse { Success = true, Token = jwt, Name = name, UserId = userId };
        }

        public async Task<LoginResponse> LoginWithToken(string jwt)
        {
            var userInfo = getUserInfo(Context);

            if (userInfo.Value.JwtID != null)
            {
                return new LoginResponse { Success = false };
            }

            if (!_jwtFactory.Verify(jwt))
            {
                return new LoginResponse { Success = false };
            }

            var parsedToken = JWTFactory.Parse(jwt);

            if (_revokedTokenIds.Contains(parsedToken.JwtID))
            {
                return new LoginResponse { Success = false };
            }

            await connectUser(Context, parsedToken);

            return new LoginResponse { Success = true, Token = jwt, Name = parsedToken.Name, UserId = parsedToken.Subject };
        }

        public async Task<LogoutResponse> Logout()
        {
            var userInfo = getUserInfo(Context);

            if (userInfo.Value.JwtID == null)
            {
                return new LogoutResponse { Success = false };
            }

            _revokedTokenIds.Add(userInfo.Value.JwtID);
            _jwtPayloadPerConnectionId.Remove(Context.ConnectionId);
            await Clients.Group(CONNECTED_GROUP).SendAsync("RemoveUser", new RemoveUserEvent { Id = userInfo.Value.Subject });
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, CONNECTED_GROUP);


            return new LogoutResponse { Success = true };
        }

        public async Task SendMessage(string text)
        {
            var userInfo = getUserInfo(Context);

            if (userInfo.Value.JwtID == null)
            {
                return;
            }

            string messageId = Guid.NewGuid().ToString();

            await Clients.Group(CONNECTED_GROUP).SendAsync("ReceiveMessage", new MessageEvent { Id = messageId, Text = text, UserId = userInfo.Value.Subject, UserName = userInfo.Value.Name });
        }
    }
}
