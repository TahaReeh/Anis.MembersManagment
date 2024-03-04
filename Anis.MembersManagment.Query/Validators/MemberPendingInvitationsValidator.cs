using Anis.MembersManagment.Query.MembersProto;
using FluentValidation;

namespace Anis.MembersManagment.Query.Validators
{
    public class MemberPendingInvitationsValidator : AbstractValidator<GetMemberPendingInvitationsRequest>
    {
        public MemberPendingInvitationsValidator()
        {
            RuleFor(r=>r.MemberId).NotEmpty();
        }
    }
}
