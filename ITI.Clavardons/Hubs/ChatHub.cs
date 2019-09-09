using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace ITI.Clavardons.Hubs
{
    public class ChatHub : Hub
    {
        public async Task ConnectWithName(string name)
        {
            await Clients.All.SendAsync("ReceiveMessage", name);
        }

        public async Task ConnectWithToken(string jwt)
        {
            await Clients.All.SendAsync("ReceiveMessage", jwt);
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
