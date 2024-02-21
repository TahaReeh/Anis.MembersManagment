using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anis.MembersManagment.Command.Test.InvitationsServiceTests.Reject
{
    public class RejectInvitationTest : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public RejectInvitationTest(WebApplicationFactory<Program> factory, ITestOutputHelper helper)
        {
            _factory = factory.WithDefaultConfigurations(helper, services =>
            {
                services.ReplaceWithInMemoryDatabase();
            });
        }

        [Theory]
        [InlineData("accountId", "SubscriptionId", "MemberId", "UserId")]
        public async Task RejectInvitation_SendValidRequestWhenPendingInvitationExists_InvitationRejectedEventSaved(
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

            var rejectRequest = new RejectInvitationRequest
            {
                Id = sendResponse.Id,
                AccountId = accountId,
                SubscriptionId = SubscriptionId,
                MemberId = MemberId,
                UserId = UserId,
            };

            await client.RejectInvitationAsync(rejectRequest);

            using var scope = _factory.Services.CreateScope();
            var eventsStore = scope.ServiceProvider.GetRequiredService<IEventStore>();
            var events = await eventsStore.GetAllAsync(sendResponse.Id, new CancellationToken());

            Assert.Equal(2, events.Count);
        }

        [Theory]
        [InlineData("accountId", "SubscriptionId", "MemberId", "UserId")]
        public async Task RejectInvitation_RejectInvitationWhenIsRejectedOrCancelled_ThrowsInvalidArgumentRpcException(
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

            var rejectRequest = new RejectInvitationRequest
            {
                Id = sendResponse.Id,
                AccountId = accountId,
                SubscriptionId = SubscriptionId,
                MemberId = MemberId,
                UserId = UserId,
            };

            await client.RejectInvitationAsync(rejectRequest);

            var exception = await Assert.ThrowsAsync<RpcException>(async () => await client.RejectInvitationAsync(rejectRequest));

            Assert.Equal(StatusCode.InvalidArgument, exception.StatusCode);
        }

    }
}
