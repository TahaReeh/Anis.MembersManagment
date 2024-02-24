using Anis.MembersManagment.Command.Commands.AcceptInvitation;
using Anis.MembersManagment.Command.Commands.CancelInvitation;
using Anis.MembersManagment.Command.Commands.RejectInvitation;
using Anis.MembersManagment.Command.Commands.SendInvitation;
using Anis.MembersManagment.Command.Domain;
using Anis.MembersManagment.Command.MembersProto;

namespace Anis.MembersManagment.Command.Extensions
{
    public static class CommandExtensions
    {
        public static SendInvitationCommand ToCommand(this SendInvitationRequest request)
            => new() { 
                AccountId = request.AccountId ,
                SubscriptionId = request.SubscriptionId ,
                MemberId = request.MemberId ,
                UserId = request.UserId ,
                Permissions = new Permission
                {
                    Transfer = request.Permissions.Transfer,
                    PurchaseCards = request.Permissions.PurchaseCards,
                    ManageDevices = request.Permissions.ManageDevices
                }
            };

        public static AcceptInvitationCommand ToCommand(this AcceptInvitationRequest request)
           => new()
           {
               Id = request.Id,
               AccountId = request.AccountId,
               SubscriptionId = request.SubscriptionId,
               MemberId = request.MemberId,
               UserId = request.UserId
           };

        public static CancelInvitationCommand ToCommand(this CancelInvitationRequest request)
           => new()
           {
               Id = request.Id,
               AccountId = request.AccountId,
               SubscriptionId = request.SubscriptionId,
               MemberId = request.MemberId,
               UserId = request.UserId
           };

        public static RejectInvitationCommand ToCommand(this RejectInvitationRequest request)
            => new()
            {
                Id = request.Id,
                AccountId = request.AccountId,
                SubscriptionId = request.SubscriptionId,
                MemberId = request.MemberId,
                UserId = request.UserId
            };
    }
}
