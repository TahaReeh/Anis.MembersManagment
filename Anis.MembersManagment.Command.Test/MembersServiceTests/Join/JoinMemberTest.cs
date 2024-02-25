using Microsoft.Azure.Amqp.Framing;

namespace Anis.MembersManagment.Command.Test.MembersServiceTests.Join
{
    public class JoinMemberTest(WebApplicationFactory<Program> factory, ITestOutputHelper helper) : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory = factory.WithDefaultConfigurations(helper, services =>
            {
                services.ReplaceWithInMemoryDatabase();
            });

        [Fact]
        public async Task JoinMember_SendValidRequest_MemberJoinedEventSaved()
        {
            var client = new Members.MembersClient(_factory.CreateGrpcChannel());

            var request = new JoinMemberRequest
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

            var response = await client.JoinMemberAsync(request);

            using var scope = _factory.Services.CreateScope();
            var eventsStore = scope.ServiceProvider.GetRequiredService<IEventStore>();
            var events = await eventsStore.GetAllLikeAsync(response.Id, new CancellationToken());

            Assert.Single(events);
            Assert.Equal($"{request.SubscriptionId}_{request.MemberId}", response.Id);
            Assert.Equal(1, events[0].Sequence);
        }
    }
}
