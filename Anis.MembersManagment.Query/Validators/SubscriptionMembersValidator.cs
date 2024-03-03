using Anis.MembersManagment.Query.MembersProto;
using FluentValidation;

namespace Anis.MembersManagment.Query.Validators
{
    public class SubscriptionMembersValidator : AbstractValidator<GetSubscriptionMembersRequest>
    {
        public SubscriptionMembersValidator()
        {
            RuleFor(r => r.SubscriptionId).NotEmpty();
        }
    }
}
