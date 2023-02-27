using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryBorrower.Models
{
    public class LibraryDbContext : DbContext
    {

        public LibraryDbContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
        public DbSet<Category> category { get; set; }
        public DbSet<LibraryItem> libraryItem { get; set; }
    }
}
