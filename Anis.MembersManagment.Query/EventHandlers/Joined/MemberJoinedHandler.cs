using Anis.MembersManagment.Query.Abstractions.IRepositories;
using Anis.MembersManagment.Query.Entities;
using Anis.MembersManagment.Query.EventHandlers.Sent;
using MediatR;
using System.ComponentModel;

namespace Anis.MembersManagment.Query.EventHandlers.Joined
{
    public class MemberJoinedHandler : IRequestHandler<MemberJoined, bool>
    {
        private readonly ILogger<InvitationSentHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public MemberJoinedHandler(IUnitOfWork unitOfWork, ILogger<InvitationSentHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<bool> Handle(MemberJoined @event, CancellationToken cancellationToken)
        {
            var subscriber = await _unitOfWork.Subscriber.GetAsync(s => s.Id == @event.AggregateId);

            if (subscriber is not null)
            {
                if (@event.Sequence <= subscriber.Sequence) return true;

                if (@event.Sequence >= subscriber.Sequence + 1)
                {
                    _logger.LogWarning("{Sequence} is not the expected sequence for subscriber {AggregateId}", @event.Sequence, @event.AggregateId);
                    return false;
                }

                await _unitOfWork.Subscriber.ChangeStatusAsync(Subscriber.FromMemberJoinedEvent(@event));
            }
            else
            {
                await _unitOfWork.Subscriber.AddAsync(Subscriber.FromMemberJoinedEvent(@event), cancellationToken);
            }

            await _unitOfWork.Permission.AddAsync(Permission.FromMemberJoinedEvent(@event), cancellationToken);

            await _unitOfWork.CommitAsync(cancellationToken);

            return true;
        }
    }
}
