using Anis.MembersManagment.Query.Abstractions.IRepositories;
using Anis.MembersManagment.Query.Exceptions;
using MediatR;

namespace Anis.MembersManagment.Query.QueryHandlers.MemberSubscriptions
{
    public class MemberSubscriptionsQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<MemberSubscriptionsQuery, MemberSubscriptionsResult>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<MemberSubscriptionsResult> Handle(MemberSubscriptionsQuery request, CancellationToken cancellationToken)
        {
            var subscribers = await _unitOfWork.Subscriber.GetAllAsync(s => s.UserId == request.UserId
            && (s.Status == "Joined" || s.Status == "Accepted"), includeProperties: "Subscription,User",
                 request.Page, request.Size
                 );

            if (!subscribers.Any())
                throw new NotFoundException("There are no subscriptions for this member");

            return new MemberSubscriptionsResult(
                Page: request.Page,
                PageSize: request.Size,
                TotalResults: subscribers.Count(),
                Subscribers: subscribers.ToList()
                );
        }
    }
}
