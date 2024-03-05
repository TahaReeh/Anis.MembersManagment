using Anis.MembersManagment.Query.Abstractions.IRepositories;
using Anis.MembersManagment.Query.Test.Fakers.Joined;
using Anis.MembersManagment.Query.Test.Helpers;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace Anis.MembersManagment.Query.Test.HandlersTests.Joined
{
    public class MemberJoinedHandlerTest : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly EventHandlerHelper _handlerHelper;

        public MemberJoinedHandlerTest(WebApplicationFactory<Program> factory, ITestOutputHelper helper)
        {
            _factory = factory.WithDefaultConfigurations(helper, services =>
            {
                services.ReplaceWithInMemoryDatabase();
            });

            _handlerHelper = new EventHandlerHelper(_factory.Services);
        }

        [Fact]
        public async Task MemberJoined_NewMemberJoinedEventHandled_NewPermissionAndNewSubscriberWithJoinedStatusSaved()
        {
            var @event = new MemberJoinedFaker(sequence: 1).Generate();

            var isHandled = await _handlerHelper.TryHandleAsync(@event);

            using var scope = _factory.Services.CreateScope();
            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            var subscriber = await unitOfWork.Subscriber.GetAllAsync();
            var permission = await unitOfWork.Permission.GetAllAsync();

            Assert.True(isHandled);
            Assert.Single(subscriber);
            Assert.Single(permission);
            Assert.Equal(@event.AggregateId, subscriber.First().Id);
            Assert.Equal("Joined", subscriber.First().Status);
        }

        [Fact]
        public async Task MemberJoined_DuplicateMemberJoinedEventHandled_DuplicateEventIgnored()
        {
            var @event = new MemberJoinedFaker(sequence: 1).Generate();
            await _handlerHelper.HandleAsync(@event);

            var isHandled = await _handlerHelper.TryHandleAsync(@event);

            using var scope = _factory.Services.CreateScope();
            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            var subscriber = await unitOfWork.Subscriber.GetAllAsync();
            var permission = await unitOfWork.Permission.GetAllAsync();

            Assert.True(isHandled);
            Assert.Single(subscriber);
            Assert.Single(permission);
        }

        [Fact]
        public async Task MemberJoined_MemberJoinedEventHandledAfterLeftOrRemoved_SubscriberStatusUpdatedNewPermissionSaved()
        {
         
        }
    }
}
