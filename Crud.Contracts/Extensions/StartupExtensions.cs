using Microsoft.Extensions.DependencyInjection;

namespace MisCanchas.Contracts
{
    public static class StartupExtensions
    {
        public static void AddMisCanchasContracts(this IServiceCollection services)
        {
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        }
    }
}
