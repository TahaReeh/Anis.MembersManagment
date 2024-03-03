using MediatR;

namespace Anis.MembersManagment.Query.QueryHandlers.MemberSubscriptions
{
    public record MemberSubscriptionsQuery(
        string UserId,
        int Page,
        int Size
        ) : IRequest<MemberSubscriptionsResult>;
}
