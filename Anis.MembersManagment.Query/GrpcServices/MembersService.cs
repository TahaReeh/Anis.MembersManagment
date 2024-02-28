using Anis.MembersManagment.Query;
using Anis.MembersManagment.Query.MembersProto;
using Grpc.Core;

namespace Anis.MembersManagment.Query.Services
{
    public class MembersService : Members.MembersBase
    {
        private readonly ILogger<MembersService> _logger;
        public MembersService(ILogger<MembersService> logger)
        {
            _logger = logger;
        }

    }
}
