using Microsoft.EntityFrameworkCore;
using LibraryManagement.Domain.Models;

namespace LibraryManagement.Persistence.Postgres.Configurations;

public class BookLoanConfiguration : IEntityTypeConfiguration<BookLoan>
{
    public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<BookLoan> builder)
    {
        builder.HasKey(bl => bl.Id);

        builder.HasOne(bl => bl.Book)
            .WithMany(b => b.BookLoans)
            .HasForeignKey(bl => bl.BookId);

        builder.HasOne(bl => bl.User)
            .WithMany(u => u.BookLoans)
            .HasForeignKey(bl => bl.UserId);
    }
}
