using Microsoft.Azure.Amqp.Framing;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;

namespace Anis.MembersManagment.Command.Test.MembersServiceTests.ChangePermission
{
    public class ChangePermissionTest(WebApplicationFactory<Program> factory, ITestOutputHelper helper) : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory = factory.WithDefaultConfigurations(helper, services =>
            {
                services.ReplaceWithInMemoryDatabase();
            });


        [Theory]
        [InlineData(true, true, true)]
        [InlineData(false, false, false)]
        public async Task ChangePermission_SendValidRequest_PermissionChangedEventSaved(
            bool transfer,
            bool purchaseCards,
            bool manageDevices)
        {
            var client = new Members.MembersClient(_factory.CreateGrpcChannel());

            var joinRequest = new JoinMemberRequest
            {
                AccountId = "AccountId",
                SubscriptionId = "subscriptionId",
                MemberId = "memberId",
                UserId = "userId",
                Permissions = new Permissions
                {
                    Transfer = true,
                    PurchaseCards = false,
                    ManageDevices = false
                }
            };

            var joinResponse = await client.JoinMemberAsync(joinRequest);

            var changePermRequest = new ChangePermissionRequest
            {
                Id = joinResponse.Id,
                AccountId = "AccountId",
                SubscriptionId = "subscriptionId",
                MemberId = "memberId",
                UserId = "userId",
                Permissions = new Permissions
                {
                    Transfer = transfer,
                    PurchaseCards = purchaseCards,
                    ManageDevices = manageDevices
                }
            };

            var changePermResponse = await client.ChangePermissionAsync(changePermRequest);

            using var scope = _factory.Services.CreateScope();
            var eventStore = scope.ServiceProvider.GetRequiredService<IEventStore>();
            var events = await eventStore.GetAllAsync(changePermResponse.Id, new CancellationToken());


            Assert.Equal(2, events.Count);
            Assert.Equal("PermissionChanged", events[1].Type);
        }


        [Theory]
        [InlineData("", "", "", "", "", null, null, null)]
        [InlineData(" ", " ", " ", " ", " ", true, false, false)]
        public async Task ChangePermission_SendInvalidRequest_ThrowsInvalidArgumentRpcException(
            string id,
            string accountId,
            string subscriptionId,
            string memberId,
            string userId,
            bool transfer,
            bool purchaseCards,
            bool manageDevices)
        {
            var client = new Members.MembersClient(_factory.CreateGrpcChannel());

            var request = new ChangePermissionRequest
            {
                Id = id,
                AccountId = accountId,
                SubscriptionId = subscriptionId,
                MemberId = memberId,
                UserId = userId,
                Permissions = new Permissions
                {
                    Transfer = transfer,
                    PurchaseCards = purchaseCards,
                    ManageDevices = manageDevices
                }
            };

            var exception = await Assert.ThrowsAsync<RpcException>(async () => await client.ChangePermissionAsync(request));

            Assert.Equal(StatusCode.InvalidArgument, exception.StatusCode);
        }

        [Fact]
        public async Task ChangePermission_SendChangePermissionRequestToNonJoinedMember_ThrowsNotFoundRpcException()
        {
            var client = new Members.MembersClient(_factory.CreateGrpcChannel());

            var request = new ChangePermissionRequest
            {
                Id = Guid.NewGuid().ToString(),
                AccountId = Guid.NewGuid().ToString(),
                SubscriptionId = Guid.NewGuid().ToString(),
                MemberId = Guid.NewGuid().ToString(),
                UserId = Guid.NewGuid().ToString(),
                Permissions = new Permissions
                {
                    Transfer = true,
                    PurchaseCards = true,
                    ManageDevices = true
                }
            };

            var exception = await Assert.ThrowsAsync<RpcException>(async () => await client.ChangePermissionAsync(request));

            Assert.Equal(StatusCode.NotFound, exception.StatusCode);
        }
    }
}
