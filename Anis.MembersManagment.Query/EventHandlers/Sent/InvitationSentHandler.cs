using Anis.MembersManagment.Query.Abstractions.IRepositories;
using Anis.MembersManagment.Query.Entities;
using MediatR;

namespace Anis.MembersManagment.Query.EventHandlers.Sent
{
    public class InvitationSentHandler : IRequestHandler<InvitationSent, bool>
    {
        private readonly ILogger<InvitationSentHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public InvitationSentHandler(IUnitOfWork unitOfWork, ILogger<InvitationSentHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
        public async Task<bool> Handle(InvitationSent @event, CancellationToken cancellationToken)
        {
            var invite = await _unitOfWork.Invitation.GetAsync(i => i.Id == @event.AggregateId);

            if (invite is not null)
            {
                if (@event.Sequence <= invite.Sequence) return true;

                if (@event.Sequence >= invite.Sequence + 1)
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
            await _unitOfWork.CommitAsync(cancellationToken);
            return true;
        }
    }
}
