using Anis.MembersManagment.Query.MembersProto;
using FluentValidation;

namespace Anis.MembersManagment.Query.Validators
{
    public class OwnerPendingInvitationsValidator : AbstractValidator<GetOwnerPendingInvitationsRequest>
    {
        public OwnerPendingInvitationsValidator()
        {
            RuleFor(r=>r.OwnerId).NotEmpty();
        }
    }
}
