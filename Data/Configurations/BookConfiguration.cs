using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.Data.Configurations
{
    public class BookConfiguration : IEntityTypeConfiguration<Book>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Book> builder)
        {
            builder.HasKey(b => b.Id);

            builder.HasIndex(b => new { b.Title, b.Author })
                .IsUnique();

            builder.Property(b => b.IsAvailable)
                .HasDefaultValue(true);

            builder.HasMany(b => b.BookLoans)
                .WithOne(bl => bl.Book)
                .HasForeignKey(bl => bl.BookId);
        }
    }
}
