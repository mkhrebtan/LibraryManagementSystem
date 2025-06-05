using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore;
using LibraryManagementSystem.Models;
using Microsoft.Extensions.Configuration;
using LibraryManagementSystem.Data.Configurations;

namespace LibraryManagementSystem.Data;

public class LibraryDbContext : DbContext
{
    public LibraryDbContext()
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var builder = new ConfigurationBuilder()
            .AddUserSecrets<Program>();
        var configuration = builder.Build();
        var secretProvider = configuration.Providers.First();
        secretProvider.TryGet("ConnectionStrings:PostDbConnection", out string? connectionString);
        optionsBuilder.UseNpgsql(connectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new BookConfiguration());
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new BookLoanConfiguration());

        base.OnModelCreating(modelBuilder);
    }

    public DbSet<Book> Books { get; set; } = null!;
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<BookLoan> BookLoans { get; set; } = null!;
}
