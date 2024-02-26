using Anis.MembersManagment.Command.Validators;
using Calzolari.Grpc.AspNetCore.Validation;

namespace Anis.MembersManagment.Command.Services
{
    public static class GrpcRegisterExtension
    {
        public static void AddGrpcWithValidators(this IServiceCollection services)
        {
            services.AddGrpc(options =>
            {
                options.EnableMessageValidation();

            });

            AddValidators(services);
        }

        public static void AddValidators(IServiceCollection services)
        {
            services.AddGrpcValidation();
            services.AddValidator<AcceptInvitationRequestValidator>();
            services.AddValidator<CancelInvitationRequestValidator>();
            services.AddValidator<ChangePermissionRequestValidator>();
            services.AddValidator<JoinMemberRequestValidator>();
            services.AddValidator<LeaveRequestValidator>();
            services.AddValidator<RejectInvitationRequestValidator>();
            services.AddValidator<RemoveMemberRequestValidator>();
            services.AddValidator<SendInvitationRequestValidator>();
        }
    }
}
