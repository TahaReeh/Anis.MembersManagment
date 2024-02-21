using Anis.MembersManagment.Command.Extensions;
using Anis.MembersManagment.Command.Infrastructure.Persistence;
using Anis.MembersManagment.Command.InvitationsProto;
using Grpc.Core;
using MediatR;

namespace Anis.MembersManagment.Command.Services
{
    public class InvitationsService : Invitations.InvitationsBase
    {
        private readonly IMediator _mediator;

        public InvitationsService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override async Task<Response> SendInvitation(SendInvitationRequest request, ServerCallContext context)
        {
            var command = request.ToCommand();

            var response = await _mediator.Send(command);

            return new Response()
            {
                Id = response,
                Message = "Invitation sent successfully"
            };
        }

        public override async Task<Response> AcceptInvitation(AcceptInvitationRequest request, ServerCallContext context)
        {
            var command = request.ToCommand();

            var response = await _mediator.Send(command);

            return new Response()
            {
                Id = response,
                Message = "Invitation accepted"
            };
        }

        public override async Task<Response> CancelInvitation(CancelInvitationRequest request, ServerCallContext context)
        {
            var command = request.ToCommand();

            var response = await _mediator.Send(command);

            return new Response()
            {
                Id = response,
                Message = "Invitation cancelled"
            };
        }

        public override async Task<Response> RejectInvitation(RejectInvitationRequest request, ServerCallContext context)
        {
            var command = request.ToCommand();

            var response = await _mediator.Send(command);

            return new Response()
            {
                Id = response,
                Message = "Invitation rejected"
            };
        }
    }
}
