using Anis.MembersManagment.Query.Abstractions.IRepositories;
using Anis.MembersManagment.Query.Entities;
using Anis.MembersManagment.Query.Infrastructure.Persistence.Repositories;
using Anis.MembersManagment.Query.Test.Fakers.EntitiesFakers;
using Anis.MembersManagment.Query.Test.Helpers;
using Anis.MembersManagment.Query.Test.MembersProto;
using Grpc.Core;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace Anis.MembersManagment.Query.Test.QueryTests
{
    public class OwnerPendingInvitationsTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public OwnerPendingInvitationsTests(WebApplicationFactory<Program> factory, ITestOutputHelper helper)
        {
            _factory = factory.WithDefaultConfigurations(helper, services =>
            {
                services.ReplaceWithInMemoryDatabase();
            });
        }

        [Fact]
        public async Task OwnerPendingInvitations_QueryExistingEntities_ReturnsOwnerPendingInvitations()
        {
            string userId = Guid.NewGuid().ToString();
            string subscriptionId = Guid.NewGuid().ToString();

            using var scope = _factory.Services.CreateScope();
            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

            await unitOfWork.Invitation.AddRangeAsync(
                new InvitationFaker().SameSupscriptionDiffrentUser(userId, subscriptionId, count: 3)
                , new CancellationToken());
            await unitOfWork.CommitAsync(new CancellationToken());

            var x = await unitOfWork.Invitation.GetAllAsync(i => i.Subscription.UserId == userId,
                includeProperties: "Subscription,User",
                1, 20);

            var client = new Members.MembersClient(_factory.CreateGrpcChannel());

            var request = new GetOwnerPendingInvitationsRequest()
            {
                OwnerId = userId,
                Page = 1,
                Size = 20
            };

            var response = await client.GetOwnerPendingInvitationsAsync(request);

            Assert.Equal(1, response.Page);
            Assert.Equal(20, response.PageSize);
            Assert.Equal(3, response.TotalResults);
            Assert.Equal(3, response.Invitations.Count);
        }

        [Fact]
        public async Task OwnerPendingInvitations_QueryUserWithNoInvitation_ThrowNotFoundRpcException()
        {
            var client = new Members.MembersClient(_factory.CreateGrpcChannel());

            var request = new GetOwnerPendingInvitationsRequest()
            {
                OwnerId = Guid.NewGuid().ToString(),
            };

            var exception = await Assert.ThrowsAsync<RpcException>(() => client.GetOwnerPendingInvitationsAsync(request).ResponseAsync);

            Assert.Equal(StatusCode.NotFound, exception.StatusCode);
            Assert.NotEmpty(exception.Status.Detail);
        }
    }
}
