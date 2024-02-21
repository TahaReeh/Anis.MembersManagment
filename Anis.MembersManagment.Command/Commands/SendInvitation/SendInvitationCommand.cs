using Anis.MembersManagment.Command.Domain;
using MediatR;

namespace Anis.MembersManagment.Command.Commands.SendInvitation
{
    public class SendInvitationCommand : IRequest<string>
    {
        public required string AccountId { get; init; }
        public required string SubscriptionId { get; init; }
        public required string MemberId { get; init; }
        public required string UserId { get; init; }
        public required Permission Permissions { get; init; }
    }
}
