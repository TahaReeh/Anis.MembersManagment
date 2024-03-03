using Anis.MembersManagment.Query;
using Anis.MembersManagment.Query.Extensions;
using Anis.MembersManagment.Query.MembersProto;
using Grpc.Core;
using MediatR;

namespace Anis.MembersManagment.Query.Services
{
    public class MembersService : Members.MembersBase
    {
        private readonly ILogger<MembersService> _logger;
        private readonly IMediator _mediator;

        public MembersService(ILogger<MembersService> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        public override async Task<GetSubscriptionMembersResponse> GetSubscriptionMembers(GetSubscriptionMembersRequest request, ServerCallContext context)
        {
            var query = request.ToQuery();

            var result = await _mediator.Send(query, context.CancellationToken);

            var outputs = result.Subscribers.Select(t => t.ToSubscriberOutput());

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

        public override Task<GetOwnerPendingInvitationsResponse> GetOwnerPendingInvitations(GetOwnerPendingInvitationsRequest request, ServerCallContext context)
        {
            return base.GetOwnerPendingInvitations(request, context);
        }

        public override Task<GetMemberPendingInvitationsResponse> GetMemberPendingInvitations(GetMemberPendingInvitationsRequest request, ServerCallContext context)
        {
            return base.GetMemberPendingInvitations(request, context);
        }

        public override Task<GetMemberSubscriptionsResponse> GetMemberSubscriptions(GetMemberSubscriptionsRequest request, ServerCallContext context)
        {
            return base.GetMemberSubscriptions(request, context);
        }
    }
}
