using Anis.MembersManagment.Query.Abstractions.IRepositories;
using Anis.MembersManagment.Query.Test.Fakers.Accepted;
using Anis.MembersManagment.Query.Test.Fakers.Sent;
using Anis.MembersManagment.Query.Test.Helpers;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel;
using Xunit.Abstractions;

namespace Anis.MembersManagment.Query.Test.HandlersTests.Accepted
{
    public class InvitationAcceptedHandlerTest : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly EventHandlerHelper _handlerHelper;

        public InvitationAcceptedHandlerTest(WebApplicationFactory<Program> factory, ITestOutputHelper helper)
        {
            _factory = factory.WithDefaultConfigurations(helper, services =>
            {
                services.ReplaceWithInMemoryDatabase();
            });

            _handlerHelper = new EventHandlerHelper(_factory.Services);
        }

        [Fact]
        public async Task InvitationAccepted_EventHandledWhenPendingInvitation_InvitationStatusUpdatedSubscriberSavedPermissionSequenceUpdated()
        {
            var sentEvent = new InvitationSentFaker(sequence: 1).Generate();

            await _handlerHelper.HandleAsync(sentEvent);

            var acceptedEvent = new InvitationAcceptedFaker(sentEvent).Generate();

            var isHandled = await _handlerHelper.TryHandleAsync(acceptedEvent);

            using var scope = _factory.Services.CreateScope();
            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            var invite = await unitOfWork.Invitation.GetAllAsync();
            var subscriber = await unitOfWork.Subscriber.GetAllAsync();
            var permission = await unitOfWork.Permission.GetAllAsync();

            Assert.True(isHandled);
            Assert.Single(invite);
            Assert.Single(subscriber);
            Assert.Single(permission);
            Assert.Equal(invite.First().SubscriptionId, subscriber.First().SubscriptionId);
            Assert.Equal(invite.First().UserId, subscriber.First().UserId);
            Assert.Equal("Accepted", invite.First().Status);
            Assert.Equal(invite.First().Sequence, permission.First().Sequence);
        }

        [Fact]
        public async Task InvitationAccepted_InvitationAcceptedEventHandledWithNoPendingInvitation_EventSetToWait()
        {
            var sentEvent = new InvitationSentFaker(sequence: 1).Generate();

            var acceptedEvent = new InvitationAcceptedFaker(sentEvent).Generate();

            var isHandled = await _handlerHelper.TryHandleAsync(acceptedEvent);

            using var scope = _factory.Services.CreateScope();
            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            var invite = await unitOfWork.Invitation.GetAllAsync();
            var subscriber = await unitOfWork.Subscriber.GetAllAsync();

            Assert.False(isHandled);
            Assert.Empty(invite);
            Assert.Empty(subscriber);
        }

        [Fact]
        public async Task InvitationAccepted_DublicateInvitationAcceptedEventHandled_DuplicateEventIgnored()
        {
            var sentEvent = new InvitationSentFaker(sequence: 1).Generate();

            await _handlerHelper.HandleAsync(sentEvent);

            var acceptedEvent = new InvitationAcceptedFaker(sentEvent).Generate();

            await _handlerHelper.HandleAsync(acceptedEvent);

            var isHandled = await _handlerHelper.TryHandleAsync(acceptedEvent);

            Assert.True(isHandled);
        }

        [Fact]
        public async Task InvitationAccepted_EventSequenceNotExpectedYet_EventSetToWait()
        {
            var sentEvent = new InvitationSentFaker(sequence: 1).Generate();
            await _handlerHelper.HandleAsync(sentEvent);

            var acceptedEvent = new InvitationAcceptedFaker(sentEvent)
                .RuleFor(e=>e.Sequence,3).Generate();

            var isHandled = await _handlerHelper.TryHandleAsync(acceptedEvent);

            Assert.False(isHandled);
        }
    }
}
