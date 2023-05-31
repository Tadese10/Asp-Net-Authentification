using Microsoft.EntityFrameworkCore;

namespace Data
{
    //db connection instance
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            
        }

         //name of the tables in DB
        public DbSet<User> Users => Set<User>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }
}