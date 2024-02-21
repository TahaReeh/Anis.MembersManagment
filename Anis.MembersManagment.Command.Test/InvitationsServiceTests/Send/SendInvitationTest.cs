using Anis.MembersManagment.Command.Abstractions;
using Anis.MembersManagment.Command.Infrastructure.Persistence;
using Anis.MembersManagment.Command.Test.Helpers;
using Anis.MembersManagment.Command.Test.InvitationsProto;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Todo.Command.Test.Helpers;
using Xunit.Abstractions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

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

        [Fact]
        public async Task SendInvitation_SendValidRequestData_InvitationSentEventSaved()
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
                    Transfer = true,
                    PurchaseCards = false,
                    ManageDevices = false
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
