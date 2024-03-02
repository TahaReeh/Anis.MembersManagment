using Anis.MembersManagment.Query.EventHandlers.Accepted;
using Anis.MembersManagment.Query.EventHandlers.Joined;
using Anis.MembersManagment.Query.EventHandlers.Left;
using Anis.MembersManagment.Query.EventHandlers.Removed;
using System;

namespace Anis.MembersManagment.Query.Entities
{
    public class Subscriber
    {

        private Subscriber(
            string id,
            int sequence,
            string subscriptionId,
            string userId,
            string status,
            DateTime joinedAt
            )
        {
            Id = id;
            Sequence = sequence;
            SubscriptionId = subscriptionId;
            UserId = userId;
            Status = status;
            JoinedAt = joinedAt;
        }

        public static Subscriber FromInvitationAcceptedEvent(InvitationAccepted @event)
          => new(
              id: @event.AggregateId,
              sequence: @event.Sequence,
              subscriptionId: @event.Data.SubscriptionId,
              userId: @event.Data.MemberId,
              status: "Accepted", // to enum
              joinedAt: @event.DateTime
              );

        public static Subscriber FromMemberJoinedEvent(MemberJoined @event)
            => new(
                id: @event.AggregateId,
                sequence: @event.Sequence,
                subscriptionId: @event.Data.SubscriptionId,
                userId: @event.Data.MemberId,
                status: "Joined", // to enum
                joinedAt: @event.DateTime
                );

        public static Subscriber FromMemberRemovedEvent(MemberRemoved @event)
            => new(
                id: @event.AggregateId,
                sequence: @event.Sequence,
                subscriptionId: @event.Data.SubscriptionId,
                userId: @event.Data.MemberId,
                status: "Removed", // to enum
                joinedAt: @event.DateTime
                );

        public static Subscriber FromMemberLeftEvent(MemberLeft @event)
           => new(
               id: @event.AggregateId,
               sequence: @event.Sequence,
               subscriptionId: @event.Data.SubscriptionId,
               userId: @event.Data.MemberId,
               status: "Left", // to enum
               joinedAt: @event.DateTime
               );

        public string Id { get; private set; }
        public int Sequence { get; private set; }
        public string SubscriptionId { get; private set; }
        public Subscription? Subscription { get; private set; }
        public string UserId { get; private set; }
        public User? User { get; private set; }
        public string Status { get; private set; }
        public DateTime JoinedAt { get; private set; }

        public void ChangeStatus(Subscriber entity)
        {
            Sequence = entity.Sequence;
            Status = entity.Status;
            JoinedAt = entity.JoinedAt; // should only change in joined and accepted events
        }
    }
}
