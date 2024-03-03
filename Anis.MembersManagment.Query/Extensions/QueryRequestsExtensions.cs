using Anis.MembersManagment.Query.MembersProto;
using Anis.MembersManagment.Query.QueryHandlers.SubscriptionMembers;

namespace Anis.MembersManagment.Query.Extensions
{
    public static class QueryRequestsExtensions
    {
        public static SubscriptionMembersQuery ToQuery(this GetSubscriptionMembersRequest request)
          => new(
              SubscriptionId: request.SubscriptionId,
              Page: request.Page ?? 1,
              Size : request.Size ?? 20
              );


    }
}
