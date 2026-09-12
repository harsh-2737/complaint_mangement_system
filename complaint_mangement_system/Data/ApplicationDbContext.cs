using complaint_mangement_system.Models;
using Microsoft.EntityFrameworkCore;

namespace complaint_mangement_system.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Complaint> Complaints { get; set; }

        public DbSet<Comment> Comments { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) 
        { 
            base.OnModelCreating(modelBuilder); 
            modelBuilder.Entity<User>().HasData
            (
                new User 
                { 
                    userid = 1, 
                    name = "Admin", 
                    email = "admin@gmail.com", 
                    password = "1234", 
                    country_code = "+91", 
                    phone_no = "9316143733", 
                    role = "Admin" 
                }
             ); 
        }
    }
}