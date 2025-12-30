using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace NetBootcamp.Repository.Repositories  
{
    public static class SeedExtensions
    {
        public static void SeedDatabase(this WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                SeedData.SeedDatabase(dbContext);
            }
        }
    }
}
