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
        public DbSet<FavoriteShow> FavoriteShows { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            //Joining Show with Tag using showId and tagId as keys
            modelBuilder.Entity<ShowTag>()
                .HasKey(st => new {st.ShowId, st.TagId });
            modelBuilder.Entity<ShowTag>()
                .HasOne(s => s.Show)
                .WithMany(st => st.ShowTags)
                .HasForeignKey(t => t.ShowId);
            modelBuilder.Entity<ShowTag>()
                .HasOne(t => t.Tag)
                .WithMany(st => st.ShowTags)
                .HasForeignKey(s => s.TagId);

            //Joining Show with Binge using showId and bingeId as keys
            modelBuilder.Entity<ShowBinge>()
                .HasKey(pc => new { pc.ShowId, pc.BingeId });
            modelBuilder.Entity<ShowBinge>()
                .HasOne(s => s.Show)
                .WithMany(sb => sb.ShowBinges)
                .HasForeignKey(b => b.ShowId);
            modelBuilder.Entity<ShowBinge>()
                .HasOne(b => b.Binge)
                .WithMany(sb => sb.ShowBinges)
                .HasForeignKey(s => s.BingeId);

            //One to Many and Many to One Relationship between Binges and User
            modelBuilder.Entity<Binge>()
                .HasOne(b => b.Author)
                .WithMany(ub => ub.Binges)
                .HasForeignKey(u => u.UserId);
            modelBuilder.Entity<User>()
                .HasMany(u => u.Binges)
                .WithOne(b => b.Author)
                .OnDelete(DeleteBehavior.Cascade);

            //Many to Many Relationsips between Users and Shows

            modelBuilder.Entity<FavoriteShow>()
                .HasKey(pc => new { pc.UserId, pc.ShowId });
            modelBuilder.Entity<User>()
                .HasMany(u => u.FavoriteShows)
                .WithOne()
                .HasForeignKey(u => u.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<FavoriteShow>()
                .HasOne(b => b.Show)
                .WithMany()
                .HasForeignKey(s => s.ShowId)
                .OnDelete(DeleteBehavior.Cascade);



        }


    }
}
