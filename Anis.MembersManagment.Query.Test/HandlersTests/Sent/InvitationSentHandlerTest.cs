using Anis.MembersManagment.Query.Abstractions.IRepositories;
using Anis.MembersManagment.Query.Test.Fakers;
using Anis.MembersManagment.Query.Test.Fakers.Sent;
using Anis.MembersManagment.Query.Test.Helpers;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Anis.MembersManagment.Query.Test.HandlersTests.Sent
{
    public class InvitationSentHandlerTest : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly EventHandlerHelper _handlerHelper;

        public InvitationSentHandlerTest(WebApplicationFactory<Program> factory, ITestOutputHelper helper)
        {
            _factory = factory.WithDefaultConfigurations(helper, services =>
            {
                services.ReplaceWithInMemoryDatabase();
            });

            _handlerHelper = new EventHandlerHelper(_factory.Services);
        }

        [Fact]
        public async Task InvitationSent_UserFirstInvitationSentEventHandled_NewPermissionAndNewInvitationWithPendingStatusSaved()
        {
            // TODO: permission should be generated from event???

            var @event = new InvitationSentFaker(sequence: 1).Generate();

            var isHandled = await _handlerHelper.TryHandleAsync(@event);

            using var scope = _factory.Services.CreateScope();
            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            var invite = await unitOfWork.Invitation.GetAllAsync();
            var permission = await unitOfWork.Permission.GetAllAsync();

            Assert.True(isHandled);
            Assert.Single(invite);
            Assert.Single(permission);
            Assert.Equal(@event.AggregateId,invite.First().Id);
            Assert.Equal("Pending", invite.First().Status);
        }

        [Fact]
        public async Task InvitationSent_DuplicateInvitationSentEventHandled_DuplicateEventIgnored()
        {
            var @event = new InvitationSentFaker(sequence: 1).Generate();
            await _handlerHelper.HandleAsync(@event);

            var isHandled = await _handlerHelper.TryHandleAsync(@event);

            using var scope = _factory.Services.CreateScope();
            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            var invite = await unitOfWork.Invitation.GetAllAsync();
            var permission = await unitOfWork.Permission.GetAllAsync();

            Assert.True(isHandled);
            Assert.Single(invite);
            Assert.Single(permission);
        }

        [Fact]
        public async Task InvitationSent_InvitationSentEventSequenceGreaterThanDbSequence_EventSetToWait()
        {
            var firstEvent = new InvitationSentFaker(sequence: 1).Generate();

            var isFirstHandled = await _handlerHelper.TryHandleAsync(firstEvent);

            var secondEvent = new InvitationSentFaker(sequence: 3)
                .RuleFor(e=>e.AggregateId,firstEvent.AggregateId)
                .Generate();

            var isSecondHandled = await _handlerHelper.TryHandleAsync(secondEvent);

            using var scope = _factory.Services.CreateScope();
            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            var invite = await unitOfWork.Invitation.GetAllAsync();
            var permission = await unitOfWork.Permission.GetAllAsync();

            Assert.True(isFirstHandled);
            Assert.False(isSecondHandled);
            Assert.Single(invite);
            Assert.Single(permission);
        }

        [Fact]
        public async Task InvitationSent_InvitationSentEventHandledAfterCanceledOrRejectedOrLeftOrRemoved_InvitationStatusUpdatedNewPermissionSaved()
        {
            
        }

    }
}
