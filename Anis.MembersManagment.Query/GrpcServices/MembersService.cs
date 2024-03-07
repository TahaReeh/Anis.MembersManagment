using Anis.MembersManagment.Query;
using Anis.MembersManagment.Query.Extensions;
using Anis.MembersManagment.Query.MembersProto;
using Grpc.Core;
using MediatR;

namespace Anis.MembersManagment.Query.Services
{
    public class MembersService(IMediator mediator) : Members.MembersBase
    {
        private readonly IMediator _mediator = mediator;

        public override async Task<GetSubscriptionMembersResponse> GetSubscriptionMembers(GetSubscriptionMembersRequest request, ServerCallContext context)
        {
            var query = request.ToQuery();

            var result = await _mediator.Send(query, context.CancellationToken);

            var outputs = result.Subscribers.Select(s => s.ToSubscriberOutput());

            return new GetSubscriptionMembersResponse()
            {
                Page = result.Page,
                PageSize = result.PageSize,
                TotalResults = result.TotalResults,
                Subscribers =
                {
                    outputs
                }
            };
        }

        public async override Task<GetOwnerPendingInvitationsResponse> GetOwnerPendingInvitations(GetOwnerPendingInvitationsRequest request, ServerCallContext context)
        {
            var query = request.ToQuery();

            var result = await _mediator.Send(query, context.CancellationToken);

            var output = result.Invitations.Select(i => i.ToInvitationOutput());

            return new GetOwnerPendingInvitationsResponse()
            {
                Page = result.Page,
                PageSize = result.PageSize,
                TotalResults = result.TotalResults,
                Invitations = { output }
            };
        }

        public async override Task<GetMemberPendingInvitationsResponse> GetMemberPendingInvitations(GetMemberPendingInvitationsRequest request, ServerCallContext context)
        {
            var query = request.ToQuery();

            var result = await _mediator.Send(query, context.CancellationToken);

            var output = result.Invitations.Select(i => i.ToInvitationOutput());

            return new GetMemberPendingInvitationsResponse()
            {
                Page = result.Page,
                PageSize = result.PageSize,
                TotalResults = result.TotalResults,
                Invitations = { output }
            };
        }

        public async override Task<GetMemberSubscriptionsResponse> GetMemberSubscriptions(GetMemberSubscriptionsRequest request, ServerCallContext context)
        {
            var query = request.ToQuery();

            var result = await _mediator.Send(query, context.CancellationToken);

            var output = result.Subscribers.Select(s => s.ToSubscriberOutput());

            return new GetMemberSubscriptionsResponse()
            {
                Page = result.Page,
                PageSize = result.PageSize,
                TotalResults = result.TotalResults,
                Subscribers = { output }
            };
        }
    }
}
