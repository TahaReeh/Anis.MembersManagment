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

        public override Task<GetSubscriptionMembersResponse> GetSubscriptionMembers(GetSubscriptionMembersRequest request, ServerCallContext context)
        {
            return base.GetSubscriptionMembers(request, context);
        }

        public override Task<GetOwnerPendingInvitationsResponse> GetOwnerPendingInvitations(GetOwnerPendingInvitationsRequest request, ServerCallContext context)
        {
            return base.GetOwnerPendingInvitations(request, context);
        }

        public override Task<GetMemberPendingInvitationsResponse> GetMemberPendingInvitations(GetMemberPendingInvitationsRequest request, ServerCallContext context)
        {
            return base.GetMemberPendingInvitations(request, context);
        }

        public override Task<GetMemberSubscriptionsResponse> GetMemberSubscriptions(GetMemberSubscriptionsRequest request, ServerCallContext context)
        {
            return base.GetMemberSubscriptions(request, context);
        }
    }
}
