using Anis.MembersManagment.Query.EventHandlers.Changed;
using Anis.MembersManagment.Query.EventHandlers.Joined;
using Anis.MembersManagment.Query.EventHandlers.Sent;

namespace Anis.MembersManagment.Query.Entities
{
    public class Permission
    {
        private Permission(
            int sequence,
            string userId,
            string subscriptionId,
            bool transfer,
            bool purchaseCards,
            bool manageDevices)
        {
            Sequence = sequence;
            UserId = userId;
            SubscriptionId = subscriptionId;
            Transfer = transfer;
            PurchaseCards = purchaseCards;
            ManageDevices = manageDevices;
        }

        public static Permission FromInvitationSentEvent(InvitationSent @event)
         => new(
             sequence: @event.Sequence,
             userId: @event.Data.MemberId,
             subscriptionId: @event.Data.SubscriptionId,
             transfer: @event.Data.Permissions.Transfer,
             purchaseCards: @event.Data.Permissions.PurchaseCards,
             manageDevices: @event.Data.Permissions.ManageDevices
             );

        public static Permission FromMemberJoinedEvent(MemberJoined @event)
         => new(
             sequence: @event.Sequence,
             userId: @event.Data.MemberId,
             subscriptionId: @event.Data.SubscriptionId,
             transfer: @event.Data.Permissions.Transfer,
             purchaseCards: @event.Data.Permissions.PurchaseCards,
             manageDevices: @event.Data.Permissions.ManageDevices
             );

        public static Permission FromPermissionChangedEvent(PermissionChanged @event)
         => new(
             sequence: @event.Sequence,
             userId: @event.Data.MemberId,
             subscriptionId: @event.Data.SubscriptionId,
             transfer: @event.Data.Permissions.Transfer,
             purchaseCards: @event.Data.Permissions.PurchaseCards,
             manageDevices: @event.Data.Permissions.ManageDevices
             );

        public int Id { get; private set; }
        public int Sequence { get; private set; }
        public string UserId { get; private set; }
        public User? User { get; private set; }
        public string SubscriptionId { get; private set; }
        public Subscription? Subscription { get; private set; }
        public bool Transfer { get; private set; }
        public bool PurchaseCards { get; private set; }
        public bool ManageDevices { get; private set; }

        public void ChangePermission(Permission entity)
        {
            Sequence = entity.Sequence;
            Transfer = entity.Transfer;
            PurchaseCards = entity.PurchaseCards;
            ManageDevices = entity.ManageDevices;
        }

        public void UpdateSequence(int sequence)
        {
            Sequence = sequence;
        }
    }
}
