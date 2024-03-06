using Anis.MembersManagment.Query.Abstractions.IRepositories;
using Anis.MembersManagment.Query.Test.Fakers;
using Anis.MembersManagment.Query.Test.Fakers.Joined;
using Anis.MembersManagment.Query.Test.Fakers.Removed;
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
        public async Task MemberJoined_EventHandled_PermissionAndSubscriberSaved()
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
        public async Task MemberJoined_MemberJoinedEventHandledAfterLeftOrRemoved_SubscriberStatusUpdatedPermissionSaved()
        {
            var firstJoinedEvent = new MemberJoinedFaker(sequence: 1).Generate();
            var removedEvent = new MemberRemovedFaker(firstJoinedEvent).Generate();

            await Task.WhenAll( 
                _handlerHelper.HandleAsync(firstJoinedEvent),
                _handlerHelper.HandleAsync(removedEvent)
                );

            var secondJoinedEvent = new MemberJoinedFaker(sequence: 3)
                .RuleFor(i => i.AggregateId, firstJoinedEvent.AggregateId)
                .Generate();

            var isHandled = await _handlerHelper.TryHandleAsync(secondJoinedEvent);

            using var scope = _factory.Services.CreateScope();
            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            var subscriber = await unitOfWork.Subscriber.GetAllAsync();
            var permission = await unitOfWork.Permission.GetAllAsync();

            Assert.True(isHandled);
            Assert.Single(subscriber);
            Assert.Single(permission);
            Assert.Equal(secondJoinedEvent.AggregateId, permission.First().Id);
            Assert.Equal("Joined", subscriber.First().Status);
        }

        [Fact]
        public async Task MemberJoined_EventSequenceNotExpectedYet_EventSetToWait()
        {
            var firstJoinedEvent = new MemberJoinedFaker(sequence: 1).Generate();
            await _handlerHelper.HandleAsync(firstJoinedEvent);

            var removedEvent = new MemberRemovedFaker(firstJoinedEvent).Generate();

            var secondJoinedEvent = new MemberJoinedFaker(sequence: 3)
                .RuleFor(i => i.AggregateId, firstJoinedEvent.AggregateId)
                .Generate();

            var isHandled = await _handlerHelper.TryHandleAsync(secondJoinedEvent);

            Assert.False(isHandled);
        }
    }
}
