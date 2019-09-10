using System;
using Microsoft.AspNetCore.SignalR.Client;
using System.Net.Http;
using System.Threading.Tasks;
using ITI.Clavardons.Tests.Helpers;
using NUnit.Framework;
using FluentAssertions;
using ITI.Clavardons.Hubs.Responses;
using ITI.Clavardons.Libraries;
using System.Collections.Generic;

namespace Tests
{
    public class TestHub
    {
        private readonly WebApiFactory _factory = new WebApiFactory();

        [SetUp]
        public void Setup()
        {
        }

        private static async Task<HubConnection> StartConnectionAsync(HttpMessageHandler handler)
        {
            var hubConnection = new HubConnectionBuilder()
                .WithUrl($"ws://localhost/", o =>
                {
                    o.HttpMessageHandlerFactory = _ => handler;
                })
                .Build();

            await hubConnection.StartAsync();

            hubConnection.Closed += async error =>
            {
                await Task.Delay(new Random().Next(0, 2) * 1000);
                await hubConnection.StartAsync();
            };

            return hubConnection;
        }

        [Test]
        public async Task T1_CheckLoginWithName()
        {
            // Arrange
            _factory.CreateClient(); // need to create a client for the server property to be available
            var server = _factory.Server;

            var connection1 = await StartConnectionAsync(server.CreateHandler());
            var connection2 = await StartConnectionAsync(server.CreateHandler());

            // Act
            var login11 = await connection1.InvokeAsync<LoginResponse>("LoginWithName", "David GUETTA");
            var login12 = await connection1.InvokeAsync<LoginResponse>("LoginWithName", "John DOE");

            var login21 = await connection2.InvokeAsync<LoginResponse>("LoginWithName", "Polo");

            await connection1.StopAsync();
            await connection2.StopAsync();

            // Assert
            login11.Success.Should().Be(true);
            login11.Name.Should().Be("David GUETTA");
            var parsedToken = JWTFactory.Parse(login11.Token);
            parsedToken.Name.Should().Be("David GUETTA");
            parsedToken.Subject.Should().NotBeNullOrWhiteSpace();

            // if a user is already connected, we won't accept a new login attempt
            login12.Success.Should().Be(false);

            var parsedToken2 = JWTFactory.Parse(login21.Token);
            // the subject must be different for every user
            parsedToken2.Subject.Should().NotBe(parsedToken.Subject);
        }

        [Test]
        public async Task T2_CheckLoginWithToken()
        {
            // Arrange
            _factory.CreateClient(); // need to create a client for the server property to be available
            var server = _factory.Server;

            var connection1 = await StartConnectionAsync(server.CreateHandler());
            var connection2 = await StartConnectionAsync(server.CreateHandler());

            // Act
            var loginWithNameRes = await connection1.InvokeAsync<LoginResponse>("LoginWithName", "David GUETTA");
            await connection1.StopAsync();
            await Task.Delay(500);
            var jwt = loginWithNameRes.Token;
            var parsedToken1 = JWTFactory.Parse(jwt);
            var loginWithTokenRes = await connection2.InvokeAsync<LoginResponse>("LoginWithToken", jwt);

            await connection2.StopAsync();

            // Assert
            loginWithTokenRes.Success.Should().Be(true);
            loginWithTokenRes.Name.Should().Be(parsedToken1.Name);
            var parsedToken2 = JWTFactory.Parse(loginWithTokenRes.Token);
            parsedToken2.Name.Should().Be(parsedToken1.Name);
            parsedToken2.Subject.Should().Be(parsedToken1.Subject);
        }

        [Test]
        public async Task T3_SendMessage()
        {
            // Arrange
            _factory.CreateClient(); // need to create a client for the server property to be available
            var server = _factory.Server;

            var user1 = await StartConnectionAsync(server.CreateHandler());
            var user2 = await StartConnectionAsync(server.CreateHandler());
            var user3 = await StartConnectionAsync(server.CreateHandler());

            // Act
            MessageEvent? messageUser1 = null;
            user1.On<MessageEvent>("ReceiveMessage", (message) =>
            {
                messageUser1 = message;
            });

            MessageEvent? messageUser2 = null;
            user2.On<MessageEvent>("ReceiveMessage", (message) =>
            {
                messageUser2 = message;
            });

            MessageEvent? messageUser3 = null;
            user3.On<MessageEvent>("ReceiveMessage", (message) =>
            {
                messageUser3 = message;
            });

            var user1LoginRes = await user1.InvokeAsync<LoginResponse>("LoginWithName", "David GUETTA");
            await user2.InvokeAsync<LoginResponse>("LoginWithName", "Philou");
            await user1.InvokeAsync("SendMessage", "Coucou !");

            await user1.StopAsync();
            await user2.StopAsync();

            await Task.Delay(500);

            // Assert

            messageUser1.Should().NotBeNull();
            messageUser1.Value.Text.Should().Be("Coucou !");
            messageUser1.Value.UserName.Should().Be(user1LoginRes.Name);
            messageUser1.Value.UserId.Should().Be(user1LoginRes.UserId);

            messageUser2.Should().NotBeNull();
            messageUser2.Value.Text.Should().Be("Coucou !");
            messageUser2.Value.UserName.Should().Be(user1LoginRes.Name);
            messageUser2.Value.UserId.Should().Be(user1LoginRes.UserId);

            // this user is not connected, he should not receive messages
            messageUser3.Should().BeNull();
        }

        [Test]
        public async Task T4_CheckLogout()
        {
            // Arrange
            _factory.CreateClient(); // need to create a client for the server property to be available
            var server = _factory.Server;

            var connection1 = await StartConnectionAsync(server.CreateHandler());
            var connection2 = await StartConnectionAsync(server.CreateHandler());

            var loginWithNameRes = await connection1.InvokeAsync<LoginResponse>("LoginWithName", "David GUETTA");
            var logout = await connection1.InvokeAsync<LogoutResponse>("Logout");

            var jwt = loginWithNameRes.Token;
            var loginWithTokenRes = await connection2.InvokeAsync<LoginResponse>("LoginWithToken", jwt);

            await connection1.StopAsync();
            await connection2.StopAsync();
            //When the user logout, we should get a success at true
            logout.Success.Should().Be(true);
            //When we try to login with the token with which we disconnected, it should fail
            loginWithTokenRes.Success.Should().Be(false);
        }

        [Test]
        public async Task T5_CheckSpam()
        {
            // Arrange
            _factory.CreateClient(); // need to create a client for the server property to be available
            var server = _factory.Server;

            var connection1 = await StartConnectionAsync(server.CreateHandler());

            bool IsSpamming = false;
            connection1.On("StopSpamming", () =>
            {
                IsSpamming = true;
            });

            var loginWithNameRes = await connection1.InvokeAsync<LoginResponse>("LoginWithName", "David GUETTA"); ;
            await connection1.SendAsync("SendMessage", "je");
            await connection1.SendAsync("SendMessage", "suis");
            await connection1.SendAsync("SendMessage", "en");
            await connection1.SendAsync("SendMessage", "train");
            await connection1.SendAsync("SendMessage", "de");
            await connection1.SendAsync("SendMessage", "spam");
            await connection1.SendAsync("SendMessage", "le");
            await connection1.SendAsync("SendMessage", "chat");
            await connection1.SendAsync("SendMessage", "de");
            await connection1.SendAsync("SendMessage", "plusieurs");
            await connection1.SendAsync("SendMessage", "messages");
            await Task.Delay(500);
            await connection1.StopAsync();
            //After sending more than 10 messages in less than 10 seconds, I should receive the OnSpamming event
            IsSpamming.Should().Be(true);

        }

        [Test]
        public async Task T6_SendUserList()
        {
            _factory.CreateClient(); // need to create a client for the server property to be available
            var server = _factory.Server;

            var user1 = await StartConnectionAsync(server.CreateHandler());
            var user2 = await StartConnectionAsync(server.CreateHandler());

            // Act
            List<NewUserEvent> newUsers = new List<NewUserEvent>();
            user2.On<NewUserEvent>("AddUser", (newUser) =>
            {
                newUsers.Add(newUser);
            });

            var user1LoginRes = await user1.InvokeAsync<LoginResponse>("LoginWithName", "David GUETTA");
            var user2LoginRes = await user2.InvokeAsync<LoginResponse>("LoginWithName", "Marvin ROGER");

            await user1.StopAsync();
            await user2.StopAsync();

            await Task.Delay(500);

            newUsers.Should().HaveCount(1);
            newUsers.Should().Contain((item) => item.Name == user1LoginRes.Name & item.Id == user1LoginRes.UserId);
        }

        [Test]
        public async Task T7_CheckUserRemove()
        {
            _factory.CreateClient(); // need to create a client for the server property to be available
            var server = _factory.Server;

            var user1 = await StartConnectionAsync(server.CreateHandler());
            var user2 = await StartConnectionAsync(server.CreateHandler());
            var user3 = await StartConnectionAsync(server.CreateHandler());

            // Act
            List<RemoveUserEvent> removedUsers = new List<RemoveUserEvent>();
            user3.On<RemoveUserEvent>("RemoveUser", (removedUser) =>
            {
                removedUsers.Add(removedUser);
            });

            var user1LoginRes = await user1.InvokeAsync<LoginResponse>("LoginWithName", "Marvin ROGER");
            var user2LoginRes = await user2.InvokeAsync<LoginResponse>("LoginWithName", "Mathieu BOISSADY");
            await user3.InvokeAsync<LoginResponse>("LoginWithName", "Francis CABREL");

            await user1.StopAsync();
            await user2.InvokeAsync<LogoutResponse>("Logout");

            await Task.Delay(500);

            await user2.StopAsync();
            await user3.StopAsync();

            // the third, not disconnected user should receive that both
            // user 1 and 2 were removed, because the first one closed its connection
            // and the second one explicitely logout'd
            removedUsers.Should().HaveCount(2);
            removedUsers.Should().Contain((item) => item.Id == user1LoginRes.UserId);
            removedUsers.Should().Contain((item) => item.Id == user2LoginRes.UserId);
        }
    }
}
