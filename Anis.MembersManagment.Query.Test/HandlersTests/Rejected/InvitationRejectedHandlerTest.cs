using Anis.MembersManagment.Query.Test.Fakers.Rejected;
using Anis.MembersManagment.Query.Test.Fakers.Sent;

namespace Anis.MembersManagment.Query.Test.HandlersTests.Rejected
{
    public class InvitationRejectedHandlerTest : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly EventHandlerHelper _handlerHelper;

        public InvitationRejectedHandlerTest(WebApplicationFactory<Program> factory, ITestOutputHelper helper)
        {
            _factory = factory.WithDefaultConfigurations(helper, services =>
            {
                services.ReplaceWithInMemoryDatabase();
            });

            _handlerHelper = new EventHandlerHelper(_factory.Services);
        }

        [Fact]
        public async Task InvitationRejected_EventHandledWhenPendingInvitation_InvitationStatusUpdatedPermissionRemoved()
        {
            var sentEvent = new InvitationSentFaker(sequence: 1).Generate();

            await _handlerHelper.HandleAsync(sentEvent);

            var rejectedEvent = new InvitationRejectedFaker(sentEvent).Generate();

            var isHandled = await _handlerHelper.TryHandleAsync(rejectedEvent);

            using var scope = _factory.Services.CreateScope();
            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            var invite = await unitOfWork.Invitation.GetAllAsync();
            var permission = await unitOfWork.Permission.GetAllAsync();

            Assert.True(isHandled);
            Assert.Single(invite);
            Assert.Equal("Rejected", invite.First().Status);
            Assert.Empty(permission);
        }

        [Fact]
        public async Task InvitationRejected_InvitationRejectedEventHandledWithNoPendingInvitation_EventSetToWait()
        {
            var sentEvent = new InvitationSentFaker(sequence: 1).Generate();

            var rejectedEvent = new InvitationRejectedFaker(sentEvent).Generate();

            var isHandled = await _handlerHelper.TryHandleAsync(rejectedEvent);

            using var scope = _factory.Services.CreateScope();
            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            var invite = await unitOfWork.Invitation.GetAllAsync();

            Assert.False(isHandled);
            Assert.Empty(invite);
        }

        [Fact]
        public async Task InvitationRejected_DuplicateInvitationRejectedEventHandled_DuplicateEventIgnored()
        {
            var sentEvent = new InvitationSentFaker(sequence: 1).Generate();

            await _handlerHelper.HandleAsync(sentEvent);

            var rejectedEvent = new InvitationRejectedFaker(sentEvent).Generate();

            await _handlerHelper.HandleAsync(rejectedEvent);

            var isHandled = await _handlerHelper.TryHandleAsync(rejectedEvent);

            Assert.True(isHandled);
        }

        [Fact]
        public async Task InvitationRejected_EventSequenceNotExpectedYet_EventSetToWait()
        {
            var sentEvent = new InvitationSentFaker(sequence: 1).Generate();

            var rejectedEvent = new InvitationRejectedFaker(sentEvent)
                .RuleFor(e=>e.Sequence,3).Generate();

            var isHandled = await _handlerHelper.TryHandleAsync(rejectedEvent);

            Assert.False(isHandled);
        }
    }
}
