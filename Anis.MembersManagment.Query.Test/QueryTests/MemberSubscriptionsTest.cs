namespace Anis.MembersManagment.Query.Test.QueryTests
{
    public class MemberSubscriptionsTest(WebApplicationFactory<Program> factory, ITestOutputHelper helper) : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory = factory.WithDefaultConfigurations(helper, services =>
            {
                services.ReplaceWithInMemoryDatabase();
            });

        [Fact]
        public async Task MemberSubscriptions_QueryExistingEntities_ReturnsSelectedMemberSubscriptions()
        {
            string userId = Guid.NewGuid().ToString();

            using var scope = _factory.Services.CreateScope();
            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

            await unitOfWork.Subscriber.AddRangeAsync(
                new SubscriberFaker().SameUserDiffrentSubscription(userId, count: 3)
                , new CancellationToken());

            await unitOfWork.CommitAsync(new CancellationToken());

            var client = new Members.MembersClient(_factory.CreateGrpcChannel());

            var request = new GetMemberSubscriptionsRequest()
            {
                MemberId = userId,
                Page = 1,
                Size = 20
            };

            var response = await client.GetMemberSubscriptionsAsync(request);

            Assert.Equal(1, response.Page);
            Assert.Equal(20, response.PageSize);
            Assert.Equal(3, response.TotalResults);
            Assert.Equal(3, response.Subscribers.Count);
        }

        [Fact]
        public async Task MemberSubscriptions_QueryUserWithNoSubscriptions_ThrowNotFoundRpcException()
        {
            var client = new Members.MembersClient(_factory.CreateGrpcChannel());

            var request = new GetMemberSubscriptionsRequest()
            {
                MemberId = Guid.NewGuid().ToString(),
            };

            var exception = await Assert.ThrowsAsync<RpcException>(() =>
            client.GetMemberSubscriptionsAsync(request).ResponseAsync);

            Assert.Equal(StatusCode.NotFound, exception.StatusCode);
            Assert.NotEmpty(exception.Status.Detail);
        }
    }
}
