﻿using MovieDatabase.Model;
using System.Data.Entity;

namespace MovieDatabase.DataAccess
{
    public class MovieDatabaseDbContext : DbContext
    {
        public MovieDatabaseDbContext() : base("MovieDatabaseDb")
        {

        }

        public DbSet<Movie> Movies { get; set; }
        public DbSet<Director> Directors { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
