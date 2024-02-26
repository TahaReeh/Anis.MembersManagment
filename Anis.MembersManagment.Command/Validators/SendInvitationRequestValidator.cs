using Anis.MembersManagment.Command.MembersProto;
using FluentValidation;

namespace Anis.MembersManagment.Command.Validators
{
    public class SendInvitationRequestValidator : AbstractValidator<SendInvitationRequest>
    {
        public SendInvitationRequestValidator()
        {
            RuleFor(r => r.AccountId).NotEmpty();

            RuleFor(r=>r.SubscriptionId).NotEmpty();

            RuleFor(r => r.MemberId).NotEmpty();

            RuleFor(r => r.UserId).NotEmpty();
        }
    }
}
