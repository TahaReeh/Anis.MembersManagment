using Anis.MembersManagment.Command;
using Anis.MembersManagment.Command.InvitationsProto;
using Grpc.Core;

namespace Anis.MembersManagment.Command.Services
{
    public class InvitationsService : Invitations.InvitationsBase
    {
        private readonly ILogger<InvitationsService> _logger;
        public InvitationsService(ILogger<InvitationsService> logger)
        {
            _logger = logger;
        }

    }
}
