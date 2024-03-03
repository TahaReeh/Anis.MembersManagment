using Anis.MembersManagment.Query.MembersProto;
using FluentValidation;

namespace Anis.MembersManagment.Query.Validators
{
    public class MemberSubscriptionsValidator : AbstractValidator<GetMemberSubscriptionsRequest>
    {
        public MemberSubscriptionsValidator()
        {
            RuleFor(r => r.MemberId).NotEmpty();
        }
    }
}
