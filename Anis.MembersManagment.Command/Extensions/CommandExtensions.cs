using Anis.MembersManagment.Command.Commands.SendInvitation;
using Anis.MembersManagment.Command.Domain;
using Anis.MembersManagment.Command.InvitationsProto;

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
    }
}
