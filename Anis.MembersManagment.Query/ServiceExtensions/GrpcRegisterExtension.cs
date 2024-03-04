using Anis.MembersManagment.Query.GrpcServices.Interceptors;
using Anis.MembersManagment.Query.Validators;
using Calzolari.Grpc.AspNetCore.Validation;

namespace Anis.MembersManagment.Query.ServiceExtensions
{
    public static class GrpcRegisterExtension
    {
        public static void AddGrpcWithValidators(this IServiceCollection services)
        {
            services.AddGrpc(options =>
            {
                options.EnableMessageValidation();
                options.Interceptors.Add<ApplicationExceptionInterceptor>();
            });

            AddValidators(services);
        }

        private static void AddValidators(IServiceCollection services)
        {
            services.AddGrpcValidation();
            services.AddValidator<MemberPendingInvitationsValidator>();
            services.AddValidator<MemberSubscriptionsValidator>();
            services.AddValidator<OwnerPendingInvitationsValidator>();
            services.AddValidator<SubscriptionMembersValidator>();
        }
    }
}
