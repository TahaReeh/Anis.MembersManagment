namespace Anis.MembersManagment.Query.Test.QueryTests
{
    public class MemberPendingInvitationsTest : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public MemberPendingInvitationsTest(WebApplicationFactory<Program> factory, ITestOutputHelper helper)
        {
            _factory = factory.WithDefaultConfigurations(helper, services =>
            {
                services.ReplaceWithInMemoryDatabase();
            });
        }

        [Fact]
        public async Task MemberPendingInvitations_QueryExistingEntities_ReturnsSelectedMemberInvitations()
        {
            string userId = Guid.NewGuid().ToString();

            using var scope = _factory.Services.CreateScope();
            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

            await unitOfWork.Invitation.AddRangeAsync(
                new InvitationFaker().SameUserDiffrentSubscription(userId, count: 3)
                ,new CancellationToken());

            await unitOfWork.CommitAsync(new CancellationToken());

            var client = new Members.MembersClient(_factory.CreateGrpcChannel());

            var request = new GetMemberPendingInvitationsRequest()
            {
                MemberId = userId,
                Page = 1,
                Size = 20
            };

            var response = await client.GetMemberPendingInvitationsAsync(request);

            Assert.Equal(1, response.Page);
            Assert.Equal(20, response.PageSize);
            Assert.Equal(3, response.TotalResults);
            Assert.Equal(3, response.Invitations.Count);
        }


        [Fact]
        public async Task MemberPendingInvitations_QueryUserWithNoInvitation_ThrowNotFoundRpcException()
        {
            var client = new Members.MembersClient(_factory.CreateGrpcChannel());

            var request = new GetMemberPendingInvitationsRequest()
            {
                MemberId = Guid.NewGuid().ToString(),
            };

            var exception = await Assert.ThrowsAsync<RpcException>(() => client.GetMemberPendingInvitationsAsync(request).ResponseAsync);

            Assert.Equal(StatusCode.NotFound, exception.StatusCode);
            Assert.NotEmpty(exception.Status.Detail);
        }
    }
}
