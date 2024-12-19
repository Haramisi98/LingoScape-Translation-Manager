using LingoScape.DataAccessLayer;
using Microsoft.Extensions.DependencyInjection;

namespace LingoScape.Repository.Infrastructure
{
    public static class RepoInitializator
    {
        public static void InitRepoServices(IServiceCollection services, string connectionString)
        {
            services.AddScoped<LingoDbContext>((sp) => new LingoDbContext(connectionString));
        }
    }
}
