using Anis.MembersManagment.Query.Entities;
using Anis.MembersManagment.Query.MembersProto;

namespace Anis.MembersManagment.Query.Extensions
{
    public static class QueryResultsExtensions
    {
        public static SubscriberOutput ToSubscriberOutput(this Subscriber subscriber) =>
            new()
            {
                Id = subscriber.Id,
                SubscriptionId = subscriber.SubscriptionId,
                SubscriptionDescription = subscriber.Subscription!.Description,
                UserId = subscriber.UserId,
                UserName = subscriber.User!.Name,
                Status = subscriber.Status,
                JoinedAt = subscriber.JoinedAt.ToUtcTimestamp()
            };

        public static InvitationOutput ToInvitationOutput(this Invitation invitation) =>
            new()
            {
                Id = invitation.Id,
                SubscriptionId = invitation.SubscriptionId,
                SubscriptionDescription = invitation.Subscription!.Description,
                MemberId = invitation.UserId,
                MemberName = invitation.User!.Name,
                Status = invitation.Status,
                SentAt = invitation.SentAt.ToUtcTimestamp()
            };
    }
}
