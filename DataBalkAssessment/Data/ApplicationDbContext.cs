using DataBalkAssessment.Models;
using Microsoft.EntityFrameworkCore;

namespace DataBalkAssessment.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }

        public DbSet<User> users { get; set; }
        public DbSet<DataBalkAssessment.Models.Task> Tasks { get; set; }  

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("users");
            modelBuilder.Entity<DataBalkAssessment.Models.Task>().ToTable("tasks");
        }

    }
}
