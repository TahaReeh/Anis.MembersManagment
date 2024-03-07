﻿using Anis.MembersManagment.Query.Test.Fakers.Joined;
using Anis.MembersManagment.Query.Test.Fakers.Left;

namespace Anis.MembersManagment.Query.Test.HandlersTests.Left
{
    public class MemberLeftHandlerTest : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly EventHandlerHelper _handlerHelper;

        public MemberLeftHandlerTest(WebApplicationFactory<Program> factory, ITestOutputHelper helper)
        {
            _factory = factory.WithDefaultConfigurations(helper, services =>
            {
                services.ReplaceWithInMemoryDatabase();
            });

            _handlerHelper = new EventHandlerHelper(_factory.Services);
        }

        [Fact]
        public async Task MemberLeft_MemberLeftEventHandled_SubscriberStatusUpdatedPermissionRemoved()
        {
            var joinedEvent = new MemberJoinedFaker(sequence: 1).Generate();
            await _handlerHelper.HandleAsync(joinedEvent);

            var leftEvent = new MemberLeftFaker(joinedEvent).Generate();
            var isHandled = await _handlerHelper.TryHandleAsync(leftEvent);

            using var scope = _factory.Services.CreateScope();
            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            var subscriber = await unitOfWork.Subscriber.GetAllAsync();
            var permission = await unitOfWork.Permission.GetAllAsync();

            Assert.True(isHandled);
            Assert.Single(subscriber);
            Assert.Equal("Left", subscriber.First().Status);
            Assert.Empty(permission);
        }

        [Fact]
        public async Task MemberLeft_MemberLeftEventHandled_WhenMemberNotJoinedYet_EventSetToWait()
        {
            var joinedEvent = new MemberJoinedFaker(sequence: 1).Generate();

            var leftEvent = new MemberLeftFaker(joinedEvent).Generate();
            var isHandled = await _handlerHelper.TryHandleAsync(leftEvent);

            using var scope = _factory.Services.CreateScope();
            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            var subscriber = await unitOfWork.Subscriber.GetAllAsync();

            Assert.False(isHandled);
            Assert.Empty(subscriber);
        }

        [Fact]
        public async Task MemberLeft_DuplicateMemberLeftEventHandled_DuplicateEventIgnored()
        {
            var joinedEvent = new MemberJoinedFaker(sequence: 1).Generate();
            var leftEvent = new MemberLeftFaker(joinedEvent).Generate();

            await Task.WhenAll(
            _handlerHelper.HandleAsync(joinedEvent),
            _handlerHelper.HandleAsync(leftEvent)
           );

            var isHandled = await _handlerHelper.TryHandleAsync(leftEvent);

            Assert.True(isHandled);
        }

        [Fact]
        public async Task MemberLeft_EventSequenceNotExpectedYet_EventSetToWait()
        {
            var joinedEvent = new MemberJoinedFaker(sequence: 1).Generate();
            await _handlerHelper.HandleAsync(joinedEvent);

            var leftEvent = new MemberLeftFaker(joinedEvent)
                .RuleFor(e => e.Sequence, 3)
                .Generate();

            var isHandled = await _handlerHelper.TryHandleAsync(leftEvent);

            using var scope = _factory.Services.CreateScope();
            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            var subscriber = await unitOfWork.Subscriber.GetAllAsync();
            var permission = await unitOfWork.Permission.GetAllAsync();

            Assert.False(isHandled);
            Assert.Single(subscriber);
            Assert.Equal("Joined", subscriber.First().Status);
            Assert.Single(permission);
        }
    }
}
