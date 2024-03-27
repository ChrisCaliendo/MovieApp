﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MovieApp.Data;

#nullable disable

namespace MovieApp.Migrations
{
    [DbContext(typeof(DataContext))]
    partial class DataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("MovieApp.Models.Binge", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Binges");
                });

            modelBuilder.Entity("MovieApp.Models.FavoriteShow", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<int>("ShowId")
                        .HasColumnType("int");

                    b.HasKey("UserId", "ShowId");

                    b.HasIndex("ShowId");

                    b.ToTable("FavoriteShows");
                });

            modelBuilder.Entity("MovieApp.Models.Show", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(0);

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImageUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("Timespan")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Shows");
                });

            modelBuilder.Entity("MovieApp.Models.ShowBinge", b =>
                {
                    b.Property<int>("ShowId")
                        .HasColumnType("int");

                    b.Property<int>("BingeId")
                        .HasColumnType("int");

                    b.HasKey("ShowId", "BingeId");

                    b.HasIndex("BingeId");

                    b.ToTable("ShowBinges");
                });

            modelBuilder.Entity("MovieApp.Models.ShowTag", b =>
                {
                    b.Property<int>("ShowId")
                        .HasColumnType("int");

                    b.Property<int>("TagId")
                        .HasColumnType("int");

                    b.HasKey("ShowId", "TagId");

                    b.HasIndex("TagId");

                    b.ToTable("ShowTags");
                });

            modelBuilder.Entity("MovieApp.Models.Tag", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Tags");
                });

            modelBuilder.Entity("MovieApp.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("MovieApp.Models.Binge", b =>
                {
                    b.HasOne("MovieApp.Models.User", "Author")
                        .WithMany("Binges")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Author");
                });

            modelBuilder.Entity("MovieApp.Models.FavoriteShow", b =>
                {
                    b.HasOne("MovieApp.Models.Show", "Show")
                        .WithMany()
                        .HasForeignKey("ShowId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MovieApp.Models.User", null)
                        .WithMany("FavoriteShows")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Show");
                });

            modelBuilder.Entity("MovieApp.Models.ShowBinge", b =>
                {
                    b.HasOne("MovieApp.Models.Binge", "Binge")
                        .WithMany("ShowBinges")
                        .HasForeignKey("BingeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MovieApp.Models.Show", "Show")
                        .WithMany()
                        .HasForeignKey("ShowId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Binge");

                    b.Navigation("Show");
                });

            modelBuilder.Entity("MovieApp.Models.ShowTag", b =>
                {
                    b.HasOne("MovieApp.Models.Show", "Show")
                        .WithMany("ShowTags")
                        .HasForeignKey("ShowId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MovieApp.Models.Tag", "Tag")
                        .WithMany("ShowTags")
                        .HasForeignKey("TagId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Show");

                    b.Navigation("Tag");
                });

            modelBuilder.Entity("MovieApp.Models.Binge", b =>
                {
                    b.Navigation("ShowBinges");
                });

            modelBuilder.Entity("MovieApp.Models.Show", b =>
                {
                    b.Navigation("ShowTags");
                });

            modelBuilder.Entity("MovieApp.Models.Tag", b =>
                {
                    b.Navigation("ShowTags");
                });

            modelBuilder.Entity("MovieApp.Models.User", b =>
                {
                    b.Navigation("Binges");

                    b.Navigation("FavoriteShows");
                });
#pragma warning restore 612, 618
        }
    }
}
