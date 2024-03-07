using Anis.MembersManagment.Query.EventHandlers.Accepted;
using Anis.MembersManagment.Query.EventHandlers.Cancelled;
using Anis.MembersManagment.Query.EventHandlers.Rejected;
using Anis.MembersManagment.Query.EventHandlers.Sent;

namespace Anis.MembersManagment.Query.Entities
{
    public class Invitation
    {
        private Invitation(
            string id,
            int sequence,
            string subscriptionId,
            string userId,
            string status,
            DateTime sentAt)
        {
            Id = id;
            Sequence = sequence;
            SubscriptionId = subscriptionId;
            UserId = userId;
            Status = status;
            SentAt = sentAt;
        }

        public static Invitation FromInvitationSentEvent(InvitationSent @event)
            => new(
                id: @event.AggregateId,
                sequence: @event.Sequence,
                subscriptionId: @event.Data.SubscriptionId,
                userId: @event.Data.MemberId,
                status: "Pending", // TODO: replace magic words
                sentAt: @event.DateTime
                );

        public static Invitation FromInvitationCancelledEvent(InvitationCancelled @event)
            => new(
                id: @event.AggregateId,
                sequence: @event.Sequence,
                subscriptionId: @event.Data.SubscriptionId,
                userId: @event.Data.MemberId,
                status: "Cancelled",
                sentAt: @event.DateTime
                );

        public static Invitation FromInvitationAcceptedEvent(InvitationAccepted @event)
            => new(
                id: @event.AggregateId,
                sequence: @event.Sequence,
                subscriptionId: @event.Data.SubscriptionId,
                userId: @event.Data.MemberId,
                status: "Accepted",
                sentAt: @event.DateTime
                );

        public static Invitation FromInvitationRejectedEvent(InvitationRejected @event)
           => new(
               id: @event.AggregateId,
               sequence: @event.Sequence,
               subscriptionId: @event.Data.SubscriptionId,
               userId: @event.Data.MemberId,
               status: "Rejected",
               sentAt: @event.DateTime
               );

        public string Id { get; private set; }
        public int Sequence { get; private set; }
        public string SubscriptionId { get; private set; }
        public Subscription Subscription { get; private set; } = default!;
        public string UserId { get; private set; }
        public User User { get; private set; } = default!;
        public string Status { get; private set; }
        public DateTime SentAt { get; private set; }

        public void ChangeStatus(Invitation entity)
        {
            Sequence = entity.Sequence;
            Status = entity.Status;
            if (entity.Status == "Sent")
                SentAt = entity.SentAt;
        }

        public void UpdateSequence(int sequence)
        {
            Sequence = sequence;
        }
    }
}
