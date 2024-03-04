﻿using Anis.MembersManagment.Query.Abstractions.IRepositories;
using Anis.MembersManagment.Query.Test.Fakers.Cancelled;
using Anis.MembersManagment.Query.Test.Fakers.Sent;
using Anis.MembersManagment.Query.Test.Helpers;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using System.Net.NetworkInformation;
using Xunit.Abstractions;
using static System.Formats.Asn1.AsnWriter;

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
        public async Task InvitationCancelled_NewInvitationCancelledEventHandledWhenPendingInvitation_InvitationStatusUpdatedPermissionRemoved()
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


    }
}
