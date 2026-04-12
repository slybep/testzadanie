using Microsoft.EntityFrameworkCore;
using URLShorter.Models;

namespace URLShorter
{
    public class URLShorterDbContext : DbContext
    {
        private readonly IConfiguration _configuration;
        public URLShorterDbContext(DbContextOptions<URLShorterDbContext> options,
            IConfiguration configuration) : base(options)
        {
            _configuration = configuration;
        }
        public DbSet<Link> links { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Link>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("timestamp")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");
            });
        }
    }
}
