using Microsoft.EntityFrameworkCore;
using SmartHomeAPI.Entity;

namespace SmartHomeAPI.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options) { }
        
        public DbSet<AppUser> Users { get; set; }
    }
}