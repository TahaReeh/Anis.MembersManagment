﻿using Azure.Core;
using Microsoft.Azure.Amqp.Framing;
using Microsoft.Identity.Client;
using System;

namespace Anis.MembersManagment.Command.Test.InvitationsServiceTests.Accept
{
    public class AcceptInvitationTest : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public AcceptInvitationTest(WebApplicationFactory<Program> factory, ITestOutputHelper helper)
        {
            _factory = factory.WithDefaultConfigurations(helper, services =>
            {
                services.ReplaceWithInMemoryDatabase();
            });
        }

        [Theory]
        [InlineData("accountId", "SubscriptionId", "MemberId", "UserId")]
        public async Task AcceptInvitation_SendValidRequestWhenPendingInvitationExists_InvitationAcceptedEventSaved(
            string accountId,
            string SubscriptionId,
            string MemberId,
            string UserId
            )
        {
            var client = new Invitations.InvitationsClient(_factory.CreateGrpcChannel());

            var sendRequest = new SendInvitationRequest
            {
                AccountId = accountId,
                SubscriptionId = SubscriptionId,
                MemberId = MemberId,
                UserId = UserId,
                Permissions = new Permissions
                {
                    Transfer = true,
                    PurchaseCards = false,
                    ManageDevices = false
                }
            };

            var sendResponse = await client.SendInvitationAsync(sendRequest);

            var acceptRequest = new AcceptInvitationRequest
            {
                Id = sendResponse.Id,
                AccountId = accountId,
                SubscriptionId = SubscriptionId,
                MemberId = MemberId,
                UserId = UserId,
            };

            await client.AcceptInvitationAsync(acceptRequest);

            using var scope = _factory.Services.CreateScope();
            var eventsStore = scope.ServiceProvider.GetRequiredService<IEventStore>();
            var events = await eventsStore.GetAllAsync(sendResponse.Id, new CancellationToken());

            Assert.Equal(2, events.Count);
        }

        [Theory]
        [InlineData("accountId", "SubscriptionId", "MemberId", "UserId")]
        public async Task AcceptInvitation_AcceptInvitationHasBeenAlreadyAccepted_ThrowsInvalidArgumentRpcException(
            string accountId,
            string SubscriptionId,
            string MemberId,
            string UserId)
        {
            var client = new Invitations.InvitationsClient(_factory.CreateGrpcChannel());

            var sendRequest = new SendInvitationRequest
            {
                AccountId = accountId,
                SubscriptionId = SubscriptionId,
                MemberId = MemberId,
                UserId = UserId,
                Permissions = new Permissions
                {
                    Transfer = true,
                    PurchaseCards = false,
                    ManageDevices = false
                }
            };

            var sendResponse = await client.SendInvitationAsync(sendRequest);

            var acceptRequest = new AcceptInvitationRequest
            {
                Id = sendResponse.Id,
                AccountId = accountId,
                SubscriptionId = SubscriptionId,
                MemberId = MemberId,
                UserId = UserId,
            };

            await client.AcceptInvitationAsync(acceptRequest);

            var exception = await Assert.ThrowsAsync<RpcException>(async () => await client.AcceptInvitationAsync(acceptRequest));

            Assert.Equal(StatusCode.InvalidArgument, exception.StatusCode);
        }

        [Theory]
        [InlineData("accountId", "SubscriptionId", "MemberId", "UserId")]
        public async Task AcceptInvitation_AcceptInvitationHasBeenCancelledOrRejected_ThrowsInvalidArgumentRpcException(
          string accountId,
          string SubscriptionId,
          string MemberId,
          string UserId)
        {
            var client = new Invitations.InvitationsClient(_factory.CreateGrpcChannel());

            var sendRequest = new SendInvitationRequest
            {
                AccountId = accountId,
                SubscriptionId = SubscriptionId,
                MemberId = MemberId,
                UserId = UserId,
                Permissions = new Permissions
                {
                    Transfer = true,
                    PurchaseCards = false,
                    ManageDevices = false
                }
            };

            var sendResponse = await client.SendInvitationAsync(sendRequest);

            var cancelRequest = new CancelInvitationRequest
            {
                Id = sendResponse.Id,
                AccountId = accountId,
                SubscriptionId = SubscriptionId,
                MemberId = MemberId,
                UserId = UserId,
            };

            await client.CancelInvitationAsync(cancelRequest);

            var acceptRequest = new AcceptInvitationRequest
            {
                Id = sendResponse.Id,
                AccountId = accountId,
                SubscriptionId = SubscriptionId,
                MemberId = MemberId,
                UserId = UserId,
            };

            var exception = await Assert.ThrowsAsync<RpcException>(async () => await client.AcceptInvitationAsync(acceptRequest));

            Assert.Equal(StatusCode.InvalidArgument, exception.StatusCode);
        }
    }
}
