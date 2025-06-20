﻿using Microsoft.EntityFrameworkCore;
using LibraryManagement.Domain.Models;

namespace LibraryManagement.Persistence.Postgres.Configurations;

public class BookSubscriptionConfiguration : IEntityTypeConfiguration<BookSubscription>
{
    public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<BookSubscription> builder)
    {
        builder.HasKey(bs => bs.Id);

        builder.HasOne(bs => bs.Book)
            .WithMany(b => b.BookSubscriptions)
            .HasForeignKey(bs => bs.BookId);

        builder.HasOne(bs => bs.User)
            .WithMany(u => u.BookSubscriptions)
            .HasForeignKey(bs => bs.UserId);
    }
}
