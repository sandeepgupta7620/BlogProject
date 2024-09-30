using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

using Microsoft.EntityFrameworkCore;

namespace SmallProjectSandeepGupta.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<User> UsersSandeepGupta { get; set; }
        public DbSet<Post> PostsSandeepGupta { get; set; }
        public DbSet<Like> LikesSandeepGupta { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // UserID is an identity column
            modelBuilder.Entity<User>()
                .HasKey(u => u.UserID);

            modelBuilder.Entity<User>()
                .Property(u => u.UserID)
                .ValueGeneratedOnAdd(); // Auto-incremented identity column

            // Define relationships
            modelBuilder.Entity<User>()
                .HasMany(u => u.Posts)
                .WithOne(p => p.User)
                .HasForeignKey(p => p.UserID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                .HasMany(u => u.Likes)
                .WithOne(l => l.User)
                .HasForeignKey(l => l.UserID)
                .OnDelete(DeleteBehavior.Cascade);

            // Define any other relationships for Posts and Likes similarly
        }
    }
}

