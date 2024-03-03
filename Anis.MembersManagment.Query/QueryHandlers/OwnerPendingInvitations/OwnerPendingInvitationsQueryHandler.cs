﻿using Anis.MembersManagment.Query.Abstractions.IRepositories;
using Anis.MembersManagment.Query.Entities;
using Anis.MembersManagment.Query.Exceptions;
using MediatR;

namespace Anis.MembersManagment.Query.QueryHandlers.OwnerPendingInvitations
{
    public class OwnerPendingInvitationsQueryHandler : IRequestHandler<OwnerPendingInvitationsQuery, OwnerPendingInvitationsResult>
    {
        private readonly IUnitOfWork _unitOfWork;

        public OwnerPendingInvitationsQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<OwnerPendingInvitationsResult> Handle(OwnerPendingInvitationsQuery request, CancellationToken cancellationToken)
        {
            var invitations = await _unitOfWork.Invitation.GetAllAsync(i => i.Subscription!.UserId == request.UserId,
                includeProperties: "Subscription,User",
                request.Page, request.Size);

            if (invitations is null)
                throw new NotFoundException("There are no pending invitations");

            return new OwnerPendingInvitationsResult(
                Page: request.Page,
                PageSize: request.Size,
                TotalResults: invitations.Count(),
                Invitations: invitations.ToList()
                );
        }
    }
}