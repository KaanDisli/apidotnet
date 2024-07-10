using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace api.Models.Repositories
{
    public class LibraryContext: DbContext

    {
        public DbSet<User> Users {get;set;}
        public DbSet<Book> Books {get;set;}
        protected readonly IConfiguration Configuration;
        //////////////////////////////////////////////////
        public LibraryContext(DbContextOptions<LibraryContext> options, IConfiguration configuration)
            : base(options)
        {
            Configuration = configuration;
        }
        /////////////////////////////////////////////////////
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            optionsBuilder.UseNpgsql(Configuration.GetConnectionString("DefaultConnection"));
            
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>().HasIndex(b=>b.serialNumber).IsUnique();
        }
    }
}