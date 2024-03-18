using Anis.MembersManagment.Query.Abstractions.IRepositories;
using Anis.MembersManagment.Query.Exceptions;
using MediatR;

namespace Anis.MembersManagment.Query.QueryHandlers.SubscriptionMembers
{
    public class SubscriptionMembersQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<SubscriptionMembersQuery, SubscriptionMembersResult>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<SubscriptionMembersResult> Handle(SubscriptionMembersQuery request, CancellationToken cancellationToken)
        {
            var subscribers = await _unitOfWork.Subscriber.GetAllAsync(s => s.SubscriptionId == request.SubscriptionId 
            && (s.Status == "Joined" || s.Status == "Accepted"),includeProperties: "Subscription,User",
                  request.Page, request.Size
                  );

            if (!subscribers.Any())
                throw new NotFoundException("There are no members in this subscription");

            return new SubscriptionMembersResult(
                Page: request.Page,
                PageSize: request.Size,
                TotalResults: subscribers.Count(),
                Subscribers: subscribers.ToList()
                );
        }
    }
}
