using Anis.MembersManagment.Query.Abstractions.IRepositories;
using Anis.MembersManagment.Query.EventHandlers.Sent;
using MediatR;

namespace Anis.MembersManagment.Query.EventHandlers.IncrementSequence
{
    public class UnknownEventHandler : IRequestHandler<UnknownEvent, bool>
    {
        private readonly ILogger<InvitationSentHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public UnknownEventHandler(IUnitOfWork unitOfWork, ILogger<InvitationSentHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<bool> Handle(UnknownEvent @event, CancellationToken cancellationToken)
        {
            var subscriber = await _unitOfWork.Subscriber.GetAsync(s => s.Id == @event.AggregateId);

            if (subscriber is null)
            {
                _logger.LogWarning("Aggregate {AggregateId} not found", @event.AggregateId);
                return false;
            }

            if (@event.Sequence <= subscriber.Sequence) return true;

            if (@event.Sequence > subscriber.Sequence + 1)
            {
                _logger.LogWarning("{Sequence} is not the expected sequence for aggregate {AggregateId}", @event.Sequence, @event.AggregateId);
                return false;
            }

            await _unitOfWork.Subscriber.UpdateSequence(@event.AggregateId,@event.Sequence);
            await _unitOfWork.Invitation.UpdateSequence(@event.AggregateId,@event.Sequence);
            await _unitOfWork.Permission.UpdateSequence(@event.AggregateId,@event.Sequence);

            await _unitOfWork.CommitAsync(cancellationToken);
            return true;
        }
    }
}
