using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using BookMgtApi.Models;

namespace BookMgtApi.Data
{
    public class AppDbContext : IdentityDbContext<User>
    {
        public AppDbContext (DbContextOptions<AppDbContext> options): base(options){}

        public DbSet<Book> Books {get; set;} 
        public DbSet<Author> Authors { get; set; }
    }
}