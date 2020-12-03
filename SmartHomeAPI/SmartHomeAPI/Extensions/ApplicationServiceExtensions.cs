using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SmartHomeAPI.Data;
using SmartHomeAPI.Interfaces;
using SmartHomeAPI.Services;

namespace SmartHomeAPI.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static void AddApplicationServices(this IServiceCollection serviceCollection,
            IConfiguration configuration)
        {
            serviceCollection.AddScoped<ITokenService, TokenService>();
            serviceCollection.AddDbContext<DataContext>(options =>
            {
                options.UseSqlite(configuration.GetConnectionString("DefaultConnection"));
            });
        }
    }
}