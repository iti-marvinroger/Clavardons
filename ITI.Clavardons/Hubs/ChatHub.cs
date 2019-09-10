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
        const int ANTI_SPAM_MESSAGES_PER_WINDOW = 10;
        readonly TimeSpan ANTI_SPAM_WINDOW = TimeSpan.FromSeconds(10);

        private static JWTFactory _jwtFactory = new JWTFactory(new byte[] { 0xDE, 0xAD, 0xBE, 0xEF });
        private static Dictionary<string, JWTPayload> _jwtPayloadPerConnectionId = new Dictionary<string, JWTPayload>();
        private static HashSet<string> _revokedTokenIds = new HashSet<string>();
        private static Dictionary<string, JWTPayload> _connectedUsers = new Dictionary<string, JWTPayload>();
        private static Dictionary<string, AntiSpam> _antiSpamPerUser = new Dictionary<string, AntiSpam>();

        private async Task<LoginResponse> connectUser(HubCallerContext context, JWTPayload payload, string jwt)
        {
            if (_connectedUsers.ContainsKey(payload.Subject)) {
                return new LoginResponse { Success = false };
            }

            await Clients.Group(CONNECTED_GROUP).SendAsync("AddUser", new NewUserEvent { Id = payload.Subject, Name = payload.Name });

            foreach (var item in _connectedUsers)
            {
                await Clients.Caller.SendAsync("AddUser", new NewUserEvent { Id = item.Value.Subject, Name = item.Value.Name });
            }

            _jwtPayloadPerConnectionId.Add(context.ConnectionId, payload);
            _connectedUsers.Add(payload.Subject, payload);
            _antiSpamPerUser.Add(payload.Subject, new AntiSpam(ANTI_SPAM_WINDOW, ANTI_SPAM_MESSAGES_PER_WINDOW));

            await Groups.AddToGroupAsync(Context.ConnectionId, CONNECTED_GROUP);

            return new LoginResponse { Success = true, Token = jwt, Name = payload.Name, UserId = payload.Subject };
        }

        private async Task handleUserDisconnect(HubCallerContext context)
        {
            var userInfo = getUserInfo(Context);

            if (userInfo.Value.JwtID == null)
            {
                return;
            }

            _connectedUsers.Remove(userInfo.Value.Subject);
            _jwtPayloadPerConnectionId.Remove(Context.ConnectionId);
            _antiSpamPerUser.Remove(userInfo.Value.Subject);
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, CONNECTED_GROUP);
            await Clients.Group(CONNECTED_GROUP).SendAsync("RemoveUser", new RemoveUserEvent { Id = userInfo.Value.Subject });
        }

        private JWTPayload? getUserInfo(HubCallerContext context)
        {
            JWTPayload payload;
            _jwtPayloadPerConnectionId.TryGetValue(context.ConnectionId, out payload);

            return payload;
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await handleUserDisconnect(Context);

            await base.OnDisconnectedAsync(exception);
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

            return await connectUser(Context, payload, jwt);
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

            return await connectUser(Context, parsedToken, jwt);
        }

        public async Task<LogoutResponse> Logout()
        {
            var userInfo = getUserInfo(Context);

            if (userInfo.Value.JwtID == null)
            {
                return new LogoutResponse { Success = false };
            }

            _revokedTokenIds.Add(userInfo.Value.JwtID);
            await handleUserDisconnect(Context);


            return new LogoutResponse { Success = true };
        }

        public async Task SendMessage(string text)
        {
            var userInfo = getUserInfo(Context);

            if (userInfo.Value.JwtID == null)
            {
                return;
            }

            var antiSpam = _antiSpamPerUser[userInfo.Value.Subject];

            if (antiSpam.Check() == false)
            {
                await Clients.Caller.SendAsync("StopSpamming");
                return;
            }

            string messageId = Guid.NewGuid().ToString();

            await Clients.Group(CONNECTED_GROUP).SendAsync("ReceiveMessage", new MessageEvent { Id = messageId, Text = text, UserId = userInfo.Value.Subject, UserName = userInfo.Value.Name });
        }
    }
}
