using Anis.MembersManagment.Command.Abstractions;
using Anis.MembersManagment.Command.Test.Helpers;
using Anis.MembersManagment.Command.Test.InvitationsProto;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Todo.Command.Test.Helpers;
using Xunit.Abstractions;

namespace Anis.MembersManagment.Command.Test.InvitationsServiceTests.Send
{
    public class SendInvitationTest : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public SendInvitationTest(WebApplicationFactory<Program> factory, ITestOutputHelper helper)
        {
            _factory = factory.WithDefaultConfigurations(helper, services =>
            {
                services.ReplaceWithInMemoryDatabase();
            });
        }

        [Theory]
        [InlineData(true,true,true)]
        [InlineData(false,false,false)]
        public async Task SendInvitation_SendValidRequestData_InvitationSentEventSaved(
            bool transfer,
            bool purchaseCards,
            bool manageDevices)
        {
            var client = new Invitations.InvitationsClient(_factory.CreateGrpcChannel());

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
            Assert.Equal($"{request.SubscriptionId}_{request.MemberId}_1", response.Id);
        }


    }
}
