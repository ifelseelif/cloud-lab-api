using Api.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace Cloud_Lab.DataAccess.Database
{
    public sealed class ApplicationContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
    }
}