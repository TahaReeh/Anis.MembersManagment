namespace Anis.MembersManagment.Command.Test.InvitationsServiceTests.Cancel
{
    public class CancelInvitationTest(WebApplicationFactory<Program> factory, ITestOutputHelper helper) : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory = factory.WithDefaultConfigurations(helper, services =>
            {
                services.ReplaceWithInMemoryDatabase();
            });

        [Theory]
        [InlineData("accountId", "SubscriptionId", "MemberId", "UserId")]
        public async Task CancelInvitation_SendValidRequestWhenPendingInvitationExists_InvitationCanclledEventSaved(
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

            var cancelRequest = new CancelInvitationRequest
            {
                Id = sendResponse.Id,
                AccountId = accountId,
                SubscriptionId = SubscriptionId,
                MemberId = MemberId,
                UserId = UserId,
            };

            await client.CancelInvitationAsync(cancelRequest);

            using var scope = _factory.Services.CreateScope();
            var eventsStore = scope.ServiceProvider.GetRequiredService<IEventStore>();
            var events = await eventsStore.GetAllAsync(sendResponse.Id, new CancellationToken());

            Assert.Equal(2, events.Count);
        }

        [Theory]
        [InlineData("accountId", "SubscriptionId", "MemberId", "UserId")]
        public async Task CancelInvitation_CancelInvitationWhenIsRejectedOrCancelled_ThrowsInvalidArgumentRpcException(
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

            var cancelRequest = new CancelInvitationRequest
            {
                Id = sendResponse.Id,
                AccountId = accountId,
                SubscriptionId = SubscriptionId,
                MemberId = MemberId,
                UserId = UserId,
            };

            await client.CancelInvitationAsync(cancelRequest);

            var exception = await Assert.ThrowsAsync<RpcException>(async () => await client.CancelInvitationAsync(cancelRequest));

            Assert.Equal(StatusCode.InvalidArgument, exception.StatusCode);
        }

    }
}
