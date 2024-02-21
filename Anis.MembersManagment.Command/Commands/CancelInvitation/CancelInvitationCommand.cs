﻿using MediatR;

namespace Anis.MembersManagment.Command.Commands.CancelInvitation
{
    public class CancelInvitationCommand : IRequest<string>
    {
        public required string Id { get; init; }
        public required string AccountId { get; init; }
        public required string SubscriptionId { get; init; }
        public required string MemberId { get; init; }
        public required string UserId { get; init; } 
    }
}
