using Microsoft.EntityFrameworkCore;
using VSATemplate.Features.Common.Entities;

namespace VSATemplate.Features.Common.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>()
                .HasQueryFilter(a => !a.IsDeleted);
        }

        public DbSet<Course> Courses { get; set; }

        public DbSet<Student> Students { get; set; }

        public DbSet<Teacher> Teachers { get; set; }
    }
}
