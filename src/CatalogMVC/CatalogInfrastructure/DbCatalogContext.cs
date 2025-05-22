using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using CatalogDomain.Model;

namespace CatalogInfrastructure;

public partial class DbCatalogContext : DbContext
{
    public DbCatalogContext()
    {
    }

    public DbCatalogContext(DbContextOptions<DbCatalogContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Actor> Actors { get; set; }

    public virtual DbSet<DirectedBy> DirectedBies { get; set; }

    public virtual DbSet<Director> Directors { get; set; }

    public virtual DbSet<Genre> Genres { get; set; }

    public virtual DbSet<Movie> Movies { get; set; }

    public virtual DbSet<MovieCast> MovieCasts { get; set; }

    public virtual DbSet<MovieGenre> MovieGenres { get; set; }

    public virtual DbSet<Rating> Ratings { get; set; }

    public virtual DbSet<ToWatchList> ToWatchLists { get; set; }

    public virtual DbSet<UserRating> UserRatings { get; set; }

    public virtual DbSet<WatchedList> WatchedLists { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=DefaultConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Actor>(entity =>
        {
            entity.ToTable("Actor");

            entity.Property(e => e.FirstName).HasMaxLength(100);
            entity.Property(e => e.LastName).HasMaxLength(100);
            entity.Property(e => e.Nationality).HasMaxLength(100);
        });

        modelBuilder.Entity<DirectedBy>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_DirectedBy_1");

            entity.ToTable("DirectedBy");

            entity.Property(e => e.DirectorId).HasColumnName("DirectorID");
            entity.Property(e => e.MovieId).HasColumnName("MovieID");

            entity.HasOne(d => d.Director).WithMany(p => p.DirectedBies)
                .HasForeignKey(d => d.DirectorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DirectedBy_Director");

            entity.HasOne(d => d.Movie).WithMany(p => p.DirectedBies)
                .HasForeignKey(d => d.MovieId)
                .HasConstraintName("FK_DirectedBy_Movie");
        });

        modelBuilder.Entity<Director>(entity =>
        {
            entity.ToTable("Director");

            entity.Property(e => e.FirstName).HasMaxLength(100);
            entity.Property(e => e.LastName).HasMaxLength(100);
            entity.Property(e => e.Nationality).HasMaxLength(100);
        });

        modelBuilder.Entity<Genre>(entity =>
        {
            entity.ToTable("Genre");

            entity.Property(e => e.GenreName).HasMaxLength(100);
        });

        modelBuilder.Entity<Movie>(entity =>
        {
            entity.ToTable("Movie");

            entity.Property(e => e.Title).HasMaxLength(100);
        });

        modelBuilder.Entity<MovieCast>(entity =>
        {
            entity.ToTable("MovieCast");

            entity.Property(e => e.ActorId).HasColumnName("ActorID");
            entity.Property(e => e.MovieId).HasColumnName("MovieID");

            entity.HasOne(d => d.Actor).WithMany(p => p.MovieCasts)
                .HasForeignKey(d => d.ActorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MovieCast_Actor");

            entity.HasOne(d => d.Movie).WithMany(p => p.MovieCasts)
                .HasForeignKey(d => d.MovieId)
                .HasConstraintName("FK_MovieCast_Movie");
        });

        modelBuilder.Entity<MovieGenre>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_MovieGenre_1");

            entity.ToTable("MovieGenre");

            entity.Property(e => e.GenreId).HasColumnName("GenreID");
            entity.Property(e => e.MovieId).HasColumnName("MovieID");

            entity.HasOne(d => d.Genre).WithMany(p => p.MovieGenres)
                .HasForeignKey(d => d.GenreId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MovieGenre_Genre");

            entity.HasOne(d => d.Movie).WithMany(p => p.MovieGenres)
                .HasForeignKey(d => d.MovieId)
                .HasConstraintName("FK_MovieGenre_Movie");
        });

        modelBuilder.Entity<Rating>(entity =>
        {
            entity.ToTable("Rating");

            entity.Property(e => e.MovieId).HasColumnName("MovieID");
            entity.Property(e => e.Source).HasMaxLength(100);

            entity.HasOne(d => d.Movie).WithMany(p => p.Ratings)
                .HasForeignKey(d => d.MovieId)
                .HasConstraintName("FK_Rating_Movie");
        });

        modelBuilder.Entity<ToWatchList>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_ToWatchList_1");

            entity.ToTable("ToWatchList");

            entity.Property(e => e.AddedDate).HasColumnType("datetime");
            entity.Property(e => e.MovieId).HasColumnName("MovieID");
            entity.Property(e => e.UserId)
                .HasMaxLength(450)
                .HasColumnName("UserID");

            entity.HasOne(d => d.Movie).WithMany(p => p.ToWatchLists)
                .HasForeignKey(d => d.MovieId)
                .HasConstraintName("FK_ToWatchList_Movie");
        });

        modelBuilder.Entity<UserRating>(entity =>
        {
            entity.ToTable("UserRating");

            entity.Property(e => e.MovieId).HasColumnName("MovieID");
            entity.Property(e => e.UserId)
                .HasMaxLength(450)
                .HasColumnName("UserID");

            entity.HasOne(d => d.Movie).WithMany(p => p.UserRatings)
                .HasForeignKey(d => d.MovieId)
                .HasConstraintName("FK_UserRating_Movie");
        });

        modelBuilder.Entity<WatchedList>(entity =>
        {
            entity.ToTable("WatchedList");

            entity.Property(e => e.MovieId).HasColumnName("MovieID");
            entity.Property(e => e.UserId)
                .HasMaxLength(450)
                .HasColumnName("UserID");
            entity.Property(e => e.WatchedDate).HasColumnType("datetime");

            entity.HasOne(d => d.Movie).WithMany(p => p.WatchedLists)
                .HasForeignKey(d => d.MovieId)
                .HasConstraintName("FK_WatchedList_Movie");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
