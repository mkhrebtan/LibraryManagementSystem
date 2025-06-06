using Microsoft.EntityFrameworkCore;
using LibraryManagementSystem.Models;

namespace LibraryManagementSystem.Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<User> builder)
        {
            builder.HasKey(u => u.Id);

            builder.HasMany(u => u.BookLoans)
                .WithOne(bl => bl.User)
                .HasForeignKey(bl => bl.UserId);

            builder.HasMany(u => u.BookSubscriptions)
                .WithOne(bs => bs.User)
                .HasForeignKey(bs => bs.UserId);
        }
    }
}
