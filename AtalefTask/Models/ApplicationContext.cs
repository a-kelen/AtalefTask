using AtalefTask.Models.Configurations;
using Microsoft.EntityFrameworkCore;

namespace AtalefTask.Models
{
    public class ApplicationContext : DbContext
    {
        public virtual DbSet<SmartMatchItem> SmartMatchResult { get; set; }

        public ApplicationContext()
        {
        }

        public ApplicationContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new SmartMatchItemConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}
