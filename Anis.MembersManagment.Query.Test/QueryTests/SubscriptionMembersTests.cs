namespace Anis.MembersManagment.Query.Test.QueryTests
{
    public class SubscriptionMembersTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public SubscriptionMembersTests(WebApplicationFactory<Program> factory, ITestOutputHelper helper)
        {
            _factory = factory.WithDefaultConfigurations(helper, services =>
            {
                services.ReplaceWithInMemoryDatabase();
            });
        }

        [Fact]
        public async Task SubscriptionMembers_QueryExistingEntities_ReturnsSelectedSubscriptionMembers()
        {
            string subscriptionId = Guid.NewGuid().ToString();

            using var scope = _factory.Services.CreateScope();
            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

            await unitOfWork.Subscriber.AddRangeAsync(
                new SubscriberFaker().SameSupscriptionDiffrentUser(subscriptionId, count: 3)
                , new CancellationToken());

            await unitOfWork.CommitAsync(new CancellationToken());

            var client = new Members.MembersClient(_factory.CreateGrpcChannel());

            var request = new GetSubscriptionMembersRequest()
            {
                SubscriptionId = subscriptionId,
                Page = 1,
                Size = 20
            };

            var response = await client.GetSubscriptionMembersAsync(request);

            Assert.Equal(1, response.Page);
            Assert.Equal(20, response.PageSize);
            Assert.Equal(3, response.TotalResults);
            Assert.Equal(3, response.Subscribers.Count);
        }

        [Fact]
        public async Task SubscriptionMembers_QuerySubscriptionWithNoMembers_ThrowNotFoundRpcException()
        {
            var client = new Members.MembersClient(_factory.CreateGrpcChannel());

            var request = new GetSubscriptionMembersRequest()
            {
                SubscriptionId = Guid.NewGuid().ToString(),
            };

            var exception = await Assert.ThrowsAsync<RpcException>(() =>
            client.GetSubscriptionMembersAsync(request).ResponseAsync);

            Assert.Equal(StatusCode.NotFound, exception.StatusCode);
            Assert.NotEmpty(exception.Status.Detail);
        }
    }
}
