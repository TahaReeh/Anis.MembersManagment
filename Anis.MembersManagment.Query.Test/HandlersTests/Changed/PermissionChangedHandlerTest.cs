using Anis.MembersManagment.Query.Abstractions.IRepositories;
using Anis.MembersManagment.Query.Test.Fakers.Changed;
using Anis.MembersManagment.Query.Test.Fakers.Joined;
using Anis.MembersManagment.Query.Test.Helpers;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Anis.MembersManagment.Query.Test.HandlersTests.PermissionChanged
{
    public class PermissionChangedHandlerTest : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly EventHandlerHelper _handlerHelper;

        public PermissionChangedHandlerTest(WebApplicationFactory<Program> factory, ITestOutputHelper helper)
        {
            _factory = factory.WithDefaultConfigurations(helper, services =>
            {
                services.ReplaceWithInMemoryDatabase();
            });

            _handlerHelper = new EventHandlerHelper(_factory.Services);
        }

        [Fact]
        public async Task PermissionChanged_EventHandled_PermissionUpdatedSubscriberSequenceUpdated()
        // TODO: add sequence increment test to all handlers
        {
            var joinedEvent = new MemberJoinedFaker(sequence: 1).Generate();
            await _handlerHelper.HandleAsync(joinedEvent);

            var changedEvent = new PermissionChangedFaker(joinedEvent).Generate();
            var isHandled = await _handlerHelper.TryHandleAsync(changedEvent);

            using var scope = _factory.Services.CreateScope();
            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            var subscriber = await unitOfWork.Subscriber.GetAllAsync();
            var permission = await unitOfWork.Permission.GetAllAsync();

            Assert.True(isHandled);
            Assert.Single(subscriber);
            Assert.Single(permission);
            Assert.Equal(2, permission.First().Sequence);
            Assert.Equal(subscriber.First().Sequence, permission.First().Sequence);
        }


        [Fact]
        public async Task PermissionChanged_EventHandledWithNoJoinedSubscriberSoNoPermissionExists_EventSetToWait()
        {
            var joinedEvent = new MemberJoinedFaker(sequence: 1).Generate();
            var changedEvent = new PermissionChangedFaker(joinedEvent).Generate();

            var isHandled = await _handlerHelper.TryHandleAsync(changedEvent);

            Assert.False(isHandled);
        }


        [Fact]
        public async Task PermissionChanged_DuplicateEventHandled_DuplicateEventIgnored()
        {
            var joinedEvent = new MemberJoinedFaker(sequence: 1).Generate();
            var changedEvent = new PermissionChangedFaker(joinedEvent).Generate();

            await Task.WhenAll(
                _handlerHelper.HandleAsync(joinedEvent),
                _handlerHelper.HandleAsync(changedEvent));

            var isHandled = await _handlerHelper.TryHandleAsync(changedEvent);

            using var scope = _factory.Services.CreateScope();
            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            var subscriber = await unitOfWork.Subscriber.GetAllAsync();
            var permission = await unitOfWork.Permission.GetAllAsync();

            Assert.True(isHandled);
        }

        [Fact]
        public async Task PermissionChanged_EventSequenceNotExpectedYet_EventSetToWait()
        {
            var joinedEvent = new MemberJoinedFaker(sequence: 1).Generate();
            await _handlerHelper.HandleAsync(joinedEvent);

            var changedEvent = new PermissionChangedFaker(joinedEvent)
                .RuleFor(e => e.Sequence, 3)
                .Generate();

            var isHandled = await _handlerHelper.TryHandleAsync(changedEvent);

            Assert.False(isHandled);
        }


    }
}
