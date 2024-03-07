using Anis.MembersManagment.Query.Test.Fakers.Cancelled;
using Anis.MembersManagment.Query.Test.Fakers.Sent;

namespace Anis.MembersManagment.Query.Test.HandlersTests.Cancelled
{
    public class InvitationCancelledHandlerTest : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly EventHandlerHelper _handlerHelper;

        public InvitationCancelledHandlerTest(WebApplicationFactory<Program> factory, ITestOutputHelper helper)
        {
            _factory = factory.WithDefaultConfigurations(helper, services =>
            {
                services.ReplaceWithInMemoryDatabase();
            });

            _handlerHelper = new EventHandlerHelper(_factory.Services);
        }

        [Fact]
        public async Task InvitationCancelled_EventHandledWhenPendingInvitation_InvitationStatusUpdatedPermissionRemoved()
        {
            var sentEvent = new InvitationSentFaker(sequence: 1).Generate();
            await _handlerHelper.HandleAsync(sentEvent);

            var cancelledEvent = new InvitationCancelledFaker(sentEvent).Generate();
            var isHandled = await _handlerHelper.TryHandleAsync(cancelledEvent);

            using var scope = _factory.Services.CreateScope();
            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            var invite = await unitOfWork.Invitation.GetAllAsync();
            var permission = await unitOfWork.Permission.GetAllAsync();

            Assert.True(isHandled);
            Assert.Single(invite);
            Assert.Equal("Cancelled", invite.First().Status);
            Assert.Empty(permission);
        }

        [Fact]
        public async Task InvitationCancelled_InvitationCancelledEventHandledWithNoPendingInvitation_EventSetToWait()
        {
            var sentEvent = new InvitationSentFaker(sequence: 1).Generate();

            var cancelledEvent = new InvitationCancelledFaker(sentEvent).Generate();

            var isHandled = await _handlerHelper.TryHandleAsync(cancelledEvent);

            using var scope = _factory.Services.CreateScope();
            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            var invite = await unitOfWork.Invitation.GetAllAsync();

            Assert.False(isHandled);
            Assert.Empty(invite);
        }

        [Fact]
        public async Task InvitationCancelled_DuplicateInvitationCancelledEventHandled_DuplicateEventIgnored()
        {
            var sentEvent = new InvitationSentFaker(sequence: 1).Generate();
            await _handlerHelper.HandleAsync(sentEvent);

            var cancelledEvent = new InvitationCancelledFaker(sentEvent).Generate();
            await _handlerHelper.HandleAsync(cancelledEvent);

            var isHandled = await _handlerHelper.TryHandleAsync(cancelledEvent);

            Assert.True(isHandled);
        }

        [Fact]
        public async Task InvitationCancelled_EventSequenceNotExpectedYet_EventSetToWait()
        {
            var sentEvent = new InvitationSentFaker(sequence: 1).Generate();
            await _handlerHelper.HandleAsync(sentEvent);

            var cancelledEvent = new InvitationCancelledFaker(sentEvent)
                .RuleFor(e => e.Sequence, 3)
                .Generate();

            var isHandled = await _handlerHelper.TryHandleAsync(cancelledEvent);

            Assert.False(isHandled);
        }
    }
}
