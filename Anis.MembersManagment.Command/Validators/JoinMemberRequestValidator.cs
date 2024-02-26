using Anis.MembersManagment.Command.MembersProto;
using FluentValidation;

namespace Anis.MembersManagment.Command.Validators
{
    public class JoinMemberRequestValidator : AbstractValidator<JoinMemberRequest>
    {
        public JoinMemberRequestValidator()
        {
            RuleFor(r => r.AccountId).NotEmpty();

            RuleFor(r => r.SubscriptionId).NotEmpty();

            RuleFor(r => r.MemberId).NotEmpty();

            RuleFor(r => r.UserId).NotEmpty();
        }
    }
}
