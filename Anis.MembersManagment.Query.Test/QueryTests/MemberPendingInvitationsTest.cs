using Anis.MembersManagment.Query.Abstractions.IRepositories;
using Anis.MembersManagment.Query.Test.Fakers.EntitiesFakers;
using Anis.MembersManagment.Query.Test.Helpers;
using Anis.MembersManagment.Query.Test.MembersProto;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

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
        public async Task MemberPendingInvitations_QueryExistingEntities_ReturnAll()
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
    }
}
