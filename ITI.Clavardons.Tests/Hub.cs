﻿using System;
using Microsoft.AspNetCore.SignalR.Client;
using System.Net.Http;
using System.Threading.Tasks;
using ITI.Clavardons.Tests.Helpers;
using NUnit.Framework;
using FluentAssertions;
using ITI.Clavardons.Hubs.Responses;
using ITI.Clavardons.Libraries;

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

            // Assert
            login11.Success.Should().Be(true);
            login11.Name.Should().Be("David GUETTA");
            var parsedToken = JWTFactory.Parse(login11.Token);
            parsedToken.Name.Should().Be("David GUETTA");
            parsedToken.Subject.Should().NotBeNullOrWhiteSpace();

            // the second attempt should not be a success
            login12.Success.Should().Be(false);

            var login21 = await connection2.InvokeAsync<LoginResponse>("LoginWithName", "Polo");
            var parsedToken2 = JWTFactory.Parse(login21.Token);
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
            //string user = null;
            //string message = null;
            //connection.On<string, string>("OnReceiveMessage", (u, m) =>
            //{
            //    user = u;
            //    message = m;
            //});

            var loginWithNameRes = await connection1.InvokeAsync<LoginResponse>("LoginWithName", "David GUETTA");
            await connection1.DisposeAsync();
            await Task.Delay(500);
            var jwt = loginWithNameRes.Token;
            var parsedToken1 = JWTFactory.Parse(jwt);
            var loginWithTokenRes = await connection2.InvokeAsync<LoginResponse>("LoginWithToken", jwt);
 
            // Assert
            loginWithTokenRes.Success.Should().Be(true);
            loginWithTokenRes.Name.Should().Be(parsedToken1.Name);
            var parsedToken2 = JWTFactory.Parse(loginWithTokenRes.Token);
            parsedToken2.Name.Should().Be(parsedToken1.Name);
            parsedToken2.Subject.Should().Be(parsedToken1.Subject);
        }

        [Test]
        public async Task T3_CheckLogout()
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
        public async Task T4_CheckSpam()
        {
            // Arrange
            bool IsSpamming = false;
            _factory.CreateClient(); // need to create a client for the server property to be available
            var server = _factory.Server;

            var connection1 = await StartConnectionAsync(server.CreateHandler());

            connection1.On("StopSpamming", () =>
            {
                IsSpamming = true;
            });

            var loginWithNameRes = await connection1.InvokeAsync<LoginResponse>("LoginWithName", "David GUETTA");;
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


    }
}
