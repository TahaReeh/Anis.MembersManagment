using Anis.MembersManagment.Query.Abstractions.IRepositories;
using MediatR;

namespace Anis.MembersManagment.Query.QueryHandlers.SubscriptionMembers
{
    public class SubscriptionMembersQueryHandler : IRequestHandler<SubscriptionMembersQuery, SubscriptionMembersResult>
    {
        private readonly IUnitOfWork _unitOfWork;

        public SubscriptionMembersQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<SubscriptionMembersResult> Handle(SubscriptionMembersQuery request, CancellationToken cancellationToken)
        {
            var subscribers = await _unitOfWork.Subscriber.GetAllAsync(s => s.SubscriptionId == request.SubscriptionId,
                  includeProperties: "Subscription,User",
                  request.Page, request.Size
                  );

            return new SubscriptionMembersResult(
                Page: request.Page,
                PageSize: request.Size,
                TotalResults: subscribers.Count(),
                Subscribers: subscribers.ToList()
                );
        }
    }
}
