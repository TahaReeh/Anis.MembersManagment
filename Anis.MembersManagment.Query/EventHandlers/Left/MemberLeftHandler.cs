using Anis.MembersManagment.Query.Abstractions.IRepositories;
using Anis.MembersManagment.Query.Entities;
using Anis.MembersManagment.Query.EventHandlers.Sent;
using MediatR;

namespace Anis.MembersManagment.Query.EventHandlers.Left
{
    public class MemberLeftHandler : IRequestHandler<MemberLeft, bool>
    {
        private readonly ILogger<InvitationSentHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public MemberLeftHandler(IUnitOfWork unitOfWork, ILogger<InvitationSentHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<bool> Handle(MemberLeft @event, CancellationToken cancellationToken)
        {
            var subscriber = await _unitOfWork.Subscriber.GetAsync(s => s.Id == @event.AggregateId);

            if (subscriber is null)
            {
                _logger.LogWarning("Subscriber {AggregateId} not found", @event.AggregateId);
                return false;
            }

            if (@event.Sequence <= subscriber.Sequence) return true;

            if (@event.Sequence > subscriber.Sequence + 1)
            {
                _logger.LogWarning("{Sequence} is not the expected sequence for subscriber {AggregateId}", @event.Sequence, @event.AggregateId);
                return false;
            }

            await _unitOfWork.Subscriber.ChangeStatusAsync(Subscriber.FromMemberLeftEvent(@event));

            var permssions = await _unitOfWork.Permission.GetAsync(p => p.Id == @event.AggregateId);

            if (permssions is not null)
                await _unitOfWork.Permission.RemoveAsync(permssions);

            await _unitOfWork.Invitation.UpdateSequence(@event.AggregateId, @event.Sequence);

            await _unitOfWork.CommitAsync(cancellationToken);
            return true;
        }
    }
}
