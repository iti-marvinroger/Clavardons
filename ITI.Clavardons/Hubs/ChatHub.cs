using System;
using System.Threading.Tasks;
using ITI.Clavardons.Hubs.Responses;
using ITI.Clavardons.Libraries;
using Microsoft.AspNetCore.SignalR;

namespace ITI.Clavardons.Hubs
{
    public class ChatHub : Hub
    {
        private JWTFactory _jwtFactory = new JWTFactory(new byte[] { 0xDE, 0xAD, 0xBE, 0xEF });

        public async Task<LoginResponse> LoginWithName(string name)
        {
            string userId = Guid.NewGuid().ToString();
            string jwtId = Guid.NewGuid().ToString();

            string jwt = _jwtFactory.Generate(new JWTPayload { JwtID = jwtId, Name = name, Subject = userId });

            // await Clients.All.SendAsync("ReceiveMessage", name);

            return new LoginResponse { Success = true, Token = jwt, Name = name, UserId = userId };
        }

        public async Task<LoginResponse> LoginWithToken(string jwt)
        {
            if (!_jwtFactory.Verify(jwt))
            {
                return new LoginResponse { Success = false };
            }

            var parsedToken = JWTFactory.Parse(jwt);

            return new LoginResponse { Success = true, Token = jwt, Name = parsedToken.Name, UserId = parsedToken.Subject };
        }

        public async Task RenewToken()
        {
        }

        public async Task Logout()
        {
        }

        public async Task SendMessage(string message)
        {
        }

        public async Task UpdateIsWriting(bool writing)
        {
        }
    }
}
