using Anis.MembersManagment.Query.Test.Helpers;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit.Abstractions;

namespace Anis.MembersManagment.Query.Test.EventHandlersTests.Sent
{
    public class InvitationSentHandlerTest : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public InvitationSentHandlerTest(WebApplicationFactory<Program> factory, ITestOutputHelper helper)
        {
            _factory = factory.WithDefaultConfigurations(helper, services =>
            {
                services.ReplaceWithInMemoryDatabase();
            });
        }

        [Fact]
        public Task InvitationSent_InvitationSentEventHandled_NewInvitationSaved()
        {
            throw new NotImplementedException();
        }
    }
}
