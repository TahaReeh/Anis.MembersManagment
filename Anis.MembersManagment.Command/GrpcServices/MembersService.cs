using Anis.MembersManagment.Command.Extensions;
using Anis.MembersManagment.Command.MembersProto;
using Grpc.Core;
using MediatR;

namespace Anis.MembersManagment.Command.GrpcServices
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
                Message = "Member joined successfully"
            };
        }

        public override async Task<Response> RemoveMember(RemoveMemberRequest request, ServerCallContext context)
        {
            var command = request.ToCommand();

            var response = await _mediator.Send(command);

            return new Response()
            {
                Id = response,
                Message = "Member removed"
            };
        }

        public override async Task<Response> Leave(LeaveRequest request, ServerCallContext context)
        {
            var command = request.ToCommand();

            var response = await _mediator.Send(command);

            return new Response()
            {
                Id = response,
                Message = "Member left"
            };
        }

        public override async Task<Response> ChangePermission(ChangePermissionRequest request, ServerCallContext context)
        {
            var command = request.ToCommand();

            var response = await _mediator.Send(command);

            return new Response()
            {
                Id = response,
                Message = "Permissions changed successfully"
            };
        }
        #endregion
    }
}
