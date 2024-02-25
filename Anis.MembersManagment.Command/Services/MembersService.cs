using Anis.MembersManagment.Command.Extensions;
using Anis.MembersManagment.Command.MembersProto;
using Grpc.Core;
using MediatR;

namespace Anis.MembersManagment.Command.Services
{
    public class MembersService(IMediator mediator) : Members.MembersBase
    {
        private readonly IMediator _mediator = mediator;

        #region Invitations
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

        #endregion

        #region Members
        public override async Task<Response> JoinMember(JoinMemberRequest request, ServerCallContext context)
        {
            var command = request.ToCommand();

            var response = await _mediator.Send(command);

            return new Response()
            {
                Id = response,
                Message  = ""
            };
        }
        #endregion
    }
}
