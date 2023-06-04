using System.Runtime.CompilerServices;

namespace Platform.ReferencialData.WebAPI
{
    public static class RolePolicyConfiguration
    {
        public static IServiceCollection PermissionConfiguration(this IServiceCollection services)
        {
          return  services.AddAuthorization(options =>
            {
                options.AddPolicy("createManagerRH", policy =>
                {
                    policy.RequireClaim("Permission", "createManagerRH");
                });
                options.AddPolicy("createManagerFacturation", policy =>
                {
                    policy.RequireClaim("Permission", "createManagerFacturation");
                });
                options.AddPolicy("createEmployee", policy =>
                {
                    policy.RequireClaim("Permission", "createEmployee");
                });
                options.AddPolicy("refillAllWallet", policy =>
                {
                    policy.RequireClaim("Permission", "refillAllWallet");
                });
                options.AddPolicy("refillWallet", policy =>
                {
                    policy.RequireClaim("Permission", "refillWallet");
                });
                options.AddPolicy("accessHistorieFromEmployerToEmployee", policy =>
                {
                    policy.RequireClaim("Permission", "accessHistorieFromEmployerToEmployee");
                });
                options.AddPolicy("accessHistorieFromDynoToEmployer", policy =>
                {
                    policy.RequireClaim("Permission", "accessHistorieFromDynoToEmployer");
                });
                options.AddPolicy("refillEmployerWallet", policy =>
                {
                    policy.RequireClaim("Permission", "refillEmployerWallet");
                });
            });
        }
    }
}
