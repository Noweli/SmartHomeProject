using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SmartHomeAPI.Data;

namespace SmartHomeAPI.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static void AddApplicationServices(this IServiceCollection serviceCollection,
            IConfiguration configuration)
        {
            serviceCollection.AddDbContext<DataContext>(options =>
            {
                options.UseSqlite(configuration.GetConnectionString("DefaultConnection"));
            });
        }
    }
}