using Anis.MembersManagment.Query.Abstractions.IRepositories;
using Anis.MembersManagment.Query.Entities;
using Anis.MembersManagment.Query.EventHandlers.Sent;
using MediatR;

namespace Anis.MembersManagment.Query.EventHandlers.Cancelled
{
    public class InvitationCancelledHandler : IRequestHandler<InvitationCancelled, bool>
    {
        private readonly ILogger<InvitationSentHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public InvitationCancelledHandler(IUnitOfWork unitOfWork, ILogger<InvitationSentHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<bool> Handle(InvitationCancelled @event, CancellationToken cancellationToken)
        {
            var invite = await _unitOfWork.Invitation.GetAsync(i => i.Id == @event.AggregateId);

            if (invite is null)
            {
                _logger.LogWarning("Invitation {AggregateId} not found", @event.AggregateId);
                return false;
            }

            if (@event.Sequence <= invite.Sequence) return true;

            if (@event.Sequence >= invite.Sequence + 1)
            {
                _logger.LogWarning("{Sequence} is not the expected sequence for invitation {AggregateId}", @event.Sequence, @event.AggregateId);
                return false;
            }

            await _unitOfWork.Invitation.ChangeStatusAsync(Invitation.FromInvitationCancelledEvent(@event));

            var permssions = await _unitOfWork.Permission.GetAsync(
                p => p.UserId == @event.Data.MemberId && p.SubscriptionId == @event.Data.SubscriptionId);

            if (permssions is not null)
                await _unitOfWork.Permission.RemoveAsync(permssions);

            await _unitOfWork.CommitAsync(cancellationToken);

            return true;
        }
    }
}
