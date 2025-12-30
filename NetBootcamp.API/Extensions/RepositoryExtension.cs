using Microsoft.EntityFrameworkCore;
using NetBootcamp.Repository;
using NetBootcamp.Repository.Repositories;

namespace NetBootcamp.API.Extensions;

public static class RepositoryExtension
{
    public static void AddRepositories(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(x =>
        {
            x.UseSqlServer(configuration.GetConnectionString("SqlServer"), x =>
            {
                x.MigrationsAssembly(typeof(RepositoryAssembly).Assembly.GetName().Name);
            });
        });
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>)); // generic birden fazla entity alırsa burada "," ekliyoruz.
        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }
}
