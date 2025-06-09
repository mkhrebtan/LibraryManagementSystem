using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore;
using LibraryManagement.Persistence.Postgres.Configurations;
using LibraryManagement.Domain.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.UserSecrets;

namespace LibraryManagement.Persistence.Postgres;

public class LibraryDbContext : DbContext
{
    private readonly IConfigurationRoot _configuration;

    public LibraryDbContext(IConfigurationRoot configuration)
    {
        _configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(_configuration.GetConnectionString("LibraryDb"));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new BookConfiguration());
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new BookLoanConfiguration());
        modelBuilder.ApplyConfiguration(new BookSubscriptionConfiguration());

        base.OnModelCreating(modelBuilder);
    }

    public DbSet<Book> Books { get; set; } = null!;
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<BookLoan> BookLoans { get; set; } = null!;
    public DbSet<BookSubscription> BookSubscriptions { get; set; } = null!;
}
