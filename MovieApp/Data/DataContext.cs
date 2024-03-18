using Microsoft.EntityFrameworkCore;
using MovieApp.Models;

namespace MovieApp.Data
{
    public class DataContext : DbContext
    {

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        { 

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Show> Shows { get; set; }
        public DbSet<Binge> Binges { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<ShowTag> ShowTags { get; set; }
        public DbSet<ShowBinge> ShowBinges { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            //Joining Show with Tag using showId and tagId as keys
            modelBuilder.Entity<ShowTag>()
                .HasKey(st => new {st.showId, st.tagId });
            modelBuilder.Entity<ShowTag>()
                .HasOne(s => s.show)
                .WithMany(st => st.showTags)
                .HasForeignKey(t => t.showId);
            modelBuilder.Entity<ShowTag>()
                .HasOne(t => t.tag)
                .WithMany(st => st.showTags)
                .HasForeignKey(s => s.tagId);

            //Joining Show with Binge using showId and bingeId as keys
            modelBuilder.Entity<ShowBinge>()
                .HasKey(pc => new { pc.showId, pc.bingeId });
            modelBuilder.Entity<ShowBinge>()
                .HasOne(s => s.show)
                .WithMany(sb => sb.showBinges)
                .HasForeignKey(b => b.showId);
            modelBuilder.Entity<ShowBinge>()
                .HasOne(b => b.binge)
                .WithMany(sb => sb.showBinges)
                .HasForeignKey(s => s.bingeId);
        }


    }
}
