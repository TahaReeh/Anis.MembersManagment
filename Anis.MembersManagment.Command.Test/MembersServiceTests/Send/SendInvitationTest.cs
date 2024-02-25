namespace Anis.MembersManagment.Command.Test.MembersServiceTests.Send
{
    public class SendInvitationTest(WebApplicationFactory<Program> factory, ITestOutputHelper helper) : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory = factory.WithDefaultConfigurations(helper, services =>
            {
                services.ReplaceWithInMemoryDatabase();
            });

        [Theory]
        [InlineData(true, true, true)]
        [InlineData(false, false, false)]
        public async Task SendInvitation_SendNewInvitationWithValidRequestData_InvitationSentEventSaved(
            bool transfer,
            bool purchaseCards,
            bool manageDevices)
        {
            var client = new Members.MembersClient(_factory.CreateGrpcChannel());

            var request = new SendInvitationRequest
            {
                AccountId = Guid.NewGuid().ToString(),
                SubscriptionId = Guid.NewGuid().ToString(),
                MemberId = Guid.NewGuid().ToString(),
                UserId = Guid.NewGuid().ToString(),
                Permissions = new Permissions
                {
                    Transfer = transfer,
                    PurchaseCards = purchaseCards,
                    ManageDevices = manageDevices
                }
            };

            var response = await client.SendInvitationAsync(request);

            using var scope = _factory.Services.CreateScope();
            var eventsStore = scope.ServiceProvider.GetRequiredService<IEventStore>();
            var events = await eventsStore.GetAllLikeAsync($"{request.SubscriptionId}_{request.MemberId}", new CancellationToken());

            Assert.Single(events);
            Assert.Equal($"{request.SubscriptionId}_{request.MemberId}", response.Id);
        }


        [Fact]
        public async Task SendInvitation_SendDuplicateInvitationWhileItsStillPending_ThrowsInvalidArgumentRpcException()
        {
            var client = new Members.MembersClient(_factory.CreateGrpcChannel());

            var request = new SendInvitationRequest
            {
                AccountId = Guid.NewGuid().ToString(),
                SubscriptionId = Guid.NewGuid().ToString(),
                MemberId = Guid.NewGuid().ToString(),
                UserId = Guid.NewGuid().ToString(),
                Permissions = new Permissions
                {
                    Transfer = true,
                    PurchaseCards = false,
                    ManageDevices = false
                }
            };

            await client.SendInvitationAsync(request);

            var exception = await Assert.ThrowsAsync<RpcException>(async () => await client.SendInvitationAsync(request));

            Assert.Equal(StatusCode.InvalidArgument, exception.StatusCode);
        }

        [Theory]
        [InlineData("accountId", "SubscriptionId", "MemberId", "UserId")]
        public async Task SendInvitation_SendDuplicateInvitationIsAlreadyAccepted_ThrowsInvalidArgumentRpcException(
            string accountId,
            string SubscriptionId,
            string MemberId,
            string UserId)
        {
            var client = new Members.MembersClient(_factory.CreateGrpcChannel());

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

            var exception = await Assert.ThrowsAsync<RpcException>(async () => await client.SendInvitationAsync(sendRequest));

            Assert.Equal(StatusCode.InvalidArgument, exception.StatusCode);
        }

        [Theory]
        [InlineData("accountId", "SubscriptionId", "MemberId", "UserId")]
        public async Task SendInvitation_SendNewInvitationToSameMemberAfterCancelOrReject_NewInvitationSentEventSaved(
           string accountId,
           string subscriptionId,
           string memberId,
           string UserId)
        {
            var client = new Members.MembersClient(_factory.CreateGrpcChannel());

            var firstSendRequest = new SendInvitationRequest
            {
                AccountId = accountId,
                SubscriptionId = subscriptionId,
                MemberId = memberId,
                UserId = UserId,
                Permissions = new Permissions
                {
                    Transfer = true,
                    PurchaseCards = false,
                    ManageDevices = false
                }
            };

            var firstSendResponse = await client.SendInvitationAsync(firstSendRequest);

            var cancelRequest = new CancelInvitationRequest
            {
                Id = firstSendResponse.Id,
                AccountId = accountId,
                SubscriptionId = subscriptionId,
                MemberId = memberId,
                UserId = UserId,
            };

            await client.CancelInvitationAsync(cancelRequest);

            var secondSendRequest = new SendInvitationRequest
            {
                AccountId = accountId,
                SubscriptionId = subscriptionId,
                MemberId = memberId,
                UserId = UserId,
                Permissions = new Permissions
                {
                    Transfer = true,
                    PurchaseCards = false,
                    ManageDevices = false
                }
            };

            var secondSendResponse = await client.SendInvitationAsync(secondSendRequest);

            using var scope = _factory.Services.CreateScope();
            var eventsStore = scope.ServiceProvider.GetRequiredService<IEventStore>();
            var events = await eventsStore.GetAllAsync($"{subscriptionId}_{memberId}", new CancellationToken());

            Assert.Equal(3,events.Count);
            Assert.Equal($"{subscriptionId}_{memberId}", firstSendResponse.Id);
            Assert.Equal($"{subscriptionId}_{memberId}", secondSendResponse.Id);
        }

    }
}
