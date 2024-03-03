using Anis.MembersManagment.Query.Abstractions.IRepositories;
using Anis.MembersManagment.Query.Exceptions;
using MediatR;

namespace Anis.MembersManagment.Query.QueryHandlers.MemberPendingInvitations
{
    public class MemberPendingInvitationsQueryHandler : IRequestHandler<MemberPendingInvitationsQuery, MemberPendingInvitationsResult>
    {
        private readonly IUnitOfWork _unitOfWork;

        public MemberPendingInvitationsQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<MemberPendingInvitationsResult> Handle(MemberPendingInvitationsQuery request, CancellationToken cancellationToken)
        {
            var invitations = await _unitOfWork.Invitation.GetAllAsync(i => i.UserId == request.UserId,
                   includeProperties: "Subscription,User",
              request.Page, request.Size);

            if (invitations is null)
                throw new NotFoundException("There are no pending invitations");

            return new MemberPendingInvitationsResult(
                Page: request.Page,
                PageSize: request.Size,
                TotalResults: invitations.Count(),
                Invitations: invitations.ToList()
                );
        }
    }
}
