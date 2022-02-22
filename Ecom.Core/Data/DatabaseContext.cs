using Microsoft.EntityFrameworkCore;
using ECom.Core.Entities;

namespace ECom.Core.Data
{
    public class DatabaseContext : DbContext
    {
        private readonly DbContextOptions _options;
        public DatabaseContext(DbContextOptions options) : base(options)
        {
            _options = options;
        }
        public DbSet<ApiClient> ApiClients { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductCategory> ProductCategory { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<ProductReview> ProductReviews { get; set; }
        public DbSet<ReviewComment> ReviewComments { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasMany(c => c.ProductReviews)
                .WithOne(e => e.Reviewer);
            modelBuilder.Entity<User>()
              .HasMany(c => c.ReviewComments)
              .WithOne(e => e.User);

        }
    }
}
