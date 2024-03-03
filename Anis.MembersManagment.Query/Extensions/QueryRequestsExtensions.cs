using Anis.MembersManagment.Query.MembersProto;
using Anis.MembersManagment.Query.QueryHandlers.MemberPendingInvitations;
using Anis.MembersManagment.Query.QueryHandlers.MemberSubscriptions;
using Anis.MembersManagment.Query.QueryHandlers.OwnerPendingInvitations;
using Anis.MembersManagment.Query.QueryHandlers.SubscriptionMembers;

namespace Anis.MembersManagment.Query.Extensions
{
    public static class QueryRequestsExtensions
    {
        public static SubscriptionMembersQuery ToQuery(this GetSubscriptionMembersRequest request)
          => new(
              SubscriptionId: request.SubscriptionId,
              Page: request.Page ?? 1,
              Size: request.Size ?? 20
              );

        public static OwnerPendingInvitationsQuery ToQuery(this GetOwnerPendingInvitationsRequest request)
            => new(
                UserId: request.OwnerId,
                Page: request.Page ?? 1,
                Size: request.Size ?? 20
                );

        public static MemberPendingInvitationsQuery ToQuery(this GetMemberPendingInvitationsRequest request)
           => new(
               UserId: request.MemberId,
               Page: request.Page ?? 1,
               Size: request.Size ?? 20
               );

        public static MemberSubscriptionsQuery ToQuery(this GetMemberSubscriptionsRequest request)
            => new(
                UserId: request.MemberId,
                Page: request.Page ?? 1,
                Size: request.Size ?? 20
                );
    }
}
