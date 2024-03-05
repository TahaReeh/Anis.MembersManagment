using Anis.MembersManagment.Query.Abstractions.IRepositories;
using Anis.MembersManagment.Query.Entities;
using Anis.MembersManagment.Query.EventHandlers.Sent;
using MediatR;
using System.ComponentModel;

namespace Anis.MembersManagment.Query.EventHandlers.Changed
{
    public class PermissionChangedHandler : IRequestHandler<PermissionChanged, bool>
    {
        private readonly ILogger<InvitationSentHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public PermissionChangedHandler(IUnitOfWork unitOfWork, ILogger<InvitationSentHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<bool> Handle(PermissionChanged @event, CancellationToken cancellationToken)
        {
            var permssions = await _unitOfWork.Permission.GetAsync(p => p.Id == @event.AggregateId);

            if (permssions is null)
            {
                _logger.LogWarning("Permissions {AggregateId} not found", @event.AggregateId);
                return false;
            }

            if (@event.Sequence <= permssions.Sequence) return true;

            if (@event.Sequence > permssions.Sequence + 1)
            {
                _logger.LogWarning("{Sequence} is not the expected sequence for permission {AggregateId}", @event.Sequence, @event.AggregateId);
                return false;
            }

            await _unitOfWork.Permission.ChangePermissions(Permission.FromPermissionChangedEvent(@event));

            await _unitOfWork.Subscriber.UpdateSequence(@event.AggregateId, @event.Sequence);
            await _unitOfWork.Invitation.UpdateSequence(@event.AggregateId, @event.Sequence);

            await _unitOfWork.CommitAsync(cancellationToken);
            return true;
        }
    }
}
