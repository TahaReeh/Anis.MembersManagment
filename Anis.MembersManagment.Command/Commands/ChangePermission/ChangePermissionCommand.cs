using Anis.MembersManagment.Command.Domain;
using MediatR;

namespace Anis.MembersManagment.Command.Commands.ChangePermission
{
    public class ChangePermissionCommand : IRequest<string>
    {
        public required string Id { get; init; }
        public required string AccountId { get; init; }
        public required string SubscriptionId { get; init; }
        public required string MemberId { get; init; }
        public required string UserId { get; init; }
        public required Permission Permissions { get; init; }
    }
}
