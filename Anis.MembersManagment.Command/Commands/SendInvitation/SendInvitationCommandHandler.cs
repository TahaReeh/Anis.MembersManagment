﻿using Anis.MembersManagment.Command.Abstractions;
using Anis.MembersManagment.Command.Domain;
using Anis.MembersManagment.Command.Exceptions;
using Grpc.Core;
using MediatR;

namespace Anis.MembersManagment.Command.Commands.SendInvitation
{
    public class SendInvitationCommandHandler(IEventStore eventStore) : IRequestHandler<SendInvitationCommand, string>
    {
        private readonly IEventStore _eventStore = eventStore;

        public async Task<string> Handle(SendInvitationCommand command, CancellationToken cancellationToken)
        {
            int newinvitationNumber = 1;
            char delimiter = '_';

            var events = await _eventStore.GetAllLikeAsync($"{command.SubscriptionId}_{command.MemberId}", cancellationToken);

            if (events.Count != 0)
            {
                if (events.Last().Type is "InvitationSent")
                    throw new RpcException(new Status(StatusCode.InvalidArgument, "Invitation still pending"));  //why not AlreadyExistException

                if (events.Last().Type is "InvitationAccepted") //or "MemberJoined"
                    throw new RpcException(new Status(StatusCode.InvalidArgument, "The member already exists in this subscription"));

                if (events.Last().Type is "InvitationCancelled" or "InvitationRejected") //or "MemberLeft" or "MemberRemoved"
                {
                    string[] aggregateParts = events.Last().AggregateId.Split(delimiter);
                    string invitationNumber = aggregateParts.Length > 1 ? aggregateParts[2] : "";

                    newinvitationNumber = Convert.ToInt32(invitationNumber) + 1;
                }
            }

            var invitation = Invitation.SendInvitation(command, newinvitationNumber.ToString());

            await _eventStore.CommitAsync(invitation, cancellationToken);

            return invitation.Id.ToString();
        }
    }
}
