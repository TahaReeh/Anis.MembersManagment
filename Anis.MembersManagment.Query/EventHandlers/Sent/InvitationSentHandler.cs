using Anis.MembersManagment.Query.Abstractions.IRepositories;
using Anis.MembersManagment.Query.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Anis.MembersManagment.Query.EventHandlers.Sent
{
    public class InvitationSentHandler(IUnitOfWork unitOfWork, ILogger<InvitationSentHandler> logger) : IRequestHandler<InvitationSent, bool>
    {
        private readonly ILogger<InvitationSentHandler> _logger = logger;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<bool> Handle(InvitationSent @event, CancellationToken cancellationToken)
        {
            var invite = await _unitOfWork.Invitation.GetAsync(i => i.Id == @event.AggregateId);

            if (invite is not null)
            {
                if (@event.Sequence <= invite.Sequence) return true;

                if (@event.Sequence > invite.Sequence + 1)
                {
                    _logger.LogWarning("{Sequence} is not the expected sequence for invitation {AggregateId}", @event.Sequence, @event.AggregateId);
                    return false;
                }

                await _unitOfWork.Invitation.ChangeStatusAsync(Invitation.FromInvitationSentEvent(@event));
            }
            else
            {
                await _unitOfWork.Invitation.AddAsync(Invitation.FromInvitationSentEvent(@event), cancellationToken);
            }

            await _unitOfWork.Permission.AddAsync(Permission.FromInvitationSentEvent(@event), cancellationToken);

            await _unitOfWork.Subscriber.UpdateSequence(@event.AggregateId, @event.Sequence);

            await _unitOfWork.CommitAsync(cancellationToken);
            return true;
        }
    }
}
